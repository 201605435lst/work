
import './CameraFly.less';
import d3 from '../d3Manager';
import { CameraFlyState } from '../../../../_utils/enum';
import { cameraFlyByPoints } from '../../src/utils/cesium-utils';
let cameraChangedHandle = null;
let keydownHandle = null;

export default {
  name: 'CameraFly',
  props: {
    viewPoints: { type: Array, default: () => [] },
    visible: { type: Boolean, default: false },
  },
  data() {
    return {
      iVisible: false,
      flyState: CameraFlyState.Stoped,
      flyIndex: 0,
      cameraLastViewPoint: null,
      speeds: [0.3, 0.5, 1, 1.5, 2, 3],
      speedIndex: 2,
    };
  },
  computed: {
    distanceTotal: function () {
      let distance = 0;
      if (this.viewPoints.length > 0) {
        distance = this.getLengthByViewPoint(this.viewPoints[this.viewPoints.length - 1]);
      }
      return distance;
    },
    current: function () {
      let current = 0;
      if (this.flyIndex > 0 && this.viewPoints.length) {
        current = this.getLengthByViewPoint(this.viewPoints[this.flyIndex]);
      }
      return current;
    },
    percent: function () {
      return this.distanceTotal == 0 ? 0 : this.current / this.distanceTotal;
    },
    marksViewPoints: function () {
      let masks = [];
      this.viewPoints.forEach(item => {
        let percent = (this.getLengthByViewPoint(item) / this.distanceTotal).toFixed(2) * 100;
        masks.push({
          key: parseInt(percent),
          viewPoint: item,
        });
      });
      return masks;
    },
    marks: function () {
      let masks = {};
      this.marksViewPoints.forEach(item => {
        masks[item.key] = item.key;
      });
      return masks;
    },
  },
  watch: {
    viewPoints: function () {
      this.flyState = CameraFlyState.Stoped;
      this.flyIndex = 0;
      this.installEvents();
    },
    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
        if (!this.visible) {
          this.close();
        }
      },
    },
  },
  created() {
  },
  destroyed() {
    this.unInstallEvents();
  },
  methods: {
    installEvents() {
      if (cameraChangedHandle != null) { return; }

      cameraChangedHandle = (data) => {
        if (this.flyState == CameraFlyState.Flying) {
          this.updateCameraLastViewPoint();
        }
      };
      d3.scene.camera.changed.addEventListener(cameraChangedHandle, 'CameraFly');
      keydownHandle = (evt) => {
        if (this.visible) {
          let key = evt.keyCode;
          let stop = false;
          switch (key) {
          // 空格
          case 32:
            this.start();
            stop = true;
            break;

            // 左箭头
          case 37:
            this.setFlyIndex(this.flyIndex - 1);
            stop = true;
            break;

            // 右箭头
          case 39:
            this.setFlyIndex(this.flyIndex + 1);
            stop = true;
            break;

            // 上
          case 38:
            this.setSpeedIndex(this.speedIndex + 1);
            stop = true;
            break;

            // 下
          case 40:
            this.setSpeedIndex(this.speedIndex - 1);
            stop = true;
            break;

          default:
            stop = false;
          }

          if (stop) {
            evt.stopPropagation();
            evt.preventDefault();
          }
        }
      };
      window.addEventListener(
        "keydown",
        keydownHandle,
      );
    },
    unInstallEvents() {
      d3.scene && d3.scene.camera && d3.scene.camera.changed.removeEventListener(cameraChangedHandle, 'CameraFly');
      window.removeEventListener("keydown", keydownHandle);
      cameraChangedHandle = null;
      keydownHandle = null;
    },
    updateCameraLastViewPoint() {
      let { position, heading, pitch, roll } = d3.scene.camera;
      this.cameraLastViewPoint = JSON.parse(JSON.stringify({
        position,
        orientation: {
          heading,
          pitch,
          roll,
        },
      }));
    },
    start() {
      this.updateCameraLastViewPoint();
      let fly = () => {
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
          this.speeds[this.speedIndex],
          this.flyIndex);
      };

      if (this.flyState != CameraFlyState.Flying) {
        if (this.flyState == CameraFlyState.Pause && this.cameraLastViewPoint) {
          let last = this.cameraLastViewPoint;
          let duration = Cesium.Cartesian3.distance(
            new Cesium.Cartesian3.clone(last.position),
            new Cesium.Cartesian3.clone(d3.scene.camera.position),
          ) * 0.1;
          duration = Math.min(duration, 1);
          d3.scene.camera.flyTo({
            destination: Cesium.Cartesian3.clone(last.position),
            orientation: last.orientation,
            duration,
          });
          setTimeout(() => {
            fly();
          }, duration * 1000);
        } else {
          fly();
        }

      } else {
        this.flyState = CameraFlyState.Pause;
        let { position, heading, pitch, roll } = d3.scene.camera;
        this.cameraLastViewPoint = JSON.parse(JSON.stringify({
          position,
          orientation: {
            heading,
            pitch,
            roll,
          },
        }));
        d3.scene.camera.cancelFlight();
      }
    },
    close() {
      this.iVisible = false;
      this.flyState = CameraFlyState.Stoped;
      this.flyIndex = 0;
      d3.scene.camera.cancelFlight();
      this.$emit('visibleChange', this.iVisible);
    },
    getLengthByViewPoint(curent, addCamera = false) {
      let length = 0;
      let endIndex = this.viewPoints.indexOf(curent);
      this.viewPoints.forEach((item, index) => {
        if (index > 0 && index <= endIndex) {
          length += Cesium.Cartesian3.distance(Cesium.Cartesian3.clone(this.viewPoints[index - 1].position), Cesium.Cartesian3.clone(this.viewPoints[index].position));
        }
      });

      // 计算相机
      if (addCamera && this.cameraLastViewPoint) {
        length += Cesium.Cartesian3.distance(Cesium.Cartesian3.clone(this.cameraLastViewPoint.position), Cesium.Cartesian3.clone(curent.position));
      }
      return length;
    },
    setFlyIndex(index) {
      if (index < 0) {
        index = this.viewPoints.length - 1;
      }
      if (index > this.viewPoints.length - 1) {
        index = 0;
      }
      this.flyIndex = index;

      let target = this.viewPoints[index];
      switch (this.flyState) {
      case CameraFlyState.Stoped:
      case CameraFlyState.Pause:
        // 如果已经停止，设置index及摄像机

        d3.scene.camera.flyTo({
          destination: Cesium.Cartesian3.clone(target.position),
          orientation: target.orientation,
          duration: 0.5,
        });
        break;

      case CameraFlyState.Flying:
        // 如果飞行中，先暂停，然后设置index，然后启动
        this.start();
        this.start();
        break;

      default:

        break;
      }
    },
    setSpeedIndex(index) {
      if (index > this.speeds.length - 1) {
        index = 0;
      }
      else if (index < 0) {
        index = this.speeds.length - 1;
      }
      this.speedIndex = index;
      if (this.flyState === CameraFlyState.Flying) {
        this.start();
        this.start();
      }
    },
  },
  render() {
    return <div class="camera-fly" >
      <div
        class="box"
        onClick={(event) => {
          event.preventDefault();
          event.stopPropagation();
        }}
        style={{
          display: this.iVisible ? 'flex' : 'none',
        }}>

        <span class="btn"
          onClick={(event) => {
            this.start();
          }}
        >
          <a-icon type={this.flyState === CameraFlyState.Flying ? "pause" : "caret-right"} />
        </span>

        <span class="btn"
          onClick={() => {
            this.setSpeedIndex(this.speedIndex + 1);
          }}
        >
          {this.speeds[this.speedIndex]} x
        </span>


        <a-slider value={this.flyIndex === 0 ? 0 : this.percent * 100}
          step={null}
          marks={this.marks}
          tooltipVisible={false}
          onChange={(value) => {
            let target = this.marksViewPoints.find(x => x.key === value);
            if (!target) {
              return;
            }
            let index = this.viewPoints.indexOf(target.viewPoint);
            this.setFlyIndex(index);
          }} />


        <span class="btn"
          onClick={() => {
            this.close();
          }}
        > <a-icon type="close" /></span>
      </div>
    </div>;
  },
};
