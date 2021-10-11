import SmFileManageModal from '../../sm-file/sm-file-manage-modal';
import ApiEquipments from '../../sm-api/sm-resource/Equipments';
import { requestIsSuccess ,getFileUrl} from '../../_utils/utils';
import { tips as tipsConfig } from '../../_utils/config';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';
import SmFileDocumentView from '../../sm-file/sm-file-manage/src/component/SmFileDocumentView';
import SmVideo from '../../sm-common/sm-video';
import './style/index.less';

let apiEquipments = new ApiEquipments();

export default {
  name: 'SmD3EquipmentInfoFiles',
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    value: { type: String, default: null },
    title: { type: String, default: '资料' },
  },
  data() {
    return {
      isShowFileModel: false, //文件弹框是否弹出
      equipment: null, //当前设备
      tags: [],//设备关联文件的标签集合
      files: [],//当前设备关联文件
      targetIndex: 0,
      isTargetTag: false,
      targetFileIndex: null,
      targetFile: null,
      fileList: [],//设备关联文件过滤之后的列表
      videoTypes:['.avi','.mov','.rmvb','.rm','.flv','.mp4','.3gp','.mpeg','.mpg'],
    };
  },

  computed: {
  },

  watch: {
    value: {
      handler: function (value, oldValue) {
        if (value) {
          this.initAxios();
          this.refresh();
        } 
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
    this.refresh();
  },

  mounted() { },

  methods: {
    initAxios() {
      apiEquipments = new ApiEquipments(this.axios);
    },

    async refresh() {
      if (!this.value) return;
      let response = await apiEquipments.getFileList({ equipmentId: this.value });
      if (requestIsSuccess(response) && response.data) {
        this.files = response.data;
        this.fileList = response.data;
        let _tags = [];
        response.data.map(item => {
          item.file.tags.map(_item => {
            if (_tags.indexOf(_item.tag.name) === -1) {
              _tags.push(_item.tag.name);
            }
          });
        });
        this.tags = _tags;
      }
      this.$emit('change', null);
    },

    add() {
      this.isShowFileModel = true;
    },

    async ok(files) {
      if (files && files.length > 0) {
        let fileIds = files.map(item => item.id);
        let response = await apiEquipments.createFile({ equipmentId: this.value, fileIds: fileIds });
        if (requestIsSuccess(response)) {
          this.$message.success('操作成功');
          this.refresh();
        }
      }
    },

    async deleteFile(item) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiEquipments.deleteFile(item.id);
            if (requestIsSuccess(response)) {
              _this.refresh();
              if (_this.targetFile && item.file.id === _this.targetFile.id) {
                _this.$emit('change', null);
              }
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },

    changeTag(index, tagName) {
      let _files = [];
      if (this.isTargetTag || this.targetIndex !== index) {
        this.files.map(item => {
          if (item.file.tags && item.file.tags.length > 0 && item.file.tags.find(_item => _item.tag.name === tagName)) {
            _files.push(item);
          }
        });
        this.fileList = _files;
      } else {
        this.fileList = this.files;
      }

    },
    checkType(type){
      return type === '.pdf'||this.videoTypes.includes(type);
    },
    // 查看文件，只支持pdf 和图片
    view(file) {
      if (file.type === '.pdf') {
        this.$refs.SmFileDocumentView.view(file);
      } else if(this.videoTypes.includes(file.type)){
        this.$refs.smVideo.preview(true,getFileUrl(file.url),file.name);
      }
      else {
        this.$message.warning('当前文件不支持预览');
      }
    },

  },

  render() {
    let tags = [];
    this.tags.map((item, index) => {
      tags.push(<a-tag
        class="tag"
        color={this.isTargetTag && this.targetIndex === index ? 'blue' : ''}
        onClick={() => {
          if (this.targetIndex !== index) {
            this.targetIndex = index;
            this.isTargetTag = true;
          } else {
            this.targetIndex = index;
            this.isTargetTag = !this.isTargetTag;
          }
          this.changeTag(index, item);
        }}
      >
        {item}
      </a-tag>);
    });

    let items = <div class="files">
      {this.fileList.map((item, index) => {
        return <div
          class={{ "fileItem": true, 'active': this.targetFileIndex === index }}
          onClick={() => {
            this.targetFileIndex = index;
            this.targetFile = item.file;
            this.$emit('change', item.file);
          }}
        >
          <div class="title" >{item.file.name}</div>
          <div class="icons">
            {this.checkType(item.file.type) ? <a-icon
              class="icon"
              type="eye"
              onClick={e => {
                e.stopPropagation();
                this.view(item.file);
              }}
            /> : undefined}

            <a-icon
              class="icon"
              type="minus"
              onClick={e => {
                e.stopPropagation();
                this.deleteFile(item);
              }}
            />
            <SmFileManageSelect
              class="icon"
              axios={this.axios}
              bordered={false}
              simple={true}
              disable={true}
              multiple={false}
              value={item.file}
            />
          </div>
        </div>;
      })}
    </div>;
    return (
      <div class="sm-d3-equipment-info-files">
        { this.files && this.files.length > 0
          ? [
            ...tags,
            this.tags.length > 0 ? <a-divider /> : undefined,
            items,
          ] :
          <div style="color: rgb(189, 189, 189); padding: 10px;">无数据</div>
        }

        <SmFileManageModal
          ref="SmFileManageModal"
          axios={this.axios}
          multiple={true}
          visible={this.isShowFileModel}
          onChange={visible => this.isShowFileModel = visible}
          onOk={this.ok}
        />

        {/* 文档浏览组件 */}
        <SmFileDocumentView ref="SmFileDocumentView" />
        {/* 视频预览插件 */}
        <SmVideo ref='smVideo' width={900} height={500}/>
      </div>
    );
  },
};
