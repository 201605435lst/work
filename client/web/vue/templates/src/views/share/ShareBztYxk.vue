<template>
  <div class="share-bzt">
    <div class="top">
      <div class="topType">
        <a-radio-group :value="type" size="small" @change="typeChange">
          <a-radio :value="1" class="radioFontSize">
            有线设备
          </a-radio>
          <a-radio :value="3" class="radioFontSize">
            无线设备
          </a-radio>
          <a-radio :value="2" class="radioFontSize">
            高铁设备
          </a-radio>
          <a-radio-group />
        </a-radio-group>
      </div>
    </div>
    <div id="charts" />
  </div>
</template>

<script>
import * as echarts from 'echarts';
import moment from 'moment';
export default {
  name: 'Share',
  data() {
    return {
      echarts: [],
      totalFinishData: [],
      time: moment().format('yyyy-MM-DD'),
      typeName: '',
      type: 1,
      orgName: null,
      repairTagKey: null,
    };
  },
  async mounted() {
    this.initAxios();
    this.initEvents();
  },
  methods: {
    async initAxios() {
      let data = {
        time: moment(this.time).format('yyyy-MM-DD'),
        type: 2,
        isLoginFree: true,
      };
      let api = '/api/app/crPlanStatistics/GetPieStatistical';
      let response = null;
      switch (this.type) {
        case 1:
          response = await this.axios({
            url: window.config.yxkPath + api,
            method: 'get',
            params: { ...data },
          });
          this.orgName = '有线设备';
          break;
        case 2:
          response = await this.axios({
            url: window.config.gtkPath + api,
            method: 'get',
            params: { ...data },
          });
          this.orgName = '高铁设备';
          break;
        case 3:
          response = await this.axios({
            url: window.config.wxkPath + api,
            method: 'get',
            params: { ...data },
          });
          this.orgName = '无线设备';
          break;
      }
      let finishCount;
      let alterCount;
      let unFinishCount;
      if (response.data && response != undefined && response.status === 200) {
        finishCount = response.data.finshedTotal;
        alterCount = response.data.changeTotal;
        unFinishCount = response.data.unFinshedTotal;

        //构造总体完成情况饼状图数据
        this.totalFinishData = [
          {
            value: finishCount.toFixed(2),
            name: '已完成',
            itemStyle: {
              color: '#91cc75',
            },
          },
          {
            value: unFinishCount.toFixed(2),
            name: '未完成',
            itemStyle: {
              color: '#ee6666',
            },
          },
        ];
        this.initCharts();
      }
    },
    initEvents() {
      window.onresize = e => {
        this.echarts.map(echart => {
          echart.resize();
        });
      };
    },
    initCharts() {
      let chartDom = document.getElementById('charts');
      let ensembleChart = echarts.init(chartDom);

      let data = this.totalFinishData;
      let option = {
        backgroundColor: '', //设置无背景色
        title: {
          // text: this.orgName + '-总体完成情况',
          left: 'center',
          top: 5,
          textStyle: {
            //文字颜色
            // color: '#ccc',
            //字体风格,'normal','italic','oblique'
            fontStyle: 'normal',
            //字体粗细 'normal','bold','bolder','lighter',100 | 200 | 300 | 400...
            fontWeight: 'bold',
            //字体系列
            fontFamily: 'sans-serif',
            //字体大小
            fontSize: 12,
          },
        },
        backgroundColor: '',
        tooltip: {
          trigger: 'item',
          formatter: '{b} <br/>占比 : {d}% ({c})',
        },
        legend: {
          data: ['已完成', '未完成'],
          selectedMode: true,
          textStyle: {
            fontSize: 10,
            color: 'white',
          },
          orient: 'vertical',
          left: 'left',
          formatter: function(name) {
            let index = 0;
            data.forEach(function(value, i) {
              if (value.name == name) {
                index = i;
              }
            });
            let totoal = (parseFloat(data[index].value) / (parseFloat(data[0].value) + parseFloat(data[1].value))) * 100;
            return name + ' ' + totoal.toFixed(2) + '%';
          },
        },
        series: [
          {
            name: '总体完成情况',
            type: 'pie',
            radius: '65%',
            center: ['50%', '50%'],
            // roseType: 'radius',
            label: {
              show: true,
              formatter: '{d}%',
            },
            emphasis: {
              label: {
                show: true,
              },
            },
            data: this.totalFinishData,
          },
        ],
      };
      ensembleChart.setOption(option);
      this.echarts.push(ensembleChart);
    },
    timeChange(value) {
      this.time = value;
      this.initAxios();
    },
    typeChange(data) {
      this.type = data.target.value;
      this.initAxios();
    },
  },
};
</script>
<style lang="less">
body {
  background: rgba(255, 255, 255, 0) !important;
}
.share-bzt {
  // top: 0;
  // left: 0;
  width: 100vw;
  height: 100vh;
  position: fixed;
  .top {
    display: flex;
    width: 90%;
    justify-content: center;
    margin-bottom: -5px;
    .topTime {
      margin-right: 20px;
    }
    .topType {
      line-height: 32px;
      text-align: center;
      .radioFontSize {
        font-size: 0.1vw;
        color: rgb(255, 255, 255);
      }
    }
  }
  #charts {
    width: 100%;
    height: 205px;
  }
}
</style>
