import ApiComponentQrCode from '../../sm-api/sm-resource/ComponentQrCode';
import { requestIsSuccess } from '../../_utils/utils';
let apiComponentQrCode = new ApiComponentQrCode();
// 图片预览组件
export default {
  name: 'SmQrCodeImageModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      iTitle:'',//标题
      iVisible:false,// 可见性
      imgSrc:null,
    };
  },
  computed: {
    visible() {
      return this.iVisible;
    },
    title(){
      return this.iTitle;
    },
  },
  watch: {},
  created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiComponentQrCode = new ApiComponentQrCode(this.axios);
    },
    close(){
      this.iVisible=false;
      this.imgSrc=null;
      this.iTitle=null;
    },
    async view(record){
      this.iVisible=true;
      this.iTitle=record.name;
      let response = await apiComponentQrCode.getView(record?record.id:'');
      if (requestIsSuccess(response)) {
        this.imgSrc='data:image/png;base64,'+response.data;
      }
    },
  },
  render() {
    return (
      <a-modal
        visible={this.visible}
        title={this.title}
        closable
        footer={false}
        width={600}
        onCancel={this.close}
      >
        <div>
          <img width="100%" src={this.imgSrc} alt={this.iTitle}/>
        </div>
      </a-modal>
    );
  },
};
