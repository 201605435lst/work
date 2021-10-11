/**
 * 说明：文件上传组件，默认上传到文件管理--我的目录下
 * 作者：easten
 */
import './style';
import { requestIsSuccess, getFileUrl } from '../../_utils/utils';
import ApiFileManage from '../../sm-api/sm-file/fileManage';
import ApiFile from '../../sm-api/sm-file/file';
import OssRepository from '../../sm-file/sm-file-manage/src/ossRepository';
import { SaveSingleFile, OssType } from '../../sm-file/sm-file-manage/src/common';
import { reject } from 'lodash-es';
import { resolve } from '@antv/x6/lib/registry/node-anchor/util';
let apiFile = new ApiFile();
let apiFileManage = new ApiFileManage();
let ossRepository = new OssRepository();
export default {
  name: 'SmFileUpload',
  model: {
    props: 'fileList',
    event: 'selected',
  },
  props: {
    axios: { type: Function, default: null },
    accept: { type: String, default: null }, // 可接收的类型
    directory: { type: Boolean, default: false }, // 是否支持上传文件夹
    multiple: { type: Boolean, default: false }, // 是否支持多文件上传
    showUploadList: { type: Boolean, default: false }, // 是否显示上传的列表
    autoSave: { type: Boolean, default: false }, // 是否自动保存，默认不是自动的
    theme: { type: String, default: 'default' }, // 上传组件的样式 default,pic,
    fileList: { type: Array, default: () => [] }, // 反向绑定文件列表
    width: { type: Number, default: 500 },
    height: { type: Number, default: 40 },
    size: { type: String, default: 'default' },
    title: { type: String, default: '文件上传' },
    mode: { type: String, default: 'edit' }, // 当前的视图模式，编辑模式edit 和查看模式 view  查看模式不能上传，只能看。也提供下载功能。
    download: { type: Boolean, default: true }, // 是否可以下载,
    custom: { type: Boolean, default: false }, // 是否自定义,
    tagDirection: { type: String, default: 'row' }, // 标签的显示方向，row,col
    single: { type: Boolean, default: false }, // 是否只支持上传一张图片。
    notCommit: { type: Boolean, default: false }, // 不支持提交，这是将提供向父级传参的事件
    showBase64: { type: Boolean, default: false }, // 是否将文件转换为base64,只有在notCommit 为true 的情况下有效。
  },
  data() {
    return {
      files: [], // 选中的文件
      picUrl: '',
      file: null,
      iAccept: '',
      downing: false,
      currentProcess: 0,// 下载进度
    };
  },
  computed: {
    selected() {
      return this.files;
    },
  },
  watch: {
    fileList: {
      handler(nVal, oVal) {
        if (nVal == null) nVal = [];
        this.files = nVal;
        this.refresh();
      },
      immediate: true,
    },
    accept: {
      handler(nVal, oVal) {
        this.iAccept = nVal;
      },
      immediate: true,
    },
  },
  async created() {
    if (this.theme === 'pic') {
      this.iAccept = 'image/*';
    }
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiFileManage = new ApiFileManage(this.axios);
      apiFile = new ApiFile(this.axios);
    },
    async refresh() { },
    async fileInputChange(evt) {
      let _this = this;
      if (this.custom) {
        if (this.single) {
          this.files = [...(await this.uploadHandler(Array.from(evt.target.files)))];
        } else {
          this.files.push(...(await this.uploadHandler(Array.from(evt.target.files))));
        }
      } else if (this.theme === 'pic') {
        if (this.notCommit) {
          let file = evt.target.files[0];
          if (this.showBase64) {
            let reader = new FileReader(); //实例化文件读取对象
            reader.readAsDataURL(file); //将文件读取为 DataURL,也就是base64编码
            reader.onload = function (ev) {
              //文件读取成功完成时触发
              let dataURL = ev.target.result; //获得文件读取成功后的DataURL,也就是base64编码
              _this.$emit('selected', dataURL);
            };
          } else {
            // 只传文件
            this.$emit('selected', file);
          }
        } else {
          this.files = await this.uploadHandler(Array.from(evt.target.files));
          let windowURL = window.URL || window.webkitURL;
          this.picUrl = windowURL.createObjectURL(this.files[0].file);
        }
      } else {
        if (this.single) {
          if (this.notCommit) {
            this.files = Array.from(evt.target.files);
          } else {
            this.files = [...(await this.uploadHandler(Array.from(evt.target.files)))];
          }
        } else {
          this.files.push(...(await this.uploadHandler(Array.from(evt.target.files))));
        }
      }
      if (this.single) {
        if (this.notCommit) {
          let file = evt.target.files[0];
          if (this.showBase64) {
            let reader = new FileReader(); //实例化文件读取对象
            reader.readAsDataURL(file); //将文件读取为 DataURL,也就是base64编码
            reader.onload = function (ev) {
              //文件读取成功完成时触发
              let dataURL = ev.target.result; //获得文件读取成功后的DataURL,也就是base64编码
              _this.$emit('selected', dataURL.substring(this.result.indexOf(',') + 1));
            };
          } else {
            // 只传文件
            this.$emit('selected', evt.target.files[0]);
          }
        } else {
          this.$emit('selected', this.files[0]);
        }
      } else {
        this.$emit('selected', this.files);
      }
      this.$refs.fileInput.value = '';
    },
    uploadClick() {
      if (this.mode === 'edit') {
        this.$refs.fileInput.click();
      }
    },
    downloadClick(file) {
      let _this = this;
      _this.downing = true;
      if (file) {
        ossRepository
          .download(getFileUrl(file.url), (process) => {
            console.log(process);
            _this.currentProcess = process;
          })
          .then(blob => {
            SaveSingleFile(`${file.name}${file.type}`, file.size, blob).then(a => {
              _this.downing = false;
              _this.currentProcess = 0;
              _this.$notification['success']({
                message: '温馨提示',
                description: `${file.name}下载成功`,
                duration: 2,
              });
            });
          });
      }
    },
    // 创建标签
    createTag(file) {
      let fileTag = (
        <div class="tag" onClick={e => e.stopPropagation()}>
          <span class="title">
            {file.name}
            {file.type}
          </span>
          {this.mode === 'edit' ? (
            <div style="display:flex; lineHeight:20px;">
              <span
                class="close"
                onClick={() => {
                  this.files = this.files.filter(a => a.name != file.name);
                  if (this.notCommit) {
                    this.$emit('selected', null);
                  } else {
                    this.$emit('selected', this.files);
                  }
                }}
              >
                ×
              </span>
              {file.upload ? <span style="margin-left: 5px; font-size: 10px; lineHeight: 20px;display:flex;">上传中:{file.progress}</span> : null}

            </div>
          ) : null}
          {this.mode === 'view' ? (
            this.download ? (
              <div style="display:flex;">
                <span class="down" onClick={() => this.downloadClick(file)}>
                  <a-icon type="cloud-download" />
                </span>
                {this.downing ? <span style="margin-left: 5px; font-size: 10px; lineHeight: 20px;display:flex;">下载进度:{this.currentProcess}</span> : null}
              </div>
            ) : null
          ) : null}
        </div>
      );
      return fileTag;
    },
    fileSelect() {
      this.files = [];
      this.$refs.fileInput.click();
    },
    async uploadHandler(fileList) {
      let _this = this;
      let arr = [];
      let pros = [];
      if (fileList.length > 0) {
        fileList.forEach(a => {
          if (a.webkitRelativePath != null) {
            a.path = a.webkitRelativePath.substring(0, a.webkitRelativePath.lastIndexOf('/'));
          }
          let sufixx = a.name.substring(a.name.lastIndexOf('.'));
          pros.push(_this.getFileData(sufixx, a));
        });
      }
      return await Promise.all(pros);
    },
    async getPersiginUrl(sufixx) {
      let promise = new Promise(async (res, err) => {
        let response = await apiFile.getPresignUrl({ sufixx });
        if (requestIsSuccess(response)) {
          res(response.data);
        }
      });
      return promise;
    },
    async getFileData(sufixx, file) {
      let data = await this.getPersiginUrl(sufixx);
      return new Promise((res, rej) => {
        let newFile = new File([file], `${data.fileId}${sufixx}`);
        res({
          file: newFile,
          size: file.size,
          name: file.name.substring(0, file.name.lastIndexOf('.')),
          type: sufixx,
          editTime: file.lastModifiedDate,
          id: data.fileId,
          progress: 0,
          error: false,
          upload: false,
          path: file.path,
          state: this.fileState,
          presignUrl: data.presignUrl,
          relativeUrl: data.relativePath,
          ossType: data.ossType,
        });
      });
    },
    commitHandle() {
      return Promise.all(
        this.files.map(data => {
          data.upload = true;
          return new Promise(async (resolve, reject) => {
            let file = {
              fileId: data.id,
              name: data.name,
              type: data.type,
              size: data.size,
              isPublic: false,
              ossFileName: data.ossFileName,
              url: data.relativeUrl,
            };
            if (data.ossType === OssType.minio || data.ossType == OssType.amazons3) {
              let success = await ossRepository.upload(data, progress => {
                console.log(progress);
                data.progress = progress;
              });
              if (success) {
                let response = await apiFile.create(file);
                if (requestIsSuccess(response)) {
                  data.upload = false;
                  resolve();
                }
              }
            } else {
              resolve();
            }
          });
        }),
      ).then(() => {
        return true;
      });
    },
    // 提交文件保存
    async commit(files) {
      if (files != null || files != undefined) {
        this.files = files;
      }
      return new Promise(async (res, rej) => {
        this.commitHandle().then(() => {
          res();
        });

      });
    },
  },

  render() {
    // 默认样式
    let defaultTheme = (
      <div
        class={`default ${this.tagDirection === 'row' ? '' : 'justify-content-left'}`}
        onClick={() => this.uploadClick()}
      >
        {this.mode === 'edit' ? (
          this.selected.length > 0 ? (
            <div class={`tag-panel ${this.tagDirection === 'row' ? 'flex-row' : 'flex-col'}`}>
              {this.selected.map(a => {
                return this.createTag(a);
              })}
            </div>
          ) : (
            <div class="title">
              <a-icon type="cloud-upload" />
              <span>{this.title}</span>
            </div>
          )
        ) : (
          <div class={`tag-panel ${this.tagDirection === 'row' ? 'flex-row' : 'flex-col'}`}>
            {this.fileList.map(a => {
              return this.createTag(a);
            })}
          </div>
        )}
      </div>
    );
    // 头像上传，单文件上传,只支持图片
    let picTheme = (
      <div class="single" onClick={() => this.uploadClick()}>
        {this.picUrl === '' ? (
          <div class="pic-content">
            <span class="plus">
              <a-icon type="plus" />
            </span>
            <span>{this.title}</span>
          </div>
        ) : (
          <img src={this.picUrl} alt="图片" />
        )}
      </div>
    );

    // 拖拽上传---待开发
    let dragTheme = <div class="drag"></div>;
    return (
      <div class="sm-file-upload" >
        {this.custom ? null : (
          <div class="container">
            {this.theme === 'default' ? defaultTheme : null}
            {this.theme === 'pic' ? picTheme : null}
          </div>
        )}
        <input
          style="display:none;"
          type="file"
          ref="fileInput"
          size={this.size}
          onChange={this.fileInputChange}
          onInput={(a, b, c) => { }}
          name="file"
          multiple={this.multiple}
          webkitdirectory={this.directory}
          accept={this.iAccept}
        ></input>
      </div>
    );
  },
};
