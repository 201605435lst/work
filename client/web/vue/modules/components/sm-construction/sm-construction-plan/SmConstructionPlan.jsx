
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiPlan from '../../sm-api/sm-construction/ApiPlan';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmConstructionPlanModal from './SmConstructionPlanModal';
import moment from 'moment';
import { PlanState } from './PlanEnum';
import WorkFlowTemplateSelect from '../../sm-bpm/sm-bpm-workflow-templates/SmBpmWorkflowSelectModal';
import SmBpmWorkflowView from '../../sm-bpm/sm-bpm-workflow-view';
import PlanApprovalModal from '../sm-construction-plan-with-gantt/PlanApprovalModal';
import { MasterPlanState } from '../sm-construction-master-plan/MasterPlanEnum';

let apiPlan = new ApiPlan();

export default {
  name: 'SmConstructionPlan',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    showOperator: { type: Boolean, default: false }, // 是否显示操作栏()
    approval: { type: Boolean, default: false },// 是否为审批页面,配合查询条件，获取审批的内容
    onlyQuery: { type: Boolean, default: false },// 只查询,不编辑
    onlyDraw: { type: Boolean, default: false },// 只编制
    showSelectRow: { type: Boolean, default: true }, // 是否显示选择栏
    isSimpleColumn: { type: Boolean, default: false }, //是否是简易column
    selectIds: { type: Array, default: () => [] }, //选择的ids
  },
  data() {
    return {
      queryParams: {
        searchKey: '', // 模糊搜索
        maxResultCount: paginationConfig.defaultPageSize, // 每页数量
        pageIndex: 1, // 当前页1 这个在 传后端 的时候 过滤掉了,放这里方便复制~
        totalCount: 0, // 总数 这个在 传后端 的时候 过滤掉了,放这里方便复制~
        waiting: true,  // 查询待我审批
        approval: this.approval, // 审批相关查询
      },
      list: [], // table 数据源
      loading: false, // table 是否处于加载状态
      iSelectPlanIds: [], // 选择的 施工计划 ids (选择框模式的时候用)
      selectedEntity: undefined, // 选择的实体(任务计划 )
      rowMultiple: false, // table Row 是否是 多选模式
      clickTimeChange: null, // 单击事件的延时变量
    };
  },
  computed: {
    btnDisable() { // 按钮 是否 禁用
      if (this.selectedEntity === undefined) { // 没有选择任何东西  的话
        return true;
      }
      return this.rowMultiple;
    },
    editBtnVisible() { // 控制编辑按钮隐藏
      if (this.approval) return false;
      if (this.onlyQuery) return false;
      return true;
    },
    iShowSelection() { // 控制 选择小圆点隐藏
      if (this.approval) return false;
      if (this.onlyQuery && !this.showSelectRow) return false;
      return this.showSelectRow;
    },
    // 控制 table 搜索 选项是否隐藏
    showTableOperator() {
      if (this.onlyQuery && this.showSelectRow) return false;
      return true;
    },
    // 自定义列表(双击row和单击row 的事件用)
    customRow() { // return 一个 lambda
      return (record, index) => ({
        on: { // 事件
          click: (event) => {
            clearTimeout(this.clickTimeChange);
            this.clickTimeChange = setTimeout( // 因为双击事件会触发 单击事件,所以定义 一个定时器 ,当 0.3秒内 点击两次的话就不算单击啦
              () => {
                if (this.rowMultiple) { // 多选的话 就 往里面加
                  if (this.iSelectPlanIds.includes(record.id)) { // 包含 往里面 删
                    this.iSelectPlanIds = this.iSelectPlanIds.filter(x => x !== record.id);
                  } else { // 不包含 就添加
                    this.iSelectPlanIds.push(record.id);
                  }
                } else { // 单选就替换
                  this.iSelectPlanIds = [record.id];
                  this.selectedEntity = record;
                }
              },
              300,
            );
          },
          dblclick: (event) => {
            clearTimeout(this.clickTimeChange); // 清除延时
            event.preventDefault();
          },
          contextmenu: (event) => {
          },
          mouseenter: (event) => {
          },  // 鼠标移入行
          mouseleave: (event) => {
          },
        },
      });
    },
    columns() {
      let baseColumns = [
        {
          title: '序号', dataIndex: 'id', width: 80, customRender: (text, record, index) => {
            let result = `${index + 1 + this.queryParams.maxResultCount * (this.queryParams.pageIndex - 1)}`;
            return (<span>{result}</span>);
          },
        },

        {
          title: '流程状态', dataIndex: 'stateStr', width: 100, customRender: (text, record, index) => {
            let span = <a-tag>{text}</a-tag>;

            switch (record.state) {
            case 0:
              span = <a-tag color='#bfbfbf'>待提交</a-tag>;
              break;
            case PlanState.ToSubmit:
              span = <a-tag color='#bfbfbf'>待提交</a-tag>;
              break;
            case PlanState.OnReview:
              span = <a-tag color='#faad14'>审核中</a-tag>;
              break;
            case PlanState.Pass:
              span = <a-tag color='#61D023'>已审核</a-tag>;
              break;
            case PlanState.UnPass:
              span = <a-tag color='#f50'>已驳回</a-tag>;
              break;
            }
            return span;
          },
        },
        {
          title: '计划名称', dataIndex: 'name', width: 180, ellipsis: true,
          customRender: (text, record, index) => {
            if (this.approval || this.onlyQuery) {
              return <a-tooltip title={text} placement='topLeft'>
                <a-button size='small' style='padding:0px;' type='link' onClick={() => {
                  this.$refs.PlanApprovalModal.view(record.id);
                }}>{text}</a-button>
              </a-tooltip>;
            }
            return <span>{text}</span>;
          },
        },
        { title: '计划描述', dataIndex: 'content', ellipsis: true },
        {
          title: '计划开始时间', dataIndex: 'planStartTime', width: 120, customRender: (text, record, index) => {
            return (<span>{moment(text).format('YYYY-MM-DD')}</span>);
          },
        },
        {
          title: '计划结束时间', dataIndex: 'planEndTime', width: 120, customRender: (text, record, index) => {
            return (<span>{moment(text).format('YYYY-MM-DD')}</span>);
          },
        },
        { title: '计划工期', dataIndex: 'period', width: 100 },
        {
          title: '负责人', dataIndex: 'charger.name', width: 100,
        },
      ];
      if (this.approval) {
        return [...baseColumns, {
          title: '操作', width: 180, customRender: (record) => {
            return (
              <div>
                <div style='display:inline' onClick={() => this.workflowView(record)}><a>查看</a></div>
                {this.queryParams.waiting &&
                  <div style='display:inline;margin-left:10px;' onClick={() =>
                    this.$refs.PlanApprovalModal.approval(record.id)}>
                    <a>审批</a>
                  </div>
                }
              </div>
            );
          },
        }];
      }
      if (this.onlyDraw) {
        return [...baseColumns, {
          title: '操作', width: 120, customRender: (record) => {
            return (
              (record.state !== PlanState.ToSubmit && record.state !== 0) &&
              <div>
                <div style='display:inline' onClick={() => this.workflowView(record)}><a>查看</a></div>
              </div>
            );
          },
        }];
      }

      return baseColumns;
    },
  },
  watch: {
    selectIds: {
      handler: function (value, oldValue) {
        this.iSelectPlanIds = value;
      },
      immediate: true,
    },

  },

  created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    // 初始化axios,将apiStandard实例化
    initAxios() {
      apiPlan = new ApiPlan(this.axios);
    },
    // 审批记录切换
    tabChange(key) {
      this.queryParams.waiting = key === 1;
      this.refresh();
    },
    // 审批选择回调
    async flowSelected(data) {
      let res = await apiPlan.createWorkFlow(this.iSelectPlanIds[0], data.id);
      if (requestIsSuccess(res)) {
        this.$message.success('提交审批成功!');
        this.refresh();
      }
    },

    // 流程信息查看
    workflowView(record) {
      this.$refs.workflowViewer.view(record.workflowId);
    },
    // 切换 选择模式
    switchSelectType() {
      this.rowMultiple = !this.rowMultiple;
      this.selectPlanContentIds = []; // 同时清空选择的东西
    },
    // 刷新获取list
    async refresh(resetPage = true, page) {
      // 刷新表之前先把 数据 弄回初始化
      this.list = [];
      this.iSelectPlanIds = [];
      this.selectedEntity = undefined;
      if (resetPage) {
        this.queryParams.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }

      this.loading = true;
      let res = await apiPlan.getList({
        skipCount: (this.queryParams.pageIndex - 1) * this.queryParams.maxResultCount,
        onlyPass: this.onlyQuery, // 只查询的话就只能查 已审核的数据
        ...this.queryParams,
      });
      if (requestIsSuccess(res) && res.data) {
        this.list = res.data.items;
        this.queryParams.totalCount = res.data.totalCount;
        // 考虑 第二页只有一条数据的情况,删除这个数据 会导致 当前第二页是空的,实际上应该跳转到第一页才行
        if (page && this.queryParams.totalCount && this.queryParams.maxResultCount) {
          let currentPage = parseInt(this.queryParams.totalCount / this.queryParams.maxResultCount);
          if (this.queryParams.totalCount % this.queryParams.maxResultCount !== 0) {
            currentPage = page + 1;
          }
          if (page > currentPage) {
            this.queryParams.pageIndex = currentPage;
            this.refresh(false, this.queryParams.pageIndex);
          }
        }
        // if (this.list.length === 0 && this.queryParams.pageIndex!==1 ) {
        //   this.queryParams.pageIndex=1;
        //   await this.refresh();
        // }
      }
      this.loading = false;
    },

    // 分页事件
    async onPageChange(page, pageSize) {
      this.queryParams.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
    // 添加(打开添加模态框)
    add() {
      this.$refs.SmConstructionPlanModal.add();
    },
    // 添加(打开添加模态框) 123
    edit(record) {
      if (record.state === PlanState.Pass) return this.$message.error("已审核,不能编辑");
      if (record.state === PlanState.OnReview) return this.$message.error("审核中,不能编辑");
      this.$refs.SmConstructionPlanModal.edit(record);
    },
    // 添加(打开添加模态框) 123
    async approve(record) {
      let id = record.id;
      if (record.state === PlanState.Pass) return this.$message.error("已审核,请勿重复审核");
      if (record.state === MasterPlanState.OnReview) return this.$message.error("审核中,不能再次审核!");
      let res = await apiPlan.hasContent(id);
      if (requestIsSuccess(res)) {
        // console.log(res.data);
        if (!res.data) { //返回true 或者 false
          return this.$message.error('该计划未编制,不能审批');
        }
        this.$refs.flowSelect.show();
      }
    },
    // 编制进度
    draw(record) {
      if (record.state === MasterPlanState.Pass) return this.$message.error("已审核,不能编制进度");
      if (record.state === MasterPlanState.OnReview) return this.$message.error("审核中,不能编制进度");
      this.$emit("draw", record.id);
      console.log('这里要跳转到编制计划详情页面');
    },
    remove(ids) {
      // if (record.state === PlanState.Pass)  return console.warn("已审核,不能编辑");
      // if (record.state === PlanState.OnReview)  return console.warn("审核中,不能编辑");
      if (this.list.filter(x => ids.includes(x.id)).some(x => x.state === PlanState.Pass || x.state === PlanState.OnReview)) {
        return this.$message.error('要删除的计划中包含审核中或者已审核的计划,不能删除!');
      }
      const _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style='color:red;'>{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            const response = await apiPlan.deleteRange(ids);
            _this.refresh(false, _this.queryParams.pageIndex);
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },
  },
  render() {
    return (
      <div class="sm-construction--plan">
        {this.showTableOperator &&
          // 操作区
          <sc-table-operator
            onSearch={() => {
              this.refresh();
            }}
            onReset={() => {
              this.queryParams.searchKey = '';
              this.queryParams.pageIndex = 1;
              this.refresh();
            }}
          >
            <a-form-item label='关键字'>
              <a-input
                placeholder={'请输入计划名称或计划描述'}
                value={this.queryParams.searchKey}
                onInput={async event => {
                  this.queryParams.searchKey = event.target.value;
                  this.queryParams.pageIndex = 1; // 查询的时候 当前页给1,不然查到了数据也不显示
                  this.refresh();
                }}
              />
            </a-form-item>

            <template slot='buttons'>
              {this.editBtnVisible && // 只有 不审批 或者不是 查看 的时候 这些按钮 才有,审批的时候这些按钮 消失
                [
                  <a-button size='small' type='primary' icon='plus' onClick={() => this.add()}>
                    添加
                  </a-button>,
                  <a-button size="small" type='primary' icon={this.rowMultiple ? 'check-circle' : 'check-square'} onClick={() => this.switchSelectType()}>
                    {this.rowMultiple ? '切到单选' : '切到多选'}
                  </a-button>,

                  <a-button size="small" type='primary' icon='' onClick={() => this.edit(this.selectedEntity)} disabled={this.btnDisable}>
                    编辑
                  </a-button>,
                  <a-button size="small" type='primary' icon='' onClick={() => this.approve(this.selectedEntity)} disabled={this.btnDisable}>
                    审批
                  </a-button>,
                  <a-button size="small" type='primary' icon='' onClick={() => this.draw(this.selectedEntity)} disabled={this.btnDisable}>
                    编制进度
                  </a-button>,
                  <a-button size="small" type='danger' icon='' onClick={() => this.remove(this.iSelectPlanIds)} disabled={this.iSelectPlanIds.length === 0}>
                    删除
                  </a-button>,
                ]
              }
            </template>
          </sc-table-operator>
        }
        {/* 审批tab切换按钮 */}
        {this.approval &&
          <a-tabs default-active-key='1' onChange={this.tabChange} size='small'>
            <a-tab-pane key={1} tab='待我审批'>
            </a-tab-pane>
            <a-tab-pane key={2} tab='我审批的' force-render>
            </a-tab-pane>
          </a-tabs>
        }


        {/*展示区*/}
        <a-table
          dataSource={this.list}
          rowKey={record => record.id}
          columns={this.columns}
          customRow={this.customRow}
          loading={this.loading}
          bordered={this.bordered}
          defaultExpandAllRows={true}
          pagination={false}
          rowSelection={this.iShowSelection ? {
            selectedRowKeys: this.iSelectPlanIds,
            // columnWidth: 30,
            type: this.rowMultiple ? 'checkbox' : 'radio',
            onChange: (selectedRowKeys, selectedRows) => {
              this.iSelectPlanIds = selectedRowKeys;
              if (!this.rowMultiple) { // 单选的时候
                this.selectedEntity = this.list.find(x => x.id === selectedRowKeys[0]);
              }
              this.$emit('selectedChange', selectedRowKeys); //往外冒泡 选择的ids
            },
          } : undefined}

        />

        {/*分页*/}
        <a-pagination
          style='margin-top:10px; text-align: right;'
          total={this.queryParams.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.queryParams.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger={true}
          showQuickJumper={true}
          size={this.isSimple || this.isFault ? 'small' : ''}
          showTotal={paginationConfig.showTotal}
        />

        {/*添加/编辑模板*/}
        <SmConstructionPlanModal
          ref='SmConstructionPlanModal'
          axios={this.axios}
          onSuccess={async () => {
            await this.refresh();
          }}
        />
        {/* 审批选择框 */}
        <WorkFlowTemplateSelect
          axios={this.axios}
          ref="flowSelect"
          onSelected={this.flowSelected}
        />
        <PlanApprovalModal
          ref='PlanApprovalModal'
          axios={this.axios}
          onSuccess={() => {
            this.refresh();
          }}
        />

        {/* 流程查看器 */}
        <SmBpmWorkflowView axios={this.axios} ref="workflowViewer" />
      </div>
    );
  },



};
