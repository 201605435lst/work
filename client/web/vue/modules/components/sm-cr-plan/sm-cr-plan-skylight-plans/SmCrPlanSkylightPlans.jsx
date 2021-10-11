import moment from 'moment';
import ApiSkyLightPlan from '../../sm-api/sm-cr-plan/SkyLightPlan';
import OrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import {
  pagination as paginationConfig,
  tips as tipsConfig,
  form as formConfig,
} from '../../_utils/config';
import { requestIsSuccess, getPlanState, getRepairLevelTitle, vIf, vP } from '../../_utils/utils';
import SmCrPlanGenerateRepairPlanModal from './SmCrPlanGenerateRepairPlanModal';
import SmCrPlanPlanTodoModal from './SmCrPlanPlanTodoModal';
import { PlanType, PlanState, RepairTagKeys, RepairLevel } from '../../_utils/enum';
import permissionsSmCrPlan from '../../_permissions/sm-cr-plan';
import SmBasicRailwayTreeSelect from '../../sm-basic/sm-basic-railway-tree-select';
import SmBasicInstallationSiteSelect from '../../sm-basic/sm-basic-installation-site-select';
import StationCascader from '../../sm-basic/sm-basic-station-cascader';
import WorkflowModal from '../../sm-bpm/sm-bpm-workflows/src/WorkflowModal';
import SmCrPlanSkylightAddWorkTicketModal from './SmCrPlanSkylightAddWorkTicketModal';
import './style';
let apiSkyLightPlan = new ApiSkyLightPlan();

export default {
  name: 'SmCrPlanSkylightPlans',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    planType: { type: Number, default: PlanType.Vertical },
    permissions: { type: Array, default: () => [] },
    // repairTagKey: { type: String, default: RepairTagKeys.RailwayHighSpeed }, //维修项标签
    repairTagKey: { type: String, default: null }, //维修项标签
  },
  data() {
    return {
      loading: false,
      skyLightPlans: [], // 列表数据源
      belongOrgs: [], //所属机构
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        workUnit: null, // 作业单位
        workTime: moment(), // 时间选择
        workSite: null, //机房
        railwayId: undefined, //线路
        planType: null, //天窗类型
        station: undefined, //车站选择
        contentMileage: '', //模糊查询
        repaireLevel: null, //RepairLevel.LevelI,//维修等级
        planState: undefined,
        maxResultCount: paginationConfig.defaultPageSize,
        dateRange: [moment(moment()).startOf('day'), moment(moment()).endOf('day')], //时间查询
        checkSkylightPlans: [],
      },
      form: this.$form.createForm(this),
      currentOrganizationId: null, //当前用户所属组织机构
      isInit: false,
      skylightPlanIds: [],
      skylightPlanId: null,
      workTicketId: null,
      opinionVisible: false, //退回原因modal
      opinion: null,
      isChange: false,//是否为计划变更(高铁新需求)
      isBackOpinion: true,
      isChangeView:false,
    };
  },
  computed: {
    // railwayRltStation() {
    //   let railwayRltStation = {
    //     railwayId: this.queryParams.railwayId,
    //     stationId: this.queryParams.station,
    //     organizationId: this.queryParams.workUnit,
    //   };
    //   return railwayRltStation;
    // },
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 60,
          scopedSlots: { customRender: 'index' },
        },
        this.planType === PlanType.OutOf && this.repairTagKey === RepairTagKeys.RailwayHighSpeed
          ? {
            width: 1,
          }
          : {
            title: '级别',
            ellipsis: true,
            dataIndex: 'level',
            scopedSlots: { customRender: 'level' },
          },
        {
          title: '线路',
          ellipsis: true,
          dataIndex: 'railway',
          scopedSlots: { customRender: 'railway' },
        },
        {
          title: '车站(区间)',
          ellipsis: true,
          dataIndex: 'stationName',
        },
        {
          title: '作业单位',
          ellipsis: true,
          dataIndex: 'workUnitName',
        },
        {
          title: '作业机房',
          ellipsis: true,
          dataIndex: 'workSiteName',
        },
        {
          title: '位置(里程)',
          ellipsis: true,
          dataIndex: 'workArea',
        },
        {
          title: '计划日期',
          dataIndex: 'workTime',
          width: 160,
          scopedSlots: { customRender: 'workTime' },
        },
        {
          title: '计划时长',
          dataIndex: 'timeLength',
          width: 90,
        },
        {
          title: '作业内容',
          ellipsis: true,
          dataIndex: 'workContent',
          scopedSlots: { customRender: 'workContent' },
        },
        {
          title: '影响范围',
          ellipsis: true,
          dataIndex: 'incidence',
          scopedSlots: { customRender: 'incidence' },
        },
        {
          title: '状态',
          dataIndex: 'planState',
          width: 90,
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
    //高铁需求变更：工作票内表头
    innerColumns() {
      return [
        {
          title: '命令号',
          dataIndex: 'orderNumber',
          // width: 75,
        },
        {
          title: '作业名称',
          dataIndex: 'workTitle',
          ellipsis: true,
          // width: 121,
        },
        {
          title: '作业地点',
          dataIndex: 'workPlace',
          ellipsis: true,
          // width: 150,
        },
        // {
        //   title: '施工维修等级',
        //   dataIndex: 'repairLevel',
        //   scopedSlots: { customRender: 'repairLevel' },
        //   ellipsis: true,
        //   // width: 150,
        // },
        {
          title: '作业内容',
          dataIndex: 'workContent',
          ellipsis: true,
          // width: 187,
        },
        {
          title: '影响范围',
          dataIndex: 'influenceRange',
          ellipsis: true,
        },
        {
          // width: 200,
          title: '制表人',
          dataIndex: 'paperMaker',
          scopedSlots: { customRender: 'paperMaker' },
          ellipsis: true,
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 140,
          // fixed: 'right',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch: {
    planType: {
      handler: function(value, oldValue) {
        this.queryParams.planType = value;
        this.form.resetFields();
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
    this.form.resetFields();
    this.queryParams.workTime =
      this.queryParams.planType === PlanType.Vertical
        ? moment().add(1, 'month')
        : moment().add(1, 'week');
    this.refresh();
    this.isInit = true;
  },

  mounted() {},

  methods: {
    initAxios() {
      apiSkyLightPlan = new ApiSkyLightPlan(this.axios);
    },

    add() {
      this.$emit(
        'add',
        this.repairTagKey !== RepairTagKeys.RailwayHighSpeed
          ? moment(this.queryParams.workTime)
            .utc()
            .format('YYYY-MM-DD')
          : moment(this.queryParams.dateRange[0]).format('YYYY-MM-DD'),
        this.queryParams.workUnit,
      );
    },
    edit(record) {
      this.$emit(
        'edit',
        record.id,
        record.workUnit,
        moment(record.workTime)
          // .utc()
          .format('YYYY-MM-DD'),
        this.isChange,
      );
    },
    view(record) {
      this.$emit(
        'view',
        record.id,
        record.workUnit,
        moment(record.workTime)
          // .utc()
          .format('YYYY-MM-DD'),
      );
    },

    dispatch(record) {
      if (record.planState == PlanState.Adopted || record.planState == PlanState.UnDispatching)
        this.$refs.SmCrPlanPlanTodoModal.dispatch(record);
      else this.$message.info('此计划暂未通过审批，无法派工');
    },

    viewDispatch(record) {
      if (record.planState == PlanState.Dispatching || record.planState == PlanState.Complete)
        this.$refs.SmCrPlanPlanTodoModal.view(record);
      else this.$message.info('此计划暂未派工，请先派工');
    },

    //撤销
    backout(record) {
      let _this = this;
      let _content = '确认要撤销该计划！';
      this.$confirm({
        title: '撤销提示',
        content: _content,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiSkyLightPlan.backoutPlan({
              id: record.id,
              repairTagKey: _this.repairTagKey,
            });
            if (requestIsSuccess(response)) {
              _this.$message.info('撤销成功');
            }
            _this.refresh();
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },

    publish() {
      let workTime = moment(this.queryParams.workTime);
      let _this = this;
      let data = {
        workUnit: this.queryParams.workUnit,
        startTime:
          this.queryParams.planType === PlanType.Vertical
            ? moment(workTime)
              .date(1)
              .hours(0)
              .minutes(0)
              .seconds(0)
              .format('YYYY-MM-DD HH:mm:ss')
            : moment(workTime)
              .weekday(0)
              .hours(0)
              .minutes(0)
              .seconds(0)
              .format('YYYY-MM-DD HH:mm:ss'),
        endTime:
          this.queryParams.planType === PlanType.Vertical
            ? moment(workTime)
              .endOf('month')
              .endOf('date')
              .format('YYYY-MM-DD HH:mm:ss')
            : moment(workTime)
              .endOf('week')
              .endOf('date')
              .format('YYYY-MM-DD HH:mm:ss'),
        station: this.queryParams.station,
        workSite: this.queryParams.workSite,
        planType: this.queryParams.planType,
        contentMileage: this.queryParams.contentMileage,
      };
      this.$confirm({
        title: '发布提示',
        content: '请确认发布会将列表中的所有计划发布？',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiSkyLightPlan.publishPlan(data);
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
            _this.refresh();
          });
        },
      });
    },

    //提交审批
    generatePlan() {
      this.$refs.SmCrPlanGenerateRepairPlanModal.generatePlan(
        this.queryParams,
        this.skylightPlanIds,
      );
    },

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
            _this.refresh(false, _this.pageIndex);
            if (requestIsSuccess(response)) {
              _this.$message.info('删除成功');
            }
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },

    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      // this.skylightPlanIds = [];
      let workTime = moment(this.queryParams.workTime);
      let queryParams = {
        workUnit: this.queryParams.workUnit,
        //高铁需求变更：时间筛选采用范围选择
        startTime:
          this.repairTagKey === RepairTagKeys.RailwayHighSpeed
            ? moment(this.queryParams.dateRange[0])
              .hours(0)
              .minutes(0)
              .seconds(0)
              .format('YYYY-MM-DD HH:mm:ss')
            : this.queryParams.planType === PlanType.Vertical
              ? moment(workTime)
                .date(1)
                .hours(0)
                .minutes(0)
                .seconds(0)
                .format('YYYY-MM-DD HH:mm:ss')
              : moment(workTime)
                .weekday(0)
                .hours(0)
                .minutes(0)
                .seconds(0)
                .format('YYYY-MM-DD HH:mm:ss'),
        endTime:
          this.repairTagKey === RepairTagKeys.RailwayHighSpeed
            ? moment(this.queryParams.dateRange[1])
              .hours(23)
              .minutes(59)
              .seconds(59)
              .format('YYYY-MM-DD HH:mm:ss')
            : this.queryParams.planType === PlanType.Vertical
              ? moment(workTime)
                .endOf('month')
                .endOf('date')
                .format('YYYY-MM-DD HH:mm:ss')
              : moment(workTime)
                .endOf('week')
                .endOf('date')
                .format('YYYY-MM-DD HH:mm:ss'),
        Station: this.queryParams.station,
        WorkSite: this.queryParams.workSite,
        contentMileage: this.queryParams.contentMileage,
        planType: this.queryParams.planType,
        railwayId: this.queryParams.railwayId,
        maxResultCount: this.queryParams.maxResultCount,
        repaireLevel: this.queryParams.repaireLevel,
        state: this.queryParams.planState,
      };
      let response = await apiSkyLightPlan.getList(
        {
          skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
          ...queryParams,
        },
        this.repairTagKey,
      );
      if (requestIsSuccess(response)) {
        this.skyLightPlans = response.data.items;
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

    getTagColor(planState) {
      let tagColor = '';
      switch (planState) {
      case PlanState.UnDispatching: {
        //未派工
        tagColor = 'orange';
        break;
      }
      case PlanState.Dispatching: {
        //已派工
        tagColor = 'blue';
        break;
      }
      case PlanState.NotIssued: {
        //未下发
        tagColor = 'orange';
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
      case PlanState.UnSubmited: {
        //待提交
        tagColor = 'volcano';
        break;
      }
      case PlanState.Submited: {
        //已提交
        tagColor = 'cyan';
        break;
      }
      case PlanState.Waitting: {
        //审批中
        tagColor = 'purple';
        break;
      }
      case PlanState.Revoke: {
        //已撤销
        tagColor = 'orange';
        break;
      }
      case PlanState.Adopted: {
        //已批复
        tagColor = 'darkTurquoise';
        break;
      }
      case PlanState.UnAdopted:
      case PlanState.OrderCancel:
      case PlanState.Backed:
      case PlanState.NaturalDisasterCancel:
      case PlanState.OtherReasonCancel:
      {
        //未批复
        tagColor = 'red';
        break;
      }
      // case PlanState.OrderCancel: {
      //   tagColor = 'red';
      //   break;
      // }
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

    //添加工作票
    addWorkTicket(record) {
      this.$refs.SmCrPlanSkylightAddWorkTicketModal.add(record);
      this.skylightPlanId = record.id;
    },
    //删除工作票
    removeWorkTicket(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let data = {
              ticketId: record.id,
            };
            let response = await apiSkyLightPlan.removeWorkTicket(data);
            if (requestIsSuccess(response)) {
              _this.$message.success('删除成功');
              _this.refresh(_this._repaireId);
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
      });
    },

    //查看工作票
    viewWorkTicket(record) {
      this.$refs.SmCrPlanSkylightAddWorkTicketModal.view(record);
    },

    //编辑工作票
    editWorkTicket(record) {
      this.skylightPlanId = record.skylightPlanId;
      this.workTicketId = record.id;
      this.$refs.SmCrPlanSkylightAddWorkTicketModal.edit(record);
    },
    
    //查看退回原因
    viewOpinion(record, changeView = false) {
      this.opinionVisible = true;
      this.opinion = record.opinion;
      if (changeView) {
        // let a = <span style="color:red">ssss</span>;
        // console.log(a);
        this.opinion = '该计划的作业时间已经变更为：' + record.changTime;
      }
    },

    //设置时间禁选项
    disabledDate(current) {
      return (
        current <=
          moment(this.queryParams.dateRange[0].format())
            .subtract(1, 'month')
            .endOf('month') ||
        current >=
          moment(this.queryParams.dateRange[0].format())
            .add(1, 'month')
            .startOf('month')
      );
    },

    //查看工作流
    viewWorkFlow(record) {
      if (record.workFlowId != null) {
        this.$refs.WorkflowModal.isInitial = false;
        this.$refs.WorkflowModal.edit(record.workFlowId);
      } else {
        this.$message.error('未提交审批/该计划已被驻台联络员退回');
      }
    },
  },
  render() {
    //维修等级
    let repairLevelOption = [];
    for (let item in RepairLevel) {
      if (
        this.repairTagKey == RepairTagKeys.RailwayWired ||
        this.repairTagKey == '' ||
        !this.repairTagKey ||
        (this.repairTagKey == RepairTagKeys.RailwayHighSpeed &&
          this.queryParams.planType != PlanType.Vertical) ||
        (this.repairTagKey == RepairTagKeys.RailwayHighSpeed &&
          this.queryParams.planType === PlanType.Vertical &&
          (RepairLevel[item] == RepairLevel.LevelI || RepairLevel[item] == RepairLevel.LevelII))
      )
        repairLevelOption.push(
          <a-select-option key={RepairLevel[item]}>
            {getRepairLevelTitle(RepairLevel[item])}
          </a-select-option>,
        );
    }
    //计划状态
    let planStateOption = [];
    for (let item in PlanState) {
      if (
        item !== 'Issued' &&
        item !== 'NotIssued' &&
        item !== 'UnDispatching' &&
        this.repairTagKey == RepairTagKeys.RailwayHighSpeed &&
        this.planType == PlanType.Vertical
      ) {
        planStateOption.push(
          <a-select-option key={PlanState[item]}>{getPlanState(PlanState[item])}</a-select-option>,
        );
      }
    }
    if (this.planType !== PlanType.Vertical || this.repairTagKey === RepairTagKeys.RailwayWired) {
      planStateOption.push(
        <a-select-option key={PlanState['Dispatching']}>
          {getPlanState(PlanState['Dispatching'])}
        </a-select-option>,
      );
      planStateOption.push(
        <a-select-option key={PlanState['UnDispatching']}>
          {getPlanState(PlanState['UnDispatching'])}
        </a-select-option>,
      );
      planStateOption.push(
        <a-select-option key={PlanState['Complete']}>
          {getPlanState(PlanState['Complete'])}
        </a-select-option>,
      );

      if (this.repairTagKey == RepairTagKeys.RailwayHighSpeed) {
        planStateOption.push(
          <a-select-option key={PlanState['OrderCancel']}>
            {getPlanState(PlanState['OrderCancel'])}
          </a-select-option>,
        );
        planStateOption.push(
          <a-select-option key={PlanState['NaturalDisasterCancel']}>
            {getPlanState(PlanState['NaturalDisasterCancel'])}
          </a-select-option>,
        );
        planStateOption.push(
          <a-select-option key={PlanState['OtherReasonCancel']}>
            {getPlanState(PlanState['OtherReasonCancel'])}
          </a-select-option>,
        );
      }
    }
    return (
      <div class="sm-cr-plan-vertical-skylight-plans">
        {/* 操作区 */}
        {
          <a-form form={this.form}>
            <sc-table-operator
              onSearch={() => {
                this.refresh();
              }}
              onReset={() => {
                this.queryParams = {
                  workUnit: this.currentOrganizationId,
                  workTime:
                    this.queryParams.planType === PlanType.Vertical
                      ? moment().add(1, 'months')
                      : moment().add(1, 'week'),
                  workSite: null,
                  railwayId: undefined,
                  station: undefined,
                  contentMileage: '',
                  planType: this.queryParams.planType,
                  repaireLevel: [],
                  dateRange: [moment(moment()).startOf('day'), moment(moment()).endOf('day')],
                };
                this.refresh();
              }}
              advancedCount={20}
            >
              <a-form-item label="作业单位">
                <OrganizationTreeSelect
                  ref="OrganizationTreeSelect"
                  axios={this.axios}
                  value={this.queryParams.workUnit}
                  autoInitial={true}
                  onInput={value => {
                    if (this.isInit) {
                      this.currentOrganizationId = value;
                    }
                    this.isInit = false;
                    this.queryParams.workUnit = value;
                    this.queryParams.station = undefined;
                    this.queryParams.railwayId = undefined;
                    this.refresh();
                  }}
                />
              </a-form-item>
              <a-form-item label="时间选择">
                {this.repairTagKey === RepairTagKeys.RailwayHighSpeed ? (
                  <a-range-picker
                    style="width: 100%"
                    allowClear={false}
                    placeholder={['计划开始时间', '计划结束时间']}
                    value={this.queryParams.dateRange}
                    onChange={value => {
                      this.queryParams.dateRange = value;
                      this.refresh();
                    }}
                  ></a-range-picker>
                ) : this.queryParams.planType == PlanType.Vertical ? (
                  <a-month-picker
                    style="width: 100%"
                    allowClear={false}
                    placeholder="请选择月份"
                    value={this.queryParams.workTime}
                    onChange={value => {
                      this.queryParams.workTime = value;
                      this.refresh();
                    }}
                  />
                ) : (
                  <a-week-picker
                    style="width: 100%"
                    placeholder="请选择周"
                    allowClear={false}
                    value={this.queryParams.workTime}
                    onChange={value => {
                      this.queryParams.workTime = value;
                      this.refresh();
                    }}
                  />
                )}
              </a-form-item>
              {this.repairTagKey == RepairTagKeys.RailwayHighSpeed &&
              this.planType == PlanType.OutOf ? null : (
                  <a-form-item label="维修等级">
                    <a-select
                      value={this.queryParams.repaireLevel}
                      allowClear
                      placeholder="请选择维修等级"
                      onChange={value => {
                        this.queryParams.repaireLevel = value;
                        // this.skylightPlanIds = [];
                        this.refresh();
                      }}
                    >
                      {repairLevelOption}
                    </a-select>
                  </a-form-item>
                )}
              <a-form-item label="线路">
                <SmBasicRailwayTreeSelect
                  organizationId={this.queryParams.workUnit}
                  axios={this.axios}
                  value={this.queryParams.railwayId}
                  onChange={value => {
                    this.queryParams.railwayId = value;
                    this.queryParams.station = undefined;
                    this.refresh();
                  }}
                />
              </a-form-item>
              <a-form-item label="车站(区间)">
                <StationCascader
                  axios={this.axios}
                  organizationId={this.queryParams.workUnit}
                  railwayId={this.queryParams.railwayId}
                  placeholder="请选择车站(区间)"
                  value={this.queryParams.station}
                  onChange={value => {
                    this.queryParams.station = value;
                    this.refresh();
                  }}
                />
              </a-form-item>

              <a-form-item label="站区(机房)">
                <SmBasicInstallationSiteSelect
                  // railwayRltStation={this.railwayRltStation}
                  axios={this.axios}
                  placeholder="请选择站区(机房)"
                  height={32}
                  value={this.queryParams.workSite}
                  onChange={value => {
                    this.queryParams.workSite = value;
                    this.refresh();
                  }}
                />
              </a-form-item>

              <a-form-item label="模糊查找">
                <a-input
                  placeholder="请输入作业内容、位置里程"
                  value={this.queryParams.contentMileage}
                  onInput={event => {
                    this.queryParams.contentMileage = event.target.value;
                    this.refresh();
                  }}
                />
              </a-form-item>
              {/* 添加计划状态过滤 */}
              {
                // this.repairTagKey === RepairTagKeys.RailwayHighSpeed ?
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
                // : null
              }

              <template slot="buttons">
                {vIf(
                  <a-button type="primary" onClick={this.add}>
                    添加
                  </a-button>,
                  vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Create),
                )}
                {/* {vIf(
              <a-button type="primary" onClick={this.publish}>
                发布
              </a-button>,
              vP(this.permissions, permissionsSmCrPlan.SkylightOutsidePlan.Release),
            )} */}
                {vIf(
                  this.repairTagKey == RepairTagKeys.RailwayHighSpeed &&
                    this.planType == PlanType.Vertical ? (
                      <a-button
                        type="primary"
                        onClick={this.generatePlan}
                        disabled={this.skylightPlanIds.length == 0}
                      >
                      提交审核
                      </a-button>
                    ) : (
                      ''
                    ),
                  vP(this.permissions, permissionsSmCrPlan.SkylightPlan.CreateRepairPlan),
                )}
              </template>
            </sc-table-operator>
          </a-form>
        }

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          dataSource={this.skyLightPlans}
          rowKey={record => record.id}
          loading={this.loading}
          pagination={false}
          rowSelection={
            this.repairTagKey === RepairTagKeys.RailwayHighSpeed &&
            this.planType == PlanType.Vertical
              ? {
                columnWidth: 30,
                selectedRowKeys: this.skylightPlanIds,
                onChange: (ids, selectRows) => {
                  // console.log(selectRows.length);
                  // console.log(ids.length);
                  this.queryParams.checkSkylightPlans = selectRows;
                  this.skylightPlanIds = ids;
                  console.log(this.queryParams.checkSkylightPlans);
                },
                getCheckboxProps: record => {
                  return {
                    props: {
                      defaultChecked: this.skylightPlanIds.includes(record.id),
                      disabled:
                          (record.planState !== PlanState.UnSubmited &&
                            record.planState !== PlanState.Backed &&
                            record.planState !== PlanState.Revoke &&
                            record.planState !== PlanState.UnAdopted) ||
                          (record.skylightPlanRltWorkTickets &&
                            record.skylightPlanRltWorkTickets.length == 0 &&
                            record.level.indexOf('1') > -1), //record.level.indexOf("1") > -1
                    },
                  };
                },
              }
              : null
          }
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return (this.pageIndex - 1) * this.queryParams.maxResultCount + (index + 1);
              },
              level: (text, record, index) => {
                let leavelList = record.level.split(',');
                let result = null;
                leavelList.map(item => {
                  if (!result) {
                    result = getRepairLevelTitle(parseInt(item));
                  } else {
                    result = result + '、' + getRepairLevelTitle(parseInt(item));
                  }
                });
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              railway: (text, record, index) => {
                let railway = record.railway ? record.railway.name : '';
                return (
                  <a-tooltip placement="topLeft" title={railway}>
                    <span>{railway}</span>
                  </a-tooltip>
                );
              },
              workTime: (text, record, index) => {
                return moment(text).format('YYYY-MM-DD HH:mm');
              },
              workContent: (text, record, index) => {
                return (
                  <a-tooltip placement="topLeft" title={record.workContent}>
                    <span>{record.workContent}</span>
                  </a-tooltip>
                );
              },
              incidence: (text, record, index) => {
                return (
                  <a-tooltip placement="topLeft" title={record.incidence}>
                    <span>{record.incidence}</span>
                  </a-tooltip>
                );
              },
              planState: (text, record, index) => {
                //record.planState === PlanState.UnDispatching && this.repairTagKey === RepairTagKeys.RailwayHighSpeed ?      <a-tag color={this.getTagColor(record.planState)}>
                return (
                  <a-tag color={this.getTagColor(record.planState)}>
                    {getPlanState(record.planState)}
                  </a-tag>
                );
              },
              operations: (text, record) => {
                return this.repairTagKey === RepairTagKeys.RailwayHighSpeed &&
                  this.planType == PlanType.Vertical ? (
                    <span>
                      {vIf(
                        <a
                          onClick={() => {
                            this.view(record);
                          }}
                        >
                        详情
                        </a>,
                        vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Detail),
                      )}
                      {vIf(
                        <a-divider type="vertical" />,
                        vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Detail) &&
                        vP(
                          this.permissions,
                          permissionsSmCrPlan.SkylightPlan.Update,
                          permissionsSmCrPlan.SkylightPlan.Dispatching,
                          permissionsSmCrPlan.SkylightPlan.Delete,
                        ),
                      )}
                      {vIf(
                        <a-dropdown trigger={['click']}>
                          <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>
                          更多 <a-icon type="down" />
                          </a>
                          <a-menu slot="overlay">
                            {record.planState == PlanState.Adopted
                              ? vIf(
                                <a-menu-item>
                                  <a
                                    onClick={() => {
                                      this.dispatch(record);
                                    }}
                                  >
                                    派工
                                  </a>
                                </a-menu-item>,
                                vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Dispatching),
                              )
                              : null}
                            {record.planState == PlanState.Waitting
                              ? vIf(
                                <a-menu-item>
                                  {
                                    <a
                                      onClick={() => {
                                        this.backout(record);
                                      }}
                                    >
                                      撤销
                                    </a>
                                  }
                                </a-menu-item>,
                                vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Revoke),
                              )
                              : null}
                            {record.planState == PlanState.UnSubmited ||
                          record.planState == PlanState.Revoke ||
                          record.planState == PlanState.Backed ||
                          record.planState == PlanState.UnAdopted
                              ? [
                                vIf(
                                  <a-menu-item>
                                    <a
                                      onClick={() => {
                                        this.isChange = false;
                                        this.edit(record);
                                      }}
                                    >
                                      编辑
                                    </a>
                                  </a-menu-item>,
                                  vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Update),
                                ),
                              ]
                              : null}
                            {record.planState == PlanState.Adopted ||
                          record.planState == PlanState.UnAdopted ||
                          record.planState == PlanState.Complete ||
                          record.planState == PlanState.Waitting
                              ? vIf(
                                <a-menu-item>
                                  <a
                                    onClick={() => {
                                      this.viewWorkFlow(record);
                                    }}
                                  >
                                    查看审批
                                  </a>
                                </a-menu-item>,
                                vP(this.permissions, permissionsSmCrPlan.SkylightPlan.ViewWorkFlow),
                              )
                              : null}
                            {record.planState == PlanState.Dispatching ||
                          record.planState == PlanState.Complete
                              ? vIf(
                                <a-menu-item>
                                  <a
                                    onClick={() => {
                                      this.viewDispatch(record);
                                    }}
                                  >
                                    派工详情
                                  </a>
                                </a-menu-item>,
                                vP(
                                  this.permissions,
                                  permissionsSmCrPlan.SkylightPlan.DispatchingView,
                                ),
                              )
                              : null}
                            {(record.planState == PlanState.Backed ||
                              record.planState == PlanState.UnAdopted) && record.opinion != null
                              ? vIf(
                                <a-menu-item>
                                  <a
                                    onClick={() => {
                                      this.viewOpinion(record);
                                    }}
                                  >
                                    退回原因
                                  </a>
                                </a-menu-item>,
                                vP(this.permissions, permissionsSmCrPlan.SkylightPlan.BackReason),
                              )
                              : null}
                            {record.planState == PlanState.UnSubmited ||
                          record.planState == PlanState.Revoke ||
                          record.planState == PlanState.Backed ||
                          record.planState == PlanState.UnAdopted
                              ? [
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
                                  vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Delete),
                                ),
                                vIf(
                                  <a-menu-item>
                                    <a
                                      onClick={() => {
                                        this.addWorkTicket(record);
                                      }}
                                    >
                                      添加工作票
                                    </a>
                                  </a-menu-item>,
                                  vP(
                                    this.permissions,
                                    permissionsSmCrPlan.SkylightPlan.AddWorkTicket,
                                  ),
                                ),
                              ]
                              : null}
                            {(record.planState == PlanState.OrderCancel ||
                            record.planState == PlanState.NaturalDisasterCancel ||
                            record.planState == PlanState.OtherReasonCancel) &&
                          !record.isChange
                              ? [
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
                                <a-menu-item>
                                  <a
                                    onClick={() => {
                                      this.isBackOpinion = false;
                                      this.viewOpinion(record);
                                    }}
                                  >
                                    取消原因
                                  </a>
                                </a-menu-item>,
                              ]
                              : undefined}
                            {record.isChange ? (
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.isBackOpinion = false;
                                    this.isChangeView = true;
                                    this.viewOpinion(record, true);
                                  }}
                                >
                                变更详情
                                </a>
                              </a-menu-item>
                            ) : (
                              undefined
                            )}
                          </a-menu>
                        </a-dropdown>,
                        vP(
                          this.permissions,
                          permissionsSmCrPlan.SkylightPlan.Update,
                          permissionsSmCrPlan.SkylightPlan.Dispatching,
                          permissionsSmCrPlan.SkylightPlan.Delete,
                        ),
                      )}
                    </span>
                  ) : (
                    <span>
                      {vIf(
                        <a
                          onClick={() => {
                            this.view(record);
                          }}
                        >
                        详情
                        </a>,
                        vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Detail),
                      )}

                      {record.planState === PlanState.UnDispatching
                        ? [
                          vIf(
                            <a-divider type="vertical" />,
                            vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Detail) &&
                              vP(
                                this.permissions,
                                permissionsSmCrPlan.SkylightPlan.Update,
                                permissionsSmCrPlan.SkylightPlan.Dispatching,
                                permissionsSmCrPlan.SkylightPlan.Delete,
                              ),
                          ),
                          vIf(
                            <a-dropdown trigger={['click']}>
                              <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>
                                更多 <a-icon type="down" />
                              </a>
                              <a-menu slot="overlay">
                                {vIf(
                                  <a-menu-item>
                                    <a
                                      onClick={() => {
                                        this.isChange = false;
                                        this.edit(record);
                                      }}
                                    >
                                      编辑
                                    </a>
                                  </a-menu-item>,
                                  vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Update),
                                )}
                                {vIf(
                                  <a-menu-item>
                                    <a
                                      onClick={() => {
                                        this.dispatch(record);
                                      }}
                                    >
                                      派工
                                    </a>
                                  </a-menu-item>,
                                  vP(
                                    this.permissions,
                                    permissionsSmCrPlan.SkylightPlan.Dispatching,
                                  ),
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
                                  vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Delete),
                                )}
                              </a-menu>
                            </a-dropdown>,
                            vP(
                              this.permissions,
                              permissionsSmCrPlan.SkylightPlan.Update,
                              permissionsSmCrPlan.SkylightPlan.Dispatching,
                              permissionsSmCrPlan.SkylightPlan.Delete,
                            ),
                          ),
                        ]
                        : record.planState === PlanState.Dispatching
                          ? [
                            vIf(
                              <a-divider type="vertical" />,
                              vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Detail) &&
                              vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Revoke),
                            ),
                            vIf(
                              <a
                                onClick={() => {
                                  this.backout(record);
                                }}
                              >
                              撤销
                              </a>,
                              vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Revoke),
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
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.isChange = true;
                                    this.edit(record);
                                  }}
                                >
                                  计划变更
                                </a>
                              </a-menu-item>
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.isBackOpinion = false;
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
                              this.isBackOpinion = false;
                              this.isChangeView = true;
                              this.viewOpinion(record, true);
                            }}
                          >
                            变更详情
                          </a>,
                        ]
                        : undefined}
                      {/* {record.planState === PlanState.OrderCancel ||
                    record.planState === PlanState.NaturalDisasterCancel ||
                    record.planState === PlanState.OtherReasonCancel
                        ? [
                          vIf(
                            <a-divider type="vertical" />,
                            vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Update),
                          ),
                          vIf(
                            <a
                              onClick={() => {
                                this.isChange = true;
                                this.edit(record);
                              }}
                            >
                              计划变更
                            </a>,
                            vP(this.permissions, permissionsSmCrPlan.SkylightPlan.Update),
                          ),
                        ]
                        : undefined} */}
                    </span>
                  );
              },
              expandedRowRender:
                this.repairTagKey === RepairTagKeys.RailwayHighSpeed &&
                this.planType == PlanType.Vertical
                  ? text => {
                    let config =
                        text.planState == PlanState.UnSubmited ||
                        text.planState == PlanState.Revoke ||
                        text.planState == PlanState.Backed ||
                        text.planState == PlanState.UnAdopted;
                    return (
                      <a-table
                        rowKey={record => record.id}
                        slot-scope="text"
                        columns={this.innerColumns}
                        dataSource={text.skylightPlanRltWorkTickets}
                        bordered={false}
                        pagination={false}
                        {...{
                          scopedSlots: {
                            // repairLevel: (index, record, text) => {
                            //   let level = record.repairLevel != null ? record.repairLevel.split(",").map(Number) : null;
                            //   let options = [];
                            //   if (level != null) {
                            //     level.forEach(element => {
                            //       switch (element) {
                            //       case 1:
                            //         options.push("天窗点内I级维修");
                            //         break;
                            //       case 2:
                            //         options.push("天窗点内II级维修");
                            //         break;
                            //       case 3:
                            //         options.push("天窗点外I级维修、");
                            //         break;
                            //       case 4:
                            //         options.push("天窗点外II级维修");
                            //         break;
                            //       default:
                            //         break;
                            //       }
                            //     });
                            //   }
                            //   return (
                            //     <a-tooltip placement="topLeft" title={options}>
                            //       <span>{options}</span>
                            //     </a-tooltip>
                            //   );
                            // },
                            operations: (index, record, text) => {
                              return [
                                <span>
                                  <a
                                    onClick={() => {
                                      this.viewWorkTicket(record);
                                    }}
                                  >
                                      详情
                                  </a>
                                  {config
                                    ? [
                                      <span>
                                        <a-divider type="vertical" />
                                        <a-dropdown trigger={['click']} hidden={this.isApply}>
                                          <a class="ant-dropdown-link" onClick={''}>
                                                更多 <a-icon type="down" />
                                          </a>
                                          <a-menu slot="overlay">
                                            <a-menu-item>
                                              <a
                                                onClick={() => {
                                                  this.editWorkTicket(record);
                                                }}
                                              >
                                                    编辑
                                              </a>
                                            </a-menu-item>
                                            <a-menu-item>
                                              <a
                                                onClick={() => {
                                                  this.removeWorkTicket(record);
                                                }}
                                              >
                                                    删除
                                              </a>
                                            </a-menu-item>
                                          </a-menu>
                                        </a-dropdown>
                                      </span>,
                                    ]
                                    : []}
                                </span>,
                              ];
                            },
                          },
                        }}
                      ></a-table>
                    );
                  }
                  : undefined,
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
        <SmCrPlanGenerateRepairPlanModal
          ref="SmCrPlanGenerateRepairPlanModal"
          axios={this.axios}
          repairTagKey={this.repairTagKey}
          organizationId={this.queryParams.workUnit}
          bordered={this.bordered}
          repairTagKey={this.repairTagKey}
          onSuccess={() => {
            this.skylightPlanIds = [];
            this.refresh(false);
          }}
        />

        {/* 派工模态框 */}
        <SmCrPlanPlanTodoModal
          ref="SmCrPlanPlanTodoModal"
          axios={this.axios}
          repairTagKey={this.repairTagKey}
          organizationId={this.queryParams.workUnit}
          workShop={true}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
        <SmCrPlanSkylightAddWorkTicketModal
          ref="SmCrPlanSkylightAddWorkTicketModal"
          axios={this.axios}
          workTicketId={this.workTicketId}
          skylightPlanId={this.skylightPlanId}
          onOk={value => {
            this.refresh(value);
          }}
        />
        {/* 退回原因 */}
        <a-modal
          visible={this.opinionVisible}
          title={this.isBackOpinion ? '退回原因' : this.isChangeView ? '变更详情' : '取消原因'}
          onCancel={() => (this.opinionVisible = false)}
          footer={null}
          destroyOnClose={true}
        >
          <a-form>
            <a-form-item
              label={this.isBackOpinion ? '退回原因' : this.isChangeView ? '变更详情' : '取消原因'}
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            >
              <a-input type="textarea" value={this.opinion} disabled></a-input>
            </a-form-item>
          </a-form>
        </a-modal>

        {/* 维修作业审批单模态框 */}
        <WorkflowModal
          ref="WorkflowModal"
          axios={this.axios}
          isInitial={false}
          // onSuccess={this.onSuccess}
        />
      </div>
    );
  },
};
