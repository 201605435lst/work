import Video from 'video.js';
import 'video.js/dist/video-js.css';
import '../style';
export default {
  name: 'SmVideoBase',
  components: {},
  props: {
    url: { type: String, default: '' },
    controls: { type: Boolean, default: () => true },// 是否显示控制条
    muted: { type: Boolean, default: () => true },// 是否静音播放
    autoplay: { type: Boolean, default: () => true },// 是否自动播放
    preload: { type: String, default: 'auto' },// 是否下载
    width: { type: String, default: '960px' },// 宽度
    height: { type: String, default: '522px' },// 高度
    playbackRates: { type: Array, default: () => [0.5, 1, 1.5, 2] },// 倍速
    poster: { type: String, default: 'https://static.shuxuejia.com/img/video_image.png' },
  },
  data() {
    return {
      player: null,
      source:'',
    };
  },
  computed: {},
  watch: {
    url: {
      handler(nVal, oVal) {
        this.source = nVal;
      },
      immediate: true,
    },
  },
  created() {
    setTimeout(() => {
      if(this.source!==''){
        this.createPlayer();
      }
    }, 1);
  },
  beforeDestroy() {
    if (this.player) {
      this.player.dispose();
    }
  },
  methods: {
    createPlayer() {
      let id = this.$refs.video;
      this.player = Video(id, {
        controls: this.controls,
        muted: this.muted,
        autoplay: this.autoplay,
        preload: this.preload,
        width: this.width,
        height: this.height,
        playbackRates: this.playbackRates,
        sources: [{
          src: this.source,
        }],
      }, () => {
      });
    },
    // 播放视频
    play(url) {
      // this.close();
      // this.source = url;
      // console.log(url);
      // if(this.player==null){
      //   setTimeout(() => {
      //     if(this.source!==''){
      //       this.createPlayer();
      //       this.playVideo(url);
      //     }
      //   }, 1);
      // }else{
      //   this.playVideo(url);
      // }
      this.player && this.player.src({
        src:url,
      });
      this.player && this.player.play();
      
    },
    playVideo(url){
      if(this.player){
        this.player.src(url);
        this.player.play();
      }
    },
    close(){
      if (this.player) {
        // this.player.dispose();
      }
    },
  },
  render() {
    return <div ref="test">
      {this.source === '' ? (<div class="no-vide" style={{ 'width': this.width, 'height': this.height }}>
        暂无视频
      </div>) :
        <video ref="video" id="video" class="video-js vjs-default-skin vjs-big-play-centered" controls preload="none">
        </video>}
    </div>;
  },
};
