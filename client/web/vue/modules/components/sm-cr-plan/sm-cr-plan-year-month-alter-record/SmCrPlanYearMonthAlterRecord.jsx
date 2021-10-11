
import './style';
import {
  requestIsSuccess,
  getDateReportTypeTitle,
  getYearMonthPlanStateType,
} from '../../_utils/utils';
import ApiCrplan from '../../sm-api/sm-cr-plan/YearMonthPlan';
import OrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import { DateReportType, YearMonthPlanState, YearMonthPlanType } from '../../_utils/enum';
import {
  pagination as paginationConfig,
} from '../../_utils/config';
import moment from 'moment';
import WorkflowModal from '../../sm-bpm/sm-bpm-workflows/src/WorkflowModal';

let apiCrplan = new ApiCrplan();

export default {
  name: 'SmCrPlanYearMonthAlterRecord',
  props: {
    axios: { type: Function, default: null },
    repairTagKey: { type: String, default: null }, //维修项标签
    // planType: { type: Number, default: YearMonthPlanType.Month }, //年月表类型
    // orgId: { type: String, default: null },
    // repairTagKey: { type: String, default: null }, //维修项标签
  },
  data() {
    return {
      dataSource: [],
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        organizationId: null,
        planType: null,
        state: YearMonthPlanState.Passed,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      orgName: null,
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
          title: '变更时间',
          dataIndex: 'extuTime',
          width: 60,
          scopedSlots: { customRender: 'extuTime' },
        },
        {
          title: '变更原因',
          dataIndex: 'changeReason',
          width: 60,
          scopedSlots: { customRender: 'changeReason' },
          ellipsis: true,
        },
        {
          title: '变更类型',
          dataIndex: 'planType',
          width: 60,
          scopedSlots: { customRender: 'planType' },
          ellipsis: true,
        },
        {
          title: '审批状态',
          dataIndex: 'state',
          width: 60,
          scopedSlots: { customRender: 'state' },
        },
        {
          title: '操作',
          dataIndex: 'operation',
          width: 60,
          scopedSlots: { customRender: 'operation' },
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
      apiCrplan = new ApiCrplan(this.axios);
    },
    async refresh(resetPage = true, page) {
      let response = await apiCrplan.getYearMonthAlterRecords(this.queryParams);
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      if (requestIsSuccess(response) && response.data) {
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
    },
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    setTagColor(key) {
      let tagColor;
      switch (key) {
      case YearMonthPlanState.Passed: {
        //已完成
        tagColor = 'green';
        break;
      }
      case YearMonthPlanState.UnCommit: {
        //待提交
        tagColor = 'volcano';
        break;
      }
      case YearMonthPlanState.UnPassed: {
        //已拒绝
        tagColor = 'red';
        break;
      }
      case YearMonthPlanState.Checking: {
        //审批中
        tagColor = 'purple';
        break;
      }
      case YearMonthPlanState.UnCheck: {
        //待审批
        tagColor = 'purple';
        break;
      }
      }
      return tagColor;
    },

    // add() {
    //   this.$emit('add', this.orgId, this.planType, this.repairTagKey);
    // },

    view(record) {
      this.$emit('view', record, this.queryParams.organizationId, this.orgName,this.repairTagKey);
    },

    // edit(record) {
    //   this.$emit('edit', record.id);
    // },

    // back(record) {},

    workflowView(record) {
      if (record.arKey != null) {
        this.$refs.WorkflowModal.isInitial = false;
        this.$refs.WorkflowModal.edit(record.arKey);
      } else {
        this.$message.error('未提交审批');
      }
    },
  },
  render() {
    let dateReportTypeOptions = [];
    for (let item in DateReportType) {
      dateReportTypeOptions.push(
        <a-select-option key={DateReportType[item]}>
          {getDateReportTypeTitle(DateReportType[item])}
        </a-select-option>,
      );
    }

    let yearMonthPlanState = [];

    for (let item in YearMonthPlanState) {
      yearMonthPlanState.push(
        <a-select-option key={YearMonthPlanState[item]}>
          {getYearMonthPlanStateType(YearMonthPlanState[item])}
        </a-select-option>,
      );
    }

    return (
      <div class="sm-cr-plan-year-month-alter-record">
        <a-form>
          <sc-table-operator
            onSearch={() => {
              this.refresh();
            }}
            onReset={() => {
              this.queryParams.organizationId = null;
              this.queryParams.planType = null;
              this.queryParams.state = null;
              this.refresh();
            }}
          >
            <a-form-item label="变更车间">
              <OrganizationTreeSelect
                ref="OrganizationTreeSelect"
                axios={this.axios}
                value={this.queryParams.organizationId}
                autoInitial={true}
                onInput={(value, name) => {
                  this.queryParams.organizationId = value;
                  this.orgName = name;
                  this.refresh();
                }}
              />
            </a-form-item>

            <a-form-item label="审批状态">
              <a-select
                onChange={value => {
                  this.queryParams.state = value;
                  this.refresh();
                }}
                value={this.queryParams.state}
                // allowClear={true}
              >
                {yearMonthPlanState}
              </a-select>
            </a-form-item>
            
            <a-form-item label="计划类型">
              <a-select
                onChange={value => {
                  this.queryParams.planType = value;
                  this.refresh();
                }}
                value={this.queryParams.planType}
                allowClear={true}
              >
                {dateReportTypeOptions}
              </a-select>
            </a-form-item>

            {/* <template slot="buttons">
              <a-button type="primary" onClick={this.add}>
                添加变更
              </a-button>
            </template> */}
          </sc-table-operator>
        </a-form>

        <a-table
          columns={this.columns}
          dataSource={this.dataSource}
          rowKey={record => record.id}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return (this.pageIndex - 1) * this.queryParams.maxResultCount + (index + 1);
              },
              extuTime: (text, record, index) => {
                return moment(record.creationTime).format('yyyy-MM-DD');
              },
              state: (text, record) => {
                return (
                  <a-tag color={this.setTagColor(record.state)}>
                    {getYearMonthPlanStateType(record.state)}
                  </a-tag>
                );
              },
              planType: (text, record) => {
                return getDateReportTypeTitle(record.planType);
              },
              operation: (text, record) => {
                return (
                  <span>
                    <a
                      onClick={() => {
                        this.view(record);
                      }}
                    >
                      详情
                    </a>
                    <a-divider type="vertical" />
                    <a
                      onClick={() => {
                        this.workflowView(record);
                      }}
                    >
                      查看审批
                    </a>
                    {/* <a-dropdown trigger={['click']}>
                      <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>
                        更多 <a-icon type="down" />
                      </a>
                      <a-menu slot="overlay">
                        {record.state == YearMonthPlanState.UnCommit ? (
                          <a-menu-item>
                            <a
                              onClick={() => {
                                this.edit(record);
                              }}
                            >
                              编辑
                            </a>
                          </a-menu-item>
                        ) : (
                          undefined
                        )}

                        {record.state !== YearMonthPlanState.UnCommit ? (
                          <a-menu-item>
                            <a
                              onClick={() => {
                                this.workflowView(record);
                              }}
                            >
                              查看审批
                            </a>
                          </a-menu-item>
                        ) : (
                          undefined
                        )}
                        {record.state == YearMonthPlanState.Checking ||
                        record.state == YearMonthPlanState.Checking ? (
                            <a-menu-item>
                              <a
                                onClick={() => {
                                  this.back(record);
                                }}
                              >
                              撤销
                              </a>
                            </a-menu-item>
                          ) : (
                            undefined
                          )}
                      </a-menu>
                    </a-dropdown> */}
                  </span>
                );
              },
            },
          }}
        ></a-table>

        <WorkflowModal
          ref="WorkflowModal"
          axios={this.axios}
          isInitial={false}
          // onSuccess={this.onSuccess}
        />
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
    );
  },
};
    