import moment from 'moment';
import {
  PageState,
  RepairLevel,
  SkylightType,
  PlanState,
  PlanType,
  RepairTagKeys,
  SelectablePlanType,
  RelateRailwayType,
  WorkContentType,
} from '../../_utils/enum';

import StationCascader from '../../sm-basic/sm-basic-station-cascader';
import StationCascader1 from '../../sm-basic/sm-basic-station-cascader';
// import StationCascader2 from '../../sm-basic/sm-basic-station-cascader';
import SmBasicRailwayTreeSelect from '../../sm-basic/sm-basic-railway-tree-select';
import ApiSkyLightPlan from '../../sm-api/sm-cr-plan/SkyLightPlan';
import ApiInfluenceRange from '../../sm-api/sm-std-basic/InfluenceRange';
import SmCrPlanAddSelectablePlanModal from '../sm-cr-plan-add-selectable-plan-modal';
import RelateEquipmentModal from './RelateEquipmentModal';
import SmAddInfluenceModal from './SmAddInfluenceModal';
import SmBasicInstallationSiteSelect from '../../sm-basic/sm-basic-installation-site-select';
import { requestIsSuccess, getRepairLevelTitle, getWorkContentTypeTitle, getPlanState } from '../../_utils/utils';
import * as utils from '../../_utils/utils';
import './style/index.less';

let apiSkyLightPlan = new ApiSkyLightPlan();
let apiInfluenceRange = new ApiInfluenceRange();

export default {
  name: 'SmCrPlanSkylightPlan',
  props: {
    organizationId: { type: String, default: null },
    planType: { type: Number, default: PlanType.OutOf },
    planDate: { type: String, default: null },
    axios: { type: Function, default: null },
    pageState: { type: String, default: PageState.Add }, // 页面状态
    id: { type: String, default: null },
    bordered: { type: Boolean, default: false },
    repairTagKey: { type: String, default: null }, //维修项标签
    isChange: { type: Boolean, default: false },
  },
  data() {
    return {
      lastSkylightPlan: null, //最后添加的一条天窗计划
      plan: null,
      iId: null,
      iOrganizationId: null,
      iPlanDate: null,
      lastSkylightPlanTime: moment().format('HH:mm:ss'), //上一次添加计划的时分秒
      railwayId: undefined,
      stationId: undefined,
      stationRelateRailwayType: RelateRailwayType.SINGLELINK,
      iPlanType: SkylightType.Vertical,
      iPageState: PageState.Add,
      iBordered: false,
      record: null,
      visible: false,
      form: this.$form.createForm(this, {}),
      installationSiteIds: [],
      planDetails: [], //待选计划选中项
      waitingPlanTime: null, //待选计划过滤时间
      planCount: 0,
      loading: false,
      defaultLevel: [],
      influenceRanges: [], //可选的影响范围
      influenceRangesOptions: [],
      workContentType: WorkContentType.MonthYearPlan, //计划内容类型
      registrationPlace: null, //登记地点
      isAdjacent: false, //是否为非相邻区间
      isOnRoad: false,//是否上道
      endStationId: null,
      endStationRelateRailwayType: RelateRailwayType.SINGLELINK,
    };
  },

  computed: {
    formFields() {
      return [
        'workArea',
        'timeLength',
        'railwayId',
        'incidence',
        'workContentType',
        this.workContentType === WorkContentType.OtherPlan ? 'workContent' : '',
      ];
    },
    planDetailIds() {
      return this.planDetails.map(item => item.dailyPlanId);
    },
    columns() {
      return [
        {
          title: '年/月表',
          dataIndex: 'planTypeStr',
          scopedSlots: { customRender: 'planTypeStr' },
          width: 80,
        },
        {
          title: '序号',
          dataIndex: 'number',
          scopedSlots: { customRender: 'number' },
          width: 110,
        },
        {
          title: '设备名称',
          ellipsis: true,
          dataIndex: 'equipName',
        },
        {
          title: '工作内容',
          ellipsis: true,
          dataIndex: 'content',
        },
        {
          title: '日期',
          dataIndex: 'planDate',
          ellipsis: true,
          scopedSlots: { customRender: 'planDate' },
        },
        {
          title: '单位',
          ellipsis: true,
          dataIndex: 'unit',
          width: 60,
        },
        {
          title: '计划数量',
          dataIndex: 'count',
        },
        {
          title: '作业数量',
          dataIndex: 'workCount',
          scopedSlots: { customRender: 'workCount' },
        },
        {
          title: '关联设备',
          dataIndex: 'relateEquipments',
          width: 240,
          scopedSlots: { customRender: 'relateEquipments' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 110,
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
    isShowOnRoad() {
      return this.repairTagKey == RepairTagKeys.RailwayHighSpeed && this.planType == PlanType.Vertical;
    },
  },
  watch: {
    organizationId: {
      handler: function (value, oldValue) {
        this.iOrganizationId = value;
      },
      immediate: true,
    },

    planDate: {
      handler: function (value, oldValue) {
        this.iPlanDate = value;
        this.waitingPlanTime =
          this.repairTagKey == RepairTagKeys.RailwayHighSpeed
            ? moment(value).format('YYYY-MM-DD')
            : moment(value).date(1); //moment(value).date(1);
      },
      immediate: true,
    },
    planType: {
      handler: function (value, oldValue) {
        this.iPlanType = value;
        this.form.resetFields();
        //普铁添加垂直和综合天窗单页时，默认等级改为：天窗点内II级维修
        if (this.repairTagKey == RepairTagKeys.RailwayWired) {
          if (this.iPlanType === PlanType.Vertical || this.iPlanType === PlanType.General) {
            this.defaultLevel = [RepairLevel.LevelII];
          }
          //普铁天窗点外计划添加单页默认等级为：天窗点外II级维修
          if (this.iPlanType === PlanType.OutOf) {
            this.defaultLevel = [RepairLevel.LevelIv];
          }
        }
        //高铁添加垂直和综合天窗单页时，默认等级改为：天窗点内I级维修
        if (this.repairTagKey == RepairTagKeys.RailwayHighSpeed) {
          if (this.iPlanType === PlanType.Vertical || this.iPlanType === PlanType.General) {
            this.defaultLevel = [RepairLevel.LevelI];
          }
        }
      },
      immediate: true,
    },
    id: {
      handler: function (value, oldValue) {
        this.iId = this.id;
        this.plan = null;
        this.planDetails = [];
        this.form.resetFields();
        if (value) {
          this.initAxios();
          this.refresh();
        }
      },
      immediate: true,
    },

    pageState: {
      handler: function (value, oldValue) {
        this.iId = this.id;
        this.iPageState = value;
        if (value != PageState.Add) {
          this.initAxios();
          this.refresh();
        } else {
          this.planDetails = [];
          this.form.resetFields();
        }
      },
      immediate: true,
    },

    bordered: {
      handler: function (value, oldValue) {
        this.iBordered = value;
      },
      // immediate: true,
    },
  },

  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
    this.form.resetFields();
    this.refresh();
    await this.getLastSkylightPlan();
  },

  methods: {
    initAxios() {
      apiSkyLightPlan = new ApiSkyLightPlan(this.axios);
      apiInfluenceRange = new ApiInfluenceRange(this.axios);
    },
    //初始化表单
    async refresh(id) {
      if (id) {
        this.iId = id;
      }
      if (!this.pageState || this.pageState === PageState.Add) {
        return;
      }
      this.getInfluenceRanges(this.defaultLevel);
      let response = await apiSkyLightPlan.get({ id: this.iId, repairTagKey: this.repairTagKey });
      if (requestIsSuccess(response)) {
        this.plan = response.data;
        this.installationSiteIds = this.plan.workSiteIds;
        this.workContentType = this.plan.workContentType;
        this.isOnRoad = this.plan.isOnRoad;
        this.$nextTick(() => {
          let values = utils.objFilterProps(this.plan, this.formFields);
          values = {
            ...values,
            level:
              this.plan && this.plan.level && this.plan.level.split(',').length > 0
                ? this.plan.level.split(',').map(Number)
                : null,
            stationId: this.plan.stationId,
            endStationId: this.plan.endStationId,
            workTime: moment(this.plan.workTime),
            workSiteIds: this.plan.workSiteIds,
          };
          this.registrationPlace = this.plan.registrationPlace;
          this.railwayId = this.plan.railwayId;
          //选中站点类型 用于站点级联选择控件
          this.stationRelateRailwayType = this.plan.stationRelateRailwayType;
          //非区间
          this.isAdjacent = this.plan.isAdjacent;
          this.endStationRelateRailwayType = this.plan.endStationRelateRailwayType;
          // this.endStationId = this.plan.endStationId;

          this.planDetails = this.plan.planDetails
            ? this.plan.planDetails.map(item => {
              return {
                ...item,
                ifdCodes: item.dailyPlan ? item.dailyPlan.ifdCodes : [],
                dailyPlanId: item.dailyPlanId,
                planTypeStr: item.dailyPlan ? item.dailyPlan.planTypeStr : '',
                planType: item.dailyPlan ? item.dailyPlan.planType : null,
                number: item.dailyPlan ? item.dailyPlan.number : null,
                equipName: item.dailyPlan ? item.dailyPlan.equipName : null,
                content: item.dailyPlan ? item.dailyPlan.content : null,
                planDate: item.dailyPlan
                  ? moment(item.dailyPlan.planDate).format('YYYY-MM-DD')
                  : '',
                unit: item.dailyPlan ? item.dailyPlan.unit : '',
                count: item.dailyPlan ? item.dailyPlan.count : null,
                unFinishCount: item.dailyPlan ? item.dailyPlan.unFinishCount : null,
                workCount: item.planCount,
                workCountOld: item.planCount,
                relateEquipments: item.relateEquipments.map(item => {
                  return {
                    name: item.equipmentName,
                    id: item.equipmentId,
                    workCount: item.planCount,
                  };
                }),
              };
            })
            : [];
          this.form.setFieldsValue(values);
        });
      }
    },
    async getLastSkylightPlan() {
      let response = await apiSkyLightPlan.getLastPlan({
        repairTagKey: this.repairTagKey,
        planType: this.iPlanType,
      });
      if (requestIsSuccess(response) && response.data) {
        this.lastSkylightPlan = response.data;
        if (this.lastSkylightPlan) {
          this.lastSkylightPlanTime = moment(this.lastSkylightPlan.workTime).format('HH:mm:ss');
          this.form.setFieldsValue({
            workTime: moment(`${this.iPlanDate} ${this.lastSkylightPlanTime}`),
            timeLength: this.lastSkylightPlan.timeLength,
          });
        }
      }
    },
    addPlans() {
      this.visible = true;
      this.waitingPlanTime = this.form.getFieldValue('workTime').format();
    },

    async relate(record = null) {
      this.record = record;
      let count = (record.unFinishCount + record.workCountOld).toFixed(3);
      this.$refs.RelateEquipmentModal.relate(record, record.ifdCodes, count);
    },

    //设置时间禁选项
    disabledDate(current) {
      if (
        this.repairTagKey !== RepairTagKeys.RailwayHighSpeed ||
        this.pageState !== PageState.Add
      ) {
        return (
          current <=
          moment(this.pageState == PageState.Add ? this.iPlanDate : this.plan.workTime)
            .subtract(1, 'month')
            .endOf('month') ||
          current >=
          moment(this.pageState == PageState.Add ? this.iPlanDate : this.plan.workTime)
            .add(1, 'month')
            .startOf('month')
        );
      }
    },

    //获取已选计划列表
    getSelectedPlans(selectVal) {
      let planDetails = JSON.parse(JSON.stringify(selectVal));
      let _planDetails = [];
      for (let item of planDetails) {
        let target = this.planDetails.find(_item => _item.dailyPlanId === item.id);
        if (!target) {
          item.dailyPlanId = item.id;
          item.workCount = item.unFinishCount;
          item.relateEquipments = [];
          item.dailyPlanId = item.planId;
          item.workCountOld = 0;
          item.influenceRangeId = null;
          _planDetails.push(item);
        }
      }
      this.planDetails = [..._planDetails, ...this.planDetails];
      this.planDetails.map(x => {
        let nums = x.number.split('-');
        let newNumber = '';
        for (let i = 0; i < nums.length; i++) {
          const ele = nums[i];
          newNumber += ele.padStart(3, '0');
        }
        x.sortNumber = newNumber;
      });
      this.planDetails.sort(function (x, y) {
        return x.sortNumber - y.sortNumber;
      });
    },

    save() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let data = JSON.parse(JSON.stringify(values));
          data.workTime = moment(values.workTime)
            .utc()
            .format();
          data.organizationId =
            this.iPageState != PageState.Add ? this.plan.organizationId : this.iOrganizationId;
          data.planType = this.iPlanType;
          data.planState =
            this.iPageState === PageState.Add
              ? this.repairTagKey === RepairTagKeys.RailwayHighSpeed
                ? PlanState.UnSubmited
                : PlanState.UnDispatching
              : this.plan.planState;
          data.level = data.level ? data.level.toString() : '';
          data.stationRelateRailwayType = this.stationRelateRailwayType;
          data.workContentType = this.workContentType;
          data.registrationPlace = this.registrationPlace;
          data.endStationId = this.endStationId;
          data.endStationRelateRailwayType = this.endStationRelateRailwayType;
          data.isAdjacent = this.isAdjacent;
          data.isOnRoad = this.isOnRoad;
          (data.planDetails =
            this.workContentType === WorkContentType.OtherPlan
              ? []
              : this.planDetails.map(item => {
                let result = {
                  dailyPlanId: item.dailyPlanId,
                  planCount: item.workCount != null ? item.workCount : 0,
                  influenceRangeId: item.influenceRangeId,
                  relateEquipments: item.relateEquipments.map(subItem => {
                    return {
                      equipmentId: subItem.id,
                      planCount: subItem.workCount,
                    };
                  }),
                };
                if (this.iPageState == PageState.Edit) {
                  result = {
                    ...result,
                    id: item.id,
                  };
                }
                return result;
              }));
          if (data.planDetails.find(item => item.planCount === 0)) {
            this.$message.error('计划内容作业数量不能为 0 !');
          } else {
            this.loading = true;
            if (this.iPageState === PageState.Add) {
              let response = await apiSkyLightPlan.create(false, data, this.repairTagKey);
              if (requestIsSuccess(response)) {
                this.$message.success('操作成功');
                this.$emit('ok');
                this.form.resetFields();
                this.planDetails = [];
                this.registrationPlace = null;
              }
            } else if (this.iPageState === PageState.Edit) {
              data.isChange = this.isChange;
              let response = await apiSkyLightPlan.update(
                false,
                { id: this.id, ...data },
                this.repairTagKey,
              );
              if (requestIsSuccess(response)) {
                this.$message.success('操作成功');
                this.$emit('ok', this.id);
                this.form.resetFields();
                this.registrationPlace = null;
                this.planDetails = [];
                this.workContentType = WorkContentType.MonthYearPlan;
              }
            }
            this.loading = false;
          }
        }
      });
    },
    remove(record) {
      let index = this.planDetails.indexOf(record);
      if (index > -1) this.planDetails.splice(index, 1);
    },

    getWorkCount(record) {
      let workCount = 0;
      for (let item of record.relateEquipments) {
        workCount += item.workCount;
      }
      record.workCount = workCount;
      return workCount;
    },

    //根据维修级别获取影响范围
    async getInfluenceRanges(val) {
      let param = {
        repairLevel: this.defaultLevel,
        isAll: true,
      };
      this.influenceRanges = [];
      this.influenceRangesOptions = [];
      let response = await apiInfluenceRange.getList(param, this.repairTagKey);
      if (requestIsSuccess(response)) {
        this.influenceRanges = response.data.items;
        this.influenceRanges.map(item => {
          this.influenceRangesOptions.push(
            <a-select-option key={item.id}>{item.content}</a-select-option>,
          );
        });
      }
    },

    //插入标准影响范围
    insertInfluence() {
      this.$refs.SmAddInfluenceModal.open();
    },
  },
  render() {
    let isDisabled =
      this.iPageState === PageState.Edit &&
      this.plan !== null &&
      this.repairTagKey === RepairTagKeys.RailwayHighSpeed &&
      this.plan.planType !== PlanType.OutOf &&
      this.plan.planState === PlanState.Adopted;
    let options = [];
    for (let item in RepairLevel) {
      if (
        this.repairTagKey == RepairTagKeys.RailwayWired ||
        this.repairTagKey == '' ||
        !this.repairTagKey ||
        (this.repairTagKey == RepairTagKeys.RailwayHighSpeed &&
          this.iPlanType != PlanType.Vertical) ||
        (this.repairTagKey == RepairTagKeys.RailwayHighSpeed &&
          this.iPlanType === PlanType.Vertical &&
          (RepairLevel[item] == RepairLevel.LevelI || RepairLevel[item] == RepairLevel.LevelII))
      )
        options.push(
          <a-select-option key={RepairLevel[item]} title={getRepairLevelTitle(RepairLevel[item])}>
            {getRepairLevelTitle(RepairLevel[item])}
          </a-select-option>,
        );
    }
    let workContentTypeOptions = [];
    for (let item in WorkContentType) {
      workContentTypeOptions.push(
        <a-select-option key={WorkContentType[item]}>
          {getWorkContentTypeTitle(WorkContentType[item])}
        </a-select-option>,
      );
    }



    return (
      <div class="sm-cr-plan-skylight-plan">
        {/* 表单区 */}
        <a-form form={this.form}>
          <a-row gutter={24}>
            {this.planType === PlanType.OutOf &&
              this.repairTagKey === RepairTagKeys.RailwayHighSpeed ? (
                undefined
              ) : (
                <a-col sm={24} md={12}>
                  <a-form-item label="维修级别" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <a-select
                      mode="multiple"
                      allowClear={false}
                      optionFilterProp="title"
                      disabled={this.iPageState == PageState.View || isDisabled}
                      placeholder={this.iPageState == PageState.View ? '' : '请选择'}
                      onChange={val => {
                        this.defaultLevel = val;
                        this.form.setFieldsValue({ incidence: '' });
                      //this.getInfluenceRanges(val);
                      }}
                      v-decorator={[
                        'level',
                        {
                          initialValue: this.defaultLevel,
                          rules: [{ required: true, message: '请选择维修级别！' }],
                        },
                      ]}
                    >
                      {options}
                    </a-select>
                  </a-form-item>
                </a-col>
              )}
            <a-col
              sm={24}
              md={
                this.planType === PlanType.OutOf &&
                  this.repairTagKey === RepairTagKeys.RailwayHighSpeed
                  ? 12
                  : 6
              }
            >
              <a-form-item
                label="计划日期"
                label-col={{
                  span:
                    this.planType === PlanType.OutOf &&
                      this.repairTagKey === RepairTagKeys.RailwayHighSpeed
                      ? 4
                      : 8,
                }}
                wrapper-col={{
                  span:
                    this.planType === PlanType.OutOf &&
                      this.repairTagKey === RepairTagKeys.RailwayHighSpeed
                      ? 20
                      : 16,
                }}
              >
                <a-date-picker
                  style="width: 100%"
                  showTime={true}
                  disabled={this.iPageState == PageState.View}
                  placeholder={this.iPageState == PageState.View ? '' : '请输入'}
                  disabledDate={this.disabledDate}
                  onChange={value => {
                    this.waitingPlanTime = moment(value)
                      .utc()
                      .format();
                  }}
                  v-decorator={[
                    'workTime',
                    {
                      initialValue: moment(
                        this.lastSkylightPlanTime
                          ? `${moment(this.iPlanDate).format('YYYY-MM-DD')} ${this.lastSkylightPlanTime
                          }`
                          : this.iPlanDate,
                      ).date(1),
                      rules: [{ required: true, message: '请输入计划日期！' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col
              sm={24}
              md={
                this.planType === PlanType.OutOf &&
                  this.repairTagKey === RepairTagKeys.RailwayHighSpeed
                  ? 12
                  : 6
              }
            >
              <a-form-item
                label="计划时长"
                label-col={{
                  span:
                    this.planType === PlanType.OutOf &&
                      this.repairTagKey === RepairTagKeys.RailwayHighSpeed
                      ? 4
                      : 8,
                }}
                wrapper-col={{
                  span:
                    this.planType === PlanType.OutOf &&
                      this.repairTagKey === RepairTagKeys.RailwayHighSpeed
                      ? 20
                      : 16,
                }}
              >
                <a-input-number
                  disabled={this.iPageState == PageState.View}
                  style="width: 100%"
                  min={0}
                  precision={0}
                  placeholder={this.iPageState == PageState.View ? '' : '请输入计划时长（分钟）'}
                  v-decorator={[
                    'timeLength',
                    {
                      initialValue: this.lastSkylightPlan ? this.lastSkylightPlan.timeLength : null,
                      rules: [{ required: true, message: '请输入计划时长！' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={12}>
              <a-form-item label="线路" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <SmBasicRailwayTreeSelect
                  organizationId={this.organizationId}
                  axios={this.axios}
                  disabled={this.iPageState == PageState.View || isDisabled}
                  placeholder={this.iPageState == PageState.View ? '' : '请选择'}
                  onChange={item => {
                    this.railwayId = item;
                    this.stationId = undefined;
                  }}
                  v-decorator={[
                    'railwayId',
                    {
                      initialValue: undefined,
                      rules: [{ required: true, message: '请选择线路' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={12}>
              {
                this.isShowOnRoad ?
                  <div style="display:flex;justify-content: space-around;">
                    <a-form-item label="区间类型" >
                      <span>
                        <a-radio-group
                          disabled={this.iPageState == PageState.View || isDisabled}
                          value={this.isAdjacent}
                          onChange={value => {
                            this.isAdjacent = value.target.value;
                            // this.railwayId = this.railwayId;
                          }}
                        >
                          <a-radio value={false}>相邻区间</a-radio>
                          <a-radio value={true}>非相邻区间</a-radio>
                        </a-radio-group>
                      </span>
                    </a-form-item>
                    <a-form-item label="是否上道" >
                      <a-radio-group
                        disabled={this.iPageState == PageState.View || isDisabled}
                        value={this.isOnRoad}
                        onChange={value => {
                          this.isOnRoad = value.target.value;
                        }}
                      >
                        <a-radio value={true}>是</a-radio>
                        <a-radio value={false}>否</a-radio>
                      </a-radio-group>
                    </a-form-item>
                  </div>
                  : <a-form-item label="区间类型" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <span>
                      <a-radio-group
                        disabled={this.iPageState == PageState.View || isDisabled}
                        value={this.isAdjacent}
                        onChange={value => {
                          this.isAdjacent = value.target.value;
                          // this.railwayId = this.railwayId;
                        }}
                      >
                        <a-radio value={false}>相邻区间</a-radio>
                        <a-radio value={true}>非相邻区间</a-radio>
                      </a-radio-group>
                    </span>
                  </a-form-item>

              }
            </a-col>

            {!this.isAdjacent ? (
              <a-col sm={24} md={12}>
                <a-form-item label="车站/区间" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <StationCascader
                    key="001"
                    axios={this.axios}
                    disabled={this.iPageState == PageState.View || isDisabled}
                    // organizationId={this.iOrganizationId}
                    placeholder={this.iPageState == PageState.View ? '' : '请选择'}
                    organizationId={this.iOrganizationId}
                    railwayId={this.railwayId}
                    isShowUpAndDown={true}
                    staRelateType={this.stationRelateRailwayType}
                    onChange={(value, relateId, relateType) => {
                      this.stationId = value;
                      this.stationRelateRailwayType = relateType;
                    }}
                    v-decorator={[
                      'stationId',
                      {
                        initialValue: undefined,
                        rules: [{ required: true, message: '请选择车站/区间' }],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
            ) : (
              [
                <a-col sm={24} md={12}>
                  <a-form-item label="起始站点" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <StationCascader
                      key="002"
                      axios={this.axios}
                      disabled={this.iPageState == PageState.View || isDisabled}
                      placeholder={this.iPageState == PageState.View ? '' : '请选择'}
                      organizationId={this.iOrganizationId}
                      railwayId={this.railwayId}
                      isShowUpAndDown={true}
                      staRelateType={this.stationRelateRailwayType}
                      onChange={(value, relateId, relateType) => {
                        this.stationId = value;
                        this.stationRelateRailwayType = relateType;
                      }}
                      v-decorator={[
                        'stationId',
                        {
                          initialValue: undefined,
                          rules: [{ required: true, message: '请选择起始站点' }],
                        },
                      ]}
                      isShowStation={true}
                    />
                  </a-form-item>
                </a-col>,
                <a-col sm={24} md={12}>
                  <a-form-item label="终止站点" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <StationCascader
                      key="003"
                      axios={this.axios}
                      disabled={this.iPageState == PageState.View || isDisabled}
                      placeholder={this.iPageState == PageState.View ? '' : '请选择'}
                      organizationId={this.iOrganizationId}
                      railwayId={this.railwayId}
                      isShowUpAndDown={true}
                      staRelateType={this.stationRelateRailwayType}
                      onChange={(value, relateId, relateType) => {
                        this.endStationId = value;
                        this.endStationRelateRailwayType = relateType;
                      }}
                      v-decorator={[
                        'endStationId',
                        {
                          initialValue: undefined,
                          rules: [{ required: true, message: '请选择终止站点' }],
                        },
                      ]}
                      isShowStation={true}
                    />
                  </a-form-item>
                </a-col>,
              ]
            )}
            <a-col sm={24} md={12} style="height: 64px">
              <a-form-item label="作业处所" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <SmBasicInstallationSiteSelect
                  // railwayRltStation={this.railwayRltStation}
                  axios={this.axios}
                  multiple={true}
                  disabled={this.iPageState == PageState.View || isDisabled}
                  railwayId={this.railwayId}
                  stationId={this.stationId}
                  onChange={value => {
                    this.installationSiteIds = value;
                  }}
                  advancedCount={20}
                  v-decorator={[
                    'workSiteIds',
                    {
                      initialValue: [],
                      rules: [
                        {
                          required: !this.isAdjacent,
                          message: "请选择作业处所",
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={12}>
              <a-form-item label="位置(里程)" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-input
                  disabled={this.iPageState == PageState.View || isDisabled}
                  placeholder={this.iPageState == PageState.View ? '' : '请输入'}
                  v-decorator={[
                    'workArea',
                    {
                      initialValue: '',
                      rules: [{ max: 120 }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            {this.repairTagKey == RepairTagKeys.RailwayHighSpeed &&
              this.planType == PlanType.Vertical ? (
                <a-col sm={24} md={12}>
                  <a-form-item label="登记地点" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                    <a-input
                      disabled={this.iPageState == PageState.View || isDisabled}
                      value={this.registrationPlace}
                      onChange={value => {
                        this.registrationPlace = value.target.value;
                      }}
                    ></a-input>
                  </a-form-item>
                </a-col>
              ) : (
                undefined
              )}

            {this.repairTagKey == RepairTagKeys.RailwayHighSpeed &&
              this.planType == PlanType.OutOf ? (
                undefined
              ) : (
                <a-col sm={24} md={24}>
                  <a-form-item label="影响范围" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                    <a-textarea
                      rows="3"
                      // span={20}
                      disabled={this.iPageState == PageState.View || isDisabled}
                      placeholder={this.iPageState == PageState.View ? '' : '请输入'}
                      v-decorator={[
                        'incidence',
                        {
                          initialValue: '',
                          rules: [
                          // {
                          //   max: 120,
                          //   message: "最多120字",
                          // },
                            {
                              required:
                              this.repairTagKey == RepairTagKeys.RailwayHighSpeed ? true : false,
                              message: '请输入影响范围',
                            },
                          ],
                        },
                      ]}
                    />
                    {this.iPageState == PageState.View || isDisabled ? (
                      undefined
                    ) : (
                      <a onClick={this.insertInfluence}>插入标准影响范围</a>
                    )}
                  </a-form-item>
                </a-col>
              )}

            {this.repairTagKey === RepairTagKeys.RailwayHighSpeed ? (
              <a-col sm={24} md={24}>
                <a-form-item
                  label="计划内容类型"
                  label-col={{ span: 2 }}
                  wrapper-col={{ span: 22 }}
                >
                  <a-select
                    disabled={this.iPageState == PageState.View || isDisabled}
                    placeholder={this.iPageState == PageState.View ? '' : '请选择'}
                    onChange={val => {
                      this.workContentType = val;
                    }}
                    v-decorator={[
                      'workContentType',
                      {
                        initialValue: WorkContentType.MonthYearPlan,
                        rules: [{ required: true, message: '请选择计划内容类型！' }],
                      },
                    ]}
                  >
                    {workContentTypeOptions}
                  </a-select>
                </a-form-item>
              </a-col>
            ) : (
              undefined
            )}
            {this.workContentType === WorkContentType.OtherPlan ? (
              <a-col sm={24} md={24}>
                <a-form-item label="计划内容" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                  <a-textarea
                    rows="3"
                    disabled={this.iPageState == PageState.View || isDisabled}
                    placeholder={this.iPageState == PageState.View ? '' : '请输入'}
                    v-decorator={[
                      'workContent',
                      {
                        initialValue: '',
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
            ) : (
              [
                this.iPageState == PageState.View || isDisabled ? (
                  undefined
                ) : (
                  <a-col
                    sm={24}
                    md={24}
                  // offset={this.repairTagKey === RepairTagKeys.RailwayHighSpeed ? 0 : 21}
                  >
                    {/* <a-form-item
                      label-col={{ span: 4 }}
                      wrapper-col={{
                        span: 20,
                        offset: this.repairTagKey === RepairTagKeys.RailwayHighSpeed ? 20 : 0,
                      }}
                    > */}
                    <a-button
                      style="margin-right:20px;float:right;margin-bottom: 10px;"
                      type="primary"
                      onClick={this.addPlans}
                    >
                      添加计划
                    </a-button>
                    {/* </a-form-item> */}
                  </a-col>
                ),

                <a-col sm={24} md={24}>
                  <a-form-item label="计划内容" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                    {/* 计划内容展示区 */}
                    <a-table
                      columns={this.columns}
                      dataSource={this.planDetails}
                      rowKey={record => record.dailyPlanId}
                      pagination={false}
                      scroll={{ y: 400 }}
                      {...{
                        scopedSlots: {
                          planDate: (text, record) => {
                            return (
                              <a-tooltip
                                placement="topLeft"
                                title={
                                  record.planDate
                                    ? moment(record.planDate).format('YYYY-MM-DD')
                                    : ''
                                }
                              >
                                <span>
                                  {record.planDate
                                    ? moment(record.planDate).format('YYYY-MM-DD')
                                    : ''}
                                </span>
                              </a-tooltip>
                            );
                          },
                          planTypeStr: (text, record, index) => {
                            return record.planType === SelectablePlanType.Month ? '月表' : '年表';
                          },
                          // influenceRange: (text, record) => {
                          //   return (
                          //     <a-select
                          //       value={record.influenceRangeId}
                          //       onChange={val => {
                          //         record.influenceRangeId = val;
                          //       }}
                          //     >{this.influenceRangesOptions}
                          //     </a-select>);
                          // },
                          workCount: (text, record) => {
                            return record.relateEquipments.length > 0 ? (
                              this.getWorkCount(record)
                            ) : (
                              <a-input-number
                                disabled={this.iPageState == PageState.View || isDisabled}
                                style="margin: -10px 0"
                                min={0}
                                max={
                                  record.workCountOld + record.unFinishCount
                                  // (record.unFinishCount !== null ? record.unFinishCount : 0)
                                }
                                precision={3}
                                value={record.workCount}
                                onChange={value => {
                                  record.workCount = value;
                                }}
                              />
                            );
                          },
                          relateEquipments: (text, record) => {
                            return (
                              <div class="relate-equipments-box">
                                {record.relateEquipments && record.relateEquipments.length > 0
                                  ? record.relateEquipments.map((item, index) => {
                                    return (
                                      <span class="relate-equipment">
                                        {`${item.name} (${item.workCount})`}
                                        {this.iPageState === PageState.View ? (
                                          undefined
                                        ) : (
                                          <span
                                            class="close-icon"
                                            onClick={() => {
                                              let index = record.relateEquipments.indexOf(item);
                                              record.relateEquipments.splice(index, 1);
                                            }}
                                          >
                                            <a-icon type="close"></a-icon>
                                          </span>
                                        )}
                                      </span>
                                    );
                                  })
                                  : undefined}
                              </div>
                            );
                          },

                          operations: (text, record) => {
                            return this.iPageState == PageState.View || isDisabled
                              ? undefined
                              : [
                                <a
                                  onClick={() => {
                                    this.relate(record);
                                  }}
                                >
                                  关联
                                </a>,
                                <a-divider type="vertical" />,
                                <a
                                  onClick={() => {
                                    this.remove(record);
                                  }}
                                >
                                  删除
                                </a>,
                              ];
                          },
                        },
                      }}
                    ></a-table>
                  </a-form-item>
                </a-col>,
              ]
            )}
          </a-row>
        </a-form>
        <div style="float: right;">
          {this.iPageState == PageState.View ? (
            <a-button
              onClick={() => {
                this.$emit('cancel');
                this.planDetails = [];
                this.form.resetFields();
                this.registrationPlace = null;
                this.workContentType = WorkContentType.MonthYearPlan;
              }}
            >
              关闭
            </a-button>
          ) : (
            [
              <a-button
                type="primary"
                disabled={this.iPageState == PageState.View}
                style="margin-right: 20px"
                onClick={this.save}
                loading={this.loading}
              >
                保存
              </a-button>,
              <a-button
                onClick={() => {
                  this.$emit('cancel');
                  this.planDetails = [];
                  this.form.resetFields();
                  this.registrationPlace = null;
                  this.workContentType = WorkContentType.MonthYearPlan;
                }}
              >
                取消
              </a-button>,
            ]
          )}
        </div>
        <SmCrPlanAddSelectablePlanModal
          axios={this.axios}
          selected={this.planDetailIds}
          visible={this.visible}
          repairTagKey={this.repairTagKey}
          date={
            this.repairTagKey === RepairTagKeys.RailwayHighSpeed
              ? new Date(this.waitingPlanTime)
              : this.iPlanType === PlanType.Vertical
                ? new Date(this.iPlanDate)
                : new Date(this.waitingPlanTime)
          }
          organizationId={this.iOrganizationId}
          skylightType={this.iPlanType}
          onOk={value => {
            this.getSelectedPlans(value);
            this.visible = false;
          }}
          onChange={() => {
            this.visible = false;
          }}
        />
        <RelateEquipmentModal
          ref="RelateEquipmentModal"
          axios={this.axios}
          installationSiteIds={this.installationSiteIds}
          bordered={this.iBordered}
          organizationId={this.iOrganizationId}
          onSuccess={values => {
            this.record.relateEquipments = values;
          }}
        />
        <SmAddInfluenceModal
          ref="SmAddInfluenceModal"
          axios={this.axios}
          repairTagKey={this.repairTagKey}
          repairLevel={this.defaultLevel}
          onSuccess={val => {
            let incidence = this.form.getFieldValue('incidence');
            this.form.setFieldsValue({ incidence: incidence + val });
          }}
        />
      </div>
    );
  },
};
