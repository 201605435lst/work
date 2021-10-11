
import './style';
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiMaterialPlan from '../../sm-api/sm-material/UsePlan';
import MaterialPurchaseFlowModal from '../sm-material-purchase/SmMaterialPurchaseFlowModal';
import MaterialPlanModal from './SmMaterialMaterialPlanModal';
import moment from 'moment';
import FileSaver from 'file-saver';
let apiMaterialPlan = new ApiMaterialPlan();
import permissionsSmMaterial from '../../_permissions/sm-material';

export default {
  name: 'SmMaterialMaterialPlan',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
    isSimple: { type: Boolean, default: false },//是否精简模式
    multiple: { type: Boolean, default: false },//是否多选
    selected: { type: Array, default: () => [] },//所选
    advancedCount: { type: Number, default: 6 },
  },
  data() {
    return {
      plans: [], //列表数据源
      form: this.$form.createForm(this),
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        startTime: null,
        endTime: null,
        maxResultCount: paginationConfig.defaultPageSize,
        isWaiting: false, //待我审批
      },
      planIds: [],
      loading: false,
      visible: false,
    };
  },
  computed: {
    columns() {
      return this.isSimple?[
        {
          title: '序号',
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '名称',
          dataIndex: 'name',
        },
        {
          title: '计划时间',
          dataIndex: 'planTime',
          scopedSlots: { customRender: 'planTime' },
        },
        {
          title: '创建人',
          dataIndex: 'creator',
        }, 
      ]:
        [
          {
            title: '序号',
            dataIndex: 'index',
            scopedSlots: { customRender: 'index' },
          },
          {
            title: '名称',
            dataIndex: 'name',
          },
          {
            title: '计划时间',
            dataIndex: 'planTime',
            scopedSlots: { customRender: 'planTime' },
          },
          {
            title: '创建时间',
            dataIndex: 'creationTime',
            scopedSlots: { customRender: 'creationTime' },
          },
          {
            title: '创建人',
            dataIndex: 'creator',
          },
          {
            title: '审核状态',
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
  watch: {
    selected: {
      handler: function (value, oldVal) {
        this.iSelected = value;
        this.planIds = value.map(item => item.id);
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiMaterialPlan = new ApiMaterialPlan(this.axios);
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiMaterialPlan.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response) && response.data) {
        this.plans = response.data.items;
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

    add(){
      this.$refs.materialPlanModal.add();
    },

    view(record){
      this.$refs.materialPlanModal.view(record);
    },

    edit(record){
      this.$refs.materialPlanModal.edit(record);
    },

    //导出
    export(ids) {
      let _this = this;
      this.loading = true;
      let data = { usePlanIds: this.planIds };
      return new Promise(async (resolve, reject) => {
        let response = await apiMaterialPlan.export(data);
        _this.loading = false;
        if (requestIsSuccess(response)) {
          FileSaver.saveAs(
            new Blob([response.data], { type:'application/vnd.ms-excel'}), `用料计划清单.xlsx`);
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
            let response = await apiMaterialPlan.delete(record.id);
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
    async viewWorkFlow(record){
      this.loading = true;
      let response = await apiMaterialPlan.getRunFlowInfo(record.workflowId,record.id);
      if (requestIsSuccess(response) && response.data) {
        this.getNodes(response.data[0]);
        this.nodes = response.data;
        this.$refs.materialPurchaseFlowModal.view(this.nodes);
      }
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
    //更新所选数据
    updateSelected(selectedRows) {
      if (this.multiple) {
        // 过滤出其他页面已经选中的
        let _selected = [];
        for (let item of this.iSelected) {
          let target = this.plans.find(subItem => subItem.id === item.id);
          if (!target) {
            _selected.push(item);
          }
        }
    
        // 把当前页面选中的加入
        for (let id of this.planIds) {
          let plan = this.plans.find(item => item.id === id);
          if (!!plan) {
            _selected.push(JSON.parse(JSON.stringify(plan)));
          }
        }
    
        this.iSelected = _selected;
      } else {
        this.iSelected = selectedRows;
      }
    
      this.$emit('change', this.iSelected);
    },
  },
  render() {
    return (
      <div class="sm-technology-material-plan">
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
                placeholder="开始时间"
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
              <p style="margin: 0 3px;">-</p>
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
                添加
                </a-button>,
                vP(this.permissions, permissionsSmMaterial.UsePlans.Create),
              )}
              {vIf(
                <a-button
                  type="primary"
                  onClick={() => this.export(this.planIds)}
                  disabled={this.planIds.length === 0}
                  loading={this.loading}
                >
                  {' '}
                  <a-icon type="export" /> 导出
                </a-button>,
                vP(this.permissions, permissionsSmMaterial.UsePlans.Export),
              )}
              <a-button
                type="primary"
                onClick={() => {
                  this.queryParams.isWaiting = false;
                  this.refresh();
                }}
              >
                全部
              </a-button>
              <a-button
                type="primary"
                onClick={() => {
                  this.queryParams.isWaiting = true;
                  this.refresh();
                }}
              >
                待我审批
              </a-button>
            </div>
          </template>
        </sc-table-operator>

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.plans}
          pagination={false}
          loading={this.loading}
          rowSelection={this.isSimple ? {
            type: this.multiple ? 'checkbox' : 'radio',
            columnWidth: 30,
            selectedRowKeys: this.planIds,
            onChange: (selectedRowKeys, selectedRows) => {
              this.planIds = selectedRowKeys;
              this.updateSelected(selectedRows);
            },
          } : undefined}
          scroll={this.isSimple ? { y: 300 } : undefined}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              planTime: (text, record) => {
                let planTime =
                moment(record.time).format('YYYY-MM-DD') != '0001-01-01'
                  ? moment(record.time).format('YYYY-MM-DD')
                  : '暂无';
                return (
                  <a-tooltip placement="topLeft" title={planTime}>
                    <span>{planTime}</span>
                  </a-tooltip>
                );
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
                    {vIf(
                      <a
                        onClick={() => {
                          this.view(record);
                        }}
                      >
                        查看
                      </a>,
                      vP(this.permissions, permissionsSmMaterial.UsePlans.Detail),
                    )}
                    {record.state == 1 &&  record.state != 4 ? (
                      <span>
                        {vIf(
                          <a-divider type="vertical" />,
                          vP(this.permissions, permissionsSmMaterial.UsePlans.Detail) &&
                          vP(this.permissions, permissionsSmMaterial.UsePlans.Update),
                        )}
                        {vIf(
                          <a
                            onClick={() => {
                              this.edit(record);
                            }}
                          >
                            编辑
                          </a>,
                          vP(this.permissions, permissionsSmMaterial.UsePlans.Update),
                        )}
                        {vIf(
                          <a-divider type="vertical" />,
                          vP(this.permissions, permissionsSmMaterial.UsePlans.Delete) &&
                          vP(this.permissions, permissionsSmMaterial.UsePlans.Update),
                        )}
                        {vIf(
                          <a
                            onClick={() => {
                              this.remove(record);
                            }}
                          >
                            删除
                          </a>,
                          vP(this.permissions, permissionsSmMaterial.UsePlans.Delete),
                        )}
                      </span>
                    ) : record.workflowId != null && record.workflowId != '00000000-0000-0000-0000-000000000000' ? (
                      <span>
                        {vIf(
                          <a-divider type="vertical" />,
                          vP(this.permissions, permissionsSmMaterial.UsePlans.Delete) &&
                          vP(this.permissions, permissionsSmMaterial.UsePlans.Flow),
                        )}
                        {vIf(
                          <a
                            onClick={() => {
                              this.viewWorkFlow(record);
                            }}
                          >
                            流程
                          </a>,
                          vP(this.permissions, permissionsSmMaterial.UsePlans.Flow),
                        )}
                      </span>
                    ) : undefined}
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

        {/* 流程查看模态框 */}
        <MaterialPurchaseFlowModal
          ref="materialPurchaseFlowModal"
          axios={this.axios}
          canSelect={false}
        />

        {/** 计划模态框 */}
        <MaterialPlanModal
          axios={this.axios}
          ref="materialPlanModal"
          onSuccess={() => {this.visible = false; this.refresh();}}
        />
      </div>
    );
  },
};