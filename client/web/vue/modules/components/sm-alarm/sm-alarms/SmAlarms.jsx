import './style';
import {
  requestIsSuccess,
  getAlarmLevel,
  getAlarmColor,
  convertKeysToLowerCase,
} from '../../_utils/utils';
import { AlarmLevel } from '../../_utils/enum';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmBasicRailwayTreeSelect from '../../sm-basic/sm-basic-railway-tree-select';
import SmBasicStationSelectByRailway from '../../sm-basic/sm-basic-station-select-by-railway';
import SmBasicInstallationSiteSelect from '../../sm-basic/sm-basic-installation-site-select';
import SmImport from '../../sm-import/sm-import-basic';
import moment from 'moment';
import ApiAlarm from '../../sm-api/sm-alarm/Alarm';
let apiAlarm = new ApiAlarm();

let timer;

export default {
  name: 'SmAlarms',
  props: {
    axios: { type: Function, default: null },
    scopeCode: { type: String, default: null },
    isD3Mode: { type: Boolean, default: false },
    systemCode: { type: String, default: null },
    signalr: { type: Function, default: null },
    systems: {
      type: Array,
      default: () => [],
    },
  },
  data() {
    return {
      alarms: [],
      totalCount: 0,
      pageIndex: 1,
      iSystemCode: null, // 系统Id（系统编码Id）
      queryParams: {
        stationId: null, // 车站Id
        railwayId: null, // 线路Id
        alarmTimeStart: null, // 告警时间起点
        alarmTimeEnd: null, // 告警时间止点
        level: null, // 告警级别
        componentCategoryId: null, // 构件分类
        keywords: null, // 关键字
        maxResultCount: paginationConfig.defaultPageSize,
      },
      importUrl: 'api/app/alarmAlarm/importEquipmentId',
      importKey: 'AlarmEquipmentId',
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: '50px',
          scopedSlots: { customRender: 'index' },
        },
        ...(this.isD3Mode
          ? []
          : [
            {
              title: '线路-站点',
              dataIndex: 'railway-station',
              scopedSlots: { customRender: 'railway-station' },
              ellipsis: true,
            },
          ]),
        {
          title: '系统',
          dataIndex: 'system',
          scopedSlots: { customRender: 'system' },
          width: '70px',
        },
        {
          title: '级别',
          dataIndex: 'level',
          scopedSlots: { customRender: 'level' },
          width: '70px',
        },
        {
          title: '告警码',
          dataIndex: 'code',
          width: '70px',
        },
        {
          title: '设备名称',
          dataIndex: 'equipment',
          scopedSlots: { customRender: 'equipment' },
          width: '100px',
          // ellipsis: true,
        },
        // {
        //   title: '告警名称',
        //   dataIndex: 'name',
        //   ellipsis: true,
        // },
        {
          title: '告警内容',
          dataIndex: 'content',
          // width: '140px',
          // ellipsis: true,
        },
        {
          title: '告警时间',
          dataIndex: 'activedTime',
          scopedSlots: { customRender: 'activedTime' },
          width: '100px',
        },
        // {
        //   title: '操作',
        //   dataIndex: 'operations',
        //   scopedSlots: { customRender: 'operations' },
        // },
      ];
    },
    filterAlarms() {
      let rst = this.alarms.filter(x => {
        if (
          this.queryParams.keywords &&
          (x.equipment.name.indexOf(this.queryParams.keywords) < 0 ||
            x.content.indexOf(this.queryParams.keywords) < 0 ||
            x.code.indexOf(this.queryParams.keywords) < 0)
        ) {
          return false;
        }

        if (
          this.queryParams.stationId &&
          this.queryParams.stationId != x.equipment.installationSite.stationId
        ) {
          return false;
        }

        if (
          this.queryParams.railwayId &&
          this.queryParams.railwayId != x.equipment.installationSite.railwayId
        ) {
          return false;
        }

        if (this.queryParams.level && this.queryParams.level != x.level) {
          return false;
        }

        if (this.iSystemCode && x.equipment.componentCategory.code.indexOf(this.iSystemCode) < 0) {
          return false;
        }

        return true;
      });

      return rst;
    },
  },
  watch: {
    systemCode: {
      handler: function(value, oldValue) {
        // this.queryParams.systemCode = value;
        this.iSystemCode = value;
        // this.refresh();
      },
      // immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.initSignalr();
  },
  destroyed() {
    clearInterval(this.timer);
  },
  methods: {
    initAxios() {
      apiAlarm = new ApiAlarm(this.axios);
    },
    initSignalr() {
      let AlarmSignalr = new this.signalr('alarm');
      AlarmSignalr.init(() => {});

      AlarmSignalr.on('Alarms', data => {
        let wrap = {
          data,
        };

        data = convertKeysToLowerCase(wrap).data;
        this.alarms = data;

        this.$nextTick(() => {
          this.$emit('change', this.filterAlarms);
        });
      });
    },
    async refresh() {
      let response = await apiAlarm.getList(this.queryParams);
      if (requestIsSuccess(response)) {
        this.alarms = response.data.items;
        this.totalCount = response.data.totalCount;
        this.$emit('change', this.alarms);
      }
    },

    getAlarmLevelNumber(level) {
      return this.filterAlarms.filter(item => item.level === level).length;
    },

    async importFileSelected(file) {
      // 构造导入参数（根据自己后台方法的实际参数进行构造）
      let importParamter = {
        'file.file': file,
        importKey: this.importKey,
      };
      // 执行文件上传
      await this.$refs.smImport.exect(importParamter);
    },
    importSuccess() {},
  },
  render() {
    let _alarmLevel = [];
    for (let item in AlarmLevel) {
      _alarmLevel.push(AlarmLevel[item]);
    }
    return (
      <div class="sm-alarms">
        {/* 操作区 */}
        {!this.isD3Mode && (
          <sc-table-operator
            onSearch={() => {
              // this.refresh();
            }}
            onReset={() => {
              this.queryParams.stationId = null;
              this.queryParams.railwayId = null;
              this.queryParams.alarmTimeStart = null;
              this.queryParams.alarmTimeEnd = null;
              this.iSystemCode = null;
              this.queryParams.level = null;
              this.queryParams.componentCategoryId = null;
              this.queryParams.keywords = null;
              // this.refresh();
            }}
          >
            <a-form-item label="关键字">
              <a-input
                placeholder={this.isD3Mode ? '请输入名称、编码' : '请输入名称、型号、厂家、编码'}
                value={this.queryParams.keywords}
                onInput={event => {
                  this.queryParams.keywords = event.target.value;
                }}
              />
            </a-form-item>

            <a-form-item label="线路 ">
              <SmBasicRailwayTreeSelect
                axios={this.axios}
                value={this.queryParams.railwayId}
                onInput={value => {
                  this.queryParams.railwayId = value;
                }}
              />
            </a-form-item>

            <a-form-item label="车站 ">
              <SmBasicStationSelectByRailway
                axios={this.axios}
                value={this.queryParams.stationId}
                onInput={value => {
                  this.queryParams.stationId = value;
                }}
              />
            </a-form-item>

            {/* <a-form-item label="安装位置">
            <SmBasicInstallationSiteSelect
              axios={this.axios}
              height={32}
              value={this.queryParams.installationSiteId}
              placeholder="请选择"
              onChange={value => {
                this.queryParams.installationSiteId = value;
                this.refresh();
              }}
            />
          </a-form-item> */}

            {/* <a-form-item label="告警时间">
            <a-range-picker show-time style="width:100%">
            </a-range-picker>
          </a-form-item> */}

            <a-form-item label="系统">
              <a-select
                value={this.iSystemCode}
                onChange={value => {
                  this.iSystemCode = value;
                  //  this.refresh();
                }}
              >
                {this.systems.map(item => (
                  <a-select-option value={item.code}>{item.name}</a-select-option>
                ))}
              </a-select>
            </a-form-item>

            <a-form-item label="告警级别">
              <a-select
                value={this.queryParams.level}
                onChange={value => {
                  this.queryParams.level = value;
                  //  this.refresh();
                }}
              >
                {_alarmLevel.map(item => (
                  <a-select-option value={item}>{getAlarmLevel(item)}</a-select-option>
                ))}
              </a-select>
            </a-form-item>

            {!this.isD3Mode && (
              <template slot="buttons">
                <div style={'display:flex'}>
                  <SmImport
                    ref="smImport"
                    url={this.importUrl}
                    axios={this.axios}
                    importKey={this.importKey}
                    downloadErrorFile={true}
                    ref="smImport"
                    onSelected={this.importFileSelected}
                    onSuccess={this.importSuccess}
                  />
                  <div class="monitor">
                    <span class="monitor-item">
                      {' '}
                      <span class="level-dot" style="background:#FF4D4F"></span>紧急告警{' '}
                      {this.getAlarmLevelNumber(AlarmLevel.Emergency)}
                    </span>
                    <span class="monitor-item">
                      {' '}
                      <span class="level-dot" style="background:#FF9100"></span>重要告警{' '}
                      {this.getAlarmLevelNumber(AlarmLevel.Important)}
                    </span>
                    <span class="monitor-item">
                      {' '}
                      <span class="level-dot" style="background:#F2D519"></span>一般告警{' '}
                      {this.getAlarmLevelNumber(AlarmLevel.Normal)}
                    </span>
                    <span class="monitor-item">
                      {' '}
                      <span class="level-dot" style="background:#5093FF"></span>预警告警{' '}
                      {this.getAlarmLevelNumber(AlarmLevel.PreAlarm)}
                    </span>
                  </div>
                </div>
              </template>
            )}
          </sc-table-operator>
        )}

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.filterAlarms}
          // bordered={this.bordered}
          // scroll={this.isD3Mode ? { y: 300 } : undefined}
          pagination={false}
          loading={this.loading}
          size={this.isD3Mode ? 'small' : 'default'}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              railway: (text, record, index) => {
                let result =
                  record.equipment &&
                  record.equipment.installationSite &&
                  record.equipment.installationSite.railway
                    ? record.equipment.installationSite.railway.name
                    : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              'railway-station': (text, record, index) => {
                let result =
                  record.equipment &&
                  record.equipment.installationSite &&
                  record.equipment.installationSite.railway
                    ? `${record.equipment.installationSite.railway.name} - ${record.equipment.installationSite.station.name}`
                    : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              station: (text, record, index) => {
                let result =
                  record.equipment &&
                  record.equipment.installationSite &&
                  record.equipment.installationSite.station
                    ? record.equipment.installationSite.station.name
                    : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              equipment: (text, record, index) => {
                let result = record.equipment ? record.equipment.name : '';
                return (
                  <span
                    class="equipment-btn"
                    type="link"
                    style={{
                      color: '#1890ff',
                      cursor: 'pointer',
                    }}
                    onClick={() => {
                      this.$emit('equipmentClick', record.equipment);
                    }}
                  >
                    {result}
                  </span>
                );
              },
              system: (text, record, index) => {
                let code =
                  (record.equipment &&
                    record.equipment.componentCategory &&
                    record.equipment.componentCategory.code) ||
                  null;
                if (code) {
                  let system = this.systems.find(x => code.indexOf(x.code) > -1);
                  if (system)
                    return (
                      <a-tooltip placement="topLeft" title={system.name}>
                        <span>{system.name}</span>
                      </a-tooltip>
                    );
                }
                return null;
              },
              level: (text, record, index) => {
                return (
                  <span class="row-level">
                    <span class="level-dot" style={{ background: getAlarmColor(text) }}></span>
                    {getAlarmLevel(text)}
                  </span>
                );
              },
              activedTime: (text, record) => {
                return moment(text).format('YYYY-MM-DD HH:mm:ss');
              },
              operations: (text, record) => {
                return [
                  <span>
                    {/* <a onClick={() => {
                      this.add(record);
                    }}>确认</a> */}

                    {/* <a-divider type="vertical" />,

                    <a-dropdown trigger={['click']}>
                      <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>     更多 <a-icon type="down" /> </a>
                      <a-menu slot="overlay">

                        <a-menu-item>
                          <a
                            onClick={() => {
                              this.view(record);
                            }}
                          >
                            详情
                          </a>
                        </a-menu-item>,

                        <a-menu-item>
                          <a
                            onClick={() => {
                              this.edit(record);
                            }}
                          >
                            编辑
                          </a>
                        </a-menu-item>,

                        <a-menu-item>
                          <a
                            onClick={() => {
                              this.remove(record);
                            }}
                          >
                            删除
                          </a>
                        </a-menu-item>

                      </a-menu>
                    </a-dropdown> */}
                  </span>,
                ];
              },
            },
          }}
        ></a-table>

        {/* 分页器 */}
        {/* <a-pagination
          style="margin-top:10px; text-align: right;"
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          // onChange={this.onPageChange}
          // onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          size={this.isD3Mode ? 'small' : ''}
          showTotal={paginationConfig.showTotal}
        /> */}
      </div>
    );
  },
};
