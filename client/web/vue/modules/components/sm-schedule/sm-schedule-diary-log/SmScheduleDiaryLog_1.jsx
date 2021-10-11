import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiDiary from '../../sm-api/sm-schedule/Diary';
import {  getFileUrl} from '../../_utils/utils';
import SmVideo from '../../sm-common/sm-video/SmVideo';
import { MemberType } from '../../_utils/enum';
import moment from 'moment';
import SmFileImageView from '../../sm-file/sm-file-manage/src/component/SmFileImageView';
import SmFileDocumentView from '../../sm-file/sm-file-manage/src/component/SmFileDocumentView';
let apiDiary = new ApiDiary();
import print from 'print-js';
export default {
  name: 'SmScheduleDiaryLog',
  props: {
    axios: { type: Function, default: null },
    record: { type: Object, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      materials: [{ name: 'sd' }, { name: 'sd' }], //主要材料设备
      iId: null,
      aRecord: null, //审批记录
      dRecord: null, //日志记录
      talkMedia: [], //班前讲话视频
      processMedia: [], //施工过程视频
      picture: [], //讲话图片
    };
  },

  computed: {},
  watch: {
    record: {
      handler: function(value, oldValue) {
        this.aRecord = value;
        this.iId = this.aRecord ? this.aRecord.id : '';
        this.initAxios();
        this.refresh();
      },
      immediate: true,
      deep: true,
    },
  },
  async created() {},
  methods: {
    initAxios() {
      apiDiary = new ApiDiary(this.axios);
    },
    async refresh() {
      let response = await apiDiary.get({ approvalId: this.iId });
      if (requestIsSuccess(response)) {
        this.dRecord = response.data;
        this.dRecord.organization = this.dRecord.organization;
        this.dRecord.fillTime =
          this.dRecord && this.dRecord.fillTime ? moment(this.dRecord.fillTime) : null;
        //施工员
        let _builders = null;
        if (this.dRecord && this.dRecord.builders.length > 0) {
          this.dRecord.builders.map(item => {
            if (item && item.id && item.builder) {
              if (!_builders) {
                _builders = item.builder.name;
              } else {
                _builders += '、' + item.builder.name;
              }
            }
          });
        }
        //负责人
        let _directors = null;
        if (this.dRecord && this.dRecord.directors.length > 0) {
          this.dRecord.directors.map(item => {
            if (item && item.id && item.builder) {
              if (!_directors) {
                _directors = item.builder.name;
              } else {
                _directors += '、' + item.builder.name;
              }
            }
          });
        }
        let _pictureFileList = [];
        let _processMediaFileList = [];
        let _talkMediaFileList = [];

        if (this.dRecord && this.dRecord.talkMedias.length > 0) {
          this.dRecord.talkMedias.map(item => {
            let file = item.file;
            if (file) {
              _talkMediaFileList.push({
                id: file.id,
                file:file,
                name: file.name,
                type: file.type,
                url: file.url,
              });
            }
          });
        }
        if (this.dRecord && this.dRecord.processMedias.length > 0) {
          this.dRecord.processMedias.map(item => {
            let file = item.file;
            if (file) {
              _processMediaFileList.push({
                id: file.id,
                file:file,
                name: file.name,
                type: file.type,
                url: file.url,
              });
            }
          });
        }
        if (this.dRecord && this.dRecord.pictures.length > 0) {
          this.dRecord.pictures.map(item => {
            let file = item.file;
            console.log(file);
            if (file) {
              _pictureFileList.push({
                id: file.id,
                file:file,
                name: file.name,
                type: file.type,
                url: file.url,
              });
            }
          });
        }
        this.picture = _pictureFileList;
        this.processMedia = _processMediaFileList;
        this.talkMedia = _talkMediaFileList;
        let weathers = JSON.parse(this.dRecord.weathers);
        this.dRecord.evening_t = weathers.evening_t;
        this.dRecord.afternoon_t = weathers.afternoon_t;
        this.dRecord.morning_t = weathers.morning_t;
        this.dRecord.directors = _directors;
        this.dRecord.builders = _builders;
      }
    },
    print(){
      printJS({
        printable: 'schedule-diary-log',
        type: 'html',
        maxWidth: '15000',
        targetStyles: ['*'],
      });
    },
    play(item) {
      let file=item.file;
      let imgtypes = ['.jpg', '.png', '.tif', 'gif', '.JPG', '.PNG', '.GIF', '.jpeg', '.JPEG'];
      let videoTypes=['.avi','.mov','.rmvb','.rm','.flv','.mp4','.3gp','.mpeg','.mpg'];
      if (file.type === '.pdf') {
        this.$refs.SmFileDocumentView.view(file);
      } else if (imgtypes.includes(file.type)) {
        this.$refs.SmFileImageView.view(file);
      } else if(videoTypes.includes(file.type)){
        this.$refs.SmVideo.preview(true,getFileUrl(file.url),file.name);
      }
      else {
        this.$message.warning('当前文件不支持预览');
      }
    },
    getMaterial(type) {
      //material,appliance,mechanical,securityProtection
      let name = null;
      let materials = [];
      switch (type) {
      case 'material':
        name = '材料名称';
        materials = this.dRecord ? this.dRecord.materialList : [];
        break;
      case 'appliance':
        name = '器具名称';
        materials = this.dRecord ? this.dRecord.applianceList : [];
        break;
      case 'mechanical':
        name = '机械名称';
        materials = this.dRecord ? this.dRecord.mechanicalList : [];
        break;
      case 'securityProtection':
        name = '安全防护用品';
        materials = this.dRecord ? this.dRecord.securityProtectionList : [];
        break;
      default:
        name = '材料/设备名称';
        materials = this.dRecord ? this.dRecord.materialList : [];
        break;
      }
      let mainMaterials = [
        <div>
          <a-card-grid class="card-content" hoverable={false}>
            {name}
          </a-card-grid>
          <a-card-grid class="card-content" hoverable={false}>
            规格型号
          </a-card-grid>
          <a-card-grid class="card-content" hoverable={false}>
            {!type ? '实际用量' : '数量'}
          </a-card-grid>
          <a-card-grid class="card-content" hoverable={false}>
            单位
          </a-card-grid>
        </div>,
      ];
      materials.map(item => {
        mainMaterials.push(
          <div>
            <a-card-grid class="card-content" hoverable={false}>
              {item.materialName}
            </a-card-grid>
            <a-card-grid class="card-content" hoverable={false}>
              {item.specModel}
            </a-card-grid>
            <a-card-grid class="card-content" hoverable={false}>
              {item.number}
            </a-card-grid>
            <a-card-grid class="card-content" hoverable={false}>
              {item.unit}
            </a-card-grid>
          </div>,
        );
      });
      return mainMaterials;
    },
  },
  render() {
    {
      /* talkMedia: [], //班前讲话视频
      processMedia: [], //施工过程视频
      picture: [], //讲话图片 */
    }
    let _approvalRltFiles=[];//技术关联资料
    let _talkMedia = [];
    let _processMedia = []; //施工过程视频
    this.talkMedia.map(item => {
      _talkMedia.push(
        <span class="media">
          <a
            onClick={() => {
              this.play(item);
            }}
          >
            {item.name + item.type}
          </a>
        </span>,
      );
    });
    this.processMedia.map(item => {
      _processMedia.push(
        <span class="media">
          <a  onClick={() => {
            this.play(item);
          }}>{item.name + item.type}</a>
        </span>,
      );
    });
    this.aRecord && this.aRecord.approvalRltFiles.map(item => {
      _approvalRltFiles.push(
        <span class="media">
          <a
            onClick={() => {
              this.play(item);
            }}
          >
            {item.file.name + item.file.type}
          </a>
        </span>,
      );
    });
    console.log(this.aRecord);
    return (
      <div class="sm-schedule-diary-log">
        <div id="schedule-diary-log">
          <div class="sm-schedule-diary-log-top">
            <a-descriptions column={6} bordered size="middle">
              <div slot="title" class="title">
                <div class="title-content_1">施工日志</div>
                <div class="title-content_2">{`编号：${this.dRecord ? this.dRecord.code : ''}`}</div>
              </div>
              <a-descriptions-item label="施工日期" span={3}>
                {this.dRecord ? moment(this.dRecord.fillTime).format('YYYY-MM-DD') : ''}
              </a-descriptions-item>
              <a-descriptions-item label="施工单位" span={3}>
                {this.dRecord ? this.dRecord.approval.organization : ''}
              </a-descriptions-item>
              <a-descriptions-item label="天气" span={3}>
                {this.dRecord
                  ? `${this.dRecord.morning_t},${this.dRecord.afternoon_t},${this.dRecord.evening_t}`
                  : ''}
              </a-descriptions-item>
              <a-descriptions-item label="温度" span={3}>
                {this.dRecord ? `${this.dRecord.temperature} ℃` : ''}
              </a-descriptions-item>
              <a-descriptions-item label="施工专业" span={3}>
                {this.aRecord ? this.aRecord.profession.name : ''}
              </a-descriptions-item>
              <a-descriptions-item label="施工部位" span={3}>
                {this.aRecord ? this.aRecord.location : ''}
              </a-descriptions-item>
              <a-descriptions-item label="施工内容" span={6}>
                {this.aRecord ? this.aRecord.name : ''}
              </a-descriptions-item>
              <a-descriptions-item label="现场负责人" span={2}>
                {this.dRecord ? this.dRecord.directors : ''}
              </a-descriptions-item>
              <a-descriptions-item label="施工员" span={2}>
                {this.dRecord ? this.dRecord.builders : ''}
              </a-descriptions-item>
              <a-descriptions-item label="劳务人员" span={2}>
                {this.dRecord ? this.dRecord.memberNum : ''}
              </a-descriptions-item>
            </a-descriptions>
          </div>
          <div class="sm-schedule-diary-log-middle">
            <a-descriptions column={6} layout="vertical" class="log-middle-des" bordered>
              <a-descriptions-item label="主要材料设备" span={6} class="log-middle-item">
                <div class="log-middle-content">{this.getMaterial()}</div>
              </a-descriptions-item>
              <a-descriptions-item label="辅助材料" span={6}>
                <div class="log-middle-content">{this.getMaterial('material')}</div>
              </a-descriptions-item>
              <a-descriptions-item label="使用器具" span={6}>
                <div class="log-middle-content">{this.getMaterial('appliance')}</div>
              </a-descriptions-item>
              <a-descriptions-item label="使用机械" span={6}>
                <div class="log-middle-content">{this.getMaterial('mechanical')}</div>
              </a-descriptions-item>
              <a-descriptions-item label="安全防护用品" span={6}>
                <div class="log-middle-content">{this.getMaterial('securityProtection')}</div>
              </a-descriptions-item>
            </a-descriptions>
          </div>
          <div class="sm-schedule-diary-log-bottom">
            <a-descriptions column={6} bordered size="middle">
              <a-descriptions-item label="技术资料" span={6}>
                {<div class="descriptions-item-media">{_approvalRltFiles}</div>}
              </a-descriptions-item>
              <a-descriptions-item label="临时设施" span={6}>
                {this.aRecord && this.aRecord.safetyCaution ? this.aRecord.safetyCaution.name : ''}
              </a-descriptions-item>
              <a-descriptions-item label="安全注意事项" span={6}>
                {this.aRecord && this.aRecord.temporaryEquipment ? this.aRecord.temporaryEquipment.name : ''}
              </a-descriptions-item>
              <a-descriptions-item label="备注" span={6}>
                {this.aRecord ? this.aRecord.remark : ''}
              </a-descriptions-item>

              <a-descriptions-item label="班前讲话视频" span={3}>
                {<div class="descriptions-item-media">{_talkMedia}</div>}
              </a-descriptions-item>
              <a-descriptions-item label="施工过程视频" span={3}>
                {<div class="descriptions-item-media">{_processMedia}</div>}
              </a-descriptions-item>
              <a-descriptions-item label="施工描述" span={6}>
                {this.dRecord ? this.dRecord.discription : ''}
              </a-descriptions-item>
              <a-descriptions-item label="存在的问题" span={6}>
                {this.dRecord ? this.dRecord.problem : ''}
              </a-descriptions-item>
            </a-descriptions>
          </div>
       
        </div>
        <div class="sm-schedule-action-button">
          <span>
            <a-button
              size="small"
              class="close"
              onClick={() => {
                this.$emit('cancel');
              }}
            >
              关闭
            </a-button>
          </span>
          <span>
            <a-button size="small" class="print" onClick={this.print}>
              打印
            </a-button>
          </span>
        </div>
        <SmVideo axios={this.axios} ref="SmVideo" />
        {/* 图片类预览组件 */}
        <SmFileImageView axios={this.axios} ref="SmFileImageView" />
        {/* 文档浏览组件 */}
        <SmFileDocumentView axios={this.axios} ref="SmFileDocumentView" />
      </div>
    );
  },
};
