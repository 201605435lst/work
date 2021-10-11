import './style';
import { getWeek, requestIsSuccess } from '../../_utils/utils';
import ApiPlan from '../../sm-api/sm-construction/ApiPlan';
import dayjs from 'dayjs';
import ApiMasterPlan from '../../sm-api/sm-construction/ApiMasterPlan';

let apiPlan = new ApiPlan();
let apiMasterPlan = new ApiMasterPlan();

const quarters = [ // 季度列表
  { value: 1, name: '第一季度' },
  { value: 2, name: '第二季度' },
  { value: 3, name: '第三季度' },
  { value: 4, name: '第四季度' },
];
const months = [  //月份列表
  { value: 1, name: '1月' },
  { value: 2, name: '2月' },
  { value: 3, name: '3月' },
  { value: 4, name: '4月' },
  { value: 5, name: '5月' },
  { value: 6, name: '6月' },
  { value: 7, name: '7月' },
  { value: 8, name: '8月' },
  { value: 9, name: '9月' },
  { value: 10, name: '10月' },
  { value: 11, name: '11月' },
  { value: 12, name: '12月' },
];
export default {
  name: 'PlanCommonHeader', //施工计划/总体计划 公用 的头部
  props: {
    axios: { type: Function, default: null },
    plan: { type: Object, default: null },
    dateFilter: { type: Object, default: () => { } },
    yearList: { type: Array, default: () => [] },
    isUseByPlan: { type: Boolean, default: false }, //是否由 plan 调用
    isSelectName: { type: Boolean, default: false }, //是否选择名称
    selectedPlanId: { type: String, default: undefined }, //选择的计划/施工计划 id
    size: { type: String, default: 'default' }, //选择的计划/施工计划 id
  },
  data() {
    return {
      iDateFilter: { // 日期筛选
        year: undefined, //年份
        quarter: undefined, //季度
        month: undefined, //月份
        dayStart: undefined, //周数 开始 天
        dayEnd: undefined, //周数 结束 天
        weekIndex: undefined,// 周数索引(标记用)
      },
      plans: [], // 计划/施工计划 列表
      iSelectedPlanId: undefined, // 选择的 计划/施工计划 id

    };
  },
  computed: {
    weeks() {
      if (this.iDateFilter.year && this.iDateFilter.month) {
        return getWeek(this.iDateFilter.year, this.iDateFilter.month);
      }
      return [];
    },
    quarters() {
      if (this.iDateFilter.year) {
        return quarters;
      }
      return [];
    },
    months() {
      if (this.iDateFilter.year) {
        if (this.iDateFilter.quarter) {
          switch (this.iDateFilter.quarter) {
          case 1:
            return [{ value: 1, name: '1月' }, { value: 2, name: '2月' }, { value: 3, name: '3月' }];
          case 2:
            return [{ value: 4, name: '4月' }, { value: 5, name: '5月' }, { value: 6, name: '6月' }];
          case 3:
            return [{ value: 7, name: '7月' }, { value: 8, name: '8月' }, { value: 9, name: '9月' }];
          case 4:
            return [{ value: 10, name: '10月' }, { value: 11, name: '11月' }, { value: 12, name: '12月' }];
          }
        }
        return months;

      }
      return [];
    },
  },
  watch: {
    dateFilter: {
      handler: function (value, oldValue) {
        this.iDateFilter = value;
      },
      deep: true,
    },

    selectedPlanId: {
      handler: function (value, oldValue) {
        this.iSelectedPlanId = value;
      },
    },
  },
  async created() {
    this.initAxios();
    if (this.getMasterPlans) {
      this.isUseByPlan ? await this.getPlans() : await this.getMasterPlans();
    }
  },
  methods: {
    initAxios() {
      apiPlan = new ApiPlan(this.axios);
      apiMasterPlan = new ApiMasterPlan(this.axios);
    },
    // 获取 总体计划列表(审批过的)
    async getMasterPlans() {
      let res = await apiMasterPlan.getList({
        onlyPass: true, // 只查询的话就只能查 已审核的数据
        maxResultCount: 999, // 一页中最大显示多少
      });
      if (requestIsSuccess(res)) {
        this.plans = res.data.items;
      }
    },
    // 获取 施工计划列表(审批过的)
    async getPlans() {
      let res = await apiPlan.getList({
        onlyPass: true, // 只查询的话就只能查 已审核的数据
        maxResultCount: 999, // 一页中最大显示多少
      });
      if (requestIsSuccess(res)) {
        this.plans = res.data.items;

      }
    },
  },
  render() {
    return <div>
      <form >
        <a-row gutter={16}>
          <a-col xs={24} sm={12} md={8} lg={6} xl={6} >
            <a-form-item label='计划名称'
              label-col={{ span: 7 }}
              wrapper-col={{ span: 17 }}
            >
              {this.isSelectName ?
                <a-select
                  size={this.size}
                  style="width: 100%"
                  placeholder={'请选择计划'}
                  value={this.iSelectedPlanId}
                  onChange={(val) => {
                    this.iSelectedPlanId = val;
                    this.$emit('selectChange', val);
                  }}>
                  {this.plans.map(x => <a-select-option value={x.id}>{x.name}</a-select-option>)}
                </a-select>
                :
                <a-input value={this.plan.name} disabled={true} />
              }
            </a-form-item>
          </a-col>
          {/* {!this.isSelectName && <a-col xs={24} sm={12} md={8} lg={6} xl={6} >
            <a-form-item label='计划开始时间'
              label-col={{ span: 7 }}
              wrapper-col={{ span: 17 }}
            >
              <a-input
                style="width: 100%"
                size={this.size}
                value={this.plan.planStartTime ? dayjs(this.plan.planStartTime).format('YYYY-MM-DD') : null}
                disabled={true}
              />
            </a-form-item>
          </a-col>}
          {!this.isSelectName && <a-col xs={24} sm={12} md={8} lg={6} xl={6} >
            <a-form-item label='计划结束时间'
              label-col={{ span: 7 }}
              wrapper-col={{ span: 17 }}
            >
              <a-input
                style="width: 100%"
                size={this.size} value={this.plan.planEndTime ? dayjs(this.plan.planEndTime).format('YYYY-MM-DD') : null} disabled={true} />
            </a-form-item>
          </a-col>}
          {!this.isSelectName && <a-col xs={24} sm={12} md={8} lg={6} xl={6} >
            <a-form-item label='总工期'
              label-col={{ span: 7 }}
              wrapper-col={{ span: 17 }}
            >
              <a-input
                style="width: 100%"
                size={this.size} value={this.plan.period} disabled={true} />
            </a-form-item>
          </a-col>} */}

          <a-col xs={24} sm={12} md={8} lg={6} xl={6} >
            <a-form-item label='年份'
              label-col={{ span: 7 }}
              wrapper-col={{ span: 17 }}
            >
              <a-select
                placeholder={'请选择年份'}
                value={this.iDateFilter.year}
                style="width: 100%"
                size={this.size}
                onChange={(val) => {
                  this.iDateFilter.year = val;
                  this.iDateFilter.quarter = undefined;
                  this.iDateFilter.month = undefined;
                  this.iDateFilter.dayStart = undefined;
                  this.iDateFilter.dayEnd = undefined;
                  this.iDateFilter.weekIndex = undefined;
                  this.$emit('dateChange', this.iDateFilter);
                }}>
                {this.yearList.map(x => <a-select-option value={x}>{x}</a-select-option>)}
              </a-select>
            </a-form-item>
          </a-col>
          <a-col xs={24} sm={12} md={8} lg={6} xl={6} >
            <a-form-item label='季度'
              label-col={{ span: 7 }}
              wrapper-col={{ span: 17 }}
            >
              <a-select
                placeholder={'请选择季度'}
                value={this.iDateFilter.quarter}
                style="width: 100%"
                size={this.size}
                onChange={(val) => {
                  this.iDateFilter.quarter = val;
                  this.iDateFilter.month = undefined;
                  this.iDateFilter.dayStart = undefined;
                  this.iDateFilter.dayEnd = undefined;
                  this.iDateFilter.weekIndex = undefined;
                  this.$emit('dateChange', this.iDateFilter);
                }}>
                {this.quarters.map(x => <a-select-option value={x.value}>{x.name}</a-select-option>)}
              </a-select>
            </a-form-item>
          </a-col>
          <a-col xs={24} sm={12} md={8} lg={6} xl={6} >
            <a-form-item label='月份'
              label-col={{ span: 7 }}
              wrapper-col={{ span: 17 }}
            >
              <a-select
                style="width: 100%"
                size={this.size}
                placeholder={'请选择月份'}
                value={this.iDateFilter.month}
                onChange={(val) => {
                  this.iDateFilter.month = val;
                  this.iDateFilter.dayStart = undefined;
                  this.iDateFilter.dayEnd = undefined;
                  this.iDateFilter.weekIndex = undefined;
                  this.$emit('dateChange', this.iDateFilter);
                }}>
                {this.months.map(x => <a-select-option value={x.value}>{x.name}</a-select-option>)}
              </a-select>
            </a-form-item>
          </a-col>
          <a-col xs={24} sm={12} md={8} lg={6} xl={6} >
            <a-form-item label='周数'
              label-col={{ span: 7 }}
              wrapper-col={{ span: 17 }}
            >
              <a-select
                placeholder={'请选择周数'}
                value={this.iDateFilter.weekIndex}
                style="width: 100%"
                size={this.size}
                onChange={(val) => {
                  this.iDateFilter.dayStart = this.weeks[val].start;
                  this.iDateFilter.dayEnd = this.weeks[val].end;
                  this.iDateFilter.weekIndex = val;
                  this.$emit('dateChange', this.iDateFilter);
                }}>
                {this.weeks.map((x, index) => <a-select-option
                  value={index}>第{index + 1}周{this.iDateFilter.year}/{this.iDateFilter.month}/{x.start} ~{this.iDateFilter.year}/{this.iDateFilter.month}/{x.end}
                </a-select-option>)
                }
              </a-select>
            </a-form-item>
          </a-col>
        </a-row>
      </form>
    </div>;
  },
};
