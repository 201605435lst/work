/**
 * 说明：二维码组件
 * 作者：easten
 */

import { requestIsSuccess, getSupplierType, vIf, vP } from '../../_utils/utils';
import ApiQRcode from '../../sm-api/sm-common/QRcode';

let apiQRcode = new ApiQRcode();
export default {
  name: 'SmQrcode',
  components: {},
  props: {
    axios: { type: Function, default: null }, // axios
    content: { type: String, default: '' }, // 二维码内容
    size: { type: Number, default: 7 }, // 二维码大小
    image: { type: File}, // 二维码中间的图片
    imageSize: { type: Number, default: 20 }, // 图片的大小
    version: { type: Number, default: 7 }, // 二维码的版本
    border: { type: Boolean, default: true }, // 是否展示边框
    show:{ type: Boolean, default: false },
  },
  data() {
    return {
      iContent: '',
      iSize: 0,
      iImage: null,
      iImageSize: 0,
      iVersion: 0,
      iBorder: true,
      imgSrc:null,
    };
  },
  computed: {},
  watch: {
    content: {
      handler(nVal, oVal) {
        this.iContent = nVal;
        this.getQrCode();
      },
      immediate: true,
    },
    size: {
      handler(nVal, oVal) {
        this.iSize = nVal;
        this.getQrCode();
      },
      immediate: true,
    },
    image: {
      handler(nVal, oVal) {
        this.iImage = nVal;
        this.getQrCode();
      },
      immediate: true,
    },
    imageSize: {
      handler(nVal, oVal) {
        this.iImageSize = nVal;
        this.getQrCode();
      },
      immediate: true,
    },
    version: {
      handler(nVal, oVal) {
        this.iVersion = nVal;
        this.getQrCode();
      },
      immediate: true,
    },
    border: {
      handler(nVal, oVal) {
        this.iBorder = nVal;
        this.getQrCode();
      },
      immediate: true,
    },
  },
  created() {
    apiQRcode = new ApiQRcode(this.axios);
  },
  methods: {
    // 获取二维码
    async getQrCode() {
      let _this=this;
      if(!this.show){
        if (this.iContent !== '') {
          let data = {
            content: this.iContent,
            size: this.iSize,
            image: this.iImage,
            imageSize: this.iImageSize,
            version: this.iVersion,
            border: this.iBorder,
          };
          const formData = new FormData();
          formData.append('image', this.iImage);
          formData.append('content', this.iContent);
          formData.append('size', this.iSize);
          formData.append('imageSize', this.iImageSize);
          formData.append('version', this.iVersion);
          formData.append('border', this.iBorder);
          let response = await apiQRcode.getQRCode(formData);
          if (requestIsSuccess(response)) {
            _this.imgSrc='data:image/png;base64,'+response.data;
          }
        }
      }else{
        apiQRcode = new ApiQRcode(this.axios);
        let response=await apiQRcode.getImage(_this.iContent);        
        if (requestIsSuccess(response)){
          _this.imgSrc='data:image/png;base64,'+response.data;
        }
      }      
    },
  },
  render() {
    return <div>
      <img src={this.imgSrc} alt=""/>
    </div>;
  },
};
