import './style';
import { requestIsSuccess, vP, vIf, getPurchaseListTypeStatus } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import FileSaver from 'file-saver';
import { ApprovalStatus, PurchaseListTypeStatus } from '../../_utils/enum';
import PurchaseList from '../../sm-api/sm-material/PurchaseList';
import moment from 'moment';
import SmMaterialPurchaseListModal from './SmMaterialPurchaseListModal';
import SmBpmWorkflowView from '../../sm-bpm/sm-bpm-workflow-view';
import permissionsMaterial from '../../_permissions/sm-material';
let apiPurchaseList = new PurchaseList();

export default {
  name: 'SmMaterialPurchaseList',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
    isPlan: { type: Boolean, default: false },
    approval: { type: Boolean, default: false },// 是否为审批页面,配合查询条件，获取审批的内容
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
        keyword: null,
        type:null,
        approval: this.approval, // 审批相关查询
        waiting: true,  // 查询待我审批
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
          title: '序号',
          dataIndex: 'index',
          width: 70,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '采购编号',
          dataIndex: 'code',
          width:250,
          ellipsis: true,
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '采购清单名称',
          dataIndex: 'name',
          width:200,
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '采购方式',
          dataIndex: 'type',
          scopedSlots: { customRender: 'type' },
        },
        {
          title: '采购时间',
          dataIndex: 'planTime',
          scopedSlots: { customRender: 'planTime' },
        },
        {
          title: '创建人',
          dataIndex: 'creator',
          scopedSlots: { customRender: 'creator' },
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
      apiPurchaseList = new PurchaseList(this.axios);
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiPurchaseList.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
        startTime: this.queryParams.startTime
          ? moment(this.queryParams.startTime)
            .format('YYYY-MM-DD')
          : '',
        endTime: this.queryParams.endTime
          ? moment(this.queryParams.endTime).add(1, 'days')
            .format('YYYY-MM-DD')
          : '',
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
      this.$refs.SmMaterialPurchaseListModal.add();
    },

    view(record) {
      this.$refs.SmMaterialPurchaseListModal.view(record);
    },

    edit(record) {
      this.$refs.SmMaterialPurchaseListModal.edit(record);
    },
    // 审批
    approvalOpt(record) {
      this.$refs.SmMaterialPurchaseListModal.approval(record);
    },
    // 流程信息查看
    workflowView(record) {
      console.log("record",record);
      this.$refs.workflowViewer.view(record.workflowId);
    },
    // 审批记录切换
    tabChange(key) {
      this.queryParams.waiting = key == 1 ? true : false;
      this.refresh();
    },
    //文件导出
    async export(record) {
      let _this = this;
      //导出按钮
      _this.isLoading = true;
      _this.$confirm({
        title: '确认导出',
        content: h => (
          <div style="color:red;">
            {`确定要导出这条数据的信息吗？`}
          </div>
        ),
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiPurchaseList.export(record ?record.id:'');
            _this.isLoading = false;
            if (requestIsSuccess(response)) {
              FileSaver.saveAs(
                new Blob([response.data], { type: 'text/plain;charset=utf-8' }),
                `${record ? record.name:''}采购单表.docx`,
              );
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
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
            let response = await apiPurchaseList.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.$message.success('操作成功');
              _this.refresh(false, _this.pageIndex);
            }
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },
  },
  render() {
    let Options = [];
    for (let item in PurchaseListTypeStatus) {
      Options.push(
        <a-select-option key={PurchaseListTypeStatus[item]}>
          {getPurchaseListTypeStatus(PurchaseListTypeStatus[item])}
        </a-select-option>,
      );
    }
    return (
      <div class="sm-material-purchase">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keyword = null;
            this.queryParams.startTime = null;
            this.queryParams.endTime = null;
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              allowClear
              placeholder="请输入名称"
              value={this.queryParams.keyword}
              onInput={event => {
                this.queryParams.keyword = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          
          <a-form-item label="采购时间">
            <div style="align-items: center;display: flex;">
              <a-date-picker
                placeholder="起始时间"
                value={this.queryParams.startTime}
                onChange={value => {
                  this.queryParams.startTime = value;
                  this.refresh();
                }}
              />
              <p style="margin: 0 3px;">—</p>
              <a-date-picker
                placeholder="结束时间"
                value={this.queryParams.endTime}
                onChange={value => {
                  this.queryParams.endTime = value;
                  this.refresh();
                }}
              />
            </div>
          </a-form-item>
          <a-form-item label="采购方式">
            <a-select
              placeholder="请选择采购方式"
              style="width:100%"
              allowClear
              onChange={value => {
                this.queryParams.type = value;
                this.refresh();
              }}
            >
              {Options}
            </a-select>
       
          </a-form-item>
          <template slot="buttons">
            <div style={'display:flex'}>


              {/* 审批tab切换按钮 */}
              {this.approval ? <div>
                <a-tabs default-active-key="1" onChange={this.tabChange} size='small'>
                  <a-tab-pane key="1" tab="待我审批">
                  </a-tab-pane>
                  <a-tab-pane key="2" tab="我审批的" force-render>
                  </a-tab-pane>
                </a-tabs>
              </div> : this.isModalSelect ? "" :
                vIf(
                  <a-button type="primary" onClick={() => this.add()}>
                添加
                  </a-button>,
                  vP(this.permissions,permissionsMaterial.PurchaseLists.Create),
                )}
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
          // rowSelection={{
          //   columnWidth: 30,
          //   onChange: selectedRowKeys => {
          //     this.purchaseIds = selectedRowKeys;
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
                    <span>{record.code}</span>
                  </a-tooltip>
                );
              },
              name: (text, record, index) => {
                return (
                  <a-tooltip placement="topLeft" title={text}>
                    <span>{this.approval ? <a onClick={() => this.view(record)}>{text}</a> : { text }}</span>
                  </a-tooltip>
                );
              },
              planTime: (text, record) => {
                let planTime = record && record.planTime
                  ? moment(record.planTime).format('YYYY-MM-DD')
                  : '';
                return (
                  <a-tooltip placement="topLeft" title={planTime}>
                    <span>{planTime}</span>
                  </a-tooltip>
                );
              },
              creator: (text, record) => {
                return record && record.creator ? record.creator.name : '';
              },
              type: (text, record) => {
                return getPurchaseListTypeStatus(record.type);
              },
              state: (text, record) => {
                let _tag = null;
                switch (record.state) {
                case ApprovalStatus.ToSubmit:
                  _tag = <a-tag color='purple'>待提交</a-tag>;
                  break;
                case ApprovalStatus.OnReview:
                  _tag = <a-tag color="blue">审核中</a-tag>;
                  break;
                case ApprovalStatus.Pass:
                  _tag = <a-tag color="green">审核通过</a-tag>;
                  break;

                case ApprovalStatus.UnPass:
                  _tag = <a-tag color="red">审核未通过</a-tag>;
                  break;
                default:
                  _tag = <a-tag color='purple'>待提交</a-tag>;
                  break;
                }
                return <span>{_tag}</span>;
              },
              operations: (text, record) => {
                return this.approval ?
                  <span>
                    {vIf(
                      <a onClick={() => this.workflowView(record)}>详情</a>,
                      vP(this.permissions, permissionsMaterial.PurchaseLists.Detail),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsMaterial.PurchaseLists.Detail) &&
                      record.state == ApprovalStatus.OnReview && this.queryParams.waiting && vP(this.permissions, permissionsMaterial.PurchaseLists.Approval),
                    )}
                    {vIf(
                      <a onClick={() => this.approvalOpt(record)}>审批</a>,
                      record.state == ApprovalStatus.OnReview && this.queryParams.waiting && vP(this.permissions, permissionsMaterial.PurchaseLists.Approval),
                    )}
                  </span>
                  :
                  [<span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.view(record);
                        }}
                      >
                      详情
                      </a>,
                      vP(this.permissions, permissionsMaterial.PurchaseLists.Detail),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsMaterial.PurchaseLists.Detail) &&
                      ( record.state !== ApprovalStatus.ToSubmit &&  vP(this.permissions,permissionsMaterial.PurchaseLists.Detail))||
                      ((record.state == ApprovalStatus.UnPass ? false : record.submit)?
                        vP(this.permissions,  permissionsMaterial.PurchaseLists.Export) :
                        vP(this.permissions, [
                          permissionsMaterial.PurchaseLists.Update,
                          permissionsMaterial.PurchaseLists.Delete,
                        ])),
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
                                onClick={() => this.workflowView(record)}
                              >
                               审批流程
                              </a>
                            </a-menu-item>,
                            record.state !== ApprovalStatus.ToSubmit &&  vP(this.permissions,permissionsMaterial.PurchaseLists.Detail),
                          )}
                        
                          {(record.state == ApprovalStatus.UnPass ? false : record.submit) ? [
                            vIf(
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.export(record);
                                  }}
                                >
                            导出
                                </a>
                              </a-menu-item>,
                              vP(this.permissions,  permissionsMaterial.PurchaseLists.Export),
                            ),
                          ] :
                            [
                              vIf(
                                <a-menu-item>
                                  <a
                                    onClick={() => {
                                      this.edit(record);
                                    }}
                                  >
                                编辑
                                  </a>
                                </a-menu-item>,
                                vP(this.permissions,  permissionsMaterial.PurchaseLists.Update),
                              ),
                              vIf(
                                <a-menu-item>
                                  <a
                                    onClick={() => {
                                      this.remove(record);
                                    }}
                                  >
                                删除
                                  </a>
                                </a-menu-item>,
                                vP(this.permissions,  permissionsMaterial.PurchaseLists.Delete),
                              ) ]
                          }
                        </a-menu>
                      </a-dropdown>,
                      ( record.state !== ApprovalStatus.ToSubmit &&  vP(this.permissions,permissionsMaterial.PurchaseLists.Detail))||
                      ((record.state == ApprovalStatus.UnPass ? false : record.submit)?
                        vP(this.permissions,  permissionsMaterial.PurchaseLists.Export) :
                        vP(this.permissions, [
                          permissionsMaterial.PurchaseLists.Update,
                          permissionsMaterial.PurchaseLists.Delete,
                        ])),
                    )}
                  </span>,
                  ];
              },
              // operations: (text, record) => {
              //   return this.approval ?
              //     <span>
              //       <a onClick={() => this.workflowView(record)}>详情</a>
              //       {record.state == ApprovalStatus.OnReview ?
              //         <span><a-divider type="vertical" /><a onClick={() => this.approvalOpt(record)}>审批</a></span> : ''}
              //     </span>
              //     :
              //     [<span>
              //       <a
              //         onClick={() => {
              //           this.view(record);
              //         }}
              //       >
              //         详情
              //       </a>
              //       <a-divider type="vertical" />
              //       <a-dropdown trigger={['click']}>
              //         <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>
              //           更多 <a-icon type="down" />
              //         </a>
              //         <a-menu slot="overlay">
              //           {record.submit?<a-menu-item>
              //             <a
              //               onClick={() => this.workflowView(record)}
              //             >
              //           审批流程
              //             </a>
              //           </a-menu-item>:''}
              //           {(record.state == ApprovalStatus.UnPass ? false : record.submit) ? [<a-menu-item>
              //             <a
              //               onClick={() => {
              //                 this.export([record.id]);
              //               }}
              //             >
              //               导出
              //             </a>
              //           </a-menu-item>,
              //           ]
              //             :
              //             [
              //               <a-menu-item>
              //                 <a
              //                   onClick={() => {
              //                     this.edit(record);
              //                   }}
              //                 >
              //                   编辑
              //                 </a>
              //               </a-menu-item>,
              //               <a-menu-item>
              //                 <a
              //                   onClick={() => {
              //                     this.remove(record);
              //                   }}
              //                 >
              //                   删除
              //                 </a>
              //               </a-menu-item>,
              //             ]

              //           }

              //         </a-menu>
              //       </a-dropdown>
              //     </span>,
              //     ];
              // },
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
        <SmMaterialPurchaseListModal
          ref="SmMaterialPurchaseListModal"
          axios={this.axios}
          onSuccess={() => { this.refresh(); }}
        />
        {/* 流程查看器 */}
        <SmBpmWorkflowView axios={this.axios} ref="workflowViewer" />
      </div>
    );
  },
};
