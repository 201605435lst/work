import './status-bar.less';

export default {
  name: 'StatusBar',
  props: {
    // 状态
    status: { Type: String, default: null },
    // 进度
    progress: { Type: Number, default: null },
    // 比例
    distance: {
      Type: Object,
      default: () => {
        return {
          width: null,
          distance: null,
          distanceRound: null,
          distanceRoundUnit: null,
        };
      },
    },
    // 相机位置
    position: {
      Type: Object,
      default: () => {
        return {
          latitude: null,
          longitude: null,
          height: null,
        };
      },
    },
    // 相机方向
    direction: {
      Type: Object,
      default: () => {
        return {
          heading: null,
          pitch: null,
          roll: null,
        };
      },
    },
    performance: {
      Type: Object,
      default: () => {
        return {
          ms: null,
          fps: null,
        };
      },
    },
  },
  data() {
    return {};
  },
  computed: {},
  render() {
    let height = this.position.height;
    let unit = 'm';
    if (height >= 1000) {
      height = height / 1000;
      unit = 'km';
    }

    return (
      <div class="status-bar">
        <div class="left">
          {this.distance && this.distance.width ? (
            <div class="distance item">
              <div class="label">比例：</div>
              <div class="value" style={{ width: this.distance.width * 1.5 + 'px' }}>
                {`${this.distance.distance.toFixed(2)} ${this.distance.distanceRoundUnit}`}
              </div>
            </div>
          ) : (
            undefined
          )}
        </div>

        <div class="center">
          {!!this.progress ? (
            <div class="item progress">
              <a-progress percent={this.progress} /> <div class="value">{this.status}</div>
            </div>
          ) : (
            undefined
          )}
        </div>

        <div class="right">
          {this.position && this.position.latitude !== undefined && this.position.latitude != null
            ? [
              <div class="item">
                <div class="label">经度：</div>
                <div class="value">{this.position.latitude.toFixed(2)}°</div>
              </div>,
              <div class="item">
                <div class="label">纬度：</div>
                <div class="value">{this.position.longitude.toFixed(2)}°</div>
              </div>,
              <div class="item">
                <div class="label">视高：</div>
                <div class="value">{`${height.toFixed(2)} ${unit}`}</div>
              </div>,
            ]
            : undefined}

          {this.direction && this.direction.heading !== undefined && this.direction.heading != null
            ? [
              <a-divider type="vertical"></a-divider>,
              <div class="item">
                <div class="label">航向角：</div>
                <div class="value">{this.direction.heading.toFixed(2)}°</div>
              </div>,
              <div class="item">
                <div class="label">俯仰角：</div>
                <div class="value">{this.direction.pitch.toFixed(2)}°</div>
              </div>,
              <div class="item">
                <div class="label">翻滚角：</div>
                <div class="value">{this.direction.roll.toFixed(2)}°</div>
              </div>,
            ]
            : undefined}

          {this.performance && this.performance.ms !== undefined && this.performance.ms != null
            ? [
              <a-divider type="vertical"></a-divider>,

              <div class="item">
                <div class="label">MS：</div>
                <div class="value">
                  {isNaN(Number(this.performance.ms))
                    ? Number(this.performance.ms).toFixed(2)
                    : this.performance.ms}
                </div>
              </div>,
              <div class="item">
                <div class="label">FPS：</div>
                <div class="value">
                  {isNaN(Number(this.performance.fps))
                    ? Number(this.performance.fps).toFixed(2)
                    : this.performance.fps}
                </div>
              </div>,
            ]
            : undefined}
        </div>
      </div>
    );
  },
};
