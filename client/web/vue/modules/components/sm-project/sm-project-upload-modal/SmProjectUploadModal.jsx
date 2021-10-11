import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiFileImport from '../../sm-api/sm-common/import';
let apiFileImport = new ApiFileImport();
import { tips as tipsConfig } from '../../_utils/config';
import SmImport from '../../sm-import/sm-import-basic/SmImport';
import moment from 'moment';
import FileSaver from 'file-saver';
export default {
  name: 'SmProjectUploadModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      load: false,
      status: false,
      fileType: 'application/vnd.ms-excel',
      templateInfo: null, //模板信息
      uploadInfo: null, //模板信息
      accept: '.xls,.xlsx',
      paramter: null,
      uploadName: null,
      uploadSize: null,
    };
  },
  computed: {
    visible() {
      return this.status;
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiFileImport = new ApiFileImport(this.axios);
    },
    isCanDownload() {
      let _this = this;
      this.$confirm({
        title: tipsConfig.download.title,
        content: h => (
          <div style="color:red;">{`确认要下载 ${
            this.templateInfo ? this.templateInfo.name : ''
          } 吗？`}</div>
        ),
        okType: 'warning',
        onOk() {
          _this.download();
        },
        onCancel() {},
      });
    },
    // 下载文件模板
    async download() {
      let response = await apiFileImport.downloadTemplate(
        this.paramter ? this.paramter.importKey : '',
      );
      if (response != null && requestIsSuccess(response)) {
        if (response.data.byteLength != 0) {
          FileSaver.saveAs(
            new Blob([response.data], { type: this.fileType }),
            this.paramter ? this.paramter.name : null,
          );
        }
      }
    },
    async upload(paramter) {
      this.templateInfo = null; //模板信息
      this.uploadInfo = null; //模板信息
      this.paramter = null;
      this.uploadName = null;
      this.uploadSize = null;
      this.paramter = paramter;
      // 获取文件模板的信息
      let response = await apiFileImport.getFileInfo(
        this.paramter ? this.paramter.importKey : null,
      );
      if (response != null && requestIsSuccess(response)) {
        this.templateInfo = response ? response.data : null;
        if (this.templateInfo && this.paramter && this.paramter.name) {
          let typeArry = this.templateInfo.name.split('.');
          let val = { ...typeArry };
          let type = val[typeArry.length - 1];
          let name =
            this.paramter.name.length > 10
              ? this.paramter.name.substring(0, 10)
              : this.paramter.name;
          this.templateInfo.name = name + '.' + type;
          this.templateInfo.size = `${this.templateInfo.size}kb`;
        }
      }
      this.status = true;
    },
    async upLoadAction() {
      // 构造导入参数（根据自己后台方法的实际参数进行构造）
      if (this.uploadInfo) {
        let importParamter = {
          'file.file': this.uploadInfo,
          importKey: this.paramter.importKey,
          parentId: this.paramter.parentId,
        };
        // 执行文件上传
        await this.$refs.smImport.exect(importParamter);
      } else {
        this.$message.warning('请先选择您要上传的文件');
      }
    },
    uploadFiles(files) {
      this.uploadInfo = files[0];
      let typeArry = this.uploadInfo.name.split('.');
      let val = { ...typeArry };
      let type = val[typeArry.length - 1];
      this.uploadName =
        this.uploadInfo.name.length > 34
          ? this.uploadInfo.name.substring(0, 30) + '.' + type
          : this.uploadInfo.name;
      this.uploadSize = `${this.uploadInfo.size}kb`;
    },
    async refresh() {},
    ok() {},

    close() {
      this.status = false;
    },
  },
  render() {
    let fileInput = (
      <input
        style="display:none;"
        type="file"
        ref="fileInput"
        onChange={event => {
          this.uploadFiles(event.target.files);
          this.$refs.fileInput.value = '';
        }}
        name="file"
        multiple={this.multiple}
        webkitdirectory={false}
        accept={this.accept}
      ></input>
    );
    return (
      <div class="sm-project-upload-modal-a">
        <div>
          <a-modal
            class="sm-project-upload-modal"
            title="下载模板/按格式修改后上传"
            visible={this.visible}
            confirmLoading={this.load}
            onOk={this.ok}
            onCancel={this.close}
          >
            <div class="upload-template">
              <div
                class="format-template_1"
                onMouseOver={() => {
                  console.log('进来了');
                }}
              >
                <div class="left-right">
                  {!this.templateInfo ? (
                    <span class="empty-span">不存在此文件的导入模板</span>
                  ) : (
                    <span class="left-right-content">
                      <div class="left">
                        <span class="icon-file">
                          <a-icon type="file-excel" />
                        </span>
                        <span>{this.templateInfo ? this.templateInfo.name : null}</span>
                      </div>
                      <div class="right">
                        <div class="top-close">
                          <span class="close"> {false ? <si-close /> : <si-seleted />}</span>
                        </div>
                        <div class="right-icon">
                          <span class="icon-download" onClick={() => this.isCanDownload()}>
                            <a-tooltip placement="top" title="下载">
                              <si-download />{' '}
                            </a-tooltip>
                          </span>
                          <span class="icon-span">
                            {this.templateInfo ? this.templateInfo.size : null}
                          </span>
                          <span class="icon-span">
                            {this.templateInfo
                              ? moment(this.templateInfo.date).format('YYYY-MM-DD')
                              : null}
                          </span>
                        </div>
                      </div>
                    </span>
                  )}
                </div>
              </div>
              <div class="format-template_2">
                <div class="left-right">
                  {!this.uploadInfo ? (
                    <span class="empty-span">请上传要导入的文件</span>
                  ) : (
                    <span class="left-right-content">
                      <div class="left">
                        <span class="icon-file">
                          <a-icon type="file-excel" />
                        </span>
                        <span>{this.uploadName}</span>
                      </div>
                      <div class="right" onClick={() => console.log('进来了')}>
                        <div class="top-close">
                          <span class="close"> {false ? <si-close /> : <si-seleted />}</span>
                        </div>
                        <div class="right-icon">
                          <span class="icon-download">
                            {/* <a-tooltip placement="top" title="下载">
                              <si-download />{' '}
                            </a-tooltip> */}
                          </span>
                          <span class="icon-span">{this.uploadSize}</span>
                          <span class="icon-span">
                            {this.uploadInfo
                              ? moment(this.uploadInfo.lastModifiedDate).format('YYYY-MM-DD')
                              : null}
                          </span>
                        </div>
                      </div>
                    </span>
                  )}
                </div>
              </div>
            </div>
            <a-divider orientation="left">上传附件</a-divider>
            <div class="upload-button">
              <div>
                <a-button
                  type="primary"
                  onClick={() => {
                    this.$refs.fileInput.click();
                  }}
                >
                  <a-icon type="cloud-upload" />
                  上传文件
                  {fileInput}
                </a-button>
                {/* <span>{this.uploadName}</span> */}
              </div>
              <div class="words">注：只能上传xls/xlxs文件，文件不超过100mb</div>
            </div>
            <template slot="footer" class="footer-button">
              <a-button type="primary" size="small" onClick={() => this.close()}>
                取消
              </a-button>
              <SmImport
                ref="smImport"
                url="url"
                axios={this.axios}
                url={this.paramter ? this.paramter.url : null}
                size="small"
                isImportButton={false}
                defaultTitle="提交"
                importKey={this.paramter ? this.paramter.importKey : null}
                downloadErrorFile={true}
                uploadInfo={this.uploadInfo}
                onUpLoadFile={() => this.upLoadAction()}
                onSuccess={item => {
                  this.$emit('success');
                  !item ? this.close() : '';
                }}
              />
            </template>
            {/* <span slot="okText" >
              <SmImport
                ref="smImport"
                url="url"
                axios={this.axios}
                url="/api/app/projectDossier/upload"
                size="small"
                defaultTitle="导入"
                importKey="projectDossier"
                downloadErrorFile={true}
                onSelected={(file) => {
                  this.fileSelected(file);
                }}
                onSuccess={() => this.refresh()}
              />
            </span> */}
          </a-modal>
        </div>
      </div>
    );
  },
};
