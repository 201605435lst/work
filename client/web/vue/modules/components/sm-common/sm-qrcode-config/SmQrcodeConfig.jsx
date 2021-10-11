/**
 * 说明：二维码信息配置
 * 作者：easten
 */
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import QrCode from '../sm-qrcode';
import SmFileUpload from '../../sm-file/sm-file-upload';
import ApiQRcode from '../../sm-api/sm-common/QRcode';
let apiQRcode = new ApiQRcode();
export default {
  name: 'SmQrcodeConfig',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      showLogo: false,
      size: 4,
      version: 7,
      imageSize: 30,
      border: true,
      image: null,
      record:null,
    };
  },
  computed: {},
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiQRcode=new ApiQRcode(this.axios);
    },
    async refresh() {
      let response=await apiQRcode.get();
      if (requestIsSuccess(response)){
        if(response.data){
          let data=response.data;
          this.record=data;
          this.size=data.size;
          this.version=data.version;
          this.imageSize=data.imageSize;
          this.border=data.border;
          this.image=data.image;
          this.record=data.record;
        }
      }
    },
    // 保存
    async save() {
      let formData = new FormData();
      formData.append('image', this.image);
      formData.append('size', this.size);
      formData.append('imageSize', this.imageSize);
      formData.append('version', this.version);
      formData.append('border', this.border);
      formData.append('showLogo', this.showLogo);
      console.log(formData);
      let response=await apiQRcode.update(formData);
      if (requestIsSuccess(response)){
        if(response.data){
          this.$message.info('保存成功');
        }
      }
    },
    picSelected(file) {
      console.log(file);
      this.image = file;
    },
  },
  render() {
    return (
      <div class="sm-qrcode-config">
        <a-row gutter={16}>
          <a-col class="gutter-row" span={12}>
            <div class="gutter-box">
              <a-form form={this.form} submit="handleSubmit">
                <a-form-item labelCol={{ span: 4 }} wrapperCol={{ span: 20 }} label="二维码尺寸">
                  <a-input-number
                    value={this.size}
                    onChange={n => (this.size = n)}
                    style="width:100%;"
                    min={0}
                    max={30}
                  ></a-input-number>
                </a-form-item>
                <a-form-item labelCol={{ span: 4 }} wrapperCol={{ span: 20 }} label="二维码版本">
                  <a-input-number
                    value={this.version}
                    onChange={n => (this.version = n)}
                    style="width:100%;"
                    min={0}
                    max={30}
                  ></a-input-number>
                </a-form-item>
                <a-form-item labelCol={{ span: 4 }} wrapperCol={{ span: 20 }} label="显示边框">
                  <a-switch
                    checked-children="是"
                    value={this.border}
                    onChange={n => (this.border = n)}
                    un-checked-children="否"
                    default-checked
                  />
                </a-form-item>
                <a-form-item labelCol={{ span: 4 }} wrapperCol={{ span: 20 }} label="显示logo">
                  <a-switch
                    checked-children="是"
                    un-checked-children="否"
                    onChange={c => {
                      this.showLogo = c;
                      this.image=null;
                      if(c) this.image=this.record.image;
                    }}
                  />
                </a-form-item>
                {this.showLogo ? (
                  <a-form-item labelCol={{ span: 4 }} wrapperCol={{ span: 20 }} label="上传Logo">
                    <SmFileUpload
                      single
                      notCommit
                      accept="image/*"
                      style="width:100%;"
                      onSelected={this.picSelected}
                    />
                  </a-form-item>
                ) : null}
                {this.showLogo ? (
                  <a-form-item labelCol={{ span: 4 }} wrapperCol={{ span: 20 }} label="Logo大小">
                    <a-input-number
                      value={this.imageSize}
                      onChange={n => (this.imageSize = n)}
                      style="width:100%;"
                      min={0}
                      max={30}
                    ></a-input-number>
                  </a-form-item>
                ) : null}
                <a-form-item wrapperCol={{ span: 24 }}>
                  <a-button style="float:right;" type="primary" onClick={() => this.save()}>
                    保存
                  </a-button>
                </a-form-item>
              </a-form>
            </div>
          </a-col>
          <a-col class="gutter-row" span={12}>
            <div class="gutter-box">
              <span>预览</span>
              <QrCode
                axios={this.axios}
                content="预览测试"
                image={this.image}
                imageSize={this.imageSize}
                size={this.size}
                version={this.version}
                border={this.border}
              />
            </div>
          </a-col>
        </a-row>
      </div>
    );
  },
};
