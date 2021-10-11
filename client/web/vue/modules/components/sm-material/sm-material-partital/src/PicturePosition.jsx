/**
 * 说明：图片相对位置管理
 * 作者：easten
 */

export default {
  name: 'PicturePosition',
  components: {},
  props: {
    src: { type: String, default: '' },
    visible: { type: Boolean, default: false },
    position: { type: Array, default: () => [] },
    height: { type: Number, default: 500},
    view: { type: Boolean, default: false },
  },
  data() {
    return {
      iVisible: false,
      iSrc: '',
      iPosition: [],
      showIcon:false,
      x:0,
      y:0,
    };
  },
  computed: {
  },
  watch: {
    visible: {
      handler(nVal, oVal) {
        this.iVisible = nVal;
      },
      immediate: true,
    },
    src: {
      handler(nVal, oVal) {
        this.iSrc = nVal;
      },
      immediate: true,
    },
    position: {
      handler(nVal, oVal) {
        if((nVal[0]!=undefined&&nVal[1]!=undefined)&&(nVal[0]!=0&&nVal[1]!=0)){
          this.showIcon=true;
          this.getPoint();
        }else{
          this.showIcon=false;
        }
        this.iPosition = nVal;

       
      },
      immediate: true,
    },
  },  
  created() {
    if(this.view){
      this.showIcon=false;
    }
  },
  async mounted(){
  },
  methods: {
    async getPoint(){
      setTimeout(async () => {
        let p= await this.getClientPosition(); 
        this.x=this.iPosition[0]*p[0]-9;
        this.y=this.iPosition[1]*p[1]-18;
      }, 20);
    },
    getClientPosition(){
      return new Promise((res,rej)=>{
        while (this.$refs.pic_container==undefined) {}
        res([this.$refs.pic_container.clientWidth,this.$refs.pic_container.clientHeight]);
      });
    },
    async picClick(e){      
      if(!this.view){
        this.showIcon=true;
        let x=e.layerX;
        let y=e.layerY;
        let lx=parseFloat(x/this.$refs.pic_container.clientWidth).toFixed(3);
        let ly=parseFloat(y/this.$refs.pic_container.clientHeight).toFixed(3);
        this.iPosition=[lx,ly];
        this.$emit('click',this.iPosition);
        this.getPoint();
      }
    },
  },
  render() {
    return (
      <div class="pic-container" ref='pic_container' style={{ display: this.iVisible ? 'block' : 'none',height:`${this.height}px`}} onClick={this.picClick}>
        <img width="100%" height="100%" src={this.iSrc} alt=""/>
        <a-icon
          type="environment"
          class="environment"
          style={{
            position: 'absolute',
            color:'red',
            top: `${this.y}px`,
            left: `${this.x}px`,
            display:this.showIcon?'unset':'none',
            fontSize:'20px',
          }}
        />
      </div>
    );
  },
};
