import ApiCableLocation from '../../sm-api/sm-resource/CableLocation';
import { requestIsSuccess } from '../../_utils/utils';
import { CableLocationDirection, CameraFlyState } from '../../_utils/enum';
import { tips as tipsConfig } from '../../_utils/config';
import { cameraFlyByPoints } from '../sm-d3/src/utils/cesium-utils';
import d3 from '../sm-d3/src/d3Manager';

import ApiEquipment from '../../sm-api/sm-resource/Equipments';
let apiEquipment = new ApiEquipment();
import './style/index.less';

export default {
  name: 'SmD3EquipmentCamera',
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false }, //面板是否弹出
    position: {
      type: Object, default: () => {
        return { left: '280px', bottom: '20px' };
      },
    },
    height: { type: String, default: '60%' },
    width: { type: String, default: '780px' },
    equipment: { type: Object, default: null },
    hiddenModelPairStrings: { type: Array, default: () => [] },
    opacityModelPairStrings: { type: Array, default: () => [] },
    viewPoints: { type: Array, default: () => [] },
  },
  data() {
    return {
      iVisible: false,
      iEquipment: null,
      flyState: CameraFlyState.Stoped,
      flyIndex: 0,
    };
  },

  computed: {
    title: function () {
      return `（${this.iEquipment ? '设备：' + this.iEquipment.name : '请选择设备'}）`;
    },
  },

  watch: {
    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
      },
      immediate: true,
    },
    value: {
      handler: function (value, oldValue) {
        if (value) {
          this.refresh();
        }
      },
      // immediate: true,
    },
    equipment: {
      handler: function (value, oldValue) {
        this.iEquipment = this.iEquipment || this.equipment;
      },
    },
    viewPoints: {
      handler: function (value, oldValue) {
        this.flyState = CameraFlyState.Stoped;
        this.flyIndex = 0;
      },
    },
  },

  async created() {
    this.initAxios();
    this.refresh();
  },

  methods: {
    initAxios() {
      apiEquipment = new ApiEquipment(this.axios);
    },
    async refresh() {
      this.iEquipment = this.equipment;
      if (this.iEquipment) {
        this.$emit('refresh');
      }
    },
  },

  render() {
    let content = [
      <sc-panel
        title="隐藏设备"
        flex="1"
        bordered={false}
        showHeaderClose={false}
        style="border-right: 1px solid #d0d0d0;"
      >
        {this.hiddenModelPairStrings.map(item => {
          return (
            <div class="equipment-item">
              <div class="name" >{item}</div>
              <div class="btns">
                <span class="btn" title="定位"
                  onClick={() => {
                    let [groupName, name] = item.split('@');
                    this.$emit('flyTo', { groupName, name });
                  }}
                ><a-icon type="environment" /></span>
                <span
                  class="btn"
                  title="移除"
                  onClick={() => { this.$emit('remove', item, 'hide'); }}  ><a-icon type="delete" />
                </span>
              </div>
            </div>
          );
        })}
      </sc-panel>,

      <sc-panel
        title="透明设备"
        flex="1"
        bordered={false}
        showHeaderClose={false}
      >
        {this.opacityModelPairStrings.map(item => {
          return (
            <div class="equipment-item">
              <div class="name" >{item}</div>
              <div class="btns">
                <span class="btn" title="定位"
                  onClick={() => {
                    let [groupName, name] = item.split('@');
                    this.$emit('flyTo', { groupName, name });
                  }}
                ><a-icon type="environment" /></span>
                <span
                  class="btn"
                  title="移除"
                  onClick={() => { this.$emit('remove', item, 'opacity'); }} ><a-icon type="delete" />
                </span>
              </div>
            </div>
          );
        })}
      </sc-panel>,

      // 视点列表
      <sc-panel
        title="视点"
        flex="1"
        bordered={false}
        showHeaderClose={false}
      >

        <template slot="headerExtraContent" >
          <span
            title="添加视点"
            class="header-btn"
            style="margin-right: -6px;"
            onClick={async () => {
              let { position, heading, pitch, roll } = d3.scene.camera;
              let item = {
                name: '视点',
                position,
                orientation: {
                  heading,
                  pitch,
                  roll,
                },
              };
              let points = [...this.viewPoints, JSON.parse(JSON.stringify(item))];
              this.$emit("viewPointsChange", points);
            }}
          >
            <a-icon type="plus" />
          </span>

          {/* //cameraFlyByPoints */}
          <span
            title={this.flyState === CameraFlyState.Flying ? "暂停" : "开始"}
            class="header-btn"
            style="margin-right: -6px;"
            onClick={async () => {
              if (this.flyState != CameraFlyState.Flying) {
                this.flyState = CameraFlyState.Flying;
                cameraFlyByPoints(
                  d3.scene.camera,
                  this.viewPoints,
                  (index, finished) => {
                    this.flyIndex = index;
                    if (finished) {
                      this.flyState = CameraFlyState.Stoped;
                      this.flyIndex = 0;
                    }
                  },
                  1,
                  this.flyIndex);
              } else {
                this.flyState = CameraFlyState.Pause;
                d3.scene.camera.cancelFlight();
              }
            }}
          >
            <a-icon type={this.flyState === CameraFlyState.Flying ? "pause" : "caret-right"} />
          </span>
        </template>


        {
          this.viewPoints.map((item, index) => {
            return (
              <div class={{ "equipment-item": true, actived: index === this.flyIndex && (this.flyState != CameraFlyState.Stoped) }}>
                <div class="index" >{index + 1}</div>
                <a-input size="small" class="name" value={item.name || ''} onChange={(evt) => { item.name = evt.target.value; }} />
                <div class="btns">

                  <span class="btn" title="定位"
                    onClick={() => {
                      let { position, orientation } = JSON.parse(JSON.stringify(item));
                      // 定位
                      d3.scene.camera.flyTo({
                        destination: Cesium.Cartesian3.clone(position),
                        orientation,
                        duration: 1,
                      });

                    }}
                  ><a-icon type="environment" /></span>

                  <span class="btn" title="更新"
                    onClick={() => {
                      let points = [...this.viewPoints];
                      let { position, heading, pitch, roll } = d3.scene.camera;
                      let _item = { name: item.name || '', position, orientation: { heading, pitch, roll } };
                      points[index] = JSON.parse(JSON.stringify(_item));
                      this.$emit("viewPointsChange", points);
                      this.$message.success('更新成功');
                    }}
                  ><a-icon type="sync" /></span>

                  <span class="btn" title="移除"
                    onClick={() => {
                      let points = [...this.viewPoints];
                      points.splice(index, 1);
                      this.$emit("viewPointsChange", points);
                    }} ><a-icon type="delete" /></span>
                </div>
              </div>
            );
          })
        }
      </sc-panel>,
    ];

    return (
      <sc-panel
        class="sm-d3-equipment-camera"
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
        // title={this.title}
        onClose={visible => {
          this.iVisible = visible;
          this.$emit('close', this.iVisible);
        }}
      >
        <a-icon slot="icon" type="video-camera" />
        {
          this.iEquipment ?
            [
              <span slot="title">
                <span>相机视角</span>
                <span
                  title="刷新设备"
                  class="header-btn"
                  style={{ color: this.equipment && this.iEquipment.id === this.equipment.id ? 'inherit' : 'red' }}
                  onClick={this.refresh}
                >
                  {this.title}
                  <a-icon type="sync" />刷新设备
                </span>
              </span>,
              <template slot="headerExtraContent" >

                <span
                  title="清除"
                  class="header-btn"
                  onClick={async () => {

                    let _this = this;
                    this.$confirm({
                      title: "确定要清除视角吗",
                      content: h => <div style="color:red;">清除后不可恢复</div>,
                      okType: 'danger',
                      onOk() {
                        // 删除角色业务逻辑
                        return new Promise(async (resolve, reject) => {
                          let response;
                          if (_this.iEquipment) {
                            response = await apiEquipment.updateGisData({
                              id: _this.iEquipment.id,
                              gisData: null,
                            });
                          }
                          setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
                        });
                      },
                    });
                  }}
                >
                  <a-icon type="delete" />清除
                </span>

                <span
                  title="更新"
                  class="header-btn"
                  onClick={async () => {

                    if (this.iEquipment) {
                      let { position, heading, pitch, roll } = d3.scene.camera;
                      let response = await apiEquipment.updateGisData({
                        id: this.iEquipment.id,
                        gisData: JSON.stringify({
                          position,
                          orientation: {
                            heading,
                            pitch,
                            roll,
                          },
                          hiddenModelPairStrings: this.hiddenModelPairStrings,
                          opacityModelPairStrings: this.opacityModelPairStrings,
                          viewPoints: this.viewPoints,
                        }),
                      });

                      if (requestIsSuccess(response)) {
                        this.$message.success("更新成功");
                      }
                    }
                  }}
                >
                  <a-icon type="check" />更新
                </span>

                <span
                  title="定位设备"
                  class="header-btn"
                  onClick={() => {
                    this.$emit('flyTo', { groupName: this.iEquipment.group.name, name: this.iEquipment.name });
                  }}
                >
                  <a-icon type="environment" />定位设备
                </span>
              </template>,
            ] : undefined
        }

        {
          content && this.iVisible ? (
            content
          ) : (
            <span
              style="
              margin-top: 10px;
              color: #bdbdbd;
              "
            >
                无数据
            </span>
          )
        }
      </sc-panel >);
  },
};
