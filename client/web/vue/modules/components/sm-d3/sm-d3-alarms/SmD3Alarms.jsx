import './style';
import { requestIsSuccess, getAlarmLevel, getAlarmColor } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { AlarmLevel } from '../../_utils/enum';
import SmAlarms from '../../sm-alarm/sm-alarms';
import ApiAlarm from '../../sm-api/sm-alarm/Alarm';
import SmD3AlarmsAudio from './src/SmD3AlarmsAudio';
let apiAlarm = new ApiAlarm();

export default {
  name: 'SmD3Alarms',
  props: {
    axios: { type: Function, default: null },
    signalr: { type: Function, default: null },
    visible: { type: Boolean, default: false }, //面板是否弹出
    position: {
      type: Object,
      default: () => {
        return { left: '280px', bottom: '20px' };
      },
    },
    height: { type: String, default: '60%' },
    width: { type: String, default: '780px' },
    scopeCode: { type: String, default: null },
    systems: { type: Array, default: () => [] },
  },
  data() {
    return {
      iVisible: false,
      alarms: [],
      totalCount: 0,
      pageIndex: 1,
      systemActivedCode: null,
      queryParams: {
        stationId: null, // 车站Id
        railwayId: null, // 线路Id
        alarmTimeStart: null, // 告警时间起点
        alarmTimeEnd: null, // 告警时间止点
        systemCode: null, // 系统Id（系统编码Id）
        level: null, // 告警级别
        componentCategoryId: null, // 构件分类
        keywords: null, // 关键字
        maxResultCount: paginationConfig.defaultPageSize,
      },
    };
  },
  computed: {},
  watch: {
    visible: {
      handler: function(value, oldValue) {
        this.iVisible = value;
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiAlarm = new ApiAlarm(this.axios);
    },
    async refresh() {},
    getAlarmLevelNumber(level) {
      return this.alarms.filter(item => item.level === level).length;
    },
    getSystemAlarmCount(systemCode) {
      return this.alarms.filter(
        item => item.equipment.componentCategory.code.indexOf(systemCode) > -1,
      ).length;
    },
  },
  render() {
    return (
      <sc-panel
        class="sm-d3-alarms"
        bordered
        borderedRadius
        visible={this.iVisible}
        position={this.position}
        bodyFlex
        bodyFlexDirection="row"
        height={this.height}
        width={this.width}
        animate="bottomEnter"
        forceRender
        icon="alert"
        resizable
        onClose={visible => {
          this.iVisible = visible;
          this.$emit('close', this.iVisible);
        }}
      >
        <a-icon slot="icon" style="margin-bottom: -2px;" type="alert" />
        <span slot="title" title="集中告警" class="header-btn" class="monitor">
          <span class="title">集中告警（共 {this.alarms.length} 个）</span>
          <span class="monitor-item">
            <span
              class="level-dot"
              style={{ background: getAlarmColor(AlarmLevel.Emergency) }}
            ></span>
            紧急告警
            <span class="number" style={{ color: getAlarmColor(AlarmLevel.Emergency) }}>
              {this.getAlarmLevelNumber(AlarmLevel.Emergency)}
            </span>
          </span>
          <span class="monitor-item">
            <span
              class="level-dot"
              style={{ background: getAlarmColor(AlarmLevel.Important) }}
            ></span>
            重要告警
            <span class="number" style={{ color: getAlarmColor(AlarmLevel.Important) }}>
              {this.getAlarmLevelNumber(AlarmLevel.Important)}
            </span>
          </span>
          <span class="monitor-item">
            <span class="level-dot" style={{ background: getAlarmColor(AlarmLevel.Normal) }}></span>
            一般告警
            <span class="number" style={{ color: getAlarmColor(AlarmLevel.Normal) }}>
              {this.getAlarmLevelNumber(AlarmLevel.Normal)}
            </span>
          </span>
          <span class="monitor-item">
            <span
              class="level-dot"
              style={{ background: getAlarmColor(AlarmLevel.PreAlarm) }}
            ></span>
            预警告警
            <span class="number" style={{ color: getAlarmColor(AlarmLevel.PreAlarm) }}>
              {this.getAlarmLevelNumber(AlarmLevel.PreAlarm)}
            </span>
          </span>
        </span>

        <SmD3AlarmsAudio slot="headerExtraContent" alarmCount={this.alarms.length} />

        <div class="systems">
          {this.systems.map(item => {
            let count = this.getSystemAlarmCount(item.code);
            return (
              <div
                class={{
                  'system-item': true,
                  alarm: !!count,
                  actived: item.code === this.systemActivedCode,
                }}
                onClick={() => {
                  if (this.systemActivedCode === item.code) {
                    this.systemActivedCode = null;
                  } else {
                    this.systemActivedCode = item.code;
                  }
                }}
              >
                {item.name}
                {!!count && <span class="count">{`(${count})`}</span>}
              </div>
            );
          })}
        </div>
        <div class="alarms">
          {/* <div class="header" >
            {this.columns.map(item => <div key={item.dataIndex} class="alarm-col">{item.title}</div>)}
          </div>
          <div class="body" >
            {this.alarms.map(alarm => {
              return <div class="alarm-row" >
                {this.columns.map(item =>
                  <div key={item.dataIndex} class="alarm-cell">
                    {alarm[item.dataIndex]}
                  </div>)
                }
              </div>;
            })}
          </div> */}

          <SmAlarms
            ref="SmAlarms"
            systemCode={this.systemActivedCode}
            systems={this.systems}
            onChange={alarms => {
              this.alarms = alarms;
              this.$emit('alarmsChange', alarms);
            }}
            onEquipmentClick={equipment => {
              this.$emit('equipmentClick', equipment);
            }}
            axios={this.axios}
            signalr={this.signalr}
            isD3Mode
          />

          {/* <a-table columns={[
            {
              title: 'Name',
              dataIndex: 'name',
            },
            {
              title: 'Age',
              dataIndex: 'age',
            },
            {
              title: 'Address',
              dataIndex: 'address',
            },
          ]} data-source={[
            {
              key: '1',
              name: 'John Brown',
              age: 32,
              address: 'New York No. 1 Lake Park',
            },
            {
              key: '2',
              name: 'Jim Green',
              age: 42,
              address: 'London No. 1 Lake Park',
            },
            {
              key: '3',
              name: 'Joe Black',
              age: 32,
              address: 'Sidney No. 1 Lake Park',
            },
          ]} size="small" /> */}
        </div>
      </sc-panel>
    );
  },
};
