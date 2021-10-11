import SmD3 from './SmD3';

SmD3.install = function(Vue) {
  Vue.component(SmD3.name, SmD3);
};

export default SmD3;

// import './style/index.less';
// import {
//   requestIsSuccess,
//   vPermission as vP,
//   isArray,
//   isString,
//   getAlarmColor,
//   parseScope,
//   d3Pair2Path,
//   d3Path2Pair,
// } from '../../_utils/utils';
// import { AlarmLevel, MarkType } from '../../_utils/enum';
// import { v4 as uuidV4 } from 'uuid';
// import {
//   getCesiumDistanceLegend,
//   getCesiumPerformanceInfo,
//   registerMouseLock,
// } from './src/utils/cesium-utils';
// import SmD3Equipments from '../sm-d3-equipments';

// import SmD3TeminalLink from '../sm-d3-terminal-link';
// import SmD3CableCores from '../sm-d3-cable-cores';
// import SmD3EquipmentCamera from '../sm-d3-equipment-camera';
// import SmD3EquipmentInfo from '../sm-d3-equipment-info';
// import SmD3EmergFaults from '../sm-d3-emerg-faults';
// import SmD3Alarms from '../sm-d3-alarms';
// import SmD3Interface from '../sm-d3-interface/SmD3Interface';
// import SmD3Quality from '../sm-d3-quality';
// import SmD3Safe from '../sm-d3-safe';
// import SmD3StationSlider from '../sm-d3-station-slider';
// import SmD3ConstructionProgress from '../sm-d3-construction-progress';
// import StatusBar from './src/status-bar/status-bar';
// import ApiEquipment from '../../sm-api/sm-resource/Equipments';
// import ApiCableLocation from '../../sm-api/sm-resource/CableLocation';
// let apiEquipment = new ApiEquipment();
// let apiCableLocation = new ApiCableLocation();
// import * as SnCesiumX from 'sn-cesium-x';
// SnCesiumX.SnStyleUtil.addInnerStyle();
// import axios from 'axios';
// import d3 from './src/d3Manager';
// import { D3Mode } from './src/utils/cesium-common';
// import d3Manager from './src/d3Manager';
// import CameraFly from './src/camera-fly/CameraFly';
// import ToolBar from './src/tool-bar/ToolBar';

// let ctrlIsDown = false;

// let cameraInfoHandle;
// let spaceEventHandler;
// let spaceEventHandlerEvent;
// let keydownHandle;
// let keyupHandle;

// const ModelState = {
//   IsSelected: 'isSelected',
//   IsHidden: 'isHidden',
//   IsOpacity: 'isOpacity',
//   IsActived: 'isActived',
// };

// let blinkId;
// export default {
//   name: 'SmD3',
//   props: {
//     axios: { type: Function, default: null },
//     signalr: { type: Function, default: null },
//     permissions: { type: Array, default: () => [] },
//     snEarthProjectUrl: { type: String }, // SnEarth 工程文件路径
//     globalTerrainUrl: { type: String, default: null }, // 全球地形路径
//     globalImageryUrl: { type: String, default: null }, // 全球影像路径
//     imageryUrls: { type: Array, default: () => [] }, // 影像路径
//     components: { type: Array, default: () => [] }, // ['TeminalLink','EmergFaults']
//     systems: { type: Array, default: () => [] },
//     select: { type: Boolean, default: false },
//     selectedEquipments: { type: Array, default: () => { } },
//   },
//   data() {
//     return {
//       cesiumConatinerId: uuidV4(),
//       equipmentPanelVisible: true, // 设备列表面板
//       equipmentInfoPanelVisible: false, // 设备信息面板
//       teminalLinkPanelVisible: false, // 配线关系面板是否显示
//       emergFaultsPanelVisible: false, // 故障应急面板
//       smD3ConstructionProgressPanelVisible: false, // 进度模拟面板
//       smD3InterfacePanelVisible: false, // 接口管理面板是否显示
//       smD3QualityPanelVisible: false, // 质量管理面板是否显示
//       smD3SafePanelVisible: false, // 安全管理面板是否显示
//       alarmsPanelVisible: false, // 集中告警面板
//       equipmentCameraPanelVisible: false, // 相机视角维护面板
//       moreInfoPanelVisible: false, // 更多菜单功能
//       cableCorePanelVisible: false, // 线芯面板
//       navCameraFlyVisible: false, //
//       stationSliderVisible: true,
//       selectEquipmentPairs: [], // 选中设备
//       viewPoints: [], // 视点
//       undergroundMode: false, // 地下模式
//       status: null, // 状态信息
//       progress: null, // 进度
//       cameraInfo: {
//         distance: {
//           // 比例
//           width: null,
//           distance: null,
//           distanceRound: null,
//           distanceRoundUnit: null,
//         },
//         position: {
//           // 相机位置
//           latitude: null,
//           longitude: null,
//           height: null,
//         },
//         direction: {
//           // 相机方向
//           heading: null,
//           pitch: null,
//           roll: null,
//         },
//         performance: {
//           // 性能指标
//           ms: null,
//           fps: null,
//         },
//       },
//       teminalLinkPanelPosition: { left: '20px', bottom: '20px' },
//       emergFaultsPanelPosition: { left: '20px', bottom: '20px' },
//       d3InterfacePanelPosition: { left: '20px', bottom: '20px' },
//       d3ConstructionProgressPosition: { left: '20px', bottom: '20px', right: '20px' },
//       cableCorePanelPosition: { right: '20px', bottom: '20px', left: '820px' },
//       defaultEquipment: null,
//       scopeCode: null,
//       models: [], // {pairString:'group@name',groupName,name,isSelected,isHidden,isOpacity,isActived,isBlink,blinkColor,blinkId}
//       alarmBlinks: [],
//       alarms: [],
//       simulationEqupmentPaths: [],
//       interfancePosition: null // {lot:0,lat:0,height:0}
//     };
//   },
//   computed: {
//     modelsPairString: function () {
//       return this.models.map(x => x.pairString);
//     },

//     selectedModels: function () {
//       return this.models.filter(x => x.isSelected);
//     },
//     hiddenModels: function () {
//       return this.models.filter(x => x.isHidden);
//     },
//     opacityModels: function () {
//       return this.models.filter(x => x.isOpacity);
//     },
//     activedModels: function () {
//       return this.models.filter(x => x.isActived);
//     },

//     selectedModelPairStrings: function () {
//       return this.selectedModels.map(x => x.pairString);
//     },
//     hiddenModelPairStrings: function () {
//       return this.hiddenModels.map(x => x.pairString);
//     },
//     opacityModelPairStrings: function () {
//       return this.opacityModels.map(x => x.pairString);
//     },
//     activedModelsPairStrings: function () {
//       return this.activedModels.map(x => x.pairString);
//     },

//     defaultModel: function () {
//       return this.selectedModels[this.selectedModels.length - 1] || null;
//     },
//     defaultModelPairString: function () {
//       return this.defaultModel ? this.defaultModel.pairString : null;
//     },
//     defaultModelPair: function () {
//       console.log("this.defaultModel", this.defaultModel);
//       return this.defaultModel
//         ? { groupName: this.defaultModel.groupName, name: this.defaultModel.name }
//         : null;
//     },
//   },
//   watch: {
//     selectedEquipments: {
//       handler: function (equipments, oldValue) {
//         if (this.select) {
//           console.log(equipments);
//           this.models = equipments.map(equip => {
//             console.log(equip);
//             let { groupName, name } = equip;
//             let pairString = `${groupName}@${name}`;
//             return {
//               pairString,
//               groupName,
//               name,
//               isSelected: true,
//               isHidden: false,
//               isOpacity: false,
//               isActived: false,
//               isBlink: false,
//               blinkColor: null,
//               blinkId: null,
//             };
//           });
//         }
//       },
//       immediate: true,
//     },
//     defaultModel: {
//       handler: function (value, oldValue) {
//         if (this.defaultModel) {
//           this.initDefaultEquipment();
//         } else {
//           this.defaultEquipment = null;
//         }
//       },
//     },
//     selectedModels: {
//       handler: function (value, oldValue) {
//         if (d3.snBusinessHelper) {
//           d3.snBusinessHelper.setColor(
//             oldValue.map(({ groupName, name }) => {
//               return d3Pair2Path(groupName, name);
//             }),
//           );
//           d3.snBusinessHelper.setColor(
//             value.map(({ groupName, name }) => {
//               return d3Pair2Path(groupName, name);
//             }),
//             Cesium.Color.BLUE.withAlpha(0.5),
//           );
//         }
//       },
//       immediate: true,
//     },
//     hiddenModels: {
//       handler: function (value, oldValue) {
//         d3.snBusinessHelper.setVisible(
//           oldValue.map(({ groupName, name }) => {
//             return d3Pair2Path(groupName, name);
//           }),
//         );
//         d3.snBusinessHelper.setVisible(
//           value.map(({ groupName, name }) => {
//             return d3Pair2Path(groupName, name);
//           }),
//           false,
//         );
//       },
//     },
//     opacityModels: {
//       handler: function (value, oldValue) {
//         d3.snBusinessHelper.setTransparent(
//           oldValue.map(({ groupName, name }) => {
//             return d3Pair2Path(groupName, name);
//           }),
//         );
//         d3.snBusinessHelper.setTransparent(
//           value.map(({ groupName, name }) => {
//             return d3Pair2Path(groupName, name);
//           }),
//           0.4,
//         );
//       },
//     },
//     activedModels: {
//       handler: function (value, oldValue) {
//         d3.snBusinessHelper.setColor(
//           oldValue.map(({ groupName, name }) => {
//             return d3Pair2Path(groupName, name);
//           }),
//         );
//         d3.snBusinessHelper.setColor(
//           value.map(({ groupName, name }) => {
//             return d3Pair2Path(groupName, name);
//           }),
//           Cesium.Color.RED.withAlpha(0.5),
//         );
//       },
//     },
//     alarms: {
//       handler: function (value, oldValue) {
//         this.onAlarmsChange();
//       },
//     },
//     alarmsPanelVisible: {
//       handler: function (value, oldValue) {
//         this.onAlarmsChange();
//       },
//     },
//     smD3InterfacePanelVisible: {
//       handler: function (value, oldValue) {
//         if (!value) {
//           /* 清除所有标签 */
//           d3.snDynamicRectLabelHelper.clear();
//         }
//       },
//     },
//     smD3ConstructionProgressPanelVisible: {
//       handler: function (value, oldValue) {
//         if (!value) {
//           /* 清除所有标签 */
//           d3.snDynamicRectLabelHelper.clear();
//         }
//       },
//     },
//     smD3QualityPanelVisible: {
//       handler: function (value, oldValue) {
//         if (!value) {
//           /* 清除所有标签 */
//           d3.snDynamicRectLabelHelper.clear();
//         }
//       },
//     },
//   },
//   async created() {
//     this.initAxios();
//   },
//   mounted() {
//     this.initScene();
//   },
//   destroyed() {
//     this.uninstallEvents();
//     // 停止之前的闪烁
//     this.alarmBlinks.map(item => {
//       d3.snBusinessHelper.endFlicker(item.id);
//     });
//   },
//   methods: {
//     initAxios() {
//       apiEquipment = new ApiEquipment(this.axios);
//       apiCableLocation = new ApiCableLocation(this.axios);
//     },
//     updateModels() {
//       let models = this.models.filter(
//         x => x.isSelected || x.isHidden || x.isOpacity || x.isActived,
//       );
//       this.models = [...models];
//     },
//     setModelState(data, key, value, multiple = false) {
//       if (isArray(data)) {
//         if (!multiple) {
//           this.models.map(item => {
//             if (data.indexOf(item) === -1) {
//               item[key] = !value;
//             }
//           });
//         }
//         data.map(item => {
//           let model = this.models.find(x => x.pairString === item);
//           if (model) {
//             model[key] = value;
//           } else {
//             model = this.getModelByPairString(item);
//             model[key] = value;
//             this.models.push(model);
//           }
//         });
//       } else if (isString(data)) {
//         if (!multiple) {
//           this.models.map(item => (item[key] = !value));
//         }
//         let model = this.models.find(x => x.pairString === data);
//         if (model) {
//           model[key] = value;
//         } else {
//           model = this.getModelByPairString(data);
//           model[key] = value;
//           this.models.push(model);
//         }
//       }
//       this.updateModels();
//     },
//     getModelByPairString(pairString) {
//       if (!pairString) return null;

//       // 如果存在返回已有
//       let target = this.models.find(x => x.pairString === pairString);
//       if (target) {
//         return target;
//       }

//       // 创建新的
//       let [groupName, name] = pairString.split('@');
//       return {
//         pairString,
//         groupName,
//         name,
//         isSelected: false,
//         isHidden: false,
//         isOpacity: false,
//         isActived: false,
//       };
//     },
//     // 载入默认选中设备数据
//     async initDefaultEquipment() {
//       console.log(!this.defaultModelPair.groupName);
//       console.log(!this.defaultModelPair.name);
//       if ( !this.defaultModelPair.groupName || !this.defaultModelPair.name) return;
//       let response = await apiEquipment.getByGroupNameAndName(this.defaultModelPair);
//       if (requestIsSuccess(response) && response.data.name) {
//         this.defaultEquipment = response.data;
//       } else {
//         this.defaultEquipment = null;
//       }
//     },

//     // 定位到场景中设备分组
//     async flyToScopeCode() {
//       // let response = await apiEquipment.getEquipmentGroupsByScopeCode({
//       //   scopeCode: this.scopeCode,
//       // });
//       // if (requestIsSuccess(response)) {
//       //   let groups = response.data;
//       //   if (groups.length > 0) {
//       //     d3.snBusinessHelper.flyTo(groups[0]);
//       //   }
//       // }

//       if (this.scopeCode) {
//         d3.snBusinessHelper.flyTo(parseScope(this.scopeCode).name);
//       }
//     },

//     /**
//      * 相机飞行
//      * @param {*} data
//      * @param {*} selected 飞行结束后，是否选中设备
//      */
//     async onFlyTo(data, selected = false) {
//       // let equipmentPair = { groupName: data.groupName, name: data.name };
//       let { groupName, name, state, position } = data;
//       console.log(position);
//       let response = await apiEquipment.getGisData({ groupName, name });
//       console.log(response);
//       if (requestIsSuccess(response) && response.data) {
//         let {
//           position,
//           orientation,
//           hiddenModelPairStrings,
//           opacityModelPairStrings,
//         } = response.data;
//         // 定位
//         d3.scene.camera.flyTo({
//           destination: new Cesium.Cartesian3(position.x, position.y, position.z),
//           orientation,
//           duration: 1,
//         });

//         hiddenModelPairStrings = hiddenModelPairStrings || [];
//         opacityModelPairStrings = opacityModelPairStrings || [];
//         this.setModelState(hiddenModelPairStrings, ModelState.IsHidden, true, true);
//         this.setModelState(opacityModelPairStrings, ModelState.IsOpacity, true, true);
//       } else {
//         if (!position) {
//           /* 获取接口路径(实体节点路径)*/
//           const _path = d3Pair2Path(groupName, name);

//           /* 通过名称定位 */
//           d3.snBusinessHelper.flyTo(_path, 1);

//           /* 1.根据接口路径获取实体 */
//           let dataEntity = d3.snBusinessHelper.getEntity(_path);

//           if (dataEntity) {
//             /* 清除所有标签 */
//             d3.snDynamicRectLabelHelper.clear();
//             // const helper = new SnCesiumX.SnDynamicRectLabelHelper(d3.viewer);
//             const { x, y, z } = dataEntity._position;
//             /* 添加单个动态标签 */

//             const data = {
//               position: new Cesium.Cartesian3(x, y, z),
//               text: name,
//               color: state === MarkType.Qualified ? Cesium.Color.LIME :
//                 state === MarkType.NoQualified ? Cesium.Color.RED :
//                   state === MarkType.NoCheck ? Cesium.Color.GOLDENROD : '',
//               fontColor: Cesium.Color.WHITE,
//             };
//             d3.snDynamicRectLabelHelper.add(data);
//           }
//         } else {
//           let { lon, lat, alt } = JSON.parse(position) ;
//           let _position = Cesium.Cartesian3.fromDegrees(lon, lat, alt);
//           /* 清除所有标签 */
//           d3.snDynamicRectLabelHelper.clear();
//           /* 添加单个动态标签 */

//           const data = {
//             position: _position,
//             text: name,
//             color: state === MarkType.Qualified ? Cesium.Color.LIME :
//               state === MarkType.NoQualified ? Cesium.Color.RED :
//                 state === MarkType.NoCheck ? Cesium.Color.GOLDENROD : '',
//             fontColor: Cesium.Color.WHITE,
//           };
//           console.log(data);
//           d3.snDynamicRectLabelHelper.add(data);
//         }
//       }

//       let pairString = `${groupName}@${name}`;

//       if (selected) {
//         this.setModelState(pairString, ModelState.IsSelected, true, true);
//       }

//       let flickerId = d3.snBusinessHelper.startFlicker(
//         [d3Pair2Path(groupName, name)],
//         Cesium.Color.BLUE.withAlpha(0.5),
//         500,
//       );
//       setTimeout(() => {
//         d3.snBusinessHelper.endFlicker(flickerId);
//         // 如果设备选中则恢复高亮
//         if (this.selectedModels.find(x => x.pairString === pairString)) {
//           d3.snBusinessHelper.setColor(
//             [d3Pair2Path(groupName, name)],
//             Cesium.Color.BLUE.withAlpha(0.5),
//           );
//         }
//       }, 2500);
//     },

//     // 初始化场景
//     async initScene() {
//       // 设置默认视角
//       Cesium.Camera.DEFAULT_VIEW_RECTANGLE = Cesium.Rectangle.fromDegrees(80, 22, 130, 50);

//       d3.viewer = new Cesium.Viewer(this.cesiumConatinerId, {
//         scene3DOnly: true,
//         geocoder: false,
//         homeButton: false,
//         sceneModePicker: false,
//         baseLayerPicker: false,
//         navigationHelpButton: false,
//         animation: false,
//         creditContainer: 'cesium-credit',
//         timeline: false,
//         fullscreenButton: false,
//         vrButton: false,
//         selectionIndicator: false,
//         infoBox: false,
//         terrainProvider: this.globalTerrainUrl
//           ? new Cesium.CesiumTerrainProvider({
//             url: this.globalTerrainUrl,
//             requestWaterMask: true,
//           })
//           : null,
//         imageryProvider: this.globalImageryUrl
//           ? new Cesium.TileMapServiceImageryProvider({
//             url: this.globalImageryUrl,
//           })
//           : null,
//       });

//       d3.viewer.clock.currentTime = Cesium.JulianDate.fromDate(new Date(2020, 1, 1, 12));

//       this.imageryUrls.forEach(imageryUrl => {
//         d3.viewer.imageryLayers.addImageryProvider(
//           new Cesium.TileMapServiceImageryProvider({
//             url: imageryUrl,
//           }),
//         );
//       });

//       d3.scene = d3.viewer.scene;
//       d3.scene.debugShowFramesPerSecond = true;
//       d3.scene.globe.depthTestAgainstTerrain = true;

//       d3.snBusinessHelper = new SnCesiumX.SnBusinessHelper(d3.viewer);
//       d3.snDynamicRectLabelHelper = new SnCesiumX.SnDynamicRectLabelHelper(d3.viewer);
//       d3.snEntityHelper = new SnCesiumX.SnEntityHelper(d3.viewer);
//       d3.snCamera = new SnCesiumX.SnCamera(d3.viewer);
//       d3.snSceneDataManager = new SnCesiumX.SnSceneDataManager(d3.viewer);

//       let tips = new SnCesiumX.SnTips(d3.viewer);
//       tips.addTips();

//       let response = await axios.get(this.snEarthProjectUrl);
//       if (requestIsSuccess(response)) {
//         d3.snBusinessHelper.loadProject(response.data);
//         if (this.scopeCode) {
//           this.flyToScopeCode();
//         } else {
//           if (d3.viewer) {
//             d3.viewer.camera.flyHome(2000);
//           }
//         }
//       }

//       this.installEvents();
//     },
//     // 注册事件
//     installEvents() {
//       let viewer = d3.viewer;
//       cameraInfoHandle = () => {
//         // 获取相机位置
//         let camera = viewer.camera;
//         let position = camera.position;
//         let ellipsoid = viewer.scene.globe.ellipsoid;
//         let pcartographic = ellipsoid.cartesianToCartographic(position);

//         this.cameraInfo.position = {
//           latitude: Cesium.Math.toDegrees(pcartographic.latitude),
//           longitude: Cesium.Math.toDegrees(pcartographic.longitude),
//           height: pcartographic.height,
//         };

//         this.cameraInfo.direction = {
//           heading: Cesium.Math.toDegrees(camera.heading),
//           pitch: Cesium.Math.toDegrees(camera.pitch),
//           roll: Cesium.Math.toDegrees(camera.roll),
//         };

//         this.cameraInfo.distance = getCesiumDistanceLegend(viewer.scene, Cesium);
//         this.cameraInfo.performance = getCesiumPerformanceInfo();
//       };
//       // 获取相机实时数据
//       viewer.scene.postRender.addEventListener(cameraInfoHandle);

//       // 注册点击事件
//       spaceEventHandler = new Cesium.ScreenSpaceEventHandler(d3.viewer.scene.canvas);
//       spaceEventHandlerEvent = click => {
//         if (d3Manager.mode === D3Mode.Select || d3Manager.mode === D3Mode.SetCamera) {
//           let cartesian = d3.viewer.scene.globe.pick(
//             d3.viewer.camera.getPickRay(click.position),
//             d3.viewer.scene,
//           );

//           let ellipsoid = d3.viewer.scene.globe.ellipsoid;
//           let cartographic = ellipsoid.cartesianToCartographic(cartesian);
//           let lon = Cesium.Math.toDegrees(cartographic.longitude);
//           let lat = Cesium.Math.toDegrees(cartographic.latitude);
//           let alt = cartographic.height;

//           // 判断是否有选中，如果没有选中任何模型，则取消上次的选中状态
//           let feature = d3.viewer.scene.pick(click.position);
//           if (Cesium.defined(feature)) {
//             console.log(
//               `左键单击事件：${click.position} 坐标：${cartographic} ${lon} ${lat} ${alt} 目标：`,
//               feature,
//             );
//             let _interfancePosition = {
//               lon,
//               lat,
//               alt
//             }
//             this.interfancePosition = JSON.stringify(_interfancePosition);
//             console.log('------------------------------------------------------');
//             let propertyNames = feature.getPropertyNames();
//             let length = propertyNames.length;
//             for (let i = 0; i < length; ++i) {
//               let propertyName = propertyNames[i];
//               console.log(propertyName + ': ' + feature.getProperty(propertyName));
//             }
//             console.log('------------------------------------------------------');

//             let { alpha, blue, green, red } = feature.color;
//             console.log(alpha, blue, green, red);
//             if (feature.tileset && feature.tileset instanceof Cesium.Cesium3DTileset) {
//               d3.slippingTileset = feature.tileset;
//               console.log(d3.slippingTileset);
//             }
//           }

//           console.log(feature);

//           let rst = d3.snBusinessHelper.reverseLookup(feature);
//           console.log(rst);
//           if (rst) {
//             let pair = d3Path2Pair(rst);
//             let pairString = `${pair.groupName}@${pair.name}`;
//             let target = this.models.find(x => x.pairString === pairString);
//             if (!this.select && !ctrlIsDown) {
//               this.selectedModels.map(item => {
//                 if (item != target) {
//                   item.isSelected = false;
//                 }
//               });
//             }
//             if (!target) {
//               target = this.getModelByPairString(pairString);
//               this.models.push(target);
//             }
//             target.isSelected = !target.isSelected;
//             console.log(target);
//             this.updateModels();
//           }
//         }
//       };
//       spaceEventHandler.setInputAction(
//         spaceEventHandlerEvent,
//         Cesium.ScreenSpaceEventType.LEFT_CLICK,
//       );

//       keydownHandle = e => {
//         let key = e.keyCode;
//         let ctrl = navigator.platform.match('Mac') ? e.metaKey : e.ctrlKey;
//         if (key == '77') {
//           ctrlIsDown = true;
//         }
//       };
//       window.addEventListener('keydown', keydownHandle);

//       keyupHandle = e => {
//         let key = e.keyCode;
//         let ctrl = navigator.platform.match('Mac') ? e.metaKey : e.ctrlKey;
//         if (key == '77') {
//           ctrlIsDown = false;
//         }
//       };
//       window.addEventListener('keyup', keyupHandle);

//       registerMouseLock(Cesium, d3Manager.viewer);
//     },
//     // 注销事件
//     uninstallEvents() {
//       let viewer = d3.viewer;
//       viewer && viewer.scene && viewer.scene.postRender && viewer.scene.postRender.removeEventListener(cameraInfoHandle);
//       spaceEventHandler && spaceEventHandler.removeInputAction(
//         spaceEventHandlerEvent,
//         Cesium.ScreenSpaceEventType.LEFT_CLICK,
//       );
//       window.removeEventListener('keydown', keydownHandle);
//       window.removeEventListener('keyup', keyupHandle);
//     },

//     /**
//      * 初始化相机飞行
//      * @param {*} fly 是否定位
//      */
//     async initCamera(fly = true) {
//       let response = await apiEquipment.getGisData({ id: this.defaultEquipment.id });
//       if (requestIsSuccess(response) && response.data) {
//         let {
//           position,
//           orientation,
//           hiddenModelPairStrings,
//           opacityModelPairStrings,
//           viewPoints,
//         } = response.data;
//         if (fly) {
//           // 定位
//           d3.scene.camera.flyTo({
//             destination: new Cesium.Cartesian3(position.x, position.y, position.z),
//             orientation,
//             duration: 1,
//           });
//         }

//         let _hiddenEquipmentPairs = hiddenModelPairStrings || [];
//         let _opacityEquipmentPairs = opacityModelPairStrings || [];
//         this.setModelState(_hiddenEquipmentPairs, ModelState.IsHidden, true);
//         this.setModelState(_opacityEquipmentPairs, ModelState.IsOpacity, true);
//         this.viewPoints = viewPoints || [];
//       } else {
//         this.hiddenModels.forEach(item => (item.isHidden = false));
//         this.opacityModels.forEach(item => (item.isHidden = false));
//         this.viewPoints = [];
//       }
//     },

//     /**
//      * 显示电缆线面板
//      * @param {*} equipmentId
//      */
//     showCableCorePanel(equipmentId) { },

//     async onSettingCamera() {
//       // 设置当前设备-模型高亮
//       this.defaultModel.isActived = true;
//       this.equipmentCameraPanelVisible = true;

//       // 初始化相机视角
//       await this.initCamera();
//       d3Manager.mode = D3Mode.SetCamera;
//     },
//     onScopeCodeChange(value) {
//       this.scopeCode = value;

//       if (this.select) {
//         this.models = this.selectedEquipments.map(equip => {
//           let { groupName, name } = equip;
//           let pairString = `${groupName}@${name}`;
//           return {
//             pairString,
//             groupName,
//             name,
//             isSelected: true,
//             isHidden: false,
//             isOpacity: false,
//             isActived: false,
//             isBlink: false,
//             blinkColor: null,
//             blinkId: null,
//           };
//         });
//       } else {
//         this.models = [];
//       }
//       this.flyToScopeCode();
//     },
//     onAlarmsChange() {
//       // 停止之前的闪烁
//       console.log('onAlarmsChange');
//       this.alarmBlinks.map(item => {
//         d3.snBusinessHelper.endFlicker(item.id);
//       });

//       if (!this.alarmsPanelVisible) {
//         return;
//       }

//       let blinks = [];
//       for (let item in AlarmLevel) {
//         let level = AlarmLevel[item];
//         let equipmentPairs = this.alarms
//           .filter(item => item.level === level)
//           .map(item => {
//             return {
//               groupName: item.equipment.group.name,
//               name: item.equipment.name,
//             };
//           });

//         // 开始闪烁
//         let id = d3.snBusinessHelper.startFlicker(
//           equipmentPairs.map(x => d3Pair2Path(x.groupName, x.name)),
//           Cesium.Color.fromCssColorString(getAlarmColor(level, true)),
//           500,
//         );

//         blinks.push({
//           id,
//           level,
//           equipmentPairs,
//         });
//       }
//       this.alarmBlinks = blinks;
//     },
//   },
//   render() {
//     return (
//       <div class="sm-d3">
//         {/* Cesium 容器 */}
//         <div class="cesium-container" id={this.cesiumConatinerId}>
//           <div id="cesium-credit"></div>
//         </div>

//         {/* 工具栏 */}
//         {
//           <ToolBar
//             axios={this.axios}
//             select={this.select}
//             // scopeCode={this.scopeCode}
//             components={this.components}
//             equipmentPanelVisible={this.equipmentPanelVisible}
//             equipmentInfoPanelVisible={this.equipmentInfoPanelVisible}
//             teminalLinkPanelVisible={this.teminalLinkPanelVisible}
//             smD3InterfacePanelVisible={this.smD3InterfacePanelVisible}
//             smD3ConstructionProgressPanelVisible={this.smD3ConstructionProgressPanelVisible}
//             emergFaultsPanelVisible={this.emergFaultsPanelVisible}
//             alarmsPanelVisible={this.alarmsPanelVisible}
//             equipmentCameraPanelVisible={this.equipmentCameraPanelVisible}
//             moreInfoPanelVisible={this.moreInfoPanelVisible}
//             cableCorePanelVisible={this.cableCorePanelVisible}
//             navCameraFlyVisible={this.navCameraFlyVisible}
//             stationSliderVisible={this.stationSliderVisible}
//             defaultEquipment={this.defaultEquipment}
//             defaultModel={this.defaultModel}
//             selectedModels={this.selectedModels}
//             hiddenModels={this.hiddenModels}
//             opacityModels={this.opacityModels}
//             onScopeCodeChange={this.onScopeCodeChange}
//             onSettingCamera={this.onSettingCamera}
//             onEquipmentPanelVisibleChange={value => {
//               this.equipmentPanelVisible = value;
//             }}
//             onSmD3InterfacePanelVisibleChange={value => {
//               this.smD3InterfacePanelVisible = value;
//             }}
//             onSmD3QualityPanelVisibleChange={value => {
//               this.smD3QualityPanelVisible = value;
//             }}
//             onSmD3SafePanelVisibleChange={value => {
//               this.smD3SafePanelVisible = value;
//             }}
//             onEquipmentInfoPanelVisibleChange={value => {
//               this.equipmentInfoPanelVisible = value;
//             }}
//             onTeminalLinkPanelVisibleChange={value => {
//               this.teminalLinkPanelVisible = value;
//               if (this.teminalLinkPanelVisible && this.emergFaultsPanelVisible) {
//                 this.emergFaultsPanelVisible = false;
//               }
//             }}
//             onEmergFaultsPanelVisibleChange={value => {
//               this.emergFaultsPanelVisible = value;
//               if (this.teminalLinkPanelVisible && this.emergFaultsPanelVisible) {
//                 this.teminalLinkPanelVisible = false;
//               }
//             }}
//             onAlarmsPanelVisibleChange={value => {
//               this.alarmsPanelVisible = value;
//             }}
//             onConstructionVisibleChange={value => {
//               this.smD3ConstructionProgressPanelVisible = value; // 进度模拟
//               if (this.teminalLinkPanelVisible && this.emergFaultsPanelVisible) {
//                 this.emergFaultsPanelVisible = false;
//               }
//             }}
//             onEquipmentCameraPanelVisibleChange={value => {
//               this.equipmentCameraPanelVisible = value;
//             }}
//             onMoreInfoPanelVisibleChange={value => {
//               this.moreInfoPanelVisible = value;
//             }}
//             onCableCorePanelVisibleChange={value => {
//               this.cableCorePanelVisible = value;
//             }}
//             onSettingCamera={async () => {
//               this.activedModels.forEach(item => (item.isActived = false));
//               if (this.defaultModel) {
//                 this.defaultModel.isActived = true;
//               }
//               this.updateModels();
//               this.equipmentCameraPanelVisible = true;
//               await this.initCamera(false);
//               let _this = this;
//               this.$nextTick(() => {
//                 _this.$refs.SmD3EquipmentCamera.refresh();
//               });
//             }}
//             onNavCameraFlyVisibleChange={async () => {
//               if (this.navCameraFlyVisible) {
//                 this.defaultModel.isActived = false;
//                 this.updateModels();
//                 this.navCameraFlyVisible = false;
//               } else {
//                 this.activedModels.forEach(item => (item.isActived = false));
//                 this.defaultModel.isActived = true;
//                 this.updateModels();
//                 this.navCameraFlyVisible = true;
//                 await this.initCamera(false);
//                 let _this = this;
//                 this.$nextTick(() => {
//                   // _this.$refs.CameraFly.start();
//                 });
//               }
//             }}
//             onStationSliderVisibleChange={visible => {
//               this.stationSliderVisible = visible;
//             }}
//             onOpacityChange={() => {
//               if (this.defaultModel) {
//                 this.defaultModel.isOpacity = !this.defaultModel.isOpacity;
//               }
//             }}
//             onHiddenChange={() => {
//               if (this.defaultModel) {
//                 this.defaultModel.isHidden = !this.defaultModel.isHidden;
//               }
//             }}
//             onShowAll={() => {
//               this.hiddenModels.map(item => (item.isHidden = false));
//               this.updateModels();
//             }}
//             onHideAll={() => {
//               this.selectedModels.map(item => (item.isHidden = true));
//               this.updateModels();
//             }}
//             onOpacityNone={() => {
//               this.opacityModels.map(item => (item.isOpacity = false));
//               this.updateModels();
//             }}
//             onOpacityAll={() => {
//               this.selectedModels.map(item => (item.isOpacity = true));
//               this.updateModels();
//             }}
//             onFlyTo={data => {
//               this.onFlyTo(data, true);
//             }}
//           />
//         }

//         {/* 弹层 */}
//         <div class="panel-layer" ref="panelLayer">
//           {/* 设备树 */}
//           <SmD3Equipments
//             axios={this.axios}
//             title="设备列表"
//             isShowLayer
//             position={{
//               left: '20px',
//               top: '20px',
//               bottom:
//                 this.teminalLinkPanelVisible ||
//                   this.emergFaultsPanelVisible ||
//                   this.equipmentCameraPanelVisible ||
//                   this.alarmsPanelVisible ||
//                   this.smD3ConstructionProgressPanelVisible ||
//                   this.smD3InterfacePanelVisible
//                   ? '360px'
//                   : '20px',
//             }}
//             visible={this.equipmentPanelVisible}
//             hiddens={this.hiddenModelPairStrings}
//             onClose={visible => (this.equipmentPanelVisible = visible)}
//             onFlyTo={data => {
//               this.onFlyTo(data, true);
//             }}
//             onVisibleChange={data => {
//               let pairString = `${data.equipmentGroupName}@${data.equipmentName}`;
//               let model = this.models.find(x => x.pairString === pairString);
//               let hidden = !!model && model.isHidden;
//               this.setModelState(pairString, ModelState.IsHidden, !hidden, true);
//             }}
//             value={this.selectedModelPairStrings}
//             scopeCode={this.scopeCode}
//             onChange={(value, equipments) => {
//               this.setModelState(value, ModelState.IsSelected, true);
//               if (this.select) {
//                 this.$emit("selectedEquipmentsChange", equipments);
//               }
//             }}
//           />

//           {/* 设备信息面板 */}
//           <SmD3EquipmentInfo
//             axios={this.axios}
//             position={{
//               right: '20px',
//               top: '20px',
//               bottom: this.cableCorePanelVisible ? '360px' : '20px',
//             }}
//             visible={this.equipmentInfoPanelVisible}
//             equipment={this.defaultEquipment}
//             onClose={visible => (this.equipmentInfoPanelVisible = visible)}
//             onCableCoreDetial={() => {
//               if (!this.cableCorePanelVisible) {
//                 this.cableCorePanelVisible = true;
//               }
//             }}
//           />

//           {/* 配线信息 */}
//           {this.components.indexOf('TeminalLink') > -1 && !this.select ? (
//             <SmD3TeminalLink
//               ref="SmD3TeminalLink"
//               equipment={this.defaultEquipment}
//               axios={this.axios}
//               visible={this.teminalLinkPanelVisible}
//               onClose={visible => {
//                 this.teminalLinkPanelVisible = visible;
//                 if (this.emergFaultsPanelVisible) {
//                   this.emergFaultsPanelVisible = false;
//                 }
//               }}
//               onSelect={pair => {
//                 let pairString = `${pair.groupName}@${pair.name}`;
//                 // 设备高亮选中
//                 this.setModelState(pairString, ModelState.IsSelected, true, true);
//               }}
//               onFlyTo={data => this.onFlyTo(data, true)}
//               height="320px"
//               width="780px"
//               position={this.teminalLinkPanelPosition}
//             />
//           ) : (
//             undefined
//           )}
//           {/* 接口管理 */}
//           {!this.select ?
//             <SmD3Interface
//               permissions={this.permissions}
//               axios={this.axios}
//               interfancePosition={this.interfancePosition}
//               visible={this.smD3InterfacePanelVisible}
//               onClose={visible => {

//                 this.smD3InterfacePanelVisible = visible;
//               }}
//               onFlyTo={data => {
//                 let { lon, lat, alt } = JSON.parse(data.position) ;
//                 let position = Cesium.Cartesian3.fromDegrees(lon, lat, alt);

//                 /* 清除所有标签 */
//                 d3.snDynamicRectLabelHelper.clear();
//                 /* 添加单个动态标签 */
//                 const labelInfo = {
//                   position,
//                   text: data.name,
//                   // color: state === MarkType.Qualified ? Cesium.Color.LIME :
//                   //   state === MarkType.NoQualified ? Cesium.Color.RED :
//                   //     state === MarkType.NoCheck ? Cesium.Color.GOLDENROD : '',
//                   fontColor: Cesium.Color.WHITE,
//                 };
//                 d3.snDynamicRectLabelHelper.add(labelInfo);


//                 d3.scene.camera.flyTo({
//                   destination: position,
//                   // orientation,
//                   duration: 1,
//                 });
//               }}
//               height="320px"
//               width="840px"
//               position={this.d3InterfacePanelPosition}

//             /> : undefined
//           }
//           {/* 安全问题管理 */}
//           {!this.select ?
//             <SmD3Safe
//               permissions={this.permissions}
//               axios={this.axios}
//               visible={this.smD3SafePanelVisible}
//               onClose={visible => {

//                 this.smD3SafePanelVisible = visible;
//               }}
//               onFlyTo={data => {
//                 this.onFlyTo(data, true);
//               }}
//               height="320px"
//               width="840px"
//               position={this.d3InterfacePanelPosition}

//             /> : undefined
//           }
//           {/* 质量问题管理 */}
//           {!this.select ?
//             <SmD3Quality
//               permissions={this.permissions}
//               axios={this.axios}
//               visible={this.smD3QualityPanelVisible}
//               onClose={visible => {

//                 this.smD3QualityPanelVisible = visible;
//               }}
//               onFlyTo={data => {
//                 this.onFlyTo(data, true);
//               }}
//               height="320px"
//               width="840px"
//               position={this.d3InterfacePanelPosition}

//             /> : undefined
//           }
//           {/* 相机视角 */}
//           {!this.select ?
//             <SmD3EquipmentCamera
//               equipment={this.defaultEquipment}
//               axios={this.axios}
//               visible={this.equipmentCameraPanelVisible}
//               hiddenModelPairStrings={this.hiddenModelPairStrings}
//               opacityModelPairStrings={this.opacityModelPairStrings}
//               ref="SmD3EquipmentCamera"
//               viewPoints={this.viewPoints}
//               onClose={visible => {
//                 this.equipmentCameraPanelVisible = visible;
//                 d3Manager.mode = D3Mode.Select;

//                 // 清空激活模型
//                 this.activedModels.map(item => (item.isActived = false));
//                 this.updateModels();
//               }}
//               onFlyTo={data => {
//                 this.onFlyTo(data, true);
//               }}
//               onRemove={(pair, type) => {
//                 if (type === 'hide') {
//                   this.getModelByPairString(pair).isHidden = false;
//                 } else if (type === 'opacity') {
//                   this.getModelByPairString(pair).isOpacity = false;
//                 }
//                 this.updateModels();
//               }}
//               onViewPointsChange={data => {
//                 this.viewPoints = data;
//               }}
//               onRefresh={this.initCamera}
//               height="320px"
//               width="780px"
//               position={this.teminalLinkPanelPosition}
//             /> : undefined
//           }

//           {/* 线芯详情 */}
//           {!this.select ?
//             <SmD3CableCores
//               axios={this.axios}
//               visible={this.cableCorePanelVisible}
//               onClose={visible => {
//                 this.cableCorePanelVisible = visible;
//               }}
//               height="320px"
//               position={this.cableCorePanelPosition}
//               equipment={this.defaultEquipment}
//               onFlyTo={data => this.onFlyTo(data, true)}
//               onBusinessPathClick={id => {
//                 this.teminalLinkPanelVisible = true;
//                 this.$nextTick(() => {
//                   this.$refs.SmD3TeminalLink.initTerminalBusinessPath(id);
//                 });
//               }}
//             /> : undefined
//           }

//           {/* 故障应急面板 */}
//           {this.components.indexOf('EmergFaults') > -1 && !this.select ? (
//             <SmD3EmergFaults
//               axios={this.axios}
//               visible={this.emergFaultsPanelVisible}
//               onClose={visible => {
//                 this.emergFaultsPanelVisible = visible;
//                 if (this.teminalLinkPanelVisible) {
//                   this.teminalLinkPanelVisible = false;
//                 }
//               }}
//               onFlyTo={data => this.onFlyTo(data, true)}
//               onView={item => this.$emit('view', item)}
//               onProcess={id => this.$emit('process', id)}
//               onAllView={() => this.$emit('allView')}
//               onApplyView={id => this.$emit('applyView', id)}
//               height="320px"
//               width="780px"
//               position={this.emergFaultsPanelPosition}
//             />
//           ) : (
//             undefined
//           )}

//           {/* 集中告警 */}
//           {this.components.indexOf('Alarms') > -1 && !this.select ? (
//             <SmD3Alarms
//               axios={this.axios}
//               signalr={this.signalr}
//               systems={this.systems}
//               visible={this.alarmsPanelVisible}
//               onClose={visible => {
//                 this.alarmsPanelVisible = visible;
//               }}
//               onAlarmsChange={alarms => {
//                 this.alarms = alarms;
//               }}
//               onEquipmentClick={equipment => {
//                 this.onFlyTo({ groupName: equipment.group.name, name: equipment.name }, true);
//               }}
//               height="320px"
//               width="780px"
//               position={this.emergFaultsPanelPosition}
//             />
//           ) : (
//             undefined
//           )}

//           {/* 车站选择面板 */}
//           <SmD3StationSlider
//             axios={this.axios}
//             scopeCode={this.scopeCode}
//             visible={
//               this.stationSliderVisible &&
//               !this.teminalLinkPanelVisible &&
//               !this.emergFaultsPanelVisible &&
//               !this.equipmentCameraPanelVisible &&
//               !this.alarmsPanelVisible &&
//               !this.smD3InterfacePanelVisible &&
//               !this.smD3ConstructionProgressPanelVisible &&
//               !this.cableCorePanelVisible
//             }
//             onChange={this.onScopeCodeChange}
//             theme="dark"
//           />
//           {/* 进度模拟 */}
//           <SmD3ConstructionProgress
//             axios={this.axios}
//             visible={this.smD3ConstructionProgressPanelVisible}
//             theme="dark"
//             height="320px"
//             onInit={(data) => {
//               console.log(data);
//             }}
//             onChange={(data) => {
//               data.map(item => {
//                 let path = d3Pair2Path(item.group, item.equipmentName);
//                 this.simulationEqupmentPaths.push(path);
//                 d3Manager.snBusinessHelper.setColor([path], Cesium.Color.RED.withAlpha(0.8));
//                 d3Manager.snBusinessHelper.flyTo(path, 1);
//               });
//               console.log(data);
//             }}
//             onClose={visible => {
//               this.smD3ConstructionProgressPanelVisible = visible;
//             }}
//             // width="840px"
//             onSuccess={(data) => {
//               // 模拟启动开始，影藏所有要模拟的设备
//               if (data) {
//                 let paths = data.map(item => d3Pair2Path(item.group, item.equipmentName));
//                 d3Manager.snBusinessHelper.setVisible(paths, false);
//               }
//             }}
//             position={this.d3ConstructionProgressPosition}
//           />
//         </div>

//         <CameraFly
//           viewPoints={this.viewPoints}
//           visible={this.navCameraFlyVisible}
//           onVisibleChange={value => {
//             this.navCameraFlyVisible = value;
//           }}
//           ref="CameraFly"
//         />

//         {/* 状态栏 */}
//         <StatusBar
//           status={this.cameraInfo.status}
//           progress={this.cameraInfo.progress}
//           distance={this.cameraInfo.distance}
//           position={this.cameraInfo.position}
//           direction={this.cameraInfo.direction}
//           performance={this.cameraInfo.performance}
//         />
//       </div>
//     );
//   },
// };
