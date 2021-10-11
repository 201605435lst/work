
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiEntity from '../../sm-api/sm-namespace/Entity';
import SmVideoBase from './src/SmVideoBase';
let apiEntity = new ApiEntity();

export default {
  name: 'SmVideo',
  props: {
    title: { type: String, default: '' },
    visible: { type: Boolean, default: ()=>false },
    url:{type: String, default: ''},
    width:{type: Number, default: ()=>500},
    height:{type: Number, default: ()=>500},
  },
  data() {
    return {
      iVisible:false,
      iTitle:'',
      iUrl:'',
    };
  },
  computed: {},
  watch:{
    visible: {
      handler(nVal, oVal) {
        this.iVisible= nVal;
      },
      immediate: true,
    },
    title: {
      handler(nVal, oVal) {
        this.iTitle= nVal;
      },
      immediate: true,
    },
    url: {
      handler(nVal, oVal) {
        this.iUrl= nVal;
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
    async refresh() { },
    close(){
      this.iVisible=false;
      //this.$refs.SmVideoBase.close();
    },
    ok(){
      this.iVisible=false;
    },
    preview(visible,url,title){
      this.iUrl=url;
      this.iVisible=visible;
      this.iTitle=title;
      setTimeout(() => {
        this.$refs.SmVideoBase.play(url);
      }, 1);
    },
  },
  render() {
    return (
      <div class="sm-video">
        <a-modal
          title={this.iTitle}
          onOk={this.ok}
          class="sm-import-modal"
          visible={this.iVisible}
          onCancel={this.close}
          width={this.width}
        >
          <div>
            <SmVideoBase
              ref='SmVideoBase'
              url={this.iUrl}
              width={`${this.width-45}px`}
              height={`${this.height-20}px`}
            />
          </div>
        </a-modal>
      </div>
    );
  },
};
