import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiApproval from '../../sm-api/sm-schedule/Approval';
import ApiDiary from '../../sm-api/sm-schedule/Diary';
import { getFileUrl } from '../../_utils/utils';
import SmVideo from '../../sm-common/sm-video/SmVideo';
import { MemberType } from '../../_utils/enum';
import moment from 'moment';
import SmFileImageView from '../../sm-file/sm-file-manage/src/component/SmFileImageView';
import SmFileDocumentView from '../../sm-file/sm-file-manage/src/component/SmFileDocumentView';
let apiDiary = new ApiDiary();
let apiApproval = new ApiApproval();

import print from 'print-js';
export default {
  name: 'SmScheduleDiaryLog',
  props: {
    axios: { type: Function, default: null },
    record: { type: Object, default: null },
    preRecored: { type: Object, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      iId: null,
      aRecord: null, //审批记录
      dRecord: null, //日志记录
      talkMedia: [], //班前讲话视频
      processMedia: [], //施工过程视频
      picture: [], //讲话图片
      directors :null,//负责人
      builders :null,//施工人

      
    };
  },

  computed: {},
  watch: {
    record: {
      handler: function(value, oldValue) {
        if(value){
          this.aRecord = value;
          this.iId = this.aRecord ? this.aRecord.id : '';
          this.initAxios();
          this.refresh();
        }
      },
      immediate: true,
      deep: true,
    },
    preRecored: {
      handler: function(value, oldValue) {
        if(value){
          this.aRecord = value.aRecord;
          this.dRecord=value.dRecord;
          this.directors = this.dRecord.buildersName;
          this.builders = this.dRecord.directorsName;
          this.picture = this.dRecord.pictures;
          this.processMedia = this.dRecord.processMedias;
          this.talkMedia = this.dRecord.talkMedias;
        }
      },
      immediate: true,
      deep: true,
    },
  },
  async created() {},
  methods: {
    initAxios() {
      apiDiary = new ApiDiary(this.axios);
      apiApproval = new ApiApproval(this.axios);
    },
    async refresh() {
      let response =this.aRecord && this.aRecord.diaryCode? await apiDiary.get({ approvalId: this.iId }) : await apiApproval.get(this.iId);
      if (requestIsSuccess(response)) {
        this.dRecord = response.data;
        this.dRecord.organization = this.dRecord.organization;
        this.dRecord.fillTime =
        (this.aRecord && this.aRecord.diaryCode) && this.dRecord && this.dRecord.fillTime ? moment(this.dRecord.fillTime) : this.dRecord.time ?moment(this.dRecord.time):null;
        //施工员
        let _builders = null;
        if (this.dRecord && this.dRecord.builders && this.dRecord.builders.length > 0) {
          console.log(this.dRecord.builders);
          this.dRecord.builders.map(item => {
            if (item && item.id && (item.builder || item.member)) {
              if (!_builders) {
                _builders = (this.aRecord && this.aRecord.diaryCode)?item.builder.name:item.member.name;
              } else {
                _builders += (this.aRecord && this.aRecord.diaryCode)?'、' + item.builder.name:'、' + item.member.name   ;
              }
            }
          });
        }
        //负责人
        let _directors = null;
        if (this.dRecord && this.dRecord.directors && this.dRecord.directors.length > 0) {
          this.dRecord.directors.map(item => {
            if (item && item.id && (item.builder || item.member)) {
              if (!_directors) {
                _directors = (this.aRecord && this.aRecord.diaryCode)?item.builder.name:item.member.name;
              } else {
                _directors += (this.aRecord && this.aRecord.diaryCode)?'、' + item.builder.name:'、' + item.member.name   ;
              }
            }
          });
        }
        let _pictureFileList = [];
        let _processMediaFileList = [];
        let _talkMediaFileList = [];

        if (this.dRecord && this.dRecord.talkMedias && this.dRecord.talkMedias.length > 0) {
          this.dRecord.talkMedias.map(item => {
            let file = item.file;
            if (file) {
              _talkMediaFileList.push({
                id: file.id,
                file: file,
                name: file.name,
                type: file.type,
                url: file.url,
              });
            }
          });
        }
        if (this.dRecord && this.dRecord.processMedias && this.dRecord.processMedias.length > 0) {
          this.dRecord.processMedias.map(item => {
            let file = item.file;
            if (file) {
              _processMediaFileList.push({
                id: file.id,
                file: file,
                name: file.name,
                type: file.type,
                url: file.url,
              });
            }
          });
        }
        if (this.dRecord && this.dRecord.pictures && this.dRecord.pictures.length > 0) {
          this.dRecord.pictures.map(item => {
            let file = item.file;
            if (file) {
              _pictureFileList.push({
                id: file.id,
                file: file,
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
        let weathers =this.dRecord && this.dRecord.weathers? JSON.parse(this.dRecord.weathers):null;
        this.dRecord.evening_t =weathers? weathers.evening_t:'';
        this.dRecord.afternoon_t = weathers?weathers.afternoon_t:'';
        this.dRecord.morning_t = weathers?weathers.morning_t:'';
        this.directors = _directors;
        this.builders = _builders;
        console.log(this.directors);
      }
    },
    print() {
      printJS({
        printable: 'schedule-diary-log',
        type: 'html',
        maxWidth: '100%',
        targetStyles: ['*'],

      });
    },
    play(file) {
      let imgtypes = ['.jpg', '.png', '.tif', 'gif', '.JPG', '.PNG', '.GIF', '.jpeg', '.JPEG'];
      let videoTypes = ['.avi', '.mov', '.rmvb', '.rm', '.flv', '.mp4', '.3gp', '.mpeg', '.mpg'];
      if (file.type === '.pdf') {
        this.$refs.SmFileDocumentView.view(file);
      } else if (imgtypes.includes(file.type)) {
        this.$refs.SmFileImageView.view(file);
      } else if (videoTypes.includes(file.type)) {
        this.$refs.SmVideo.preview(true, getFileUrl(file.url), file.name);
      } else {
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
        name = '使用器具';
        materials = this.dRecord ? this.dRecord.applianceList : [];
        break;
      case 'mechanical':
        name = '使用机械';
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
       
        <a-row class="rows">
          <a-col span="6" class="title-matreial center">
            {name}
          </a-col>
          <a-col span="6" class="title-matreial center">
              规格型号
          </a-col>
          <a-col span="6" class="title-matreial center">
            {!type ? '实际用量' : '数量'}
          </a-col>
          <a-col span="6" class="title-matreial center">
              单位
          </a-col>
        </a-row>,
      ];
      materials.map(item => {
        mainMaterials.push(
          <a-row class="rows">
            <a-col span="6" class="title-matreial center">
              {item.materialName ? item.materialName : `  `}
            </a-col>
            <a-col span="6" class="title-matreial center">
              {item.specModel ? item.specModel : `  `}
            </a-col>
            <a-col span="6" class="title-matreial center">
              {item.number ? item.number : `  `}
            </a-col>
            <a-col span="6" class="title-matreial center">
              {item.unit ? item.unit : ` `}
            </a-col>
          </a-row>,
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
    let _approvalRltFiles = []; //技术关联资料
    let _talkMedia = [];
    let _picture=[];
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
    this.picture.map(item => {
      _picture.push(
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
    this.aRecord &&
      this.aRecord.approvalRltFiles.map(item => {
        _approvalRltFiles.push(
          <span class="media">
            <a
              onClick={() => {
                this.play(item.file);
              }}
            >
              {item.file.name + item.file.type}
            </a>
          </span>,
        );
      });
    return (
      <div class="sm-schedule-diary-log" >
        <div id="schedule-diary-log">
          <a-row>
            <a-col span="24" class="body-title">施工日志</a-col>
          </a-row>
          <a-row class="code">
            <a-col span="24" class="body-code">
              <span >{`编号：${this.dRecord ? this.dRecord.code : ''}`}</span>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
                施工日期
            </a-col>
            <a-col span="9">
              <span class="text">
                {this.dRecord ? moment(this.dRecord.fillTime).format('YYYY-MM-DD') : ''}
              </span>
            </a-col>
            <a-col span="3" class="title">
                施工单位
            </a-col>
            <a-col span="9">
              <span class="text">{this.aRecord ? this.aRecord.organization: ''}</span>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
                天气
            </a-col>
            <a-col span="9">
              <span class="text">
                {this.dRecord && (this.aRecord && this.aRecord.diaryCode)
                  ? `${this.dRecord.morning_t},${this.dRecord.afternoon_t},${this.dRecord.evening_t}`
                  : ''}
              </span>
            </a-col>
            <a-col span="3" class="title">
                温度
            </a-col>
            <a-col span="9">
              <span class="text">{this.dRecord && this.dRecord.temperature? `${this.dRecord.temperature} ℃` : ''}</span>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
                施工专业
            </a-col>
            <a-col span="9">
              <span class="text">{this.aRecord ? this.aRecord.profession.name : ''}</span>
            </a-col>
            <a-col span="3" class="title">
                施工部位
            </a-col>
            <a-col span="9">
              <span class="text">{this.aRecord ? this.aRecord.location : ''}</span>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
                施工内容
            </a-col>
            <a-col span="21">
              <span class="text">{this.aRecord ? this.aRecord.name : ''}</span>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
                现场负责人
            </a-col>
            <a-col span="6">
              <span class="text">{this.directors ? this.directors : ''}</span>
            </a-col>
            <a-col span="3" class="title">
                施工员
            </a-col>
            <a-col span="6">
              <span class="text">{this.builders ? this.builders : ''}</span>
            </a-col>
            <a-col span="3" class="title">
                劳务人员
            </a-col>
            <a-col span="3">
              <span class="text">{this.dRecord ? this.dRecord.memberNum : ''}</span>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="24" class="title-matreial">
              <span class="text">
                主要材料设备
              </span>
            </a-col>
          </a-row>
          {this.getMaterial()}
          <a-row class="rows">
            <a-col span="24" class="title-matreial">
              <span class="text">
              辅助材料
              </span>
            </a-col>
          </a-row>
          {this.getMaterial('material')}
          <a-row class="rows">
            <a-col span="24" class="title-matreial">
              <span class="text">
              使用器具
              </span>
            </a-col>
          </a-row>
          {this.getMaterial('appliance')}
          <a-row class="rows">
            <a-col span="24" class="title-matreial">
              <span class="text">
              使用机械
              </span>
                
            </a-col>
          </a-row>
          {this.getMaterial('mechanical')}
          <a-row class="rows">
            <a-col span="24" class="title-matreial">
              <span class="text">
              安全防护用品
              </span>
                
            </a-col>
          </a-row>
          {this.getMaterial('securityProtection')}
        
       
          <a-row class="rows">
            <a-col span="3" class="title">
                技术资料
            </a-col>
            <a-col span="21">
              {<div class="descriptions-item-media">{_approvalRltFiles}</div>}
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
                临时设施
            </a-col>
            <a-col span="21">
              <span class="text">
              </span>
              {this.aRecord && this.aRecord.safetyCaution ? this.aRecord.safetyCaution.name : ''}
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">

                安全注意事项
            </a-col>
            <a-col span="21">
              <span class="text">
                {this.aRecord && this.aRecord.temporaryEquipment
                  ? this.aRecord.temporaryEquipment.name
                  : ''}
              </span>
              
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
                备注
            </a-col>
            <a-col span="21">
              <span class="text">
                {this.aRecord ? this.aRecord.remark : ''}
              </span>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
                班前讲话视频
            </a-col>
            <a-col span="21">{<div class="descriptions-item-media">{_talkMedia}</div>}</a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
                班前讲话图片
            </a-col>
            <a-col span="21">
              <a-col span="21">{<div class="descriptions-item-media">{_picture}</div>}</a-col>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
                施工过程视频
            </a-col>
            <a-col span="21">{<div class="descriptions-item-media">{_processMedia}</div>}</a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
                施工描述
            </a-col>
            <a-col span="21">
              <span class="text">
                {this.dRecord ? this.dRecord.discription : ''}
              </span>
            </a-col>
          </a-row>
          <a-row class="rows row-bottom">
            <a-col span="3" class="title">
                存在的问题
            </a-col>
            <a-col span="21"> 
              <span class="text">
                {this.dRecord ? this.dRecord.problem : ''}
              </span>
            </a-col>
          </a-row>
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
