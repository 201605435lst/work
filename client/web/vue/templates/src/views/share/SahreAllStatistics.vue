<template>
  <div class="share_all_statistics" :style="{ 'background-image': `url(${background})` }">
    <div>
      <div class="topSpan" :style="{ 'background-image': `url(${nav})` }">
        <span>{{ statisticTitle }}</span>
        <!-- <div>
          <a><a-icon type="api" /></a>
        </div> -->
      </div>
      <div class="top">
        <span class="timeStyle">时间:</span>
        <a-month-picker :value="time" class="monthPicker" :allow-clear="false" @change="timeChange" />
        <a-radio-group :value="type" @change="typeChange">
          <a-radio :value="1">
            <span class="radioStyle">年表</span>
          </a-radio>
          <a-radio :value="2">
            <span class="radioStyle">月表</span>
          </a-radio>
        </a-radio-group>
      </div>
    </div>
    <div class="pie">
      <div id="pieYxk" ref="chart_4" class="top_content" />
      <div id="pieWxk" ref="chart_6" class="top_content" />
      <div id="pieGtk" ref="chart_5" class="top_content" />
    </div>
    <div class="histogram">
      <div v-if="isShowEquipmentCard" class="equipmentCard">
        <div class="equipmentCardButton">
          <a class="closeButton" @click="changeShowEquipmentCard">
            <a-icon type="close" />
          </a>
          <a><a-icon type="rollback" @click="back" /></a>
        </div>
        <div id="equipmentCharts" ref="equipmentCharts" />
      </div>
      <div class="link">
        <a :href="yxkLoginPath" target="_blank">>>></a>
      </div>
      <div
        id="histogramYxk"
        ref="chart_1"
        :class="[isShowEquipmentCard ? 'middleHistogram' : 'middleHistogramSpread']"
      />
      <div class="link">
        <a :href="wxkLoginPath" target="_blank">>>></a>
      </div>
      <div
        id="histogramWxk"
        ref="chart_2"
        :class="[isShowEquipmentCard ? 'middleHistogram' : 'middleHistogramSpread']"
      />
      <div class="link">
        <a :href="gtkLoginPath" target="_blank">>>></a>
      </div>
      <div
        id="histogramGtk"
        ref="chart_3"
        :class="[isShowEquipmentCard ? 'middleHistogram' : 'middleHistogramSpread']"
      />
    </div>
  </div>
</template>
<script>
import moment from 'moment';
import * as echarts from 'echarts';
import background from './img/bg1.png';
import nav from './img/dh.png';
const comonState = {
  finished: '#78e08f',
  unFinished: '#e55039',
  changed: '#f6b93b', //#e1a182、e9c090、f6efa6
  plan: '#8085e8',
  fontColor: '#52ffff',
  fontSize: 12,
  fontFamily: 'Microsoft Yahei',
};

const barWidth = 23;

const yxkPath = window.config.yxkPath;
const wxkPath = window.config.wxkPath;
const gtkPath = window.config.gtkPath;
export default {
  name: 'ShereAllStatistic',
  data() {
    return {
      background,
      type: 1,
      nav,
      time: moment().format('yyyy-MM-DD'),
      echarts: [],
      statisticTitle: window.config.statisticTitle,
      isShowEquipmentCard: false,
      transientEquipmentChartData: [],
      transientOrgName: null,
      name:null,
      yxkLoginPath: window.config.yxkLoginPath,
      wxkLoginPath: window.config.wxkLoginPath,
      gtkLoginPath: window.config.gtkLoginPath,
    };
  },
  async mounted() {
    await this.initData();
    this.initEvents();
  },
  methods: {
    async initData(isInitAll = true) {
      //车间总体完成情况
      let yxkOrganizationData = await this.initAxios(yxkPath, 1);
      let wxkOrganizationData = await this.initAxios(wxkPath, 1);
      let gtkOrganizationData = await this.initAxios(gtkPath, 1);

      if (yxkOrganizationData !== undefined && gtkOrganizationData !== undefined && wxkOrganizationData !== undefined) {
        if (isInitAll) {
          echarts.dispose(this.$refs.chart_4);
          echarts.dispose(this.$refs.chart_5);
          echarts.dispose(this.$refs.chart_6);
          this.initPieCharts('pieYxk', yxkOrganizationData, '有线设备');
          this.initPieCharts('pieWxk', wxkOrganizationData, '无线设备');
          this.initPieCharts('pieGtk', gtkOrganizationData, '高铁设备');

          echarts.dispose(this.$refs.chart_1);
          echarts.dispose(this.$refs.chart_2);
          echarts.dispose(this.$refs.chart_3);

          this.initHistogramCharts('histogramYxk', '有线设备', yxkOrganizationData);
          this.initHistogramCharts('histogramWxk', '无线设备', wxkOrganizationData);
          this.initHistogramCharts('histogramGtk', '高铁设备', gtkOrganizationData);
        } else {
          echarts.dispose(this.$refs.chart_1);
          echarts.dispose(this.$refs.chart_2);
          echarts.dispose(this.$refs.chart_3);

          this.initHistogramCharts('histogramYxk', '有线设备', yxkOrganizationData);
          this.initHistogramCharts('histogramWxk', '无线设备', wxkOrganizationData);
          this.initHistogramCharts('histogramGtk', '高铁设备', gtkOrganizationData);
        }
      }
      return true;
    },

    async initAxios(path, type, params) {
      let apiPath = path;
      let data = {};

      switch (type) {
        case 1:
          apiPath += '/api/app/crPlanStatistics/GetPieStatistical';
          data = {
            time: moment(this.time).format('yyyy-MM-DD'),
            type: this.type,
            isLoginFree: true,
          };
          break;
        case 2:
          apiPath += '/api/app/crPlanStatistics/GetEquipmentStatistics';
          data = params;
          break;
        case 3:
          apiPath += '/api/app/crPlanStatistics/GetRepairGroupFinishData';
          data = params;
          break;
        default:
          break;
      }
      let response = await this.axios({
        url: apiPath,
        method: 'get',
        params: data,
      });
      if (response.data && response != undefined && response.status === 200) {
        return response.data;
      }
    },

    //创建饼状图数据
    initPieCharts(ele, dataSource, title) {
      let chartDom = document.getElementById(ele);
      let ensembleChart = echarts.init(chartDom);

      let finishCount = dataSource.finshedTotal;
      let alterCount = dataSource.changeTotal;
      let unFinishCount = dataSource.unFinshedTotal;
      let data = [
        {
          value: finishCount.toFixed(2),
          name: '已完成',
          itemStyle: {
            color: comonState.finished,
          },
        },
        this.type == 1
          ? {
              value: alterCount.toFixed(2),
              name: '已变更',
              itemStyle: {
                color: comonState.changed,
              },
            }
          : null,
        {
          value: unFinishCount.toFixed(2),
          name: '未完成',
          itemStyle: {
            color: comonState.unFinished, //'#e1433b',
          },
        },
      ];

      let option = {
        title: {
          text: title + '总体完成情况',
          left: 'center',
          top: 5,
          textStyle: {
            fontSize: comonState.fontSize,
            fontFamily: comonState.fontFamily,
            color: comonState.fontColor,
          },
        },
        tooltip: {
          trigger: 'item',
          formatter: '{b} <br/>占比 : {d}% ({c})',
        },
        legend: {
          data: this.type == 1 ? ['已完成', '已变更', '未完成'] : ['已完成', '未完成'],
          selectedMode: true,
          top: 35,
          left: 'left',
          orient: 'vertical',
          textStyle: { color: comonState.fontColor, ...comonState },
        },
        series: [
          {
            name: '总体完成情况',
            type: 'pie',
            radius: '70%',
            center: ['50%', '50%'],
            roseType: 'radius',
            itemStyle: {
              borderRadius: 5,
            },
            top: 28,
            label: {
              show: false,
            },
            emphasis: {
              label: {
                show: true,
              },
            },
            data: data,
            itemStyle: {
              normal: {
                label: {
                  show: true,
                  formatter: '{b} : {c} ({d}%)',
                },
                labelLine: { show: true },
              },
            },
          },
        ],
      };
      ensembleChart.setOption(option);
      this.echarts.push(ensembleChart);
    },

    //创建柱状图数据
    initHistogramCharts(ele, org, dataSource) {
      let chartDom = document.getElementById(ele);
      let myChart = echarts.init(chartDom);
      let option;
      let orgNames = [];
      let finshedCount = [];
      let unFinshedCount = [];
      let alterCount = [];
      let style = this.chartStyle;
      let title = this.type === 1 ? '年表完成进度' : '月表完成进度';
      let timeTitle =
        moment(this.time).year() +
        '年' +
        moment(this.time)
          .add(1, 'M')
          .month() +
        '月';
      if (dataSource.histogramInfos.length == 0) {
        option = {
          title: {
            text: org + timeTitle + '-' + title,
            textStyle: {
              color: comonState.fontColor,
              ...comonState,
            },
            subtext: '无数据',
            subtextStyle: {
              fontSize: 14,
              color: 'white',
            },
          },
          top: 5,
          subtextStyle: {
            fontSize: 17,
          },
          legend: [],
          series: [],
        };
      } else {
        dataSource.histogramInfos.forEach(element => {
          orgNames.push(element.organizationName);
          finshedCount.push(element.finshedTotal);
          unFinshedCount.push(element.unFinshedTotal);
          alterCount.push(element.changeTotal);
        });
        option = {
          title: {
            text: org + timeTitle + '-' + title,
            top: 0,
            textStyle: {
              color: comonState.fontColor,
              ...comonState,
            },
          },
          dataType: 1,
          tooltip: {
            trigger: 'axis',
            axisPointer: {
              type: 'shadow',
            },
            formatter: '{b} <br /> {a0}: {c0}' + '%' + '<br />{a1}: {c1}' + '%' + ' <br /> {a2}: {c2}' + '%',
          },
          legend: {
            data: this.type == 1 ? ['完成', '变更', '未完成'] : ['完成', '未完成'],
            selectedMode: true,
            top: 30,
            textStyle: {
              color: '#f37b1d',
            },
          },
          grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true,
          },
          yAxis: {
            type: 'value',
            min: 0,
            max: 100,
            axisLabel: {
              formatter: '{value} %',
              color: 'white',
            },
          },
          xAxis: {
            type: 'category',
            axisLabel: { rotate: 30, fontSize: 11, color: 'white', interval: 0 },
            data: orgNames,
            splitLine: {
              show: true,
            },
            splitLine: { show: false }, //去除网格线
          },

          seriesData: [
            {
              value: 1,
              id: '2',
            },
          ],

          series: [
            {
              name: '完成',
              type: 'bar',
              stack: '总量',
              data: finshedCount,
              itemStyle: {
                normal: {
                  color: comonState.finished,
                },
              },
              barWidth: barWidth, //柱图宽度
            },
            {
              name: '变更',
              type: 'bar',
              stack: '总量',
              data: alterCount,
              itemStyle: {
                normal: { color: comonState.changed },
              },
              barWidth: barWidth, //柱图宽度
            },
            {
              name: '未完成',
              type: 'bar',
              stack: '总量',
              data: unFinshedCount,
              label: {
                position: 'top',
                show: true,
                position: 'insideLeft',
                formatter: function(params) {
                  let value = Math.abs((100 - params.value).toFixed(2));
                  return `${value}%`;
                },
              },
              itemStyle: {
                normal: {
                  color: comonState.unFinished,
                },
              },
              barWidth: barWidth, //柱图宽度
            },
          ],
        };
      }
      option && myChart.setOption(option);

      //  添加点击事件,进入设备统计;
      let _this = this;
      myChart.on('click', async params => {
        if (!this.isShowEquipmentCard) {
          _this.initData(false);
        }
        this.isShowEquipmentCard = true;

        let data = {
          orgId: dataSource.histogramInfos[params.dataIndex].orgId,
          time: moment(_this.time).format('yyyy-MM-DD'),
          type: this.type,
        };

        let res;
        if (org.indexOf('有线') > -1) {
          res = await _this.initAxios(yxkPath, 2, data);
        } else if (org.indexOf('无线') > -1) {
          res = await _this.initAxios(wxkPath, 2, data);
        } else {
          res = await _this.initAxios(gtkPath, 2, data);
        }

        _this.initEquipmentCharts(res, org, 1, params.name);
        //暂存数据
        this.transientEquipmentChartData = res;
        this.transientOrgName = org;
        this.name = params.name;
      });

      this.echarts.push(myChart);
    },

    //创建设备柱状图数据
    initEquipmentCharts(data, orgName, equipmentType, name, equipmentName) {
      let chartDom = document.getElementById('equipmentCharts');
      let equipmentChart = echarts.init(chartDom);
      let repairGroupNames = [];
      let finishData = [];
      let alterData = [];
      let unFinishData = [];

      data.forEach(element => {
        if (equipmentType == 1) {
          repairGroupNames.push(element.groupName);
          finishData.push(element.finshed);
          alterData.push(element.changed);
          unFinishData.push(element.unFinshed);
        } else {
          let singleUnfished = (100 - element.finishInfo.alterPercent - element.finishInfo.finishPercent).toFixed(2);
          repairGroupNames.push(element.name);
          finishData.push(element.finishInfo.finishPercent);
          alterData.push(element.finishInfo.alterPercent);
          unFinishData.push(singleUnfished);
        }
      });

      let _this = this;
      let option = {
        title: {
          text:
            equipmentType === 1
              ? '各项设备完成统计(' + orgName + '-' + name + ')'
              : '各项设备完成统计(' + orgName + '-' + name + '-'+equipmentName + ')',
          top: 5,
          textStyle: {
            color: comonState.fontColor,
            ...comonState,
          },
        },
        tooltip: {
          trigger: 'axis',
          axisPointer: {
            type: 'shadow',
          },
          formatter: '{b} <br /> {a0}: {c0}' + '%' + '<br />{a1}: {c1}' + '%' + ' <br /> {a2}: {c2}' + '%',
        },
        // legend: {
        //   data: ['完成', '变更'],
        //   selectedMode: false,
        //   top: 30,
        //   textStyle: {
        //     color: comonState.fontColor,
        //   },
        // },

        grid: {
          left: '25%',
        },
        xAxis: {
          type: 'value',
          min: 0,
          max: 100,
          axisLabel: {
            formatter: '{value} %',
            color: 'white',
          },
        },
        yAxis: {
          type: 'category',
          axisLabel: { rotate: 55, color: 'white' },
          data: repairGroupNames,
          splitLine: {
            show: false,
          },
        },
        dataZoom: [
          //给y轴设置滚动条
          {
            // start: data.length-1, //默认为0
            // end: 100 - 1500 / 31, //默认为100
            start: 0,
            end: 8,
            maxValueSpan: 7,
            minValueSpan: 7,
            type: 'slider',
            show: true,
            yAxisIndex: [0],
            handleSize: 0, //滑动条的 左右2个滑动条的大小
            height: '80%', //组件高度
            left: 650, //左边的距离
            right: 15, //右边的距离
            top: 50, //右边的距离
            borderColor: 'rgba(43,48,67,.8)',
            fillerColor: '#33384b',
            backgroundColor: 'rgba(43,48,67,.8)', //两边未选中的滑动条区域的颜色
            showDataShadow: false, //是否显示数据阴影 默认auto
            showDetail: false, //即拖拽时候是否显示详细数值信息 默认true
            realtime: true, //是否实时更新
            // filterMode: 'filter',
            // yAxisIndex: [0, 1], //控制的 y轴
          },
          // 下面这个属性是里面拖到
          {
            type: 'inside',
            yAxisIndex: [0],
            start: 8,
            end: 0,
          },
        ],
        seriesData: [
          {
            value: 1,
            id: '2',
          },
        ],
        series: [
          {
            name: '完成',
            type: 'bar',
            stack: '总量',
            data: finishData,
            itemStyle: {
              normal: { color: comonState.finished },
            },
            barWidth: barWidth, //柱图宽度
          },
          {
            name: '变更',
            type: 'bar',
            stack: '总量',
            data: alterData,
            itemStyle: {
              normal: { color: comonState.changed },
            },
            barWidth: barWidth, //柱图宽度
          },
          {
            name: '未完成',
            type: 'bar',
            stack: '总量',
            data: unFinishData,
            label: {
              show: true,
              position: 'insideLeft',
              color: 'white',
              formatter: function(params) {
                let value = Math.abs((100 - params.value).toFixed(2));
                return `${value}%`;
              },
            },
            itemStyle: {
              normal: { color: comonState.unFinished },
            },
            barWidth: barWidth, //柱图宽度
          },
        ],
      };
      equipmentChart.setOption(option);
      this.echarts.push(equipmentChart);

      //添加点击事件，选择单项设备
      if (equipmentType == 1) {
        equipmentChart.on('click', async params => {
          console.log(params);
          console.log(data);
          let info = {
            groupName: params.name,
            orgId: data[params.dataIndex].orgizationId,
            time: moment(_this.time).format('yyyy-MM-DD'),
            isLoginFree: true,
            type: _this.type,
          };
          //
          let res;
          echarts.dispose(this.$refs.equipmentCharts);
          if (orgName.indexOf('有线') > -1) {
            info['RepairTagKey'] = 'RepairTag.RailwayWired';
            res = await _this.initAxios(yxkPath, 3, info);
          } else if (orgName.indexOf('无线') > -1) {
            info['RepairTagKey'] = 'RepairTag.RailwayWired';
            res = await _this.initAxios(wxkPath, 3, info);
          } else {
            info['RepairTagKey'] = 'RepairTag.RailwayHighSpeed';
            res = await _this.initAxios(gtkPath, 3, info);
          }

          if (res && res !== undefined) {
            _this.initEquipmentCharts(res, orgName, 2, name,params.name);
          }
        });
      }
    },
    //自适应
    initEvents() {
      window.onresize = e => {
        this.echarts.map(echart => {
          echart.resize();
        });
      };
    },

    typeChange(data) {
      this.type = data.target.value;
      this.initData();
      this.isShowEquipmentCard = false;
    },

    timeChange(data) {
      this.time = moment(data);
      this.initData();
      this.isShowEquipmentCard = false;
    },

    changeShowEquipmentCard() {
      this.isShowEquipmentCard = false;
      this.initData(false);
    },
    back() {
      console.log(this.transientEquipmentChartData, this.transientOrgName);
      this.initEquipmentCharts(this.transientEquipmentChartData, this.transientOrgName, 1,this.name);
    },
  },
};
</script>
<style lang="less">
#fontStyle() {
  .style {
    font-family: 'Microsoft Yahei', Arial, sans-serif;
    // font-size: 36px;
    color: rgb(255, 255, 255);
    text-shadow: rgb(255 255 255) 0px 0px 15px;
    font-weight: bold;
  }
}
.share_all_statistics {
  width: 100vw;
  height: 100vh;
  color: white;

  background-size: 100% 100%;
  background-repeat: no-repeat;
  position: fixed;
  .fontStyle {
    color: white;
  }
  .topSpan {
    text-align: center;
    margin-bottom: 10px;
    // /* margin-right: 6px; */
    // font-family: 'Microsoft Yahei', Arial, sans-serif;
    // color: #ffffff;
    // text-shadow: rgb(255 255 255) 1px 0px 1px;
    // font-weight: bold;
    // font-size: 39px;
    // width: 100px;
    color: #52ffff;
    cursor: pointer;
    // font-size: 14px;
    line-height: 60px;
    text-align: center;
    font-size: 30px;
  }
  .top {
    height: 10%;
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    margin-bottom: 5px;
    .timeStyle {
      margin-right: 5px;
      #fontStyle.style() // justify-content: center;
        // text-align: center;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
    }
    .radioStyle {
      #fontStyle.style();
    }
    .ant-calendar-picker-input {
      background: none;
      color: white;
      text-align: center;
      border-radius: 50px;
    }
    & > * {
      margin-right: 60px;
    }
  }
  .pie {
    height: 20%;
    width: 100%;
    display: flex;
    justify-content: space-around;
    // .top_content{
    //     width: 30%;
    //     margin-right: 20px;
    // }
    & > * {
      // background: rgb(14, 226, 216);
      width: 30%;
      justify-content: center;
      align-items: center;
      display: flex;
    }
  }
  .histogram {
    height: 70%;
    .equipmentCard {
      margin-right: 20px;
      width: 25%;
      height: 98%;
      float: right;
      background: #2c477b59;
      .equipmentCardButton {
        width: 100%;
        height: 4%;
        display: flex;
        justify-content: flex-start;
        font-size: 18px;
        .closeButton {
          margin-right: 15px;
          margin-left: 15px;
        }
      }
      #equipmentCharts {
        width: 100%;
        height: 96%;
      }
    }

    .link {
      width: 10%;
      /* background: red; */
      display: flex;
      justify-content: flex-end;
    }
    .middleHistogram {
      height: 30%;
      width: 75%;
    }
    .middleHistogramSpread {
      height: 30%;
      width: 100%;
    }
    // .middle_wxk {
    //   height: 33%;
    //   width: 76%;
    // }
    // .middle_gtk {
    //   height: 34%;
    //   width: 76%;
    // }
  }
}
</style>
