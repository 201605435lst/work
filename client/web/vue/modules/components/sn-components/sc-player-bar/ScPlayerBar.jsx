import './style';
import ApiEntity from '../../sm-api/sm-namespace/Entity';

let apiEntity = new ApiEntity();

// 播放条组件
export default {
  name: 'ScPlayerBar',
  props: {
    axios: { type: Function, default: null },
    max:{type:Number, default: 0}, // 条条最大值
    value:{type:Number, default: 0}, // 条条值(小圆点当前处于那个地方)
    toolTipText:{type:String, default:''}, // 条条显示文本(小圆点 tootip 悬浮文字)
  },
  data() {
    return {
      icon_type: 'play-circle',
      sliderTime: 200,
      iValue:0,
      audio: {
        // 该字段是音频是否处于播放状态的属性
        playing: false,
        // 音频当前播放时长
        currentTime: 0,
        // 音频最大播放时长
        maxTime: 0,
        // 倍速
        playbackRate: 1,
      },
    };
  },
  computed: {
    tooltipVisible() {
      return this.iValue !== 0;
    },
  },

  watch: {
    value: {
      handler: function(val, oldValue) {
        // console.log('watch_value',val);
        this.iValue = val;
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
      apiEntity = new ApiEntity(this.axios);
    },
    formatValue() {
      return this.toolTipText;
    },
    async refresh() { },
    // 控制音频的播放与暂停
    startPlayOrPause() {
      if (this.icon_type === 'pause-circle') {
        this.icon_type = 'play-circle';
      } else {
        this.icon_type = 'pause-circle';
      }
      this.audio.playing = !this.audio.playing;
      return this.audio.playing ? this.play() : this.pause();
    },
    // 播放完成
    playComplete() {
      this.icon_type = 'play-circle';
      this.audio.playing = false;
    },
    // 播放
    play() {
      this.$emit('play');
    },
    // 暂停
    pause() {
      this.$emit('pause');
    },
    // 当音频播放
    onPlay() {
      this.audio.playing = true;
    },
    // 当音频暂停
    onPause() {
      this.audio.playing = false;
    },
    // 当加载语音流元数据完成后，会触发该事件的回调函数
    // 语音元数据主要是语音的长度之类的数据
    onLoadedmetadata(res) {
      this.audio.maxTime = parseInt(res.target.duration);
    },
    // 当音频当前时间改变后，进度条也要改变
    onTimeupdate(res) {
      this.audio.currentTime = parseInt(res.target.currentTime);
      this.sliderTime = this.audio.currentTime;
      if (this.audio.currentTime == this.audio.maxTime) {
        // this.onPause();
        this.startPlayOrPause();
      }
    },
    // changeSpeed() {
    //   if (this.audio.playbackRate == 2) {
    //     this.audio.playbackRate = 1;
    //   } else {
    //     this.audio.playbackRate += 0.5;
    //   }
    //   this.$refs.audio.playbackRate = this.audio.playbackRate;
    // },
  },
  render() {
    return (
      <div class="sc-player-bar">
        {/*音频播放控件*/}
        <div class="out-box">
          <span class="box">
            <a-icon
              type={this.icon_type }
              onClick={this.startPlayOrPause }
              style="font-size:25px;color:#40a9ff"
              theme="filled"
            />
          </span>
          <span class="box slider">
            <a-slider
              onChange={val=> {
                this.iValue = val;
                this.$emit('change', val);
              }}
              value={this.iValue}
              class='slider'
              step={1}
              max={this.max}
              min={0}
              id='test'
              tipFormatter={this.formatValue}
              tooltipVisible={this.tooltipVisible}
              default-value={0}
              style='width:100%'
            />
          </span>
          {/*<span*/}
          {/*  class="box"*/}
          {/*  style="margin-left:10px"*/}
          {/*>{ this.formatSecond(this.audio.currentTime) } / {this.formatSecond(this.audio.maxTime)}</span>*/}
          {/* <div class="speed box">
            <span>设置</span>
          </div> */}
        </div>
      </div>
    );
  },
};
