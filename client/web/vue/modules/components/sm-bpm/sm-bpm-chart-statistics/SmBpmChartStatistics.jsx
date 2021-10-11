import './style';
import moment from 'moment';
import { requestIsSuccess, addNum, subNum } from '../../_utils/utils';
import { WorkflowState, UserWorkflowGroup } from '../../_utils/enum';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiBpm from '../../sm-api/sm-bpm/Workflow';
import ApiCrPlan from '../../sm-api/sm-cr-plan/YearMonthPlan';

let apiBpm = new ApiBpm();
let apiCrPlan = new ApiCrPlan();
const echarts = require('echarts');
const barWidth = 30;
const stateColor = {
  approve: '#19CCD2',
  unsubmited: '#E9C090',
  refuse: '#DC143C',
  waitting: '#32CD32',
};
export default {
  name: 'SmBpmChartStatistics',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      queryParams: {
        time: moment(),
      },
      isShowFullScreen: false,
      isShowBackButton: false,
    };
  },
  computed: {
    isShow() {//是否显示年表与月表
      return moment(this.queryParams.time).month() === 0;
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiBpm = new ApiBpm(this.axios);
      apiCrPlan = new ApiCrPlan(this.axios);
    },
    refresh() {
      this.initChart;
      this.$nextTick(function() {
        this.getHistogram();
        this.getAllTotalPie();
      });
    },
    //统计整体流程情况：饼状图
    initChart() {
      this.getAllTotalPie();
    },
    async getAllTotalPie() {
      let data = {
        submitStartTime: moment(this.queryParams.time)
          .date(1)
          .hours(0)
          .minutes(0)
          .seconds(0)
          .format('YYYY-MM-DD HH:mm:ss'),
        submitEndTime: moment(this.queryParams.time)
          .endOf('month')
          .endOf('date')
          .format('YYYY-MM-DD HH:mm:ss'),
        group: UserWorkflowGroup.All,
        state: WorkflowState.All,
        maxResultCount: paginationConfig.defaultPageSize,
      };
      let response = await apiBpm.getTotal(data);
      let responseData;
      if (requestIsSuccess(response) && response.data) {
        responseData = response.data;
      }
      let pieChart;
      if (this.$refs.pieStatistics != undefined) {
        echarts.dispose(this.$refs.pieStatistics);
        pieChart = echarts.init(this.$refs.pieStatistics);
      }
      // 指定图表的配置项和数据
      let option = {
        title: {
          text: '流程审批月统计',
          left: 'center',
        },
        tooltip: {
          trigger: 'item',
        },
        legend: {
          orient: 'vertical',
          left: 'left',
        },
        series: [
          {
            name: '数量统计',
            type: 'pie',
            radius: '50%',
            data: [
              { value: responseData.approvedTotal, name: '我已审批' },
              { value: responseData.waitingTotal, name: '待我审批' },
              { value: responseData.ccTotal, name: '抄送我的' },
              { value: responseData.initialTotal, name: '我发起的' },
            ],
            emphasis: {
              itemStyle: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: 'rgba(0, 0, 0, 0.5)',
              },
            },
          },
        ],
      };
      // 使用刚指定的配置项和数据显示图表。
      if (pieChart != undefined) {
        pieChart.setOption(option);
      }
    },
    //统计年月表数据
    async getHistogram() {
      let data = { time: moment(this.queryParams.time).format('yyyy-MM-DD') };
      let response = await apiCrPlan.getTotal(data);

      let responseData;
      if (requestIsSuccess(response) && response.data) {
        responseData = response.data;
      }

      echarts.dispose(this.$refs.histogramStatistics);
      let histogramChart = echarts.init(this.$refs.histogramStatistics);

      let option = {
        tooltip: {
          trigger: 'axis',
          axisPointer: {
            type: 'shadow', // 'shadow' as default; can also be 'line' or 'shadow'
          },
        },
        legend: {
          data: ['通过', '未提报', '待审批'],
          selectedMode: false,
          top: 30,
        },
        title: {
          text: '年月表提报数据月统计',
          // left: 'center',
        },
        xAxis: {
          type: 'category',
          data: !this.isShow ? ['年表月度'] : ['年表', '月表', '年表月度'],
          axisLabel: {
            formatter: '{value}',
          },
        },
        yAxis: {
          type: 'value',
        },
        series: [
          {
            name: '通过',
            type: 'bar',
            stack: '总量',
            data: !this.isShow
              ? [responseData.approvedMonthOfYearTotal]
              : [
                responseData.approvedYearTotal,
                responseData.approvedMonthTotal,
                responseData.approvedMonthOfYearTotal,
              ],
            itemStyle: {
              normal: { color: stateColor.approve },
            },
            barWidth: barWidth, //柱图宽度
          },
          // {
          //   name: '拒绝',
          //   type: 'bar',
          //   stack: '总量',
          //   data: !this.isShow
          //     ? [responseData.refausedMonthOfYearTotal]
          //     : [
          //       responseData.refausedYearTotal,
          //       responseData.refausedMonthTotal,
          //       responseData.refausedMonthOfYearTotal,
          //     ],
          //   itemStyle: {
          //     normal: { color: stateColor.refuse },
          //   },
          //   barWidth: barWidth, //柱图宽度
          // },
          {
            name: '未提报',
            type: 'bar',
            stack: '总量',
            data: !this.isShow
              ? [responseData.unSubmitedMonthOfYearTotal]
              : [
                responseData.unSubmitedYearTotal,
                responseData.unSubmitedMonthTotal,
                responseData.unSubmitedMonthOfYearTotal,
              ],
            itemStyle: {
              normal: { color: stateColor.unsubmited },
            },
            barWidth: barWidth, //柱图宽度
          },
          {
            name: '待审批',
            type: 'bar',
            stack: '总量',
            data: !this.isShow
              ? [responseData.waittingMonthOfYearTotal]
              : [
                responseData.waittingYearTotal,
                responseData.waittingMonthTotal,
                responseData.waittingMonthOfYearTotal,
              ],
            itemStyle: {
              normal: { color: stateColor.waitting },
            },
            barWidth: barWidth, //柱图宽度
          },
        ],
      };
      histogramChart.setOption(option);
      let _this = this;
      histogramChart.on('click', 'series', function(params) {
        let organizationIds;
        let title = params.name + params.seriesName + '车间统计';
        let color = params.color;
        let subtitle = params.seriesName;
        switch (params.name) {
        case '年表':
          switch (params.seriesName) {
          case '通过':
            organizationIds = responseData.arrovedYearOrganizationIds;
            break;
          // case '拒绝':
          //   organizationIds = responseData.refausedYearOrganizationIds;
          //   break;
          case '未提报':
            organizationIds = responseData.unSubmitedYearOrganizationIds;
            break;
          case '待审批':
            organizationIds = responseData.waittingYearOrganizationIds;
            break;
          default:
            break;
          }
          break;
        case '月表':
          switch (params.seriesName) {
          case '通过':
            organizationIds = responseData.arrovedMonthOrganizationIds;
            break;
          // case '拒绝':
          //   organizationIds = responseData.refausedMonthOrganizationIds;
          //   break;
          case '未提报':
            organizationIds = responseData.unSubmitedMonthOrganizationIds;
            break;
          case '待审批':
            organizationIds = responseData.waittingMonthOrganizationIds;
            break;
          default:
            break;
          }
          break;
        case '年表月度':
          switch (params.seriesName) {
          case '通过':
            organizationIds = responseData.arrovedMonthOfYearOrganizationIds;

            break;
          // case '拒绝':
          //   organizationIds = responseData.refausedMonthOfYearOrganizationIds;
          //   break;
          case '未提报':
            organizationIds = responseData.unSubmitedMonthOfYearOrganizationIds;
            break;
          case '待审批':
            organizationIds = responseData.waittingMonthOfYearOrganizationIds;
            break;
          default:
            break;
          }
          break;
        default:
          break;
        }
        _this.getOrganizationStatistics(organizationIds, title, color, subtitle);
      });
    },
    //统计提报车间情况
    async getOrganizationStatistics(params, title, color, subtitle) {
      let organizationIds = [];
      let series = [];
      params.map(item => {
        organizationIds.push(item);
        series.push(1);
      });
      let data = {
        organizationIds: organizationIds,
      };
      let response = await apiCrPlan.getOrganizationNames(data);
      let responseData;
      if (requestIsSuccess(response) && response.data) {
        responseData = response.data;
      }
      this.isShowBackButton = true;
      echarts.dispose(this.$refs.histogramStatistics);
      let organizationStatistics = echarts.init(this.$refs.histogramStatistics);
      let option = {
        tooltip: {
          trigger: 'axis',
          axisPointer: {
            type: 'shadow', // 'shadow' as default; can also be 'line' or 'shadow'
          },
        },
        title: {
          text: title,
          left: 'center',
        },
        xAxis: {
          type: 'category',
          data: responseData,
          axisLabel: {
            interval: 0,
            rotate: 40,
          },
          nameTextStyle: {
            fontSize: 20,
            color: 'black',
          },
        },
        yAxis: {
          axisLabel: {
            show: false,
          },
        },
        dataZoom: [
          {
            type: 'slider',
            show: true,
            xAxisIndex: [0],
            left: '9%',
            start: 0,
            end: 70, //初始化滚动条
          },
        ],
        series: [
          {
            data: series,
            type: 'bar',
            color: color,
            barWidth: barWidth, //柱图宽度
          },
        ],
      };
      organizationStatistics.setOption(option);
    },
    //放大车间提报情况统计表
    showMax() {
      this.isShowFullScreen = !this.isShowFullScreen;
      if (this.isShowFullScreen) {
        this.$nextTick(function() {
          this.getHistogram();
        });
        if (this.$refs.pieStatistics != undefined) {
          echarts.dispose(this.$refs.pieStatistics);
        }
        // this.getHistogram();
        // echarts.dispose(this.$refs.pieStatistics);
      } else {
        this.$nextTick(function() {
          this.getAllTotalPie();
          this.getHistogram();
        });
      }
    },
  },
  render() {
    return (
      <div class="sm-bpm-chart-statistics">
        <a-row gutter={24} style="margin-bottom:20px;margin-left:-5px">
          <a-col sm={8} md={8}>
            <a-form-item
              label="时间"
              style="margin-bottom:0"
              label-col={{ span: 4 }}
              wrapper-col={{ span: 20 }}
            >
              <a-month-picker
                placeholder="请选择月份"
                allowClear={false}
                style="width: 100%;"
                value={moment(this.queryParams.time)}
                format="YYYY/MM"
                onChange={(date, dateString) => {
                  this.queryParams.time = dateString;
                  this.refresh();
                }}
              />
            </a-form-item>
          </a-col>
          <a-col sm={8} md={8}>
            <a-form-item style="margin-bottom:0" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
              <a-button type="primary" onClick={this.refresh}>
                查询
              </a-button>
              {/* <a-button onClick={this.queryParams.time} style="margin-left:10px">
                重置
              </a-button> */}
            </a-form-item>
          </a-col>
        </a-row>
        <div class="statisticsStyle">
          {this.isShowFullScreen ? null : <div ref="pieStatistics" class="pieStyle"></div>}
          <div
            ref="histogramStatistics"
            class={`${!this.isShowFullScreen ? 'histogramStyle' : 'histogramMaxStyle'}`}
          ></div>
          {this.isShowBackButton ? (
            <a-button
              style="margin-left:-56px;margin-top: 7px; "
              onClick={() => {
                this.getHistogram();
                this.isShowBackButton = false;
              }}
            >
              <a-icon type="rollback" />
            </a-button>
          ) : null}

          <a-button style="margin-left:-100px;margin-top: 7px; " onClick={this.showMax}>
            <a-icon type={`${!this.isShowFullScreen ? 'fullscreen' : 'fullscreen-exit'}`} />
          </a-button>
        </div>
      </div>
    );
  },
};
    