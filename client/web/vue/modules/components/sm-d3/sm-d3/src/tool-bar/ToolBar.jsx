import './ToolBar.less';
import SmBasicScopeSelect from '../../../../sm-basic/sm-basic-scope-select';
import * as SnCesiumX from 'sn-cesium-x';
import d3 from '../../src/d3Manager';

let heatmap, heatmapTimer;
export default {
  name: 'ToolBar',
  props: {
    axios: { type: Function, default: null },
    scopeCode: { type: String, default: null },
    equipmentPanelVisible: { type: Boolean, default: false },
    equipmentInfoPanelVisible: { type: Boolean, default: false },
    smD3ConstructionProgressPanelVisible: { type: Boolean, default: false }, // 进度模拟可见性
    smD3InterfacePanelVisible: { type: Boolean, default: false },
    smD3QualityPanelVisible: { type: Boolean, default: false },
    smD3SafePanelVisible: { type: Boolean, default: false },
    teminalLinkPanelVisible: { type: Boolean, default: false },
    emergFaultsPanelVisible: { type: Boolean, default: false },
    alarmsPanelVisible: { type: Boolean, default: false },
    equipmentCameraPanelVisible: { type: Boolean, default: false },
    moreInfoPanelVisible: { type: Boolean, default: false },
    cableCorePanelVisible: { type: Boolean, default: false },
    navCameraFlyVisible: { type: Boolean, default: false },
    stationSliderVisible: { type: Boolean, default: false },
    components: { type: Array, default: () => [] }, // ['TeminalLink','EmergFaults']
    defaultEquipment: { type: Object, default: null },
    defaultModel: { type: Object, default: null },
    selectedModels: { type: Array, default: () => [] },
    hiddenModels: { type: Array, default: () => [] },
    opacityModels: { type: Array, default: () => [] },
    select: { type: Boolean, default: false },
  },
  data() {
    return {
      undergroundMode: false, // 地形模式
      clippingMode: false, // 剖切模式
      firstCharacterMode: false, // 第一视角
      heatMapMode: false, // 热力图（客流量）
    };
  },
  created() { },

  methods: {
    /**
     * 显示导航栏更多菜单
     * @param {*} data
     */
    async onBtnMoreClick(data) {
      switch (data.key) {
        case 'setting-camera':
          this.$emit('settingCamera');
          break;

        // 取消隐藏
        case 'show-all':
          this.$emit('showAll');
          break;

        // 隐藏所选
        case 'hide-all':
          this.$emit('hideAll');
          break;

        // 取消透明
        case 'opacity-none':
          this.$emit('opacityNone');
          break;

        // 透明所选
        case 'opacity-all':
          this.$emit('opacityAll');
          break;

        // 热力图
        case 'heatMapMode':
          if (!this.heatMapMode) {
            let bounds = {
              west: 120.79131926542337,
              south: 36.4820893508981,
              east: 120.79258681793064,
              north: 36.48242721477444,
            };

            heatmap = new SnCesiumX.SnHeatmap(d3.viewer, bounds, {
              radius: 20,
            });
            let data = heatmap.getRandomData(1000);
            heatmap.setData(data);
            heatmap.draw();

            heatmapTimer = setInterval(() => {
              let data = heatmap.getRandomData(1000);
              heatmap.setData(data);
            }, 1000);
          } else {
            clearInterval(heatmapTimer);
            heatmap.destroy();
          }
          this.heatMapMode = !this.heatMapMode;
          break;

        default:
          break;
      }
    },
  },
  render() {
    return (
      <div class="toolbar">
        <div class="left">
          {/* <SmBasicScopeSelect
            style={{ width: 'auto', minWidth: '260px' }}
            size="small"
            round
            axios={this.axios}
            value={this.scopeCode}
            autoInitial
            onChange={value => {
              this.$emit('scopeCodeChange', value);
            }}
          /> */}
          <a-button
            type={this.equipmentPanelVisible ? 'primary' : 'default'}
            size="small"
            shape="round"
            onClick={() => {
              this.$emit('equipmentPanelVisibleChange', !this.equipmentPanelVisible);
            }}
            icon="menu-unfold"
          >
            设备
          </a-button>
        </div>

        <div class="nav-bar" ref="navBar">
          <div class="nav-bar-box">
            <div
              class="nav-item"
              title="放大"
              onClick={() => {
                d3.viewer.camera.zoomIn();
              }}
              onMousemove={e => {
                e.stopPropagation();
              }}
            >
              <a-icon type="plus" />
            </div>

            <div
              class="nav-item"
              title="缩小"
              onClick={() => {
                d3.viewer.camera.zoomOut();
              }}
              onMousemove={e => {
                e.stopPropagation();
              }}
            >
              {' '}
              <a-icon type="minus" />
            </div>

            <div
              class={{ 'nav-item': true, actived: this.undergroundMode }}
              title="地下模式"
              onClick={() => {
                this.undergroundMode = !this.undergroundMode;
                let terrain = new SnCesiumX.SnTerrain(d3.viewer);
                terrain.undergroundMode = !terrain.undergroundMode;
                if (terrain.undergroundMode === true) {
                  terrain.setAlpha(0.5);
                } else {
                  terrain.setAlpha(1);
                }
              }}
              onMousemove={e => {
                e.stopPropagation();
              }}
            >
              <si-underground-mode size={20} />
            </div>

            <div
              class={{ 'nav-item': true, actived: this.firstCharacterMode }}
              title="第一视角"
              onClick={() => {
                let controller = new SnCesiumX.SnFirstCharacterController(d3.viewer, () => {
                  this.firstCharacterMode = false;
                });
                controller.start();
                this.firstCharacterMode = true;
              }}
              onMousemove={e => {
                e.stopPropagation();
              }}
            >
              <si-walk-mode size={20} />
            </div>

            <div
              class={{ 'nav-item': true, actived: this.clippingMode }}
              title="剖切"
              onClick={() => {
                let clipping = d3.snClippingBox;
                if (clipping.isOpened()) {
                  clipping.close();
                  this.clippingMode = false;
                } else {
                  clipping.open();
                  this.clippingMode = true;
                  if (d3.slippingTileset) {
                    clipping.setTarget(d3.slippingTileset);
                  }
                }
              }}
              onMousemove={e => {
                e.stopPropagation();
              }}
            >
              <a-icon type="scan" />
            </div>

            <div
              title="车站"
              class={{ 'nav-item': true }}
              onClick={() => {
                this.$emit('stationSliderVisibleChange', !this.stationSliderVisible);
              }}
            >
              <a-icon type={this.stationSliderVisible ? 'down' : 'up'} />
            </div>

            <div
              title="定位"
              class={{ 'nav-item': true, disabled: !this.defaultModel }}
              onClick={() => {
                this.$emit('flyTo', {
                  groupName: this.defaultModel.groupName,
                  name: this.defaultModel.name,
                });
              }}
            >
              {' '}
              <a-icon type="environment" />
            </div>

            <div
              title="漫游"
              class={{
                'nav-item': true,
                disabled: !this.defaultEquipment,
                actived: this.navCameraFlyVisible,
              }}
              onClick={() => {
                this.$emit('navCameraFlyVisibleChange', !this.navCameraFlyVisible);
              }}
            >
              <a-icon type="video-camera" />
            </div>
            <div
              class={{ 'nav-item': true, disabled: !this.defaultModel }}
              title={this.defaultModel && !this.defaultModel.isOpacity ? '设置透明' : '取消透明'}
              onClick={() => {
                this.$emit('opacityChange');
              }}
            >
              <a-icon type={this.defaultModel && !this.defaultModel.isOpacity ? 'bulb' : 'alert'} />
            </div>

            <div
              class={{ 'nav-item': true, disabled: !this.defaultModel }}
              title={this.defaultModel && !this.defaultModel.isHidden ? '隐藏' : '显示'}
              onClick={() => {
                this.$emit('hiddenChange');
              }}
            >
              <a-icon
                type={this.defaultModel && !this.defaultModel.isHidden ? 'eye-invisible' : 'eye'}
              />
            </div>

            <a-dropdown
              trigger={['click']}
              overlayClassName="btn-more-overlay"
              getPopupContainer={() => {
                return this.$refs.navBar;
              }}
            >
              <a-menu slot="overlay" onClick={this.onBtnMoreClick}>
                <a-menu-item key="setting-camera" disabled={!this.defaultEquipment}>
                  {' '}
                  <a-icon type="video-camera" />
                  相机视角
                </a-menu-item>

                <a-menu-divider />
                <a-menu-item key="show-all" disabled={this.hiddenModels.length === 0}>
                  <a-icon type="eye" />
                  取消隐藏{this.hiddenModels.length ? `（${this.hiddenModels.length}）` : ''}
                </a-menu-item>
                <a-menu-item key="hide-all" disabled={this.selectedModels.length === 0}>
                  <a-icon type="eye-invisible" />
                  隐藏所选
                </a-menu-item>

                <a-menu-divider />
                <a-menu-item key="opacity-none" disabled={this.opacityModels.length === 0}>
                  <a-icon type="alert" />
                  取消透明{this.opacityModels.length ? `（${this.opacityModels.length}）` : ''}
                </a-menu-item>
                <a-menu-item key="opacity-all" disabled={this.selectedModels.length === 0}>
                  <a-icon type="bulb" />
                  设置透明
                </a-menu-item>

                <a-menu-divider />
                <a-menu-item key="heatMapMode">
                  <a-icon type="radar-chart" />
                  客流量分析
                </a-menu-item>
              </a-menu>

              <div class={{ 'nav-item': true }} title="更多">
                <a-icon type="ellipsis" />
              </div>
            </a-dropdown>
          </div>
        </div>

        <div class="right">

          {this.components.indexOf('Alarms') > -1 && !this.select ? (
            <a-button
              size="small"
              shape="round"
              icon="alert"
              type={this.alarmsPanelVisible ? 'primary' : 'default'}
              onClick={() => {
                this.$emit('alarmsPanelVisibleChange', !this.alarmsPanelVisible);
              }}
            >
              告警
            </a-button>
          ) : (
            undefined
          )}

          {this.components.indexOf('EmergFaults') > -1 && !this.select ? (
            <a-button
              size="small"
              shape="round"
              icon="warning"
              type={this.emergFaultsPanelVisible ? 'primary' : 'default'}
              onClick={() => {
                this.$emit('emergFaultsPanelVisibleChange', !this.emergFaultsPanelVisible);
              }}
            >
              应急
            </a-button>
          ) : (
            undefined
          )}

          {this.components.indexOf('TeminalLink') > -1 && !this.select ? (
            <a-button
              size="small"
              shape="round"
              icon="control"
              type={this.teminalLinkPanelVisible ? 'primary' : 'default'}
              onClick={() => {
                this.$emit('teminalLinkPanelVisibleChange', !this.teminalLinkPanelVisible);
              }}
            >
              端子
            </a-button>
          ) : (
            undefined
          )}


          {!this.select ?
            <a-button
              size="small"
              shape="round"
              icon="bars"
              type={this.equipmentInfoPanelVisible ? 'primary' : 'default'}
              onClick={() => {
                this.$emit('equipmentInfoPanelVisibleChange', !this.equipmentInfoPanelVisible);
              }}
            >
              属性
            </a-button> : undefined
          }

          {!this.select ?
            <a-button
              type={this.smD3SafePanelVisible ? 'primary' : 'default'}
              size="small"
              shape="round"
              onClick={() => {
                this.$emit('smD3SafePanelVisibleChange', !this.smD3SafePanelVisible);
              }}
              icon="menu-unfold"
            >
              安全
            </a-button> : undefined
          }
          {!this.select ?
            <a-button
              type={this.smD3QualityPanelVisible ? 'primary' : 'default'}
              size="small"
              shape="round"
              onClick={() => {
                this.$emit('smD3QualityPanelVisibleChange', !this.smD3QualityPanelVisible);
              }}
              icon="menu-unfold"
            >
              质量
            </a-button> : undefined
          }
          {!this.select ?
            <a-button
              type={this.smD3InterfacePanelVisible ? 'primary' : 'default'}
              size="small"
              shape="round"
              onClick={() => {
                this.$emit('smD3InterfacePanelVisibleChange', !this.smD3InterfacePanelVisible);
              }}
              icon="menu-unfold"
            >
              接口
            </a-button> : undefined
          }
          {!this.select ?
            <a-button
              size="small"
              shape="round"
              icon="share-alt"
              type={this.constructionVisibleChange ? 'primary' : 'default'}
              onClick={() => {
                this.$emit('constructionVisibleChange', !this.smD3ConstructionProgressPanelVisible);
              }}
            >
              进度
            </a-button> : undefined
          }
        </div>
      </div>
    );
  },
};
