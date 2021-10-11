/**
 * 说明：用料计划维护表单
 * 作者：easten
 */

import './style';
import { requestIsSuccess, vIf, vP } from '../../_utils/utils';
import ApiMaterial from '../../sm-api/sm-technology/Material';
import ApiPurchasePlan from '../../sm-api/sm-material/PurchasePlan';
import moment from 'moment';
import FileSaver from 'file-saver';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmTechnologyMaterialPlanModal from './SmTechnologyMaterialPlanModal';
import { ApprovalStatus } from '../../_utils/enum';
import WorkFlowTemplateSelect from '../../sm-bpm/sm-bpm-workflow-templates/SmBpmWorkflowSelectModal';
import SmBpmWorkflowView from '../../sm-bpm/sm-bpm-workflow-view';
import permissionsTechnology from '../../_permissions/sm-technology';
import permissionsMaterial from '../../_permissions/sm-material';
let apiMaterial = new ApiMaterial();
let apiPurchasePlan = new ApiPurchasePlan();

export default {
  name: 'SmTechnologyMaterialPlan',
  props: {
    permissions: { type: Array, default: () => [] },
    isMaterialPlanSelect: { type: Boolean, default: false },//是否精简模式
    multiple: { type: Boolean, default: false },//是否多选
    selected: { type: Array, default: () => [] },//所选计划
    axios: { type: Function, default: null },
    advancedCount: { type: Number, default: 6 },
    default: { type: Boolean, default: false },//  默认状态
    approval: { type: Boolean, default: false },// 是否为审批页面,配合查询条件，获取审批的内容
    material: { type: Boolean, default: false },// 是否在物资管理模块中使用，此模式下只有生成采购计划按钮
    small: { type: Boolean, default: false }, // 最小尺寸模式
    isSelect: { type: Boolean, default: false },//是否选择模式
  },
  data() {
    return {
      queryParams: {
        keywords: null,
        maxResultCount: paginationConfig.defaultPageSize,
        startTime: null,
        endTime: null,
        approval: this.approval, // 审批相关查询
        waiting: true,  // 查询待我审批
        submit:undefined,
      },
      selectedPlanIds: [],//已选计划ids
      iSelected: [],//已选计划
      totalCount: 0,
      pageIndex: 1,
      loading: false,
      materialPlans: [],//用料计划数据
      quantities: [],
      commitSelected: null,// 流程提交的选中值
    };
  },
  computed: {
    columns() {
      let columnsArr =
        [
          {
            title: '序号',
            dataIndex: 'index',
            align: 'center',
            width: 70,
            scopedSlots: { customRender: 'index' },
          },
          {
            title: '计划名称',
            dataIndex: 'planName',
            ellipsis: true,
            scopedSlots: { customRender: 'planName' },
          },
          {
            title: '计划时间',
            dataIndex: 'planTime',
            ellipsis: true,
            scopedSlots: { customRender: 'planTime' },
          },
          {
            title: '创建时间',
            dataIndex: 'creationTime',
            ellipsis: true,
            scopedSlots: { customRender: 'creationTime' },
          },
          {
            title: '创建人',
            dataIndex: 'creator.userName',
            ellipsis: true,
          },
        ];
      //
      if (this.material) {
        // 物资模块使用
        columnsArr.splice(5, 0, {
          title: '状态',
          dataIndex: 'submit',
          scopedSlots: { customRender: 'submit' },
        });
        columnsArr.push({
          title: '操作',
          align: 'center',
          dataIndex: 'operation',
          scopedSlots: { customRender: 'operation' },
          width: 200,
        });
      } else if (this.default || this.approval) {
        columnsArr.splice(5, 0, {
          title: '流程状态',
          align: 'center',
          dataIndex: 'status',
          scopedSlots: { customRender: 'status' },
        }); columnsArr.push({
          title: '操作',
          align: 'center',
          dataIndex: 'operation',
          scopedSlots: { customRender: 'operation' },
          width: 200,
        });

      } else if (this.isMaterialPlanSelect || this.multiple) {
        columnsArr.splice(5, 0, {
          title: '流程状态',
          dataIndex: 'status',
          scopedSlots: { customRender: 'status' },
        });
      }
      return columnsArr;
    },
    size() {
      if (this.small) return "small";
      else return "default";
    },
  },
  watch: {
    selected: {
      handler: function (value, oldVal) {
        this.iSelected = value;
        this.selectedPlanIds = value.map(item => item.id);
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
      apiMaterial = new ApiMaterial(this.axios);
      apiPurchasePlan = new ApiPurchasePlan(this.axios);
    },
    async refresh(resetPage = true, page) {
      this.materialIds = [];
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      this.queryParams.approval = this.approval;
      this.queryParams.materialUse = this.material;
      let response = await apiMaterial.getPlanList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
        isSelect: this.isSelect,
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
        this.quantities = response.data.items;
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
    // 查看用料计划详情
    view(record) {
      // 打开用料计划弹窗
      this.$refs.smTechnologyMaterialPlanModal.view(record);
    },
    // 流程信息查看
    workflowView(record) {
      this.$refs.workflowViewer.view(record.workflowId);
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
            let response = await apiMaterial.exportPlan(record ?record.id:'');
            _this.isLoading = false;
            if (requestIsSuccess(response)) {
              FileSaver.saveAs(
                new Blob([response.data], { type: 'text/plain;charset=utf-8' }),
                `${record ? record.planName:''}用料计划表.docx`,
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
    // 编辑用料计划
    edit(record) {
      this.$refs.smTechnologyMaterialPlanModal.edit(record);
    },
    // 提交
    submit(record) {
      this.commitSelected = record;
      this.$refs.flowSelect.show();
    },
    // 流程模板选择回调
    async flowSelected(data) {
      console.log(data.id);
      // 确认流程，并提交
      let response = await apiMaterial.createWorkFlow(this.commitSelected.id, data.id);
      if (requestIsSuccess(response)) {
        if (response.data) {
          this.$message.success("流程提交成功");
          this.commitSelected = null;
          this.refresh();
        }
      }
    },
    // 审批
    approvalOpt(record) {
      this.$refs.smTechnologyMaterialPlanModal.approval(record);
    },
    // 审批记录切换
    tabChange(key) {
      this.queryParams.waiting = key == 1 ? true : false;
      this.refresh();
    },
    // 删除用料计划
    delete(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content} </div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiMaterial.planDelete(record.id);
            if (requestIsSuccess(response)) {
              _this.refresh(false, _this.pageIndex);
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },
    // 新增用料计划
    materialPlan() {
      this.$refs.smTechnologyMaterialPlanModal.create();
    },
    // 用料计划生成采购计划 -调用物资接口
    async generatePurchasePlan(record) {
      // 获取材料信息
      let purchasePlanRltMaterials = [];
      let response = await apiMaterial.getPlanMaterials(record.id);
      if (requestIsSuccess(response)) {
        response.data.forEach(a => {
          purchasePlanRltMaterials.push({
            id: a.material.id,
            number: a.count,
            price: a.material.price,
          });
        });
      }
      let data = {
        name: `${record.planName}-采购计划`,
        planTime: moment(),
        purchasePlanRltMaterials,
      };
      let purchaseResponse = await apiPurchasePlan.create(data);
      if (requestIsSuccess(purchaseResponse)) {
        // 提交成功，更新当前用料计划的状态
        this.$message.success("采购计划生成成功");
        this.updatePlanSubmitState(record.id);
      }
    },
    // 更新用料计划提交状态
    async updatePlanSubmitState(id) {
      let response = await apiMaterial.planSubmitStateUpdate(id);
      if (requestIsSuccess(response)) {
        this.refresh();
      }
    },
    // 菜单定义
    operationMenuDefine(text, record, index) {
      // 台账菜单项
      let defaultMenu = <div class='row-menu'>
        {vIf(
          <span> <a onClick={() => this.workflowView(record)}>查看</a></span>,
          record.status !== ApprovalStatus.ToSubmit &&
          vP(this.permissions, permissionsTechnology.MaterialPlans.Detail),
        )}
        {vIf(
          <span> <a onClick={() => this.export(record)}>导出</a></span>, record.status !== ApprovalStatus.ToSubmit &&
        vP(this.permissions, permissionsTechnology.MaterialPlans.Export),
        )}
        {vIf(
          <span> <a onClick={() => this.submit(record)}>提交</a></span>, (record.status == ApprovalStatus.ToSubmit || record.status == ApprovalStatus.UnPass) &&
        vP(this.permissions, permissionsTechnology.MaterialPlans.Submit),
        )}
        {vIf(
          <span> <a onClick={() => this.edit(record)}>编辑</a></span>, (record.status == ApprovalStatus.ToSubmit || record.status == ApprovalStatus.UnPass) &&
        vP(this.permissions, permissionsTechnology.MaterialPlans.Update),
        )}
        {vIf(
          <span> <a onClick={() => this.delete(record)}>删除</a></span>, (record.status == ApprovalStatus.ToSubmit || record.status == ApprovalStatus.UnPass) &&
        vP(this.permissions, permissionsTechnology.MaterialPlans.Delete),
        )}

      </div>;
      // 审批界面菜单项
      let approvalMenu = <div class='row-menu'>
        {vIf(
          <span> <a onClick={() => this.workflowView(record)}>查看</a></span>,
          vP(this.permissions, permissionsTechnology.MaterialPlans.Detail),
        )}
        {vIf(
          <span> <a onClick={() => this.approvalOpt(record)}>审批</a></span>,
          this.queryParams.waiting &&
          vP(this.permissions, permissionsTechnology.MaterialPlans.Approval),
        )}
      </div>;
      // 物资管理模块使用的菜单项
      let materialMenu = <div class='row-menu'  >
        {/* <span> <a onClick={() => this.view(record)}>查看</a></span> */}
        {vIf(
          <a-popconfirm
            title="确定要生成采购计划吗?"
            ok-text="是"
            cancel-text="否"
            disabled={record.submit}
            onConfirm={() => this.generatePurchasePlan(record)}
            onCancel="cancel"
          >
            <a   disabled={record.submit} href="#">生成采购计划</a>
          </a-popconfirm>,
          vP(this.permissions, permissionsMaterial.MaterialPlans.GenerateMaterialPlan),
        )}

      </div>;
      if (this.approval) {
        return approvalMenu;
      } else if (this.material) {
        return materialMenu;
      } else return defaultMenu;
    },
    //更新所选数据
    updateSelected(selectedRows) {
      if (this.multiple) {
        // 过滤出其他页面已经选中的
        let _selected = [];
        for (let item of this.iSelected) {
          let target = this.quantities.find(subItem => subItem.id === item.id);
          if (!target) {
            _selected.push(item);
          }
        }

        // 把当前页面选中的加入
        for (let id of this.selectedPlanIds) {
          let _quantities = this.quantities.find(item => item.id === id);
          if (!!_quantities) {
            _selected.push(JSON.parse(JSON.stringify(_quantities)));
          }
        }

        this.iSelected = _selected;
      } else {
        this.iSelected = selectedRows;
      }

      this.$emit('change', this.iSelected);
    },
    handleChange(value){
      this.queryParams.submit=value;
      this.refresh();
    },
  },
  render() {
    return (
      <div class="sm-technology-material-plan">
        {/* 工具栏*/}
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keywords = null;
            this.queryParams.startTime = null;
            this.queryParams.endTime = null;
            this.queryParams.submit = undefined;
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              size={this.size}
              allowClear
              placeholder="请输入名称"
              value={this.queryParams.keywords}
              onInput={event => {
                this.queryParams.keywords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>

          <a-form-item label="计划时间">
            <div style="align-items: center;display: flex;">
              <a-date-picker
                size={this.size}
                style="width:100%"
                allowClear
                placeholder="起始时间"
                value={this.queryParams.startTime}
                onChange={value => {
                  this.queryParams.startTime = value;
                  this.refresh();
                }}
              />
              <p style="margin: 0 3px;">—</p>
              <a-date-picker
                allowClear
                style="width:100%"
                size={this.size}
                placeholder="结束时间"
                value={this.queryParams.endTime}
                onChange={value => {
                  this.queryParams.endTime = value;
                  this.refresh();
                }}
              />
            </div>
          </a-form-item>
          {this.material?
            <a-form-item label="生成状态">
              <a-select  placeholder="请选择" value={this.queryParams.submit}  allowClear style="width: 100%" onChange={this.handleChange}>
                <a-select-option value="true">
              已生成
                </a-select-option>
                <a-select-option value="false">
            未生成
                </a-select-option>
              </a-select>
            </a-form-item>:''  
          }
          <template slot="buttons">
            <div style={'display:flex'}>
              {/* 审批tab切换按钮 */}
              { this.default  ? 
                vIf(
                  <a-button type="primary" size={this.size} onClick={() => this.materialPlan()}>
                新增用料计划
                  </a-button>,
                  vP(this.permissions,permissionsTechnology.MaterialPlans.Create),
                ):''}
            </div>
          </template>
        </sc-table-operator>

        {/* 审批tab切换按钮 */}
        {this.approval ? <div>
          <a-tabs default-active-key="1" onChange={this.tabChange} size='small'>
            <a-tab-pane key="1" tab="待我审批">
            </a-tab-pane>
            <a-tab-pane key="2" tab="我审批的" force-render>
            </a-tab-pane>
          </a-tabs>
        </div> : null}
        {/* 数据表格 */}
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.quantities}
          pagination={false}
          size={this.size}
          rowSelection={this.isMaterialPlanSelect ? {
            type: this.multiple ? 'checkbox' : 'radio',
            columnWidth: 30,
            selectedRowKeys: this.selectedPlanIds,
            onChange: (selectedRowKeys, selectedRows) => {
              this.selectedPlanIds = selectedRowKeys;
              this.updateSelected(selectedRows);
            },
          } : undefined}
          scroll={this.isMaterialPlanSelect ? { y: 300 } : undefined}
          loading={this.loading}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              planName: (text, record, index) => {
                return <a onClick={() => this.view(record)}>{text}</a>;
              },
              productCategoryName: (text, record, index) => {
                return <a-tooltip title={text}>{text}</a-tooltip>;
              },
              planTime: (text, record, index) => {
                return moment(record.planTime).format("YYYY-MM-DD");
              },
              creationTime: (text, record, index) => {
                return moment(record.creationTime).format("YYYY-MM-DD HH:mm:ss");
              },
              submit: (text, record, index) => {
                if (record.submit) {
                  return <a-tag color="#bfbfbf">已生成</a-tag>;
                } else {
                  return <a-tag color="#61D023">未生成</a-tag>;
                }
              },
              status: (text, record, index) => {
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
              operation: this.operationMenuDefine,
            },
          }}
        >
        </a-table>
        {/* 表格分页器 */}
        <a-pagination
          style="margin-top:10px; text-align: right;"
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          size="small"
          showTotal={paginationConfig.showTotal}
        />
        {/* 用料计划新增/编辑框 */}
        <SmTechnologyMaterialPlanModal
          ref="smTechnologyMaterialPlanModal"
          onSuccess={() => {
            this.refresh(false);
          }}
          axios={this.axios}
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
