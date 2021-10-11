import { pagination as paginationConfig } from '../../_utils/config';
import { requestIsSuccess } from '../../_utils/utils';
import { State, PageState } from '../../_utils/enum';
import moment from 'moment';
import ApiEmergFaults from '../../sm-api/sm-emerg/Fault';
import './style/index.less';
import SmEmergFaultsModal from '../../sm-emerg/sm-emerg-faults/SmEmergFaultsModal';


let apiEmergFaults = new ApiEmergFaults();

const FaultsRouterPath = '/components/sm-emerg-faults-cn';
const FaultRouterPath = '/components/sm-emerg-fault-cn';
const ProcessRouterPath = '/components/sm-emerg-plan-cn';

export default {
  name: 'SmD3EmergFaults',
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false }, //面板是否弹出
    height: { type: String, default: '60%' },
    width: { type: String, default: '780px' },
    position: {
      type: Object, default: () => {
        return { left: '280px', bottom: '20px' };
      },
    },
  },
  data() {
    return {
      faults: [], // 列表数据源
      iValue: null,
      iD3Visible: false, //3D故障应急集成面板是否弹出
      iVisible: false, //马上处理弹框是否弹出
      targetFile: null,
      equipment: null,//当前设备
      totalCount: 0,
      pageIndex: 1,
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        pendingAndUnchecked: 1000,  //查询未销记及待处理
        maxResultCount: paginationConfig.defaultPageSize,
      },
    };
  },

  computed: {
  },

  watch: {
    visible: {
      handler: function (value, oldValue) {
        this.iD3Visible = value;
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
    this.refresh();
  },

  mounted() { },

  methods: {
    initAxios() {
      apiEmergFaults = new ApiEmergFaults(this.axios);
    },

    //更多
    allView() {
      this.$router.push({
        path: FaultsRouterPath,
      });
      this.$emit('allView');
    },
    // 详细
    view(record) {
      this.$router.push({
        path: FaultRouterPath,
        query: {
          faultId: record.id,
          pageState: PageState.View,
        },
      });
      this.$emit('view', record.id);
    },
    //马上处理
    immediatelyDispose(record) {
      this.$refs.SmEmergFaultsModal;
      this.iVisible = true;
      this.iValue = record;
    },
    //处理流程
    process(record) {
      this.$router.push({
        path: ProcessRouterPath,
        query: {
          faultId: record.id,
          isApply: true,
        },
      });
      this.$emit('process', record.id);
    },

    //刷新列表
    async refresh() {
      this.loading = true;
      let response = await apiEmergFaults.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });

      if (requestIsSuccess(response) && response.data) {
        this.faults = response.data.items;
        this.totalCount = response.data.totalCount;
      }
      this.loading = false;
    },
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh();
      }
    },
  },

  render() {
    let symbol;
    let equipmentName;
    let content = [
      <div class="sm-emerg-fault">
        <div class="sm-emerg-fault-top">
          {
            this.faults.map(item => {
              equipmentName = item.equipmentNames.split(',');
              return (
                <div class="D3EmergFaultsData">
                  <a-popover placement="topLeft">
                    {
                      item.faultRltEquipments.map((items, index) => {
                        equipmentName = equipmentName.filter(name => name != items.equipment.name);
                        index == item.faultRltEquipments.length - 1 ? (equipmentName.length > 0 ? symbol = '、' : symbol = undefined) : symbol = '、';
                        return (
                          <a slot="content" onClick={() => this.$emit("flyTo", { groupName: items.equipment.group.name, name: items.equipment.name })} title={items.equipment.name}>{items.equipment.name}{symbol}</a>
                        );
                      })}
                    {
                      equipmentName.map((items, index) => {
                        index == equipmentName.length - 1 ? symbol = undefined : symbol = '、';
                        return (
                          <a slot="content" onClick={() => this.$emit("flyTo", { groupName: '', name: items })} title={items}>{items}{symbol}</a>
                        );
                      })
                    }
                    <span style={"width:37%"}>
                      {
                        item.faultRltEquipments.map((items, index) => {
                          equipmentName = equipmentName.filter(name => name != items.equipment.name);
                          index == item.faultRltEquipments.length - 1 ? (equipmentName.length > 0 ? symbol = '、' : symbol = undefined) : symbol = '、';
                          return (
                            <a onClick={() => this.$emit("flyTo", { groupName: items.equipment.group.name, name: items.equipment.name })} title={items.equipment.name}>{items.equipment.name}{symbol}</a>
                          );
                        })
                      }
                      {
                        equipmentName.map((items, index) => {
                          index == equipmentName.length - 1 ? symbol = undefined : symbol = '、';
                          return (
                            <a onClick={() => this.$emit("flyTo", { groupName: '', name: items })} title={items}>{items}{symbol}</a>
                          );
                        })
                      }
                    </span>

                  </a-popover>
                  <span title={item.station.name} style={"width:14%"}>{item.station.name}</span>
                  <span title={moment(item.checkInTime).format('YYYY-MM-DD HH:mm:ss')} style={"width:19%"}>{item.checkInTime ? moment(item.checkInTime).format('YYYY-MM-DD HH:mm:ss') : ''}</span>
                  <a title={"详情"} onClick={() => this.view(item)} style={"width:5%"}>详情</a>
                  <a
                    onClick={item.emergPlanRecordId != null ? () => this.process(item)
                      : () => this.immediatelyDispose(item)
                    }
                    title={(item.state != State.UnSubmitted && item.state != State.Submitted) ? (item.emergPlanRecordId != null ? '处理流程' : '马上处理') : undefined}
                    style={"width:9%"}
                  >
                    {(item.state != State.UnSubmitted && item.state != State.Submitted) ? (item.emergPlanRecordId != null ? '处理流程' : '马上处理') : undefined}
                  </a>
                </div>
              );
            })
          }
        </div>
      
        <a-pagination
          style="margin:3px 0px 11px 12px"
          size="small"
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          //onShowSizeChange={this.onPageChange}
          showTotal={paginationConfig.showTotal}
        />
      
        <SmEmergFaultsModal
          ref="SmEmergFaultsModal"
          axios={this.axios}
          visible={this.iVisible}
          value={this.iValue}
          onView={id => this.$emit('applyView', id)}
          onSuccess={() => {
            this.refresh(false);
          }}
          onChange={v => (this.iVisible = v)}
        />
      </div>,
    ];



    return (
      <sc-panel
        class='sm-d3-emerg-faults'
        visible={this.iD3Visible}
        title="故障应急"
        height={this.height}
        width={this.width}
        borderedRadius={true}
        position={this.position}
        animate="bottomEnter"
        bodyFlex
        // resizable
        bodyFlexDirection="row"
        onClose={visible => {
          this.iD3Visible = visible;
          this.$emit('close', this.iD3Visible);
        }}
      >
        <template slot="icon">
          <a-icon type="warning" />
        </template>

        <template slot="headerExtraContent">
          <span class="sm-d3-emerg-faults-header-right"
            title="更多"
            onClick={() => {
              this.allView();
            }}>
            更多
          </span>
        </template>

        {content && this.iD3Visible ? (
          content
        ) : (
          <span
            style="
              margin-top: 10px;
              color: #bdbdbd;"
          >
              无数据
          </span>
        )}
      </sc-panel>
    );
  },
};
