import SmD3CableLocationModal from './SmD3CableLocationModal';
import ApiCableLocation from '../../sm-api/sm-resource/CableLocation';
import { requestIsSuccess } from '../../_utils/utils';
import { CableLocationDirection } from '../../_utils/enum';
import { tips as tipsConfig } from '../../_utils/config';
import './style/index.less';

import { v4 as uuidv4 } from 'uuid';

import * as SnCesiumX from 'sn-cesium-x';

import d3 from '../sm-d3/src/d3Manager';
import d3Manager from '../sm-d3/src/d3Manager';
import { D3Mode } from '../sm-d3/src/utils/cesium-common';

let apiCableLocation = new ApiCableLocation();

const options = {
  dimensions: null,
  point: {
    graphics: new Cesium.PointGraphics({
      show: true,
      color: Cesium.Color.RED,
      pixelSize: 10,
      outlineColor: Cesium.Color.WHITE,
      outlineWidth: 2,
    }),
  },
  polyline: {
    graphics: new Cesium.PolylineGraphics({
      show: true,
      width: 2,
      material: Cesium.Color.BLUE,
    }),
  },
  label: {
    graphics: new Cesium.LabelGraphics({
      showBackground: true,
      backgroundColor: Cesium.Color.TRANSPARENT,
      scale: 0.5,
      font: 'normal 32px MicroSoft YaHei',
      style: Cesium.LabelStyle.FILL,
      pixelOffset: new Cesium.Cartesian2(0.0, -16),
      horizontalOrigin: Cesium.HorizontalOrigin.CENTER,
      verticalOrigin: Cesium.VerticalOrigin.CENTER,
    }),
  },
};

export default {
  name: 'SmD3CableLocation',
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    value: { type: String, default: null }, //电缆Id
  },
  data() {
    return {
      cableLocations: [], //电缆埋深信息集合
      showAll: false,
    };
  },

  computed: {},

  watch: {
    value: {
      handler: function (value, oldValue) {
        if (value) {
          this.initAxios();
          this.refresh();
        }
      },
      immediate: true,
    },
    visible: {
      handler: function (value, oldValue) {
      },
    },
  },

  async created() {
    this.initAxios();
    this.refresh();
  },

  mounted() { },

  destroyed() {
    this.clearRender();
  },

  methods: {
    initAxios() {
      apiCableLocation = new ApiCableLocation(this.axios);
    },

    async refresh() {
      this.clearRender();
      if (!this.value) return;
      let response = await apiCableLocation.getList({ cableId: this.value });
      if (requestIsSuccess(response) && response.data) {
        this.cableLocations = response.data.sort((a, b) => a.order - b.order);
        if (this.showAll) {
          this.render();
        }
      }
    },

    edit(item) {
      this.$refs.SmD3CableLocationModal.edit(item);
    },

    render() {
      this.clearRender();
      this.cableLocations.map(item => {
        let positions = JSON.parse(item.positions);
        let start = Cesium.Cartesian3.clone(positions.start);
        let end = Cesium.Cartesian3.clone(positions.end);
        this.renderCableLocation(item.id, start, end, item.value, item.direction);
      });
    },

    clearRender() {
      this.cableLocations.map(item => {
        // d3.snSceneDataManager.removeEntity(item.id);
      });
    },

    async delete(item) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiCableLocation.delete(item.id);
            if (requestIsSuccess(response)) {
              _this.refresh();
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },

    // 添加数据到 cesiumx 数据列表
    addCableLocationToDataList(id, start, end, dimensions, direction, relateEntityIds) {
      let _dir;
      switch (direction) {
      case CableLocationDirection.Horizontal:
        _dir = SnCesiumX.SnEntityType.ET_HORIZONTAL_MARKING;
        break;
      case CableLocationDirection.Vertical:
        _dir = SnCesiumX.SnEntityType.ET_VERTICAL_MARKING;
        break;
      case CableLocationDirection.Straight:
        _dir = SnCesiumX.SnEntityType.ET_SPACE_MARKING;
        break;
      }

      const entity = new SnCesiumX.SnMarkingEntity();
      entity._id = id;
      entity._name = SnCesiumX.generateName('Marking');
      entity._visible = true;
      entity._position = Cesium.Cartesian3.divideByScalar(
        Cesium.Cartesian3.add(start, end, new Cesium.Cartesian3()),
        2,
        new Cesium.Cartesian3(),
      );
      entity._entityType = _dir;
      entity._startPoint = start;
      entity._endPoint = end;
      entity._dimensions = dimensions;
      entity._pointOptions = SnCesiumX.SnUtil.fromGraphics(options.point.graphics);
      entity._polylineOptions = SnCesiumX.SnUtil.fromGraphics(options.polyline.graphics);
      entity._labelOptions = SnCesiumX.SnUtil.fromGraphics(options.label.graphics);
      entity._relateEntityIds = relateEntityIds;
      d3.snSceneDataManager.addEntity(entity);
    },

    // 场景中加入标注
    renderCableLocation(id, start, end, dimensions, direction) {
      let _options = { ...options, dimensions: dimensions };
      let method;
      switch (direction) {
      case CableLocationDirection.Horizontal:
        method = 'createHorizontalMarking';
        break;
      case CableLocationDirection.Vertical:
        method = 'createVerticalMarking';
        break;
      case CableLocationDirection.Straight:
        method = 'createSpaceMarking';
        break;
      }
      if (!method) {
        return;
      }

      d3.snEntityHelper[method](
        start,
        end,
        _options,
        (start, end, _dimensions, relateEntityIds) => {
          this.addCableLocationToDataList(id, start, end, _dimensions, direction, relateEntityIds);
        },
      );
    },

    add(direction) {
      d3Manager.mode = null;
      let draw;
      switch (direction) {
      case CableLocationDirection.Horizontal:
        draw = 'drawHorizontalMarking';
        break;
      case CableLocationDirection.Vertical:
        draw = 'drawVerticalMarking';
        break;
      case CableLocationDirection.Straight:
        draw = 'drawSpaceMarking';
        break;
      }
      if (!draw) {
        return;
      }
      // 加入到场景
      d3.snEntityHelper[draw](options, async (start, end, dimensions, relateEntityIds) => {
        if (!Cesium.defined(start) || !Cesium.defined(end)) {
          return;
        }

        d3Manager.mode = D3Mode.Select;

        let id = uuidv4();
        this.addCableLocationToDataList(id, start, end, dimensions, direction, relateEntityIds);

        // 数据库增加记录（目的是为了删除）
        let cableLocation = {
          cableId: this.value,
          name: dimensions,
          value: dimensions,
          direction,
          positions: JSON.stringify({ start, end }),
        };
        let response = await apiCableLocation.create(cableLocation);
        if (requestIsSuccess(response)) {
          // 删除
          d3.snSceneDataManager.removeEntity(id);
          this.showAll = true;
          this.refresh();
        }
      });
    },
  },

  render() {
    return (
      <div class="sm-d3-cable-location">
        {this.cableLocations.length > 0 ? (
          <div class="info-box">
            {this.cableLocations.map((item, index) => {
              let icon;
              switch (item.direction) {
              case CableLocationDirection.Horizontal:
                icon = <si-dimension-horizontal />;
                break;
              case CableLocationDirection.Vertical:
                icon = <si-dimension-vertical />;
                break;
              case CableLocationDirection.Straight:
                icon = <si-dimension-straight />;
                break;
              }

              return (
                <div class="info-item">
                  <div class="item-title" onClick={() => {
                    this.render();
                    this.$nextTick(() => {
                      d3.snCamera.flyTo(item.id, 1.0);
                    });
                  }}>
                    <a-icon type="environment" style="margin-right:10px" />{`基点${index + 1}`}
                  </div>
                  <div class="item-icon">
                    {icon}
                  </div>
                  <div class="item-value">
                    {`${item.value.toFixed(3)} m`}
                  </div>
                  <div class="item-operator">
                    <div class="icons">
                      <a-icon
                        type="minus"
                        onClick={() => {
                          this.delete(item);
                        }}
                      />
                      <a-icon
                        type="edit"
                        theme="filled"
                        onClick={() => {
                          this.edit(item);
                        }}
                      />
                    </div>
                  </div>
                </div>
              );
            })}
          </div>
        ) : (
          <span style="color: rgb(189, 189, 189); margin: 20px;">无数据</span>
        )
        }

        <a-button-group style="width:100%" >
          <a-button
            size="small"
            style="width:50%"
            onClick={() => {
              this.showAll = true;
              this.render();
            }}
          >
            <a-icon type="eye" />显示
          </a-button>
          <a-button
            size="small"
            style="width:50%"
            onClick={() => {
              this.showAll = false;
              this.clearRender();
            }}
          >
            <a-icon type="eye-invisible" />隐藏
          </a-button>
        </a-button-group >

        <a-button-group style="width:100%; margin-top: 5px" >
          <a-button
            type="primary"
            size="small"
            style="width:33.333333%"
            onClick={() => {
              this.add(CableLocationDirection.Horizontal);
            }}
          >
            <si-dimension-horizontal style="margin-right: 6px;" />水平
          </a-button>
          <a-button
            type="primary"
            size="small"
            style="width:33.333333%"
            onClick={() => {
              this.add(CableLocationDirection.Vertical);
            }}
          >
            <si-dimension-vertical style="margin-right: 6px;" />垂直
          </a-button>
          <a-button
            type="primary"
            size="small"
            style="width:33.333333%"
            onClick={() => {
              this.add(CableLocationDirection.Straight);
            }}
          >
            <si-dimension-straight style="margin-right: 6px;" />空间
          </a-button>
        </a-button-group>

        <SmD3CableLocationModal
          ref="SmD3CableLocationModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh();
          }}
        />
      </div >
    );
  },
};
