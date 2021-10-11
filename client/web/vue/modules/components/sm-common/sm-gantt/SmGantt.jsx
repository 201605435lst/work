import "./style";
import Highcharts from "highcharts/highstock";
import HighchartsGantt from "highcharts/modules/gantt";
import local_zh from "./highchart_zh";
HighchartsGantt(Highcharts);
Highcharts.setOptions(local_zh);
let today = new Date(),
  day = 1000 * 60 * 60 * 24,
  map = Highcharts.map,
  dateFormat = Highcharts.dateFormat,
  datas;
today.setUTCHours(0);
today.setUTCMinutes(0);
today.setUTCSeconds(0);
today.setUTCMilliseconds(0);
today = today.getTime();
export default {
  name: "SmGantt",
  components: {},
  props: {
    datas: { type: Array, default: () => [] },
    columns: {
      type: Array,
      default: () => [
        { title: "序号", field: "index" },
        { title: "专业", field: "major" },
        { title: "任务名称", field: "name" },
        { title: "开始时间", field: "start" },
        { title: "结束时间", field: "end" },
        { title: "进度", field: "completed" },
        { title: "状态", field: "state" },
      ],
    },
    gridCellHeight: { type: Number, default: 40 },
  },
  data() {
    return {
      gantt: null,
      ganttOptions: {},
      series: null,
    };
  },
  computed: {
    ganttColumns() {
      let _columns = [];
      _columns = this.columns.map((item) => {
        return {
          title: { text: `<div style="width:100%;height:100%;border:1px solid red">${item.title}</div>` },
          labels: {
            //format:`{point.${item.field}}`,
            formatter: function () {
              let point = this.point;
              if (item.field === "start" || item.field === "end") {
                return Highcharts.dateFormat("%Y-%m-%d", point[item.field]);
              } else if(item.field==='completed'){
                return `${(point[item.field])*100}%`;
              }
              else {
                return point[item.field];   //数据datas中的data格式错时会报： state is undefined
              }
            },
          },
        };
      });
      return _columns;
    },
  },
  watch: {
    datas: {
      handler(nVal, oVal) {
        this.series = nVal;
      },
      immediate: true,
    },
  },
  mounted() {
    this.buildGantt();
  },
  created() {
    this.createOptions();
  },
  methods: {
    // 实例化一个甘特图
    buildGantt() {
      if (!this.gantt) {
        this.gantt = Highcharts.ganttChart(
          this.$refs.container,
          this.ganttOptions,
        );
      }
    },
    createOptions() {
      let _this = this;
      this.ganttOptions = {
        credits:{
          enabled:false,
        },
        plotOptions: {
          // 进度条配置
          gantt: {
            colors: ["#82EA87"],
          },
        },
        xAxis: {
          type: 'datetime',
          tickPixelInterval:70,
          visible:false,
          units: [[
            'millisecond', // unit name
            [1, 2, 5, 10, 20, 25, 50, 100, 200, 500], // allowed multiples
          ], [
            'second',
            [1, 2, 5, 10, 15, 30],
          ], [
            'minute',
            [1, 2, 5, 10, 15, 30],
          ], [
            'hour',
            [1, 2, 3, 4, 6, 8, 12],
          ], [
            'day',
            [1],
          ], [
            'week',
            [1],
          ], [
            'month',
            [1, 3, 6],
          ], [
            'year',
            null,
          ]],   
        },
        yAxis: {
          type: "category",
          staticScale: this.gridCellHeight,
          grid: {
            borderWidth: 1,
            columns: this.ganttColumns,
          },
        },
        tooltip: {
          formatter: function (tooltipe) {
            return _this.tooltipFormatter(this);
          },
        },
        series: this.series,
        navigator: {
          enabled: true,
          series: {
            type: "gantt",
            pointPlacement: 0.5,
            pointPadding: 0.25,
          },
          yAxis: {
            min: 0,
            max: 3,
            reversed: true,
            categories: [],
          },
          height:20,
        },
        scrollbar: {
          enabled: false,
        },
      };
    },
    tooltipFormatter(data) {
      return `<b>${data.series.name}</b><br/><b>${
        data.key
      }</b><br/><span>开始时间：${Highcharts.dateFormat(
        "%Y-%m-%d",
        data.point.start,
      )}</span><br/><span>结束时间：${Highcharts.dateFormat(
        "%Y-%m-%d",
        data.point.end,
      )}</span>`;
    },
  },
  render() {
    return (
      <div class="scrolling-container">
        <div id="container" ref="container" class="left-container" />
      </div>
    );
  },
};
