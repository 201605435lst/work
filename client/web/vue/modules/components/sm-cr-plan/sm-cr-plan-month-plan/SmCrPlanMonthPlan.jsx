import ApiYearMonthPlan from '../../sm-api/sm-cr-plan/YearMonthPlan';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { requestIsSuccess, getRepairTypeTitle, vP, vIf } from '../../_utils/utils';
import SmImport from '../../sm-import/sm-import-basic';
import OrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select/index';
import SmCrPlanMonthPlanModal from './SmCrPlanMonthPlanModal';
import {
  RepairType,
  YearMonthPlanState,
  YearMonthPlanType,
  RepairTagKeys,
} from '../../_utils/enum';
import FileSaver from 'file-saver';
import permissionsSmCrPlan from '../../_permissions/sm-cr-plan';

let apiYearMonthPlan = new ApiYearMonthPlan();

export default {
  name: 'SmCrPlanMonthPlan',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    permissions: { type: Array, default: () => [] },
    repairTagKey: { type: String, default: null }, //维修项标签
  },
  data() {
    return {
      isLoading: false,
      monthItems: [],
      totalCount: 0,
      fileList: [],
      isCanCreate: false, //是否可以生成
      isCanImport: false, //是否可以导入
      isCanExport: true, //是否可以导出
      isCanChange: true, //是否可以变更
      queryParams: {
        keyword: null,
        skipCount: 0,
        maxResultCount: paginationConfig.defaultPageSize,
        year: new Date().getFullYear(),
        orgId: null,
        repairlType: undefined,
        planType: 2, //年表类型
      },
      queryParamsSubmit: {
        keyword: null,
        skipCount: 0,
        maxResultCount: paginationConfig.defaultPageSize,
        year: new Date().getFullYear(),
        orgId: null,
        repairlType: undefined,
        planType: 2, //年表类型
      },
      form: this.$form.createForm(this),
      isInit: false,
      currentOrganizationId: null,
      orgName: null,
      isShowAllTab: true, //页面表格是否显示所有项Tab
      monthItemsSubmit: [],
      totalCountSubmit: 0,
      pageIndexSubmit: 1,
      defaultKey: 'ALL',
      isCanClear: true,
    };
  },
  computed: {
    columns() {
      let arr = [];
      for (let i = 1; i <= 31; i++) {
        arr.push({
          title: i,
          width: 100,
          dataIndex: `col_${i}`,
        });
      }

      return [
        {
          title: '序号',
          dataIndex: 'number',
          width: 120,
          scopedSlots: { customRender: 'number' },
          ellipsis: true,
        },
        {
          title: '维修类别',
          width: 100,
          dataIndex: 'repairTypeStr',
          scopedSlots: { customRender: 'repairTypeStr' },
          ellipsis: true,
        },
        {
          title: '设备名称',
          dataIndex: 'deviceName',
          width: 120,
          scopedSlots: { customRender: 'deviceName' },
          ellipsis: true,
        },
        {
          title: '执行单位',
          dataIndex: 'executableUnitStr',
          width: 100,
          ellipsis: true,
        },
        this.repairTagKey == RepairTagKeys.RailwayWired
          ? {
            title: '编制执行单位',
            width: 120,
            dataIndex: 'compiledOrganization',
            ellipsis: true,
          }
          : {},
        {
          title: '设备处所',
          width: 150,
          dataIndex: 'equipmentLocation',
          scopedSlots: { customRender: 'equipmentLocation' },
          ellipsis: true,
        },
        {
          title: '工作内容',
          width: 250,
          dataIndex: 'repairContent',
          scopedSlots: { customRender: 'repairContent' },
          ellipsis: true,
        },
        {
          title: '天窗类型',
          width: 150,
          dataIndex: 'skyligetType',
          scopedSlots: { customRender: 'skyligetType' },
          ellipsis: true,
        },
        {
          title: '单位',
          width: 100,
          dataIndex: 'unit',
          scopedSlots: { customRender: 'unit' },
          ellipsis: true,
        },
        {
          title: '总数量',
          width: 100,
          dataIndex: 'total',
          scopedSlots: { customRender: 'total' },
          ellipsis: true,
        },
        {
          title: '每月次数',
          width: 100,
          dataIndex: 'times',
          scopedSlots: { customRender: 'times' },
          ellipsis: true,
        },
        ...arr,
        {
          title: '操作',
          dataIndex: 'operations',
          width: 80,
          scopedSlots: { customRender: 'operations' },
          fixed: 'right',
        },
      ];
    },
  },
  watch: {},
  async created() {
    this.initAxios();
    this.refresh();
    this.refreshSubmit();
    this.isInit = true;
  },
  methods: {
    add() {
      let _this = this;
      if (!this.queryParams.orgId) {
        this.$message.info('请选择生成车间');
        return;
      }
      this.$confirm({
        title: '重要提示',
        content: h => <div style="color:red;">请先核对设备数据，确认设备台账无误再制定计划！</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            _this.isLoading = true;
            let response = await apiYearMonthPlan.create(_this.queryParams, _this.repairTagKey);
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
            _this.refresh();
            _this.refreshSubmit();
            _this.isLoading = false;
          });
        },
      });
    },
    edit(record) {
      //编辑天窗类型
      this.$refs.SmCrPlanMonthPlanModal.edit(record);
    },
    async fileSelected(file) {
      let hasExist = this.fileList.some(item => item.name === file.name);
      if (!hasExist) {
        this.fileList = [...this.fileList, file];
      }

      if (!this.queryParams.orgId) {
        this.$message.info('请选择生成车间');
        return;
      }
      // 构造导入参数（根据自己后台方法的实际参数进行构造）
      let importParamter = {
        'file.file': this.fileList[0],
        importKey: 'yearPlan4Month',
        PlanType: this.queryParams.planType,
        OrgId: this.queryParams.orgId,
        Year: this.queryParams.year,
        RepairTagKey: this.repairTagKey,
      };
      // 执行文件上传
      await this.$refs.smImport.exect(importParamter);
      this.fileList = [];
    },
    async export(record) {
      //导出按钮
      if (!this.queryParams.orgId) {
        this.$message.info('请选择车间');
        return;
      }
      this.isLoading = true;
      let response = await apiYearMonthPlan.downLoad(this.queryParams, this.repairTagKey);
      this.isLoading = false;
      if (requestIsSuccess(response)) {
        FileSaver.saveAs(
          new Blob([response.data], { type: 'application/vnd.ms-excel' }),
          `${this.queryParams.year}年${this.orgName}月表.xlsx`,
        );
      }
    },
    async subimtExam() {
      //提交审核
      if (!this.queryParams.orgId) {
        this.$message.info('请选择车间');
        return;
      }
      let _this = this;
      this.$confirm({
        title: '重要提示',
        content: h => <div style="color:red;">确认要提交审核！</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            _this.isLoading = true;
            let response = await apiYearMonthPlan.submitForExam(
              _this.queryParams,
              _this.repairTagKey,
            );
            if (requestIsSuccess(response)) {
              _this.$message.info('操作成功');
              _this.refresh();
              _this.refreshSubmit();
            }
            _this.isLoading = false;
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },
    initAxios() {
      apiYearMonthPlan = new ApiYearMonthPlan(this.axios);
    },
    async refresh(resetPage = true) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
        this.queryParams.skipCount = (this.pageIndex - 1) * this.queryParams.maxResultCount;
      }

      let response = await apiYearMonthPlan.getListMonth(this.queryParams, this.repairTagKey);
      this.loading = false;
      if (requestIsSuccess(response)) {
        this.monthItems = response.data.items;
        this.totalCount = response.data.totalCount;
        //显示审核状态
        if (this.totalCount > 0) {
          let item = this.monthItems[0];
          switch (item.state) {
          case YearMonthPlanState.UnCommit: {
            //未提交
            this.examColor = 'volcano';
            this.isCanClear = true;
            break;
          }
          case YearMonthPlanState.UnCheck: {
            //待审核
            this.examColor = 'LightSkyBlue';
            this.isCanClear = false;
            break;
          }
          case YearMonthPlanState.Checking: {
            //审核中
            this.examColor = 'DeepSkyBlue';
            this.isCanClear = false;
            break;
          }
          case YearMonthPlanState.Passed: {
            //审核完成
            this.examColor = 'green';
            this.isCanClear = false;
            break;
          }
          case YearMonthPlanState.UnPassed: {
            //审核驳回
            this.examColor = 'red';
            this.isCanClear = true;
            break;
          }
          }
        }
        let nowYear = new Date().getFullYear();
        this.isCanCreate = false;
        this.isCanChange = true;
        this.isCanImport = false;
        this.isCanExport = true;
        if (this.totalCount > 0) {
          this.isCanExport = false;
          let firstModel = this.monthItems[0];
          if (
            !(
              firstModel.state == YearMonthPlanState.UnCommit ||
              firstModel.state == YearMonthPlanState.UnPassed
            )
          ) {
            //生成按钮
            this.isCanCreate = true;
            this.isCanImport = true;
          }
          if (firstModel.state == YearMonthPlanState.Passed && nowYear == this.queryParams.year) {
            //只能变更今年的数据
            //变更按钮
            this.isCanChange = false;
          }
        } else {
          this.isCanImport = true;
        }

        if (response.data.isCanCreate == false) {
          this.isCanCreate = true;
        }
      }
    },
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      this.queryParams.skipCount = (this.pageIndex - 1) * this.queryParams.maxResultCount;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    //页面表格tab切换
    tableTabChange(tabKey) {
      this.defaultKey = tabKey;
      this.isShowAllTab = tabKey == 'ALL';
    },

    //刷新已提交项数据
    async refreshSubmit(resetPage = true) {
      this.isLoading = true;
      if (resetPage) {
        this.pageIndexSubmit = 1;
        this.queryParamsSubmit.maxResultCount = paginationConfig.defaultPageSize;
        this.queryParamsSubmit.skipCount =
          (this.pageIndexSubmit - 1) * this.queryParamsSubmit.maxResultCount;
      }

      this.queryParamsSubmit.year = this.queryParams.year;
      this.queryParamsSubmit.keyword = this.queryParams.keyword;
      this.queryParamsSubmit.orgId = this.queryParams.orgId;
      this.queryParamsSubmit.repairlType = this.queryParams.repairlType;
      let response = await apiYearMonthPlan.getSubmitedListMonth(
        this.queryParamsSubmit,
        this.repairTagKey,
      );
      this.isLoading = false;
      if (requestIsSuccess(response)) {
        this.monthItemsSubmit = response.data.items;
        this.totalCountSubmit = response.data.totalCount;
      }
    },
    async onPageChangeSubmit(page, pageSize) {
      this.pageIndexSubmit = page;
      this.queryParamsSubmit.maxResultCount = pageSize;
      this.queryParamsSubmit.skipCount =
        (this.pageIndexSubmit - 1) * this.queryParamsSubmit.maxResultCount;
      if (page !== 0) {
        await this.refreshSubmit(false);
      }
    },

    //清空已填报项
    async clearFilled() {
      let _this = this;
      this.$confirm({
        title: '重要提示',
        content: h => <div style="color:red;">确认清空已填报项！</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            _this.isLoading = true;

            let response = await apiYearMonthPlan.clearFilled(
              _this.queryParams,
              _this.repairTagKey,
            );
            if (requestIsSuccess(response)) {
              _this.$message.info('操作成功');
              _this.refresh();
              _this.refreshSubmit();
            }
            _this.isLoading = false;
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },
  },
  render() {
    //维修类别
    let repairTypeOption = [];
    for (let item in RepairType) {
      repairTypeOption.push(
        <a-select-option key={RepairType[item]}>
          {getRepairTypeTitle(RepairType[item])}
        </a-select-option>,
      );
    }

    //年度
    let yearOption = [];
    let maxYear = new Date().getFullYear() - 2;
    for (let i = maxYear; i < maxYear + 10; i++) {
      yearOption.push(<a-select-option key={i}>{i}年</a-select-option>);
    }

    return (
      <div>
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
            this.refreshSubmit();
          }}
          onReset={() => {
            this.queryParams.year = new Date().getFullYear();
            this.queryParams.keyword = null;
            this.queryParams.orgId = this.currentOrganizationId;
            this.queryParams.repairlType = undefined;
            this.queryParamsSubmit.year = new Date().getFullYear();
            this.queryParamsSubmit.keyword = null;
            this.queryParamsSubmit.orgId = this.currentOrganizationId;
            this.queryParamsSubmit.repairlType = undefined;
            this.refresh();
            this.refreshSubmit();
          }}
        >
          <a-form-item label="车间">
            <OrganizationTreeSelect
              axios={this.axios}
              autoInitial={true}
              value={this.queryParams.orgId}
              onInput={(value, obj) => {
                if (this.isInit) {
                  this.currentOrganizationId = value;
                }
                this.isInit = false;
                this.queryParams.orgId = value;
                this.orgName = obj;
                this.refresh();
                this.refreshSubmit();
              }}
            />
          </a-form-item>
          <a-form-item label="年度">
            <a-select
              placeholder="请选择年度！"
              axios={this.axios}
              value={this.queryParams.year}
              onChange={value => {
                this.queryParams.year = value;
                this.refresh();
                this.refreshSubmit();
              }}
            >
              {yearOption}
            </a-select>
          </a-form-item>
          <a-form-item label="维修类别">
            <a-select
              placeholder="请选择维修类别！"
              allowClear
              axios={this.axios}
              value={this.queryParams.repairlType}
              onChange={value => {
                this.queryParams.repairlType = value;
                this.refresh();
                this.refreshSubmit();
              }}
            >
              {repairTypeOption}
            </a-select>
          </a-form-item>
          <a-form-item label="关键字">
            <a-input
              placeholder="设备名称/设备处所/工作内容"
              value={this.queryParams.keyword}
              onInput={event => {
                this.queryParams.keyword = event.target.value;
                this.refresh();
                this.refreshSubmit();
              }}
            />
          </a-form-item>
          <template slot="buttons">
            <div style="display:flex;">
              {vIf(
                <a-button type="primary" icon="plus" onClick={this.add} disabled={this.isCanCreate}>
                  生成
                </a-button>,
                this.isShowAllTab && vP(this.permissions, permissionsSmCrPlan.MonthPlan.Create),
              )}
              {vIf(
                <a-tooltip title="导出所有项">
                  <a-button type="primary" onClick={this.export} disabled={this.isCanExport}>
                    <a-icon type="download" /> 导出
                  </a-button>
                </a-tooltip>,
                this.isShowAllTab && vP(this.permissions, permissionsSmCrPlan.MonthPlan.Export),
              )}
              {vIf(
                <a-tooltip title="导入填报项">
                  <SmImport
                    isImport={this.isCanImport}
                    ref="smImport"
                    url="api/app/crPlanYearMonthPlan/upload"
                    axios={this.axios}
                    importKey="yearPlan4Month"
                    repairTagKey={this.repairTagKey}
                    downloadErrorFile={true}
                    onSelected={file => this.fileSelected(file)}
                    onIsSuccess={() => {
                      this.refresh();
                      this.refreshSubmit();
                      this.defaultKey = 'SUBMIT';
                      this.isShowAllTab = false;
                    }}
                  />
                </a-tooltip>,
                vP(this.permissions, permissionsSmCrPlan.MonthPlan.Import),
              )}
              {!this.isShowAllTab
                ? vIf(
                  <a-button type="primary" onClick={this.subimtExam} disabled={this.isCanImport}>
                    <a-icon type="import" />
                      提交审批
                  </a-button>,
                  vP(this.permissions, permissionsSmCrPlan.MonthPlan.SubmitApproval),
                )
                : undefined}
              {vIf(
                <a-button
                  type="primary"
                  disabled={this.isCanChange}
                  onClick={() => {
                    this.$emit(
                      'change',
                      YearMonthPlanType.Month,
                      this.queryParams.orgId,
                      this.orgName,
                    );
                  }}
                >
                  <a-icon type="diff" />
                  变更
                </a-button>,
                vP(this.permissions, permissionsSmCrPlan.MonthPlan.Alter),
              )}
              {!this.isShowAllTab
                ? this.totalCount > 0 &&
                  (this.monthItems[0].state === YearMonthPlanState.UnCommit ||
                    this.monthItems[0].state === YearMonthPlanState.UnPassed)
                  ? vIf(
                    <a-button
                      type="primary"
                      disabled={!this.isCanClear}
                      onClick={() => {
                        this.clearFilled();
                      }}
                    >
                        清空
                    </a-button>,
                    vP(this.permissions, permissionsSmCrPlan.MonthPlan.Cleared),
                  )
                  : null
                : undefined}
              {!this.isShowAllTab ? (
                this.totalCount > 0 ? (
                  <a-tag
                    style="margin-left:40px;height:32px;line-height:30px"
                    color={this.examColor}
                  >
                    {this.monthItems[0].stateStr}
                  </a-tag>
                ) : null
              ) : (
                undefined
              )}
            </div>
          </template>
        </sc-table-operator>

        {/* 展示区 */}
        <div>
          <a-tabs active-key={this.defaultKey} onChange={key => this.tableTabChange(key)}>
            <a-tab-pane key="ALL" tab="所有项">
              <a-table
                scroll={{ x: 1300, y: 500 }}
                columns={this.columns}
                rowKey={record => record.id}
                dataSource={this.monthItems}
                bordered={this.bordered}
                pagination={false}
                // loading={this.loading}
                {...{
                  scopedSlots: {
                    index: (text, record, index) => {
                      return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                    },
                    repairTypeStr: (text, record) => {
                      return getRepairTypeTitle(record.repairType);
                    },
                    operations: (text, record) => {
                      return [
                        <span>
                          {vIf(
                            <a
                              onClick={() => {
                                this.edit(record);
                              }}
                            >
                              编辑
                            </a>,
                            vP(this.permissions, permissionsSmCrPlan.MonthPlan.Update),
                          )}
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
            <a-tab-pane key="SUBMIT" tab="已填报项">
              <a-table
                scroll={{ x: 1300, y: 500 }}
                columns={this.columns}
                rowKey={record => record.id}
                dataSource={this.monthItemsSubmit}
                bordered={this.bordered}
                pagination={false}
                // loading={this.loading}
                {...{
                  scopedSlots: {
                    index: (text, record, index) => {
                      return (
                        index +
                        1 +
                        this.queryParamsSubmit.maxResultCount * (this.pageIndexSubmit - 1)
                      );
                    },
                    repairTypeStr: (text, record) => {
                      return getRepairTypeTitle(record.repairType);
                    },
                    operations: (text, record) => {
                      return [
                        <span>
                          {vIf(
                            <a
                              onClick={() => {
                                this.edit(record);
                              }}
                            >
                              编辑
                            </a>,
                            vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Update),
                          )}
                        </span>,
                      ];
                    },
                  },
                }}
              ></a-table>

              {/* 分页器 */}
              <a-pagination
                style="float:right; margin-top:10px"
                total={this.totalCountSubmit}
                pageSize={this.queryParamsSubmit.maxResultCount}
                current={this.pageIndexSubmit}
                onChange={this.onPageChangeSubmit}
                onShowSizeChange={this.onPageChangeSubmit}
                showSizeChanger
                showQuickJumper
                showTotal={paginationConfig.showTotal}
              />
            </a-tab-pane>
          </a-tabs>
        </div>

        <SmCrPlanMonthPlanModal
          ref="SmCrPlanMonthPlanModal"
          axios={this.axios}
          bordered={this.bordered}
          state={this.monthItems.length > 0 ? this.monthItems[0].state : null}
          repairTagKey={this.repairTagKey}
          onSuccess={() => {
            this.refresh(false);
            this.refreshSubmit();
          }}
        />
        {this.isLoading ? (
          <div style="position:fixed;left:0;right:0;top:0;bottom:0;z-index:9999;">
            <div style="position: relative;;top:45%;left:50%">
              <a-spin tip="Loading..." size="large"></a-spin>
            </div>
          </div>
        ) : null}
      </div>
    );
  },
};
