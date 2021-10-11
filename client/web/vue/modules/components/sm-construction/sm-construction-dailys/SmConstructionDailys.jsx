import './style';
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import moment from 'moment';
import FileSaver from 'file-saver';
import * as  permissionsConstruction from '../../_permissions/sm-construction';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiDaily from '../../sm-api/sm-construction/Daily';
import WorkFlowTemplateSelect from '../../sm-bpm/sm-bpm-workflow-templates/SmBpmWorkflowSelectModal';
import SmBpmWorkflowView from '../../sm-bpm/sm-bpm-workflow-view';
import { DiaryState, ApprovalStatus } from '../../_utils/enum';
let apiDaily = new ApiDaily();

export default {
  name: 'SmConstructionDailys',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
    approval: { type: Boolean, default: false },// 是否为审批页面,配合查询条件，获取审批的内容
  },
  data() {
    return {
      dataSource: null, //数据--日志
      loading: false,
      totalCount: 0,
      record: null,
      recordList: [],
      selectedRowKeys: [],
      pageIndex: 1,
      dailyId: null,
      queryParams: {
        keyWords: null, //关键字
        startTime: null,
        endTime: null,
        approval: this.approval, // 审批相关查询
        waiting: true,  // 查询待我审批
        maxResultCount: paginationConfig.defaultPageSize,
      },
      commitSelected: null,// 流程提交的选中值
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 60,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '日志编号',
          dataIndex: 'code',
          width: 160,
          ellipsis: true,
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '施工人数',
          ellipsis: true,
          dataIndex: 'builderCount',
          scopedSlots: { customRender: 'builderCount' },
        },
        {
          title: '创建人',
          ellipsis: true,
          dataIndex: 'informant',
          scopedSlots: { customRender: 'informant' },
        },
        {
          title: '填报日期',
          dataIndex: 'date',
          ellipsis: true,
          scopedSlots: { customRender: 'date' },
        },
        {
          title: '流程状态',
          ellipsis: true,
          dataIndex: 'state',
          scopedSlots: { customRender: 'state' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 169,
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiDaily = new ApiDaily(this.axios);
    },
    add() {
      this.$emit('add');
    },
    async view(record) {
      this.$emit('view', record.id);
    },
    edit(record) {
      this.$emit('edit', record.id);
    },
    // 审批
    approvalOpt(record) {
      this.$emit('approval', record.id);
    },
    // 提交
    submit(record) {
      this.commitSelected = record;
      this.$refs.flowSelect.show();
    },
    // 流程模板选择回调
    async flowSelected(data) {
      // 确认流程，并提交
      let response = await apiDaily.createWorkFlow(this.commitSelected.id, data.id);
      if (requestIsSuccess(response)) {
        if (response.data) {
          this.$message.success("流程提交成功");
          this.commitSelected = null;
          this.refresh();
        }
      }
    },
    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiDaily.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.$message.success('操作成功');
              _this.refresh(false, _this.pageIndex);
            }
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },
    async export() {
      let _this = this;
      //导出按钮
      _this.isLoading = true;
      _this.$confirm({
        title: '确认导出',
        content: h => (
          <div style="color:red;">
            {this.selectedRowKeys.length == 0
              ? '确定要导出全部数据吗？'
              : `确定要导出这 ${this.selectedRowKeys.length} 条数据吗？`}
          </div>
        ),
        okType: 'danger',
        onOk() {
          let data = {
            ..._this.queryParams,
            ids: _this.selectedRowKeys,
          };
          return new Promise(async (resolve, reject) => {
            let response = await apiDiary.export(
              data,
            );
            _this.isLoading = false;
            if (requestIsSuccess(response)) {
              FileSaver.saveAs(
                new Blob([response.data], { type: 'application/vnd.ms-excel' }),
                `日志填报记录表.xlsx`,
              );
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },
    async refresh(resetPage = true, page) {
      this.selectedRowKeys = [];
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
        startTime: this.queryParams.startTime
          ? moment(this.queryParams.startTime)
            .format('YYYY-MM-DD')
          : '',
        endTime: this.queryParams.endTime
          ? moment(this.queryParams.endTime).add(1, 'days')
            .format('YYYY-MM-DD')
          : '',
      };
      let response = await apiDaily.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...data,
      });
      if (requestIsSuccess(response)) {
        this.dataSource = response.data.items;
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
      if (this.dataSource.length > 0) {
        this.isCanExport = false;
      } else {
        this.isCanExport = true;
      }
      this.loading = false;
    },
    //切换页码
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
    // 审批记录切换
    tabChange(key) {
      this.queryParams.waiting = key == 1 ? true : false;
      this.refresh();
    },
    // 流程信息查看
    workflowView(record) {
      console.log(record);
      this.$refs.workflowViewer.view(record.workflowId);
    },
    actionOption(text, record) {
      let beforeSubmit =
        <span>
          {vIf(
            <a onClick={() => this.view(record)}>详情</a>,
            vP(this.permissions, permissionsConstruction.Dailys.Detail),
          )}
          {vIf(
            <a-divider type="vertical" />,
            vP(this.permissions, permissionsConstruction.Dailys.Detail) &&
            vP(this.permissions, [
              permissionsConstruction.Dailys.Update,
              permissionsConstruction.Dailys.Submit,
              permissionsConstruction.Dailys.Delete,
            ]),
          )}
          {vIf(
            <a-dropdown trigger={['click']}>
              <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>
                更多 <a-icon type="down" />
              </a>
              <a-menu slot="overlay">
                {vIf(
                  <a-menu-item>
                    <a
                      onClick={() => {
                        this.edit(record);
                      }}
                    >
                      编辑
                    </a>
                  </a-menu-item>,
                  vP(this.permissions, permissionsConstruction.Dailys.Update),
                )}
                {vIf(
                  <a-menu-item>
                    <a
                      onClick={() => {
                        this.submit(record);
                      }}>提交
                    </a>
                  </a-menu-item>,
                  vP(this.permissions, permissionsConstruction.Dailys.Submit),
                )}
                {vIf(
                  <a-menu-item>
                    <a
                      onClick={() => {
                        this.remove(record);
                      }}
                    >
                      删除
                    </a>
                  </a-menu-item>,
                  vP(this.permissions, permissionsConstruction.Dailys.Delete),
                )}
              </a-menu>
            </a-dropdown>,
            vP(this.permissions, [
              permissionsConstruction.Dailys.Update,
              permissionsConstruction.Dailys.Submit,
              permissionsConstruction.Dailys.Delete,
            ]),
          )}
        </span>;
      let afterSubmit =
        <span>
          {vIf(
            <a onClick={() => this.view(record)}>详情</a>,
            vP(this.permissions, permissionsConstruction.Dailys.Detail),
          )}
          <a-divider type="vertical" />
          {vIf(
            <a-dropdown trigger={['click']}>
              <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>
                更多 <a-icon type="down" />
              </a>
              <a-menu slot="overlay">
                {vIf(
                  <a-menu-item>
                    <a
                      onClick={() => {
                        this.workflowView(record);
                      }}
                    >
                      审批流程
                    </a>
                  </a-menu-item>,
                  vP(this.permissions, permissionsConstruction.Dailys.ApproveFlow),
                )}
              </a-menu>
            </a-dropdown>,
            vP(this.permissions, [
              permissionsConstruction.Dailys.ApproveFlow,
            ]),
          )}
        </span>;
      let approvalAction =
        <span>
          {vIf(
            <a onClick={() => this.view(record)}>详情</a>,
            vP(this.permissions, permissionsConstruction.Dailys.Detail),
          )}
          {vIf(
            <a-divider type="vertical" />,
            vP(this.permissions, permissionsConstruction.Dailys.Detail) &&
              this.queryParams.waiting ? vP(this.permissions, permissionsConstruction.Dailys.Approve) : vP(this.permissions, permissionsConstruction.Dailys.ApproveFlow),
          )}
          {this.queryParams.waiting ?
            vIf(
              <a
                onClick={() => {
                  this.approvalOpt(record);
                }}
              >
                审批
              </a>,
              vP(this.permissions, permissionsConstruction.Dailys.Approve),
            )
            :
            vIf(
              <a
                onClick={() => {
                  this.workflowView(record);
                }}
              >
                审批流程
              </a>,
              vP(this.permissions, permissionsConstruction.Dailys.ApproveFlow),
            )
          }
        </span>;
      return this.approval ? approvalAction : record.status == ApprovalStatus.ToSubmit   || record.status==ApprovalStatus.UnPass ? beforeSubmit : afterSubmit;
    },
  },
  render() {
    return (
      <div class="sm-schedule-diarys">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keyWords = null;
            this.queryParams.startTime = null;
            this.queryParams.endTime = null;
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              allowClear={true}
              axios={this.axios}
              placeholder={'请输入填报人'}
              value={this.queryParams.keyWords}
              onInput={event => {
                this.queryParams.keyWords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="时间选择">
            <div style="align-items: center;display: flex;">
              <a-date-picker
                size={this.size}
                style="width:100%"
                placeholder="起始时间"
                value={this.queryParams.startTime}
                onChange={value => {
                  this.queryParams.startTime = value;
                  this.refresh();
                }}
              />
              <p style="margin: 0 3px;">—</p>
              <a-date-picker
                style="width:100%"
                placeholder="结束时间"
                value={this.queryParams.endTime}
                onChange={value => {
                  this.queryParams.endTime = value;
                  this.refresh();
                }}
              />
            </div>
          </a-form-item>
          <template slot="buttons">

            {/* 审批tab切换按钮 */}
            {this.approval ? <div>
              <a-tabs default-active-key="1" onChange={this.tabChange} size='small'>
                <a-tab-pane key="1" tab="待我审批">
                </a-tab-pane>
                <a-tab-pane key="2" tab="我审批的" force-render>
                </a-tab-pane>
              </a-tabs>
            </div> : vIf(<a-button type="primary" onClick={() => this.add()}>
              新增
            </a-button>,
            vP(this.permissions, permissionsConstruction.Dailys.Create),
            )}
          </template>
        </sc-table-operator>

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.dataSource}
          bordered={this.bordered}
          pagination={false}
          loading={this.loading}
          // rowSelection={{
          //   selectedRowKeys: this.selectedRowKeys,
          //   onChange: (selectedRowKeys, recordList) => {
          //     this.recordList = recordList;
          //     this.selectedRowKeys = selectedRowKeys;
          //   },
          // }}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              code: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.code}>
                    <span
                      // class="table-location"
                      onClick={() => {
                        this.view(record);
                      }}
                    >
                      {record.code}
                    </span>
                  </a-tooltip>
                );
              },
              builderCount: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.builderCount}>
                    <span>{record.builderCount}</span>
                  </a-tooltip>
                );
              },
              informant: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.informant ? record.informant.name : null}>
                    <span>{record.informant ? record.informant.name : null}</span>
                  </a-tooltip>
                );
              },
              date: (text, record) => {
                let creationTime =
                  record && record.date ? moment(record.date).format('YYYY-MM-DD') : '';
                return (
                  <a-tooltip placement="topLeft" title={creationTime}>
                    <span>{creationTime}</span>
                  </a-tooltip>
                );
              },
              state: (text, record) => {
                switch (record.status) {
                case ApprovalStatus.ToSubmit:
                  return <a-tag color="#bfbfbf">待提交</a-tag>;
                case ApprovalStatus.OnReview:
                  return <a-tag color="#faad14">审核中</a-tag>;
                case ApprovalStatus.Pass:
                  return <a-tag color="#61D023">已审核</a-tag>;
                case ApprovalStatus.UnPass:
                  return <a-tag color="#f50">已驳回</a-tag>;
                default:
                  return "";
                }
              },
              operations: this.actionOption,
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
        {/* 流程选择框 */}
        <WorkFlowTemplateSelect
          axios={this.axios}
          ref="flowSelect"
          onSelected={this.flowSelected}
        />
        {/* 流程查看器 */}
        <SmBpmWorkflowView axios={this.axios} ref="workflowViewer" />
      </div>
    );
  },
};
