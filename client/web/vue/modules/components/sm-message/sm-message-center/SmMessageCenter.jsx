
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiBpmMessage from '../../sm-api/sm-common/message';
import { pagination as paginationConfig } from '../../_utils/config';
import moment from 'moment';
import './style/index.less';

let apiBpmMessage = new ApiBpmMessage();


let bpmMessage = null;
export default {
  name: 'SmMessageCenter',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
  },
  data() {
    return {
      bpmMessages: [], // 列表数据源
      totalCount: 0,
      pageIndex: 1,
      pageSize: paginationConfig.defaultPageSize,
      queryParams:{
        keyword:null,
        isProcess:null,//已读或未读
        maxResultCount: paginationConfig.defaultPageSize,
      },
      loading: false,
      selectedRowKeys: [], // 表格选中的行Ids
    };
  },
  computed: {
    columns(){
      return[
        {
          title:'#',
          dataIndex:'index',
          scopedSlots:{customRender: 'index'},
        },
        {
          title:'标题内容',
          dataIndex:'titleContent',
          scopedSlots:{customRender:'titleContent'},
        },
        {
          title:'消息类型',
          dataIndex:'messageType',
          scopedSlots:{customRender:'messageType'},
        },
        {
          title:'接收时间',
          dataIndex:'creationTime',
          scopedSlots:{customRender:'creationTime'},
        },
        {
          title:'发起人',
          dataIndex:'sponsor',
          scopedSlots: { customRender: 'sponsor' },
        },
        {
          title:'操作',
          dataIndex:'operations',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch:{
    noticeArray:{
      handler:function(val,oldVal){
        this.bpmMessages.push(val[val.length - 1]);
      },
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiBpmMessage = new ApiBpmMessage(this.axios);
    },

    async refresh(resetPage = true, page) {
      this.selectedRowKeys = [];
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiBpmMessage.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response) && response.data) {
        this.bpmMessages = response.data.items;
        this.totalCount = response.data.totalCount;
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

    allBpmMessage(){
      this.queryParams.isProcess = null;
      this.refresh();
    },
    unreadBpmMessage(){
      this.queryParams.isProcess = false;
      this.refresh();
    },
    readBpmMessage(){
      this.queryParams.isProcess = true;
      this.refresh();
    },
    //标记已读
    signRead(){
      if (this.selectedRowKeys && this.selectedRowKeys.length > 0) {
        let selectedMessages = this.selectedRowKeys;
        let _this = this;
        this.$confirm({
          title: '确认设为已读',
          content: h => (
            <div style="color:red;">
              {this.selectedRowKeys.length > 1 ? '确定要将这几条消息标记已读？' : '确认将此消息标记已读？'}
            </div>
          ),
          okType: 'warning',
          onOk() {
            return new Promise(async (resolve, reject) => {
              let response = await apiBpmMessage.updateRange(selectedMessages);
              if (requestIsSuccess(response)) {
                _this.$message.success('操作成功');
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
        this.$message.error('请选择要标记已读的消息！');
      }
    },
    //删除
    remove(id){
      let _this = this;
      this.$confirm({
        title: '消息删除',
        content: h => (
          <div style="color:red;">
            {'确定要删除这条消息？'}
          </div>
        ),
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiBpmMessage.delete(id);
            if (requestIsSuccess(response)) {
              _this.$message.success('消息已删除');
              _this.refresh();
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },
  },
  render() {
    return (
      <div class="sm-message-center">
        <div class="tabsTop">
          <div class="tabsTopLeft"><span>消息类型</span></div>
          {/* 操作区 */}
        </div>
        
        <a-tabs 
          defaultActiveKey="MessageBpm"
          tabPosition="left"
          type="line"
          onChange="callback"
        >
          <a-tab-pane key="MessageBpm" tab="工作流通知">
            
            <sc-table-operator
              onSearch={() => {
                this.refresh();
              }}
              onReset={() => {
                this.queryParams= {    
                  keyword:null,
                };
                this.refresh();
              }}
            >
              <template slot="buttons">
                <span>状态：</span>
                <a-button type="primary" size="small" onClick={() => this.allBpmMessage()}>
                全部
                </a-button>
                <a-button size="small" onClick={() => this.unreadBpmMessage()}>
                未读
                </a-button>
                <a-button size="small" onClick={() => this.readBpmMessage()}>
                已读
                </a-button>
                <a-button type="primary" size="small" icon="check-circle" onClick={() => this.signRead()}>
                标记已读
                </a-button>
              </template>
              
              <a-form-item label="关键字">
                <a-input
                  placeholder="请输入关键字"
                  value={this.queryParams.keyword}
                  onInput={event => {
                    this.queryParams.keyword = event.target.value;
                    this.refresh();
                  }}
                />
              </a-form-item>
            </sc-table-operator>
            <a-table
              columns={this.columns}
              dataSource={this.bpmMessages}
              rowKey={record => record.id}
              bordered={this.bordered}
              loading={this.loading}
              pagination={false}
              rowSelection={{
                columnWidth: 30,
                selectedRowKeys: this.selectedRowKeys,
                onChange: (key,items) => {
                  this.selectedRowKeys = key;
                },
                getCheckboxProps: a => {
                  return {
                    props: {
                      disabled:a.process,
                    },
                  };
                },
              }}
              {...{
                scopedSlots: {
                  index: (text, record, index) => {
                    return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                  },

                  titleContent:(text, record) => {
                    let result = <span style={record.process == true ? "padding-left:10px" : "color:red"}>{record.process == true ? '' : '•'}</span>;
                    return (record.sponsor.name ? <p> {result} <span>{record.sponsor.name}提交的流程需要您的审批，请知晓。</span> </p>: '');
                  },
              
                  messageType: (text, record) => {
                    return record.type == 0 ? "发起" : (record.type == 1 ? "审批" : "抄送");
                  },

                  creationTime:(text,record) => {
                    let creationTime = record.creationTime ? moment(record.creationTime).format('YYYY-MM-DD HH:mm:ss') : '';
                    return (<a-tooltip placement='topLeft' title={creationTime}><span>{creationTime}</span></a-tooltip>);
                  },
                  sponsor:(text,record) => {
                    return record.sponsor.name ? record.sponsor.name : '';
                  },

                  operations: (text, record) => {
                    return [
                      <span>
                        <a-tooltip placement='topLeft' arrowPointAtCenter title={"删除"}>
                          <a
                            onClick={() => {this.remove(record.id);}}
                          >
                            <si-ashbin style={"font-size: 21px;color: red"}/>
                          </a>
                        </a-tooltip>
                      </span>,
                    ];
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
          </a-tab-pane>

          <a-tab-pane key="MessageSystem" tab="系统消息">
            <span slot="tab">
              <a-badge>
                  待办
              </a-badge>
            </span>
              暂无消息
          </a-tab-pane>

          <a-tab-pane key="MessageBacklog" tab="待办消息">
            <span slot="tab">
              <a-badge>
                  待办
              </a-badge>
            </span>
              暂无消息
          </a-tab-pane>

          <a-tab-pane key="MessageOther" tab="其他消息">
            <span slot="tab">
              <a-badge>
                  待办
              </a-badge>
            </span>
              暂无消息
          </a-tab-pane>
          
        </a-tabs>
      </div>
    );
  },
};
    