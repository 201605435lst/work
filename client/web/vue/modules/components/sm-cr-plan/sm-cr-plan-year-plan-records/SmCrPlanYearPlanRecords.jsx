import ApiYearMonthPlan from '../../sm-api/sm-cr-plan/YearMonthPlan';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { requestIsSuccess, getRepairTypeTitle, vIf, vP } from '../../_utils/utils';
import SmImport from '../../sm-import/sm-import-basic';
import OrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select/index';
import { RepairType, YearMonthPlanState, YearMonthPlanType } from '../../_utils/enum';
import FileSaver from 'file-saver';
import permissionsSmCrPlan from '../../_permissions/sm-cr-plan';

let apiYearMonthPlan = new ApiYearMonthPlan();

export default {
  name: 'SmCrPlanYearPlanRecords',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    permissions: { type: Array, default: () => [] },
    repairTagKey: { type: String, default: null }, //维修项标签
  },
  data() {
    return {
      isLoading: false,
      yearItems: [],
      totalCount: 0,
      fileList: [],
      examColor: null, //审核标记颜色
      examStr: null, //审核标记文字
      queryParams: {
        keyword: null,
        skipCount: 0,
        maxResultCount: paginationConfig.defaultPageSize,
        year: new Date().getFullYear(),
        orgId: null,
        repairlType: undefined,
        planType: 1, //年表类型
      },
      form: this.$form.createForm(this),
      isInit: false,
      currentOrganizationId: null,
      orgName: null,
    };
  },
  computed: {
    columns() {
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
          width: 120,
          dataIndex: 'repairTypeStr',
          scopedSlots: { customRender: 'repairTypeStr' },
          ellipsis: true,
        },
        {
          title: '设备名称',
          dataIndex: 'deviceName',
          width: 200,
          scopedSlots: { customRender: 'deviceName' },
          ellipsis: true,
        },
        {
          title: '执行单位',
          dataIndex: 'executableUnitStr',
          width: 150,
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
          title: '单位',
          width: 100,
          dataIndex: 'unit',
          scopedSlots: { customRender: 'unit' },
          ellipsis: true,
        },
        {
          title: '总设备数量',
          width: 120,
          dataIndex: 'total',
          ellipsis: true,
        },
        {
          title: '年计划总数量',
          width: 120,
          dataIndex: 'planCount',
          scopedSlots: { customRender: 'planCount' },
          ellipsis: true,
        },
        {
          title: '每年次数',
          width: 100,
          dataIndex: 'times',
          scopedSlots: { customRender: 'times' },
          ellipsis: true,
        },
        {
          title: '一月',
          width: 100,
          dataIndex: 'col_1',
          scopedSlots: { customRender: 'col_1' },
        },
        {
          title: '二月',
          width: 100,
          dataIndex: 'col_2',
          scopedSlots: { customRender: 'col_2' },
        },
        {
          title: '三月',
          width: 100,
          dataIndex: 'col_3',
          scopedSlots: { customRender: 'col_3' },
        },
        {
          title: '四月',
          width: 100,
          dataIndex: 'col_4',
          scopedSlots: { customRender: 'col_4' },
        },
        {
          title: '五月',
          width: 100,
          dataIndex: 'col_5',
          scopedSlots: { customRender: 'col_5' },
        },
        {
          title: '六月',
          width: 100,
          dataIndex: 'col_6',
          scopedSlots: { customRender: 'col_6' },
        },
        {
          title: '七月',
          width: 100,
          dataIndex: 'col_7',
          scopedSlots: { customRender: 'col_7' },
        },
        {
          title: '八月',
          width: 100,
          dataIndex: 'col_8',
          scopedSlots: { customRender: 'col_8' },
        },
        {
          title: '九月',
          width: 100,
          dataIndex: 'col_9',
          scopedSlots: { customRender: 'col_9' },
        },
        {
          title: '十月',
          width: 100,
          dataIndex: 'col_10',
          scopedSlots: { customRender: 'col_10' },
        },
        {
          title: '十一月',
          width: 100,
          dataIndex: 'col_11',
          scopedSlots: { customRender: 'col_11' },
        },
        {
          title: '十二月',
          width: 100,
          dataIndex: 'col_12',
          scopedSlots: { customRender: 'col_12' },
        },
      ];
    },
    innerColumns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 90,
          scopedSlots: { customRender: 'index' },
          ellipsis: true,
        },
        {
          title: '组织机构',
          width: 100,
          dataIndex: 'responsibleUnitStr',
          ellipsis: true,
        },
        {
          title: '设备处所',
          width: 100,
          dataIndex: 'equipmentLocation',
          ellipsis: true,
        },
        {
          title: '天窗类型',
          width: 100,
          dataIndex: 'skyligetType',
          ellipsis: true,
        },
        {
          title: '年计划总数量',
          width: 120,
          dataIndex: 'planCount',
          scopedSlots: { customRender: 'planCount' },
          ellipsis: true,
        },
        {
          title: '一月',
          width: 100,
          dataIndex: 'col_1',
          scopedSlots: { customRender: 'col_1' },
        },
        {
          title: '二月',
          width: 100,
          dataIndex: 'col_2',
          scopedSlots: { customRender: 'col_2' },
        },
        {
          title: '三月',
          width: 100,
          dataIndex: 'col_3',
          scopedSlots: { customRender: 'col_3' },
        },
        {
          title: '四月',
          width: 100,
          dataIndex: 'col_4',
          scopedSlots: { customRender: 'col_4' },
        },
        {
          title: '五月',
          width: 100,
          dataIndex: 'col_5',
          scopedSlots: { customRender: 'col_5' },
        },
        {
          title: '六月',
          width: 100,
          dataIndex: 'col_6',
          scopedSlots: { customRender: 'col_6' },
        },
        {
          title: '七月',
          width: 100,
          dataIndex: 'col_7',
          scopedSlots: { customRender: 'col_7' },
        },
        {
          title: '八月',
          width: 100,
          dataIndex: 'col_8',
          scopedSlots: { customRender: 'col_8' },
        },
        {
          title: '九月',
          width: 100,
          dataIndex: 'col_9',
          scopedSlots: { customRender: 'col_9' },
        },
        {
          title: '十月',
          width: 100,
          dataIndex: 'col_10',
          scopedSlots: { customRender: 'col_10' },
        },
        {
          title: '十一月',
          width: 100,
          dataIndex: 'col_11',
          scopedSlots: { customRender: 'col_11' },
        },
        {
          title: '十二月',
          width: 100,
          dataIndex: 'col_12',
          scopedSlots: { customRender: 'col_12' },
        },
      ];
    },
  },
  watch: {},
  async created() {
    this.initAxios();
    this.refresh();
    this.isInit = true;
  },
  methods: {
    async export(record) {
      //导出按钮
      if (!this.queryParams.orgId) {
        this.$message.info('请选择车间');
        return;
      }
      this.isLoading = true;
      let response = await apiYearMonthPlan.exporYearStatisticData(
        this.queryParams,
        this.repairTagKey,
      );
      this.isLoading = false;
      if (requestIsSuccess(response)) {
        FileSaver.saveAs(
          new Blob([response.data], { type: 'application/vnd.ms-excel' }),
          `${this.queryParams.year}年${this.orgName}年表.xlsx`,
        );
      }
    },
    initAxios() {
      apiYearMonthPlan = new ApiYearMonthPlan(this.axios);
    },
    async refresh(resetPage = true) {
      this.isLoading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
        this.queryParams.skipCount = (this.pageIndex - 1) * this.queryParams.maxResultCount;
      }
      let response = await apiYearMonthPlan.getYearStatisticData(this.queryParams, this.repairTagKey);
      this.isLoading = false;
      if (requestIsSuccess(response)) {
        this.yearItems = response.data.items;
        this.totalCount = response.data.totalCount;
        //显示审核状态
        if (this.totalCount > 0) {
          let item = this.yearItems[0];
          this.examStr = item.stateStr;
          switch (item.state) {
          case YearMonthPlanState.UnCommit: {
            //未提交
            this.examColor = 'volcano';
            break;
          }
          case YearMonthPlanState.UnCheck: {
            //待审核
            this.examColor = 'LightSkyBlue';
            break;
          }
          case YearMonthPlanState.Checking: {
            //审核中
            this.examColor = 'DeepSkyBlue';
            break;
          }
          case YearMonthPlanState.Passed: {
            //审核完成
            this.examColor = 'green';
            break;
          }
          case YearMonthPlanState.UnPassed: {
            //审核驳回
            this.examColor = 'red';
            break;
          }
          }
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
          }}
          onReset={() => {
            this.queryParams.year = new Date().getFullYear();
            this.queryParams.keyword = null;
            this.queryParams.orgId = this.currentOrganizationId;
            this.queryParams.repairlType = undefined;
            this.refresh();
          }}
        >
          <a-form-item label="组织机构">
            <OrganizationTreeSelect
              axios={this.axios}
              value={this.queryParams.orgId}
              autoInitial={true}
              onInput={(value, obj) => {
                this.orgName = obj;
                if (this.isInit) {
                  this.currentOrganizationId = value;
                }
                this.isInit = false;
                this.queryParams.orgId = value;
                this.refresh();
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
              }}
            />
          </a-form-item>
          <template slot="buttons">
            <div style={'display:flex'}>
              <a-button onClick={this.export} type="primary">
                导出
              </a-button>
            </div>
          </template>
        </sc-table-operator>

        {/* 展示区 */}
        <div>
          <a-table
            scroll={{ x: 1300 }}
            columns={this.columns}
            rowKey={record => record.id}
            dataSource={this.yearItems}
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
                expandedRowRender: (record, index, indent, expanded) => {
                  return (
                    <a-table
                      rowKey={row => row.id}
                      slot-scope="text"
                      columns={this.innerColumns}
                      dataSource={record.childItems}
                      pagination={false}
                      bordered={this.bordered}
                      // loading={this.loading}
                      {...{
                        scopedSlots: {
                          index: (text, record, index) => {
                            return index + 1;
                          },
                        },
                      }}
                    ></a-table>
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
