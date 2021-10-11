import alarms_audio_base64 from './alarms_audio';
import { localStorage as lsConfig } from './config';

export default {
  name: 'SmD3AlarmsAudio',
  props: {
    alarmCount: { type: Number, default: 0 },
  },
  data() {
    return {
      autoPlay: true,
    };
  },
  watch: {
    alarmCount: function(value, oldValue) {
      this.$nextTick(()=>{
        this.checkPlay();
      });
    },
  },
  mounted() {
    let autoPlay = localStorage.getItem(lsConfig.alramAudioAutoPlay);
    this.autoPlay = autoPlay == 'true';
  },
  methods: {
    checkPlay() {
      if (this.autoPlay && this.alarmCount) {
        this.$refs.audio.play();
      } else {
        this.$refs.audio.pause();
      }
    },
  },
  render() {
    return (
      <span>
        <span
          title="自动播放"
          class="header-btn"
          onClick={() => {
            let autoPlay = localStorage.getItem(lsConfig.alramAudioAutoPlay);
            if (autoPlay == 'true') {
              autoPlay = false;
            } else {
              autoPlay = true;
            }
            localStorage.setItem(lsConfig.alramAudioAutoPlay, autoPlay);
            this.autoPlay = autoPlay;
            this.$nextTick(() => {
              this.checkPlay();
            });
          }}
        >
          <a-icon
            style={{ opacity: this.autoPlay ? 1 : 0.6, color: this.alarmCount ? 'red' : 'inherit' }}
            type={'audio'}
          />
        </span>

        {/* <span
            title="播放或停止"
            class="header-btn"
            onClick={() => {
              this.$emit('flyTo', {
                groupName: this.iEquipment.group.name,
                name: this.iEquipment.name,
              });
            }}
          >
            <a-icon type="environment" />
          </span> */}

        <audio
          loop={true}
          ref="audio"
          autoPlay={this.autoPlay}
          src={alarms_audio_base64}
        ></audio>
      </span>
    );
  },
};
