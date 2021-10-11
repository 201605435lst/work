
import './style';
import { requestIsSuccess, getTaskGroup, getPriorityType, vP, vIf} from '../../_utils/utils';
import ApiTask from '../../sm-api/sm-task/Task';
import { TaskGroup, PriorityType } from '../../_utils/enum';
import ApiAccount from '../../sm-api/sm-system/Account';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import moment from 'moment';
import permissionsSmTask from '../../_permissions/sm-task';
import FileSaver from 'file-saver';

let apiTask = new ApiTask();
let apiAccount = new ApiAccount();

export default {
  name: 'SmTaskTasks',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    group: { type: Number, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      tasks: [],
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        keywords: null,
        group: TaskGroup.Initial,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      form: this.$form.createForm(this),
      loading: false,
      loadingMemberId: undefined,
      taskIds: [],// 导出
    };
  },
  computed: {
    columns() {
      return this.queryParams.group == null || this.queryParams.group == TaskGroup.Initial ? [
        {
          title:'#',
          dataIndex:'index',
          scopedSlots:{customRender: 'index'},
        },
        {
          title:'任务主题',
          dataIndex:'name',
        },
        {
          title:'状态',
          dataIndex:'state',
          scopedSlots:{customRender:'state'},
        },
        {
          title:'开始时间',
          dataIndex:'startTime',
          scopedSlots:{customRender:'startTime'},
        },
        {
          title:'结束时间',
          dataIndex:'endTime',
          scopedSlots:{customRender:'endTime'},
        },
        {
          title:'优先级',
          dataIndex:'priority',
          scopedSlots:{customRender:'priority'},
        },
        {
          title:'负责人',
          dataIndex:'manager',
          scopedSlots:{customRender:'manager'},
        },
        // {
        //   title:'操作员',
        //   dataIndex:'manager',
        //   scopedSlots:{customRender:'manager'},
        // },
        
        {
          title:'操作',
          dataIndex:'operations',
          width: 169,
          scopedSlots: { customRender: 'operations' },
          fixed: 'right',
        },
      ]
        :
        [
          {
            title:'#',
            dataIndex:'index',
            scopedSlots:{customRender: 'index'},
          },
          {
            title:'任务主题',
            dataIndex:'name',
          },
          {
            title:'状态',
            dataIndex:'state',
            scopedSlots:{customRender:'state'},
          },
          {
            title:'开始时间',
            dataIndex:'startTime',
            scopedSlots:{customRender:'startTime'},
          },
          {
            title:'结束时间',
            dataIndex:'endTime',
            scopedSlots:{customRender:'endTime'},
          },
          {
            title:'优先级',
            dataIndex:'priority',
            scopedSlots:{customRender:'priority'},
          },
          {
            title:'负责人',
            dataIndex:'manager',
            scopedSlots:{customRender:'manager'},
          },
          {
            title:'指派人',
            dataIndex:'assign',
            scopedSlots:{customRender:'assign'},
          },
          {
            title:'是否过期',
            dataIndex:'pastDue ',
            scopedSlots:{customRender:'pastDue'},
          },
          {
            title:'创建时间',
            dataIndex:'createTime',
            scopedSlots:{customRender:'createTime'},
          },
          // {
          //   title:'操作',
          //   dataIndex:'operations',
          //   width: 169,
          //   scopedSlots: { customRender: 'operations' },
          //   fixed: 'right',
          // },
        ];
    },
  },
  watch: {
    group: {
      handler: function (value, oldValue) {
        this.queryParams.group = value || TaskGroup.Initial;
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
    this.getLoadingMember();
  },
  methods: {
    initAxios() {
      apiTask = new ApiTask(this.axios);
      apiAccount = new ApiAccount(this.axios);
    },
    async getLoadingMember(){ //获取当前登录人id
      let response = await apiAccount.getAppConfig();
      if(requestIsSuccess(response) && response.data){
        this.loadingMemberId = response.data.currentUser.id;
      }
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiTask.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response) && response.data) {
        this.tasks = response.data.items;
        this.totalCount = response.data.totalCount;
        //console.log(this.tasks);
        if (page && this.totalCount && this.queryParams.maxResultCount) {
          let currentPage = parseInt(this.totalCount / this.queryParams.maxResultCount);
          if (this.totalCount % this.queryParams.maxResultCount !== 0) {
            currentPage = page + 1;
          }
          if (page > currentPage) {
            this.pageIndex = currentPage;
            this.refresh(false, this.pageIndex);
          }
        }
      }
      this.loading = false;
    },
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
    //删除
    remove(selectedRowKeys) {
      if (selectedRowKeys && selectedRowKeys.length > 0) {
        let _this = this;
        this.$confirm({
          title: tipsConfig.remove.title,
          content: h => (
            <div style="color:red;">
              {tipsConfig.remove.content}
            </div>
          ),
          okType: 'danger',
          onOk() {
            return new Promise(async (resolve, reject) => {
              let response = await apiTask.delete(selectedRowKeys);
              if (requestIsSuccess(response)) {
                _this.$message.success('任务已删除');
                _this.refresh();
                setTimeout(resolve, 100);
              } else {
                setTimeout(reject, 100);
              }
            });
          },
          onCancel() { },
        });
      } else {
        this.$message.error('请选择要删除的任务！');
      }
    },
    //添加
    add(){
      this.$emit('add');
    },
    //查看/发起人可编辑
    edit(record){
      this.$emit('edit', record);
    },
    //反馈/详情
    view(record) {
      this.$emit('view', record);
    },
    //导出
    export(){
      let _this = this;
      this.loading = true;
      let data = { taskIds:this.taskIds};
      return new Promise(async (resolve, reject) => {
        let response = await apiTask.export(data);
        _this.loading = false;
        if (requestIsSuccess(response)) {
          FileSaver.saveAs(
            new Blob([response.data], { type: 'application/vnd.ms-excel' }),
            `任务清单.xlsx`,
          );
          setTimeout(resolve, 100);
        } else {
          setTimeout(reject, 100);
        }
      });
    },
    //计算过期时常
    timeDiff(result,nowData,endTime){
      let sDate1 = moment(nowData).format('YYYY-MM-DD');
      let sDate2 = moment(endTime).format('YYYY-MM-DD');
      let aDate,oDate1,oDate2;
      aDate = sDate1.split("-");  
      oDate1 = new Date(aDate[1]  +  '-'  +  aDate[2]  +  '-'  +  aDate[0]);    //转换为12-18-2002格式  
      aDate = sDate2.split("-");  
      oDate2 = new Date(aDate[1]  +  '-'  +  aDate[2]  +  '-'  +  aDate[0]);  
      result = '超期' + parseInt(Math.abs(oDate1  -  oDate2)  /  1000  /  60  /  60  /24) + '天';    //把相差的毫秒数转换为天数
      console.log(result);
      //return result;
    },
  },
  render() {
    let _taskGroup = [];
    for (let item in TaskGroup) {
      _taskGroup.push(TaskGroup[item]);
    }
    return (
      <div class="sm-task-tasks">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keywords = null;
            this.queryParams.group = this.group;
            this.refresh();
          }}
        >
          {this.group === null ? (
            <a-form-item label="分组">
              <a-select
                //allowClear
                axios={this.axios}
                value={this.queryParams.group}
                onChange={value => {
                  this.queryParams.group = value;
                  this.refresh();
                }}
              >
                {_taskGroup.map(item => (
                  <a-select-option value={item}>{getTaskGroup(item)}</a-select-option>
                ))}
              </a-select>
            </a-form-item>
          ) : (
            undefined
          )}

          {[TaskGroup.Initial, TaskGroup.Manage, TaskGroup.Cc].indexOf(this.group) >= 0
          ||this.group === null ? (
              <a-form-item label="关键字">
                <a-input
                  axios={this.axios}
                  placeholder={'请输入项目名称或任务名称'}
                  value={this.queryParams.keywords}
                  onInput={event => {
                    this.queryParams.keywords = event.target.value;
                    this.refresh();
                  }}
                />
              </a-form-item>
            ) : (
              undefined
            )}
          <template slot="buttons">
            {this.queryParams.group == 1 || this.queryParams.group == null ? 
              vIf(
                <a-button type="primary" onClick={() =>this.add()} disabled={this.isCanExport}> <a-icon type="plus" /> 添加</a-button>,
                vP(this.permissions, permissionsSmTask.Tasks.Create),
              )
              : undefined
            }
            {
              vIf(
                <a-button type="primary" onClick={() => this.export()} disabled={this.taskIds.length === 0} loading={this.loading}> <a-icon type="export" /> 导出</a-button>,
                vP(this.permissions, permissionsSmTask.Tasks.Export),
              )
            }
            
          </template>
        </sc-table-operator>

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.tasks}
          bordered={this.bordered}
          pagination={false}
          loading={this.loading}
          rowSelection={{
            columnWidth: 30,
            onChange: selectedRowKeys => {
              this.taskIds = selectedRowKeys;
            },
          }}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              state: (text, record, index) => {
                return record.state == 5 ? <a-tooltip placement='topLeft' title='未启动'><si-country style={"font-size:18px;color:#ebb00c"} /></a-tooltip> : 
                  (record.state == 6 ? <a-tooltip placement='topLeft' title='暂停'><si-suspended style={"font-size:18px;color:#eb0c0c"} /></a-tooltip> : 
                    (record.state == 1 ? <a-tooltip placement='topLeft' title='进行中'><si-success style={"font-size:18px;color:#1890ff"} /></a-tooltip> : 
                      (record.state == 2 ? <a-tooltip placement='topLeft' title='结项'><si-reduce style={"font-size:18px;color:#eb0c0c"} /></a-tooltip> : 
                        (record.state == 3 ? <a-tooltip placement='topLeft' title='完成'><si-smile style={"font-size:18px;color:#3ae367"} /></a-tooltip> : 
                          (record.state == 4 ? <a-tooltip placement='topLeft' title='驳回'><si-reeor style={"font-size:18px;color:#eb0c0c"} /></a-tooltip> : undefined)))));
              },
              startTime: (text,record) => {
                let startTime = moment(record.startTime).format('YYYY-MM-DD') != '0001-01-01' ? moment(record.startTime).format('YYYY-MM-DD HH:mm:ss') : '暂无';
                return (<a-tooltip placement='topLeft' title={startTime}><span>{startTime}</span></a-tooltip>);
              },
              endTime: (text,record) => {
                let endTime = record.endTime ? moment(record.endTime).format('YYYY-MM-DD HH:mm:ss') : '';
                return (<a-tooltip placement='topLeft' title={endTime}><span>{endTime}</span></a-tooltip>);
              },
              priority: (text, record, index) => {
                return getPriorityType(record.priority);
              },
              manager: (text, record, index) => {
                let result = '';
                result = record.taskRltMembers.length > 0 ? record.taskRltMembers.map((item,ind) => item.responsible == 2 ? result + item.member.name + (record.taskRltMembers.length - 1 == ind ? "" : ",") : result + '') : null;
                result = result.join('');
                result = result != '' ? (result.indexOf(',') != -1 && result.lastIndexOf(',') == result.length - 1 ? result.substring(0,result.lastIndexOf(',')) : result) : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>);
              },
              assign: (text,record) => {
                let result = '';
                result = record.taskRltMembers.map(item => item.responsible == 1 ? result = item.task.taskRltMembers[0].member.name : result = '');
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>);
              },
              pastDue: (text,record) => {
                let nowData = new Date();
                let result = '';
                let sDate1 = moment(nowData).format('YYYY-MM-DD');
                let sDate2 = moment(record.endTime).format('YYYY-MM-DD');
                if(sDate1< sDate2){
                  result = '否';
                }else{
                  let aDate,oDate1,oDate2;
                  aDate = sDate1.split("-");  
                  oDate1 = new Date(aDate[1]  +  '-'  +  aDate[2]  +  '-'  +  aDate[0]);    //转换为12-18-2002格式  
                  aDate = sDate2.split("-");  
                  oDate2 = new Date(aDate[1]  +  '-'  +  aDate[2]  +  '-'  +  aDate[0]);  
                  result = parseInt(Math.abs(oDate1  -  oDate2)  /  1000  /  60  /  60  /24); //把相差的毫秒数转换为天数
                  return (
                    <a-tooltip placement="topLeft" title={result}>
                      <span style="display:flex">超期<p style="color:red">{result}</p>天</span>
                    </a-tooltip>);
                }
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>);
              },
              createTime: (text,record) => {
                let createTime = moment(record.creationTime).format('YYYY-MM-DD');
                return (<a-tooltip placement='topLeft' title={createTime}><span>{createTime}</span></a-tooltip>);
              },
              
              operations: (text, record) => {
                return (
                  <span>
                    {vIf(
                      <a-tooltip placement='top' arrowPointAtCenter title={"查看"}>
                        <a
                          onClick={() => {this.edit(record);}}
                        >
                          <a-icon type="eye" />
                        </a>
                      </a-tooltip>,
                      vP(this.permissions, permissionsSmTask.Tasks.Detail),
                    )}
                    {record.state == 5 ? undefined : 
                      <a-divider type="vertical" />}
                    {record.state == 5 ? undefined : //任务未启动时不可反馈
                      vIf(
                        <a-tooltip placement='top' arrowPointAtCenter title={"反馈"}>
                          <a
                            onClick={() => {this.view(record);}}
                          >
                            <si-edit style={"font-size: 21px;"}/>
                          </a>
                        </a-tooltip>,
                        vP(this.permissions, permissionsSmTask.Tasks.Update),
                      )
                    }
                    <a-divider type="vertical" />
                    {vIf(
                      <a-tooltip placement='top' arrowPointAtCenter title={"删除"}>
                        <a
                          onClick={() => {this.remove(record.id);}}
                        >
                          <si-ashbin style={"font-size: 21px;color: red"}/>
                        </a>
                      </a-tooltip>,
                      vP(this.permissions, permissionsSmTask.Tasks.Delete),
                    )}
                  </span>
                );
              },
            },
          }}
        ></a-table>

        {/* 分页器 */}
        <a-pagination
          style="float:right; margin-top:10px"
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          showTotal={paginationConfig.showTotal}
        />
      </div>
    );
  },
};
    