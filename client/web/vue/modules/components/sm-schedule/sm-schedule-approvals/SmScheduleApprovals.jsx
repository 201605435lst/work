import './style';
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import moment from 'moment';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import ApprovalTableModal from '../sm-schedule-approval-table/SmScheduleApprovalModal';
import SmScheduleApprovalsModal from './SmScheduleApprovalsModal';
import FileSaver from 'file-saver';
import ApiApproval from '../../sm-api/sm-schedule/Approval';
import ApiSchedule from '../../sm-api/sm-schedule/Schedule';

let apiApproval = new ApiApproval();
let apiSchedule = new ApiSchedule();

export default {
  name: 'SmScheduleApprovals',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
    isAuthorizationModal: { type: Boolean, default: false },
    data: { type: Array, default: () => [] },
  },
  data() {
    return {
      approvals: [], //列表数据源
      form: this.$form.createForm(this),
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        keywords: null,
        professionId: null,
        startTime: null,
        endTime: null,
        maxResultCount: paginationConfig.defaultPageSize,
        isInitiate: false, //我发起的
        isCC: false, //我接收的
      },
      loading: false,
      approvalIds: [],
      approvalId: null, //审批单审批Id
      visible: false,
      flowVisible: false,
      nodes: [], //审批流程详情数据
    };
  },

  computed: {
    columns() {
      
      let col= [
        {
          title: '#',
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '编号',
          dataIndex: 'code',
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '专业',
          dataIndex: 'profession',
          scopedSlots: { customRender: 'profession' },
        },
        {
          title: '施工部位',
          dataIndex: 'location',
        },
        {
          title: '施工任务',
          dataIndex: 'name',
        },
        {
          title: '负责人',
          dataIndex: 'director',
          scopedSlots: { customRender: 'director' },
        },
        {
          title: '施工日期',
          dataIndex: 'startTime',
          scopedSlots: { customRender: 'startTime' },
        },
        {
          title: '录入人',
          dataIndex: 'creator',
          scopedSlots: { customRender: 'creator' },
        },
        {
          title: '录入时间',
          dataIndex: 'creationTime',
          scopedSlots: { customRender: 'creationTime' },
        },
      ];
      !this.isAuthorizationModal ? col.push(
        {
          title: '状态',
          dataIndex: 'state',
          scopedSlots: { customRender: 'state' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 169,
          scopedSlots: { customRender: 'operations' },
          fixed: 'right',
        },
      ) : undefined;
      return col;
    },
  },
  watch: {
    data: {
      handler: function(value, oldValue) {
        if (this.data && this.data.length > 0) {
          this.approvals = value;
        }
      },
      immediate: true,
      deep: true,
    },
  },
  async created() {
    this.initAxios();
    this.isAuthorizationModal ? undefined : this.refresh();
  },
  methods: {
    initAxios() {
      apiApproval = new ApiApproval(this.axios);
      apiSchedule = new ApiSchedule(this.axios);
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiApproval.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response) && response.data) {
        this.approvals = response.data.items;
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

    add() {
      this.$emit('add');
    },
    edit(record) {
      this.$emit('edit', record);
    },
    viewTable(record) {
      this.approvalId = record.id;
      this.visible = true;
    },
    //查看工作流审批
    async viewWorkFlow(record) {
      this.loading = true;
      let response = await apiSchedule.getFlowInfo(record.schedule.workflowId,record.schedule.id);
      if (requestIsSuccess(response) && response.data) {
        this.getNodes(response.data[0]);
        this.nodes = response.data;
        this.flowVisible = true;
      }
      this.loading = false;
      // if (record.schedule.workflowId != null) {
      //   this.$refs.SmScheduleApprovalsModal.visible = true;
      // } else {
      //   this.$message.error('未提交审批');
      // }
    },
    //递归拼装nodes
    getNodes(data) {
      let name = '';
      data.approvers.length > 0
        ? data.approvers.map(
          (item, ind) => (name += item.name + (data.approvers.length - 1 == ind ? '' : ',') + ''),
        )
        : undefined;
      if(data.type != 'bpmEnd'){
        data.comments.length > 0
          ? data.comments.map(item => (item.content = item.content.toString()))
          : undefined;
      }
      
      data.comments.length > 0
        ? data.comments.map(
          item => (item.approveTime = moment(item.approveTime).format('YYYY-MM-DD')),
        )
        : undefined;

      data.date =
        data.comments && data.comments.length > 0
          ? moment(data.comments[0].approveTime).format('YYYY-MM-DD')
          : '0001-01-01';
      data.creator = name;

      if (data.children) {
        data.children = data.children.filter(x => x.type != 'bpmCc');
        data.children.map(item => {
          this.getNodes(item);
        });
      }
    },

    //导出
    export(isExcel) {
      let _this = this;
      this.loading = true;
      let data = { approvalIds: this.approvalIds, isExcel: isExcel };
      return new Promise(async (resolve, reject) => {
        let response = await apiApproval.export(data);
        _this.loading = false;
        if (requestIsSuccess(response)) {
          FileSaver.saveAs(
            new Blob([response.data], {
              type: isExcel ? 'application/vnd.ms-excel' : 'application/pdf',
            }),
            isExcel ? `计划审批清单.xlsx` : '计划审批清单.pdf',
          );
          setTimeout(resolve, 100);
        } else {
          setTimeout(reject, 100);
        }
      });
    },
    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiApproval.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.$message.success('操作成功');
              _this.refresh(false, null, _this.pageIndex);
            }
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },
  },
  render() {
   
    return (
      <div class="sm-schedule-approval">
        {/* 操作区 */}
        {this.isAuthorizationModal ? null :
          (<sc-table-operator
            onSearch={() => {
              this.refresh();
            }}
            onReset={() => {
              this.queryParams.keywords = null;
              (this.queryParams.professionId = null),
              (this.queryParams.startTime = null),
              (this.queryParams.endTime = null),
              this.refresh();
            }}
          >
            <a-form-item label="施工专业">
              <DataDictionaryTreeSelect
                axios={this.axios}
                groupCode={'Profession'}
                placeholder="请选择施工专业"
                value={this.queryParams.professionId}
                onChange={value => {
                  this.queryParams.professionId = value;
                  this.refresh();
                }}
              />
            </a-form-item>
           
            <a-form-item label="时间选择">
              <div style="display:flex">
                <a-date-picker
                  placeholder="起始时间"
                  value={this.queryParams.startTime ? this.queryParams.startTime : null}
                  onChange={value => {
                    this.queryParams.startTime = value
                      ? moment(value._d).format('YYYY-MM-DD')
                      : null;
                    if (
                      (value != null && this.queryParams.endTime != null) ||
                        (value == null && this.queryParams.endTime == null)
                    ) {
                      this.refresh();
                    }
                  }}
                />
                <p style="margin: 0 3px;">—</p>
                <a-date-picker
                  placeholder="结束时间"
                  value={this.queryParams.endTime ? this.queryParams.endTime : null}
                  onChange={value => {
                    this.queryParams.endTime = value
                      ? moment(value._d).format('YYYY-MM-DD')
                      : null;
                    if (
                      (value != null && this.queryParams.startTime != null) ||
                        (value == null && this.queryParams.startTime == null)
                    ) {
                      this.refresh();
                    }
                  }}
                />
              </div>
            </a-form-item>
          

            <a-form-item label="关键字">
              <a-input
                axios={this.axios}
                placeholder={'请输入专业/任务名称/工期'}
                value={this.queryParams.keywords}
                onInput={event => {
                  this.queryParams.keywords = event.target.value;
                  this.refresh();
                }}
              />
            </a-form-item>
         
            <template slot="buttons">
              <div style={'display:flex'}>
                <a-button
                  type="primary"
                  onClick={() => {
                    this.queryParams.isInitiate = false;
                    this.queryParams.isCC = false;
                    this.refresh();
                  }}
                >
                  {' '}
                全部
                </a-button>
                <a-button
                  type="primary"
                  onClick={() => {
                    this.queryParams.isInitiate = true;
                    this.queryParams.isCC = false;
                    this.refresh();
                  }}
                >
                  {' '}
                我发起的
                </a-button>
                <a-button
                  type="primary"
                  onClick={() => {
                    this.queryParams.isCC = true;
                    this.queryParams.isInitiate = false;
                    this.refresh();
                  }}
                >
                  {' '}
                我接收的
                </a-button>
                <a-button
                  type="primary"
                  onClick={() => this.export(false)}
                  disabled={this.approvalIds.length === 0}
                  loading={this.loading}
                >
                  {' '}
                  <a-icon type="export" /> pdf导出
                </a-button>
                <a-button
                  type="primary"
                  onClick={() => this.export(true)}
                  disabled={this.approvalIds.length === 0}
                  loading={this.loading}
                >
                  {' '}
                  <a-icon type="export" /> excel导出
                </a-button>
                {this.queryParams.isInitiate ? (
                  <a-button type="primary" onClick={() => this.add()}>
                    {' '}
                  新增
                  </a-button>
                ) : (
                  undefined
                )}
              </div>
            </template>
          </sc-table-operator>
          )}
        {/* 展示区 */}
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.approvals}
          pagination={false}
          loading={this.loading}
          rowSelection={
            this.isAuthorizationModal
              ? null
              : {
                columnWidth: 30,
                onChange: selectedRowKeys => {
                  this.approvalIds = selectedRowKeys;
                },
              }
          }
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              profession: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.profession.name}>
                    <span>{record.profession.name}</span>
                  </a-tooltip>
                );
              },
              code: (text, record) => {
                return (
                  <a
                    onClick={() => {
                      this.viewTable(record);
                    }}
                  >
                    {record.code}
                  </a>
                );
              },
              startTime: (text, record) => {
                let startTime =
                  moment(record.time).format('YYYY-MM-DD') != '0001-01-01'
                    ? moment(record.time).format('YYYY-MM-DD')
                    : '暂无';
                return (
                  <a-tooltip placement="topLeft" title={startTime}>
                    <span>{startTime}</span>
                  </a-tooltip>
                );
              },
              director: (text, record) => {
                let result = record.approvalRltMembers.filter(x => x.type === 2)[0] ? record.approvalRltMembers.filter(x => x.type === 2)[0].member.name : '暂无';
                return result;
              },
              creator: (text, record) => {
                return record.approvalRltMembers.length>0 && record.approvalRltMembers.filter(x => x.type === 1)[0].member 
                  ?record.approvalRltMembers.filter(x => x.type === 1)[0].member.name:'';
              },
              creationTime: (text, record) => {
                let creationTime =
                  moment(record.creationTime).format('YYYY-MM-DD') != '0001-01-01'
                    ? moment(record.creationTime).format('YYYY-MM-DD')
                    : '暂无';
                return (
                  <a-tooltip placement="topLeft" title={creationTime}>
                    <span>{creationTime}</span>
                  </a-tooltip>
                );
              },
              state: (text, record) => {
                return record.state == 1 ? (
                  <a-tag color="red">待提交</a-tag>
                ) : record.state == 2 ? (
                  <a-tag color="blue">审核中</a-tag>
                ) : (
                  <a-tag color="green">审核通过</a-tag>
                );
              },
              operations: (text, record) => {
                return [
                  <span>
                    <a
                      onClick={() => {
                        this.viewWorkFlow(record);
                      }}
                    >
                      查看
                    </a>
                    {record.state == 1 ? (
                      <span>
                        <a-divider type="vertical" />
                        <a
                          onClick={() => {
                            this.edit(record);
                          }}
                        >
                          编辑
                        </a>
                        <a-divider type="vertical" />
                        <a
                          onClick={() => {
                            this.remove(record);
                          }}
                        >
                          删除
                        </a>{' '}
                      </span>
                    ) : (
                      undefined
                    )}
                  </span>,
                ];
              },
            },
          }}
        ></a-table>

        {/* 分页器 */}
        {this.isAuthorizationModal ? null : (
          <a-pagination
            style="margin-top:10px; text-align: right;"
            total={this.totalCount}
            pageSize={this.queryParams.maxResultCount}
            current={this.pageIndex}
            onChange={this.onPageChange}
            onShowSizeChange={this.onPageChange}
            showSizeChanger
            showQuickJumper
            showTotal={paginationConfig.showTotal}
          />
        )}
        {/* 审批单模态框 */}
        <ApprovalTableModal
          ref="approvalTableModal"
          axios={this.axios}
          id={this.approvalId}
          visible={this.visible}
          onCancel={value => (this.visible = value)}
        ></ApprovalTableModal>

        {/* 审批流程模态框 */}
        <SmScheduleApprovalsModal
          ref="SmScheduleApprovalsModal"
          axios={this.axios}
          nodes={this.nodes}
          visible={this.flowVisible}
          onChange={value => {
            this.flowVisible = value;
            this.nodes = [];
          }}
        />
      </div>
    );
  },
};
