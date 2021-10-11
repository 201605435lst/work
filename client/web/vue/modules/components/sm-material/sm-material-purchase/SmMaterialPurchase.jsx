import './style';
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import FileSaver from 'file-saver';
import ApiPurchase from '../../sm-api/sm-material/Purchase';
import moment from 'moment';
import MaterialPurchaseModal from './SmMaterialPurchaseModal';
import MaterialPurchaseFlowModal from './SmMaterialPurchaseFlowModal';
import MaterialPurchaseApprovalModal from './SmMaterialPurchaseApprovalModal';
import permissionsSmMaterial from '../../_permissions/sm-material';

let apiPurchase = new ApiPurchase();

export default {
  name: 'SmMaterialPurchase',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      purchases: [], //列表数据源
      form: this.$form.createForm(this),
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        startTime: null,
        endTime: null,
        maxResultCount: paginationConfig.defaultPageSize,
        isInitiate: false, //我发起的
      },
      purchaseIds: [],
      loading: false,
      visible: false,

      purchaseId: '', //审批后，需要更新状态的采购计划的Id
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '#',
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '计划编号',
          dataIndex: 'code',
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '用料计划名称',
          dataIndex: 'name',
        },
        {
          title: '计划采购时间',
          dataIndex: 'planTime',
          scopedSlots: { customRender: 'planTime' },
        },
        {
          title: '创建人',
          dataIndex: 'creator',
          scopedSlots: { customRender: 'creator' },
        },
        {
          title: '创建时间',
          dataIndex: 'creationTime',
          scopedSlots: { customRender: 'creationTime' },
        },
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
      ];
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiPurchase = new ApiPurchase(this.axios);
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiPurchase.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response) && response.data) {
        this.purchases = response.data.items;
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
      this.$refs.materialPurchaseModal.add();
    },

    view(record) {
      this.$refs.materialPurchaseModal.view(record);
    },

    approval(record) {
      this.$refs.materialPurchaseApprovalModal.approval(record);
    },

    edit(record) {
      this.$refs.materialPurchaseModal.edit(record);
    },

    //导出
    export(ids) {
      let _this = this;
      this.loading = true;
      let data = { purchaseIds: ids };
      return new Promise(async (resolve, reject) => {
        let response = await apiPurchase.export(data);
        _this.loading = false;
        if (requestIsSuccess(response)) {
          FileSaver.saveAs(
            new Blob([response.data], { type: 'application/vnd.ms-excel' }), `采购计划清单.xlsx`);
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
            let response = await apiPurchase.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.$message.success('操作成功');
              _this.refresh(false, null, _this.pageIndex);
            }
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },

    //查看流程
    async viewWorkFlow(record) {
      this.loading = true;
      let response = await apiPurchase.getRunFlowInfo(record.workflowId, record.id);
      if (requestIsSuccess(response) && response.data) {
        this.getNodes(response.data[0]);
        this.nodes = response.data;
        this.$refs.materialPurchaseFlowModal.view(this.nodes);
      }
      console.log(this.nodes);
      this.loading = false;
    },
    //递归拼装nodes
    getNodes(data) {
      let name = '';
      data.approvers.length > 0
        ? data.approvers.map(
          (item, ind) => (name += item.name + (data.approvers.length - 1 == ind ? '' : ',') + ''),
        )
        : undefined;
      if (data.type != 'bpmEnd') {
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

    //更新采购计划审批的状态（每个审批节点完都会进行更新）
    async updatePurchaseState() {
      let response = await apiPurchase.updateState(this.purchaseId);
      if (requestIsSuccess(response)) {
        this.refresh();
      }
    },
  },
  render() {
    return (
      <div class="sm-material-purchase">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            (this.queryParams.startTime = null), (this.queryParams.endTime = null), this.refresh();
          }}
        >
          <a-form-item label="计划时间">
            <div style="display:flex">
              <a-date-picker
                placeholder="起始时间"
                value={this.queryParams.startTime ? this.queryParams.startTime : null}
                onChange={value => {
                  this.queryParams.startTime = value ? moment(value._d).format('YYYY-MM-DD') : null;
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
                  this.queryParams.endTime = value ? moment(value._d).format('YYYY-MM-DD') : null;
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
          <template slot="buttons">
            <div style={'display:flex'}>
              {vIf(
                <a-button type="primary" onClick={() => this.add()}>
                  {' '}
                  新增
                </a-button>,
                vP(this.permissions, permissionsSmMaterial.Purchases.Create),
              )}
              {vIf(
                <a-button
                  type="primary"
                  onClick={() => this.export(this.purchaseIds)}
                  disabled={this.purchaseIds.length === 0}
                  loading={this.loading}
                >
                  {' '}
                  <a-icon type="export" /> 导出
                </a-button>,
                vP(this.permissions, permissionsSmMaterial.Purchases.Export),
              )}
              {vIf(
                <a-button type="primary" onClick={() => this.$refs.materialPurchaseFlowModal.add()}>流程配置</a-button>,
                vP(this.permissions, permissionsSmMaterial.Purchases.SetFlow),
              )}
              <a-button
                type="primary"
                onClick={() => {
                  this.queryParams.isInitiate = false;
                  this.refresh();
                }}
              >
                全部
              </a-button>
              <a-button
                type="primary"
                onClick={() => {
                  this.queryParams.isInitiate = true;
                  this.refresh();
                }}
              >
                我发起的
              </a-button>
            </div>
          </template>
        </sc-table-operator>

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.purchases}
          pagination={false}
          loading={this.loading}
          rowSelection={{
            columnWidth: 30,
            onChange: selectedRowKeys => {
              this.purchaseIds = selectedRowKeys;
            },
          }}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              code: (text, record) => {
                return (
                  <a
                    onClick={() => {
                      this.view(record);
                    }}
                  >
                    {record.code}
                  </a>
                );
              },
              planTime: (text, record) => {
                let planTime =
                  moment(record.planTime).format('YYYY-MM-DD') != '0001-01-01'
                    ? moment(record.planTime).format('YYYY-MM-DD')
                    : '暂无';
                return (
                  <a-tooltip placement="topLeft" title={planTime}>
                    <span>{planTime}</span>
                  </a-tooltip>
                );
              },
              creator: (text, record) => {
                return record.userName;
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
                  <a-tag color="blue">审批中</a-tag>
                ) : record.state == 3 ? (
                  <a-tag color="green">审批通过</a-tag>
                ) : (
                  <a-tag color="red">审批未通过</a-tag>
                );
              },
              operations: (text, record) => {
                return [
                  <span>
                    {record.state == 1 && record.state != 4 ? (
                      <span>
                        {vIf(
                          <a
                            onClick={() => {
                              this.edit(record);
                            }}
                          >
                            编辑
                          </a>,
                          vP(this.permissions, permissionsSmMaterial.Purchases.Update),
                        )}
                        {vIf(
                          <a-divider type="vertical" />,
                          vP(this.permissions, permissionsSmMaterial.Purchases.Update) &&
                          vP(this.permissions, permissionsSmMaterial.Purchases.Delete),
                        )}
                        {vIf(
                          <a
                            onClick={() => {
                              this.remove(record);
                            }}
                          >
                            删除
                          </a>,
                          vP(this.permissions, permissionsSmMaterial.Purchases.Delete),
                        )}
                      </span>
                    ) : (
                      <span>
                        {record.workflowId != null && record.workflowId != '00000000-0000-0000-0000-000000000000' ? (
                          <span>
                            {vIf(
                              <a
                                onClick={() => {
                                  this.viewWorkFlow(record);
                                }}
                              >
                                流程
                              </a>,
                              vP(this.permissions, permissionsSmMaterial.Purchases.Flow),
                            )}
                            {vIf(
                              <a-divider type="vertical" />,
                              vP(this.permissions, permissionsSmMaterial.Purchases.Flow) &&
                              vP(this.permissions, permissionsSmMaterial.Purchases.Approval),
                            )}
                          </span>
                        ) : undefined}
                        {record.canApproval ? (
                          <span>
                            {vIf(
                              <a
                                onClick={() => {
                                  this.purchaseId = record.id;
                                  this.approval(record);
                                }}
                              >
                                审批
                              </a>,
                              vP(this.permissions, permissionsSmMaterial.Purchases.Approval),
                            )}
                            {vIf(
                              <a-divider type="vertical" />,
                              vP(this.permissions, permissionsSmMaterial.Purchases.Approval) &&
                              vP(this.permissions, permissionsSmMaterial.Purchases.Export),
                            )}
                          </span>
                        ) : undefined}
                        {vIf(
                          <a
                            onClick={() => {
                              this.export([record.id]);
                            }}
                          >
                            导出
                          </a>,
                          vP(this.permissions, permissionsSmMaterial.Purchases.Export),
                        )}
                      </span>
                    )}
                  </span>,
                ];
              },
            },
          }}
        ></a-table>

        {/* 分页器 */}
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

        {/* 添加计划模态框 */}
        <MaterialPurchaseModal
          ref="materialPurchaseModal"
          axios={this.axios}
          onSuccess={() => { this.visible = false; this.refresh(); }}
        />

        {/* 流程设置模态框 */}
        <MaterialPurchaseFlowModal
          ref="materialPurchaseFlowModal"
          axios={this.axios}
        />

        {/* 流程审批模态框 */}
        <MaterialPurchaseApprovalModal
          ref="materialPurchaseApprovalModal"
          axios={this.axios}
          onSuccess={() => { this.updatePurchaseState(); }}
        />
      </div>
    );
  },
};
