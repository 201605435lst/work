import './style';
import { InitMapScript } from './plugin/map';
import amap from './plugin/amap';
export default {
  name: 'SmMapControl',
  props: {
    height: { type: Number, default: 400 }, // 地图高度
    mode: { type: String, default: 'view' }, // 模式，view:普通查看，可定位和传值标注。mark :点击地图，进行标注。
    position: { type: Array, default: () => [] }, // 初始化的点
    zoom: { type: Number, default: 10 }, // 缩放范围
    showPoint: { type: Boolean, default: false }, // 是否显示点标记
    visible:{type:Boolean,default:false},
    singlePoint:{type:Boolean,default:false},// 是否为单一的点
  },
  data() {
    return {
      map: null,
      container: '',
      iZoom:0,
      iVisible:false,
      iPosition:[],
    };
  }, 
  computed: {},
  watch:{
    zoom: {
      handler(nVal, oVal) {
        this.iZoom = nVal;
      },
      immediate: true,
    },
    visible: {
      handler(nVal, oVal) {
        this.iVisible= nVal;
      },
      immediate: true,
    },
    position: {
      handler(nVal, oVal) {
        this.iPosition = nVal;
        if(nVal!=null){
          this.location();
        }
      },
      immediate: true,
    },
  },
  async created() {
    let timestamp = new Date().getTime();
    this.container = 'mapContainer_' + timestamp;
  },
  mounted() {
    InitMapScript().then(() => {
      this.initMap();
    });
  },
  methods: {
    initMap() {
      this.map = new amap({
        container: this.container,
        zoom: 13,
      });
      this.location();
      if (this.mode === 'mark') {
        // 注册点击事件
        this.map.addListener('click', this.mapClick);
      }
    },
    async refresh() {},
    mapClick(point) {
      this.map.createMarker('mark',point,null,this.singlePoint);
      this.$emit('click',point);
    },
    location(){
      if (this.showPoint&&this.iVisible) {
        // 添加标记
        this.map.createMarker('init', this.iPosition,'',this.singlePoint);
        this.map.setCenter(this.iPosition);
      }
    },
  },
  render() {
    return (
      <div class="sm-map-control" style={{display:this.iVisible?'block':'none'}}>
        <div id={this.container} class="map-container" style={`height:${this.height}px;`}></div>
      </div>
    );
  },
};
