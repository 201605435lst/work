
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiContract from '../../sm-api/sm-costmanagement/Contract';
let echarts = require('echarts/lib/echarts');
require('echarts/lib/chart/line');


// 以下的组件按需引入
require('echarts/lib/component/tooltip'); // tooltip组件
require('echarts/lib/component/title'); //  title组件
require('echarts/lib/component/legendScroll'); // legend组件
require('echarts/lib/component/legend'); // legend组件
let apiContract = new ApiContract();

export default {
  name: 'SmCostmanagementBreakevenAnalysis',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource:[],
      totalExpenditure:[],
      contractAmount:[],
      styleType: 'line',
      organizationName: '', //当前登录用户的组织机构
      dates: [], //时间
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
      apiContract = new ApiContract(this.axios);
    },
    async refresh() {
      this.loading = true;
      let data = {
        
      };
      let response = await apiContract.getStatistics({
        ...data,
      });
      if (requestIsSuccess(response) && response.data) {
        this.dataSource = response.data;
        this.organizationName = this.dataSource.organizationName;
        this.dates = this.dataSource.dates;
        this.totalExpenditure = this.dataSource.totalExpenditure;
        this.contractAmount = this.dataSource.contractAmount;
      }
      this.loading = false;
    },
    async drawLine() {
      // vue文件中引入echarts工具
      let myChart = echarts.init(document.getElementById('myChart'));
      let option = {
        color: ['#fd8686', '#5af4f0'],
        title: {
          text: `${this.organizationName}`,
          subtext: '成本分析盈亏图',
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
          data: ['合同额', '总支出'],
        },
        xAxis: {
          type: 'category', // 还有其他的type，可以去官网喵两眼哦
          data: this.dates, // x轴数据
          name: '日期', // x轴名称
          // axisLabel: { interval: 0, rotate: 30 },
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
            name: '合同额',
            data: this.contractAmount,
            type: 'line',
            width: 2,
            smooth: true,
          },
          {
            name: '总支出',
            data: this.totalExpenditure,
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
      <div class="sm-costmanagement-breakeven-analysis">
        <a-card>
          <div class="myChart" id="myChart"></div>
        </a-card>
      </div>
    );
  },
};
