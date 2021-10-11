
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import ApiMoneyList from '../../sm-api/sm-costmanagement/MoneyList';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
let echarts = require('echarts/lib/echarts');
require('echarts/lib/chart/line');


// 以下的组件按需引入
require('echarts/lib/component/tooltip'); // tooltip组件
require('echarts/lib/component/title'); //  title组件
require('echarts/lib/component/legendScroll'); // legend组件
require('echarts/lib/component/legend'); // legend组件
import moment from 'moment';
import './style';


let apiMoneyList = new ApiMoneyList();

export default {
  name: 'SmCostmanagementCapitalReport',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource: [],
      loading: false,
      styleType: 'line',
      organizationName: '', //当前登录用户的组织机构
      dates: [], //时间
      dues: [], //应付
      paids: [], //已付
      receivables: [], //应收
      receiveds: [], //已收
      queryParams: {
        startTime: null,
        endTime: null,
      },
    };
  },
  computed: {},
  async created() {
    this.initAxios();
  },
  beforeMount() {},
  async mounted() {
    await this.refresh();
    this.drawLine();
  },
  methods: {
    initAxios() {
      apiMoneyList = new ApiMoneyList(this.axios);
    },
    async refresh() {
      this.loading = true;
      let data = {
        startTime: this.queryParams.startTime
          ? moment(this.queryParams.startTime)
            .format('YYYY-MM')
          : '',
        endTime: this.queryParams.endTime
          ? moment(this.queryParams.endTime)
            .add(1, 'months')
            .format('YYYY-MM')
          : '',
      };
      let response = await apiMoneyList.getStatistics({
        ...data,
      });
      if (requestIsSuccess(response) && response.data) {
        this.dataSource = response.data;
        this.organizationName = this.dataSource.organizationName;
        this.dates = this.dataSource.dates;
        this.dues = this.dataSource.dues;
        this.paids = this.dataSource.paids;
        this.receivables = this.dataSource.receivables;
        this.receiveds = this.dataSource.receiveds;
      }
      this.loading = false;
    },
    // async organization(){
    //   console.log("2");

    //   let response = await apiMoneyList.getOrganization();
    //   if (requestIsSuccess(response) && response.data) {
    //     this.organizationName = response.data.organizationName;
    //   }
    // },
    async drawLine() {
      // vue文件中引入echarts工具
      let myChart = echarts.init(document.getElementById('myChart'));
      let option = {
        color: ['#fd8686', '#d37c11', '#5af4f0', '#ffd66f'],
        title: {
          text: `${this.organizationName}`,
          subtext: '成本资金统计图',
          left: 'center',
          textStyle: {
            fontSize: 20,
          },
          subtextStyle: {
            fontSize: 15,
          },
        },
        legend: {
          type: 'scroll',
          orient: 'vertical',
          right: 10,
          top: 'middle',
          bottom: 20,
          data: ['应付', '已付', '应收', '已收'],
        },
        xAxis: {
          type: 'category', // 还有其他的type，可以去官网喵两眼哦
          data: this.dates, // x轴数据
          name: '日期', // x轴名称
      
          boundaryGap: false,
          // x轴名称样式
          nameTextStyle: {
            fontWeight: 400,
            fontSize: 16,
          },
        },
        yAxis: {
          type: 'value',
          name: '万元', // y轴名称
          // y轴名称样式
          nameTextStyle: {
            fontWeight: 400,
            fontSize: 16,
          },
        },
        label: {},
        tooltip: {
          trigger: 'axis', // axis   item   none三个值
        },
        toolbox: {
          show: true,
          feature: {
            magicType: {
              type: ['line', 'bar'],
            },
            saveAsImage: { show: true },
           
          },
        },
        series: [
          {
            name: '应付',
            data: this.dues,
            type: 'line',
            width: 2,
            smooth: true,
          },
          {
            name: '已付',
            data: this.paids,
            type: 'line',
            width: 2,
            smooth: true,
          },
          {
            name: '应收',
            data: this.receivables,
            type: 'line',
            width: 2,
            smooth: true,
          },
          {
            name: '已收',
            data: this.receiveds,
            type: 'line',
            width: 2,
            smooth: true,
          },
        ],
      };
      myChart.setOption(option);
    },
  },
  render() {
    return (
      <div class="sm-costmanagement-capital-report">
        <sc-table-operator
          onSearch={async () => {
            await this.refresh();
            await this.drawLine();
          }}
          onReset={async () => {
            this.queryParams.endTime = null;
            this.queryParams.startTime = null;
            await this.refresh();
            await this.drawLine();
          }}
        >
          <div class="costmanagement-capital-report-date">
            <a-form-item label="时间">
              <a-month-picker
                style="width: 100%;"
                placeholder="开始时间"
                allowClear={false}
                value={this.queryParams.startTime}
                onChange={async value => {
                  this.queryParams.startTime = value;
                  await this.refresh();
                  await this.drawLine();
                }}
              />
              <span class="month-picker_">-</span>
              <a-month-picker
                style="width: 100%;"
                allowClear={false}
                value={this.queryParams.endTime}
                onChange={async value => {
                  this.queryParams.endTime = value;
                  await this.refresh();
                  await this.drawLine();
                }}
                placeholder="结束时间"
              />
            </a-form-item>
          </div>
        </sc-table-operator>
        <a-card>
          <div class="myChart" id="myChart"></div>
        </a-card>
      </div>
    );
  },
};
