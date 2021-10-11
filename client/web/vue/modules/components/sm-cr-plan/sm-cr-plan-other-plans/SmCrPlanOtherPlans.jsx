import moment from 'moment';
import ApiSkyLightPlan from '../../sm-api/sm-cr-plan/SkyLightPlan';
import ApiOrganization from '../../sm-api/sm-system/Organization';
import SmSystemOrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import { pagination as paginationConfig, tips as tipsConfig, tips } from '../../_utils/config';
import { requestIsSuccess, getPlanState, vIf, vP } from '../../_utils/utils';
import { PlanState, RepairTagKeys } from '../../_utils/enum';
import permissionsSmCrPlan from '../../_permissions/sm-cr-plan';

let apiSkyLightPlan = new ApiSkyLightPlan();
let apiOrganization = new ApiOrganization();

export default {
  name: 'SmCrPlanOtherPlans',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    permissions: { type: Array, default: () => [] },
    repairTagKey: { type: String, default: null }, //维修项标签
  },
  data() {
    return {
      loading: false,
      crateLoading: false,
      belongOrgs: [], //所属机构
      otherPlans: [], // 列表数据源
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        startTime: null, //开始时间
        endTime: null, //结束时间
        workUnitId: null, //当前用户所属组织机构
        workAreaId: null, //工区选择（本车间下）
        workContent: '', //按照【作业内容】模糊查找
        maxResultCount: paginationConfig.defaultPageSize,
        planState: null,
      },
      dateRange: [moment(moment()).startOf('month'), moment(moment()).endOf('month')],
      currentOrganizationId: null, //当前用户所属组织机构
      isInit: false,
      isChange: false,
      monthPickerShow: false,
      isChangeView: false,
      opinionVisible: false,
      opinion: "无",
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 90,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '计划时间',
          width: 120,
          dataIndex: 'workTime',
          scopedSlots: { customRender: 'workTime' },
        },
        {
          title: '作业内容',
          ellipsis: true,
          dataIndex: 'workContent',
        },
        {
          title: '作业单位',
          ellipsis: true,
          dataIndex: 'workUnitName',
        },
        {
          title: '作业工区',
          dataIndex: 'workAreaName',
        },
        {
          title: '状态',
          dataIndex: 'planState',
          scopedSlots: { customRender: 'planState' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 140,
          scopedSlots: { customRender: 'operations' },
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
    add() {
      this.$emit(
        'add',
        this.queryParams.workUnitId,
        moment(this.dateRange[0])
          .utc()
          .format(),
        this.queryParams.workAreaId,
      );
    },
    edit(record) {
      this.$emit(
        'edit',
        record.id,
        this.queryParams.workUnitId,
        moment(this.dateRange[0])
          .utc()
          .format(),
        this.isChange,
      );
    },
    view(record) {
      this.$emit(
        'view',
        record.id,
        this.queryParams.workUnitId,
        moment(this.dateRange[0])
          .utc()
          .format(),
      );
    },

    initAxios() {
      apiSkyLightPlan = new ApiSkyLightPlan(this.axios);
      apiOrganization = new ApiOrganization(this.axios);
    },

    // 发布计划
    publishOtherPlan() {
      let _this = this;
      let data = {
        workUnitId: this.queryParams.workUnitId,
        workAreaId: this.queryParams.workAreaId,
        startTime: this.queryParams.startTime,
        endTime: this.queryParams.endTime,
        workContent: this.queryParams.workContent,
      };
      this.$confirm({
        title: '发布提示',
        content: '请确认该月其他计划已全部制定完毕，再进行下发！',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiSkyLightPlan.publishOtherPlan(data, _this.repairTagKey);
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
            _this.refresh();
          });
        },
      });
    },

    // 删除计划
    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiSkyLightPlan.remove({
              id: record.id,
              repairTagKey: _this.repairTagKey,
            });
            _this.refresh();
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },

    // 获取数据列表
    async refresh(resetPage = true) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      this.queryParams.startTime = this.dateRange[0] ? this.dateRange[0].format() : null;
      this.queryParams.endTime = this.dateRange[1] ? this.dateRange[1].format() : null;
      let queryParams = {
        startTime: this.queryParams.startTime,
        endTime: this.queryParams.endTime,
        workUnitId: this.queryParams.workUnitId,
        workAreaId: this.queryParams.workAreaId,
        workContent: this.queryParams.workContent,
        maxResultCount: this.queryParams.maxResultCount,
        planState: this.queryParams.planState,
      };

      let response = await apiSkyLightPlan.getOtherPlanList(
        {
          skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
          ...queryParams,
        },
        this.repairTagKey,
      );
      if (requestIsSuccess(response)) {
        this.otherPlans = response.data.items;
        this.totalCount = response.data.totalCount;
      }
      this.loading = false;
    },

    getTagColor(planState) {
      let tagColor = '';
      switch (planState) {
      case PlanState.UnDispatching: {
        //未派工
        tagColor = 'red';
        break;
      }
      case PlanState.Dispatching: {
        //已派工
        tagColor = 'blue';
        break;
      }
      case PlanState.NotIssued:
      case PlanState.OrderCancel:
      case PlanState.NaturalDisasterCancel:
      case PlanState.OtherReasonCancel: {
        //未下发
        tagColor = 'red';
        break;
      }
      case PlanState.Issued: {
        //已下发
        tagColor = 'blue';
        break;
      }
      case PlanState.Complete: {
        //已完成
        tagColor = 'green';
        break;
      }
      }
      return tagColor;
    },

    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    //一键生成月计划
    async onTouchMonth(value) {
      this.monthPickerShow = false;
      let _this = this;
      let dateMonth = moment(value).format('yyyy-MM');
      let month = moment(value).format('MM');
      this.$confirm({
        title: '确认生成？',
        content: h => (
          <div style="color:red;">
            {_this.monthPickerShow
              ? `正在生成中，请勿退出！`
              : `"确认生成${month}月份计划并覆盖原有计划!"`}
          </div>
        ),
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            _this.crateLoading = true;
            let response = await apiSkyLightPlan.oneTouchMonthPlan({
              workUnitId: _this.queryParams.workUnitId,
              month: dateMonth,
            });
            if (requestIsSuccess(response) && response.data) {
              _this.$message.info('生成成功');
              _this.crateLoading = false;
              _this.refresh();
            }
            _this.crateLoading = false;
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },

    //查看退回原因
    viewOpinion(record,changeView = false) {
      this.opinionVisible = true;
      this.opinion = record.opinion;
      if (changeView) {
        this.opinion = '该计划的作业时间已经变更为：' + record.changTime;
      }
    },
  },
  render() {
    //计划状态
    let planStateOption = [];
    for (let item in PlanState) {
      if (
        item === 'NotIssued' ||
        item === 'Issued' ||
        item === 'Complete' ||
        item === 'Revoke' ||
        item === 'OrderCancel' ||
        item === 'NaturalDisasterCancel' ||
        item === 'OtherReasonCancel'
      ) {
        planStateOption.push(
          <a-select-option key={PlanState[item]}>{getPlanState(PlanState[item])}</a-select-option>,
        );
      }
    }

    return (
      <div class="sm-cr-plan-vertical-skylight-plans">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            console.log(moment().month());
            this.refresh();
          }}
          onReset={() => {
            this.queryParams = {
              startTime: null,
              endTime: null,
              workAreaId: null,
              workContent: '',
              workUnitId: this.currentOrganizationId,
            };
            this.dateRange = [moment(moment()).startOf('month'), moment(moment()).endOf('month')];
            this.refresh();
          }}
          advancedCount={20}
        >
          <a-form-item label="作业单位">
            <SmSystemOrganizationTreeSelect
              axios={this.axios}
              autoInitial={true}
              placeholder="车间选择"
              value={this.queryParams.workUnitId}
              onInput={value => {
                if (this.isInit) {
                  this.currentOrganizationId = value;
                }
                this.isInit = false;
                this.queryParams.workUnitId = value;
                this.refresh();
              }}
            />
          </a-form-item>

          {/* <a-form-item label="作业工区">
            <SmSystemOrganizationTreeSelect
              axios={this.axios}
              isAutoDisableOrg={true}
              placeholder="工区选择（本车间下）"
              value={this.queryParams.workAreaId}
              onInput={value => {
                this.queryParams.workAreaId = value;
                this.refresh();
              }}
            />
          </a-form-item> */}

          <a-form-item label="时间选择">
            <a-range-picker
              style="width: 100%"
              value={this.dateRange}
              allowClear={false}
              onChange={value => {
                this.dateRange = value;
                this.refresh();
              }}
            />
          </a-form-item>

          {this.repairTagKey === RepairTagKeys.RailwayHighSpeed ? (
            <a-form-item label="计划状态">
              <a-select
                value={this.queryParams.planState}
                placeholder="请选择计划状态"
                allowClear
                onChange={value => {
                  this.queryParams.planState = value;
                  this.refresh();
                }}
              >
                {planStateOption}
              </a-select>
            </a-form-item>
          ) : (
            undefined
          )}

          <a-form-item label="模糊查找">
            <a-input
              placeholder="请输入作业内容"
              value={this.queryParams.workContent}
              onInput={event => {
                this.queryParams.workContent = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <template slot="buttons">
            {vIf(
              <a-button type="primary" onClick={this.add} disabled={this.crateLoading}>
                添加
              </a-button>,
              vP(this.permissions, permissionsSmCrPlan.OtherPlan.Create),
            )}
            {vIf(
              <a-button type="primary" onClick={this.publishOtherPlan} disabled={this.crateLoading}>
                下发
              </a-button>,
              vP(this.permissions, permissionsSmCrPlan.OtherPlan.Issue),
            )}
            {vIf(
              <a-button
                type="primary"
                loading={this.crateLoading}
                onClick={() => {
                  this.monthPickerShow = !this.monthPickerShow;
                }}
              >
                一键生成计划
              </a-button>,
              vP(this.permissions, permissionsSmCrPlan.OtherPlan.Create),
            )}
            {this.monthPickerShow ? (
              <a-month-picker
                size="default"
                placeholder="请选择生成月份"
                disabled={this.crateLoading}
                onChange={value => {
                  this.onTouchMonth(value);
                }}
              />
            ) : (
              undefined
            )}
          </template>
        </sc-table-operator>

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          dataSource={this.otherPlans}
          rowKey={record => record.id}
          pagination={false}
          loading={this.loading}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return (this.pageIndex - 1) * this.queryParams.maxResultCount + (index + 1);
              },
              workTime: (text, record, index) => {
                return record.workTime ? moment(record.workTime).format('YYYY-MM-DD') : null;
              },
              planState: (text, record, index) => {
                return (
                  <a-tag color={this.getTagColor(record.planState)}>
                    {getPlanState(record.planState)}
                  </a-tag>
                );
              },
              operations: (text, record) => {
                return record.planState != PlanState.Backout
                  ? [
                    <span>
                      {vIf(
                        <a
                          onClick={() => {
                            this.view(record);
                          }}
                        >
                            详情
                        </a>,
                        vP(this.permissions, permissionsSmCrPlan.OtherPlan.Detail),
                      )}
                      {record.planState == PlanState.NotIssued
                        ? [
                          vIf(
                            <a-divider type="vertical" />,
                            vP(this.permissions, permissionsSmCrPlan.OtherPlan.Detail) &&
                                  vP(this.permissions, [
                                    permissionsSmCrPlan.OtherPlan.Update,
                                    permissionsSmCrPlan.OtherPlan.Delete,
                                  ]),
                          ),

                          vIf(
                            <a-dropdown trigger={['click']}>
                              <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>
                                    更多 <a-icon type="down" />
                              </a>
                              <a-menu slot="overlay">
                                <a-menu-item>
                                  {vIf(
                                    <a
                                      onClick={() => {
                                        this.isChange = false;
                                        this.edit(record);
                                      }}
                                    >
                                          编辑
                                    </a>,
                                    vP(this.permissions, permissionsSmCrPlan.OtherPlan.Update),
                                  )}
                                </a-menu-item>
                                <a-menu-item>
                                  {vIf(
                                    <a
                                      onClick={() => {
                                        this.remove(record);
                                      }}
                                    >
                                          删除
                                    </a>,
                                    vP(this.permissions, permissionsSmCrPlan.OtherPlan.Delete),
                                  )}
                                </a-menu-item>
                              </a-menu>
                            </a-dropdown>,
                            vP(this.permissions, [
                              permissionsSmCrPlan.OtherPlan.Update,
                              permissionsSmCrPlan.OtherPlan.Delete,
                            ]),
                          ),
                        ]
                        : undefined}
                      {(record.planState == PlanState.OrderCancel ||
                          record.planState == PlanState.NaturalDisasterCancel ||
                          record.planState == PlanState.OtherReasonCancel) &&
                        !record.isChange
                        ? [
                          <a-divider type="vertical" />,
                          <a-dropdown trigger={['click']}>
                            <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>
                                  更多 <a-icon type="down" />
                            </a>
                            <a-menu slot="overlay">
                              {vIf(
                                <a-menu-item>
                                  <a
                                    onClick={() => {
                                      this.isChange = true;
                                      this.edit(record);
                                    }}
                                  >
                                        计划变更
                                  </a>
                                </a-menu-item>,
                                vP(this.permissions, permissionsSmCrPlan.OtherPlan.Update),
                              )}
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.isChangeView = true;
                                    this.viewOpinion(record);
                                  }}
                                >
                                      取消原因
                                </a>
                              </a-menu-item>
                            </a-menu>
                          </a-dropdown>,
                        ]
                        : undefined}
                      {record.isChange
                        ? [
                          <a-divider type="vertical" />,
                          <a
                            onClick={() => {
                              this.isChangeView = true;
                              this.viewOpinion(record, true);
                            }}
                          >
                                变更详情
                          </a>,
                        ]
                        : undefined}
                    </span>,
                  ]
                  : undefined;
              },
            },
          }}
        ></a-table>

        <a-modal
          visible={this.opinionVisible}
          title={this.isChangeView ? '变更详情' : '取消原因'}
          onCancel={() => (this.opinionVisible = false)}
          footer={null}
          destroyOnClose={true}
        >
          {/* <a-form> */}
          {/* <a-form-item
              label={this.isBackOpinion ? '退回原因' : this.isChangeView ? '变更详情' : '取消原因'}
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            > */}
          <a-input type="textarea" value={this.opinion} disabled></a-input>
          {/* </a-form-item>
          </a-form> */}
        </a-modal>

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
