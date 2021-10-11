import './style/index';
import moment from 'moment';
import print from 'print-js';
import * as utils from '../../_utils/utils';
import { form as formConfig, tips } from '../../_utils/config';
import { ModalStatus, SafeRecordState, SafeRecordType } from '../../_utils/enum';
import { requestIsSuccess, getQualityProblemType, getQualityProblemLevel } from '../../_utils/utils';
import ApiProblem from '../../sm-api/sm-quality/QualityProblem';
import { getFileUrl } from '../../_utils/utils';
import SmVideo from '../../sm-common/sm-video/SmVideo';
import SmFileImageView from '../../sm-file/sm-file-manage/src/component/SmFileImageView';
import SmFileDocumentView from '../../sm-file/sm-file-manage/src/component/SmFileDocumentView';
import OssRepository from '../../sm-file/sm-file-manage/src/ossRepository';
import { SaveSingleFile } from '../../sm-file/sm-file-manage/src/common';

let ossRepository = new OssRepository();
let apiProblem = new ApiProblem();

export default {
  name: 'SmQualityProblemReportModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      files: [],
      imgtypes: ['.jpg', '.png', '.tif', 'gif', '.JPG', '.PNG', '.GIF', '.jpeg', '.JPEG'],
      record: {}, // 表单绑定的对象,
      loading: false, //确定按钮加载状态
    };
  },
  computed: {
    title() {
      // 计算模态框的标题变量
      return utils.getModalTitle(this.status);
    },
    visible() {
      // 计算模态框的显示变量k
      return this.status !== ModalStatus.Hide;
    },
  },
  async created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiProblem = new ApiProblem(this.axios);
    },
    view(record) {
      this.status = ModalStatus.Add;
      this.record = record;
      this.refresh();
    },
    async refresh() {
      try {
        let response = await apiProblem.getQualityProblemReport(this.record.id);
        if (requestIsSuccess(response) && response.data) {
          this.record = response.data;
          //抄送人
          let _ccUsers = '';
          if (this.record && this.record.ccUsers.length > 0) {
            this.record.ccUsers.map(item => {
              _ccUsers += item.ccUser.name + '、';
            });
            this.record.ccUsers = _ccUsers ? _ccUsers.slice(0, _ccUsers.length - 1) : '';
          }
          //设备
          let _equipments = '';
          if (this.record && this.record.equipments.length > 0) {
            this.record.equipments.map(item => {
              _equipments += item.equipment.name + '、';
            });
            this.record.equipments = _equipments ? _equipments.slice(0, _equipments.length - 1) : '';
          }
        }
      } catch (error) {
        this.$message.warning(error.message);
      }
    },
    print() {
      printJS({
        printable: 'sm-quality-problem-report-modal',
        type: 'html',
        maxWidth: '100%',
        targetStyles: ['*'],

      });
    },
    play(file) {
      let imgtypes = ['.jpg', '.png', '.tif', 'gif', '.JPG', '.PNG', '.GIF', '.jpeg', '.JPEG'];
      // let videoTypes = ['.avi', '.mov', '.rmvb', '.rm', '.flv', '.mp4', '.3gp', '.mpeg', '.mpg'];
      let videoTypes = [];
      if (file.type === '.pdf') {
        this.$refs.SmFileDocumentView.view(file);
      } else if (imgtypes.includes(file.type)) {
        this.$refs.SmFileImageView.view(file);
      } else if (videoTypes.includes(file.type)) {
        this.$refs.SmVideo.preview(true, getFileUrl(file.url), file.name);
      } else {
        this.$message.warning('当前文件不支持预览，开始下载中...');
        this.downloadClick(file);
      }
    },
    downloadClick(file) {
      let _this = this;
      if (file) {
        ossRepository
          .download(getFileUrl(file.url), () => { })
          .then(blob => {
            SaveSingleFile(`${file.name}${file.type}`, file.size, blob).then(a => {
              _this.$notification['success']({
                message: '温馨提示',
                description: `${file.name}下载成功`,
                duration: 2,
              });
            });
          });
      }
    },
    fileShow(files) {
      let _file = [];
      files && files.map(item => {
        _file.push(
          <span class="file" onClick={() => {
            this.play(item.file);
          }}>
            {this.imgtypes.includes(item.file.type) ?
              <img src={getFileUrl(item.file.url)} alt={`${item.file.name}`} class="picture" />
              :
              <a-tooltip placement="topLeft" title={item.file.name + item.file.type}>
                <a-tag>{`${item.file && item.file.name && item.file.name.length < 8 ?
                  item.file.name : item.file.name.substring(0, 8) + '...'}` + item.file.type}</a-tag>
              </a-tooltip>

            }
          </span>,
        );
      });
      return _file;
    },
    safeRecordType(type) {
      let name = null;
      switch (type) {
      case SafeRecordState.Checking:
        name = '检查中';
        break;
      case SafeRecordState.NotPass:
        name = '未通过';
        break;
      case SafeRecordState.Passed:
        name = '通过';
        break;
      }
      return name;
    },
    safeProblemReport() {
      let _qualityProblem = [];
      this.record && this.record.qualityProblemRecord && this.record.qualityProblemRecord.map((item, index) => {
        _qualityProblem.push(
          <div style="margin-top: 20px;">
            <a-row class="rows">
              <a-col span="24">
                <span class="text" style="font-weight: bold;">
                  {item.type == SafeRecordType.Improve ? `整改记录 (${this.safeRecordType(item.state)})` : `验证记录 (${this.safeRecordType(item.state)})`}
                </span>
              </a-col>
            </a-row>
            <a-row class="rows">
              <a-col span="3" class="title">
                {item.type == SafeRecordType.Improve ? "整改人" : '验证人'}
              </a-col>
              <a-col span="9" class="longText">
                <span class="text">{item.user ? item.user.name : ''}</span>
              </a-col>
              <a-col span="3" class="title">
                {item.type == SafeRecordType.Improve ? "整改时间" : '验证时间'}
              </a-col>
              <a-col span="9" class="longText">
                <span class="text">{item.time ? moment(item.time).format('YYYY-MM-DD') : ''}</span>
              </a-col>

            </a-row>
            <a-row class="rows">
              <a-col span="3" class="title">
                {item.type == SafeRecordType.Improve ? "整改意见" : '验证意见'}
              </a-col>
              <a-col span="21" class="longText">{<div class="descriptions-item-media">{item.content ? item.content : ''}</div>}</a-col>
            </a-row>
            <a-row class={{
              "rows": true,
              "file-row": true,
              "row-bottom": index == this.record.qualityProblemRecord.length - 1,
            }}>
              <a-col span="3" class="title">
                附件
              </a-col>
              <a-col span="21" class="longText">{<div class="descriptions-item-media">{this.fileShow(item.files)}</div>}</a-col>
            </a-row>
          </div>,
        );
      });
      return _qualityProblem;
    },
    close() {
      this.record = null;
      this.files = [];
      this.status = ModalStatus.Hide;
    },
    rewHeight() {
      let rews = document.getElementsByClassName('rows');
      for (let item of rews) {
        let rewHeight = item.offsetHeight;
        item.style.height = rewHeight - 1 + 'px';
      };
    },
  },
  render() {
    setTimeout(() => { this.rewHeight(); }, 1);
    return (
      <a-modal
        title={`质量问题报告`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.loading}
        destroyOnClose={true}
        visible={this.visible}
        footer={null}
        width={1000}
      >
        <div class="sm-quality-problem-report-modal" >
          <div id="sm-quality-problem-report-modal">
            <a-row>
              <a-col span="24" class="body-title">{this.record ? this.record.title : ''}</a-col>
            </a-row>
            <a-row class="code">
              <a-col span="24" class="body-code">
                <span >{`问题编号：${this.record ? this.record.code : ''}`}</span>
              </a-col>
            </a-row>
            <a-row class="rows">
              <a-col span="3" class="title">
                问题标题
              </a-col>
              <a-col span="9" class="longText">
                <span class="text">
                  {this.record ? this.record.title : ''}
                </span>
              </a-col>
              <a-col span="3" class="title">
                问题类型
              </a-col>
              <a-col span="9" class="longText">
                <span class="text">{this.record && this.record.type ? getQualityProblemType(this.record.type) : ''}</span>
              </a-col>
            </a-row>
            <a-row class="rows">
              <a-col span="3" class="title">
                检查时间
              </a-col>
              <a-col span="9" class="longText">
                <span class="text">
                  {this.record && this.record.checkTime ? moment(this.record.checkTime).format('YYYY-MM-DD') : ''}
                </span>
              </a-col>
              <a-col span="3" class="title">
                限期时间
              </a-col>
              <a-col span="9" class="longText">
                {this.record && this.record.limitTime ? moment(this.record.limitTime).format('YYYY-MM-DD') : ''}
              </a-col>
            </a-row>
            <a-row class="rows">
              <a-col span="3" class="title">
                检查人
              </a-col>
              <a-col span="9" class="longText">
                <span class="text">{this.record && this.record.checker ? this.record.checker.name : ''}</span>
              </a-col>
              <a-col span="3" class="title">
                责任单位
              </a-col>
              <a-col span="9" class="longText">
                <span class="text">{this.record ? this.record.responsibleUnit : ''}</span>
              </a-col>
            </a-row>

            <a-row class="rows">
              <a-col span="3" class="title">
                责任人
              </a-col>
              <a-col span="9" class="longText">
                <span class="text">{this.record && this.record.responsibleUser ? this.record.responsibleUser.name : ''}</span>
              </a-col>
              <a-col span="3" class="title">
                抄送人
              </a-col>
              <a-col span="9" class="longText">
                <span class="text">{this.record ? this.record.ccUsers : ''}</span>
              </a-col>
            </a-row>
            <a-row class="rows">
              <a-col span="3" class="title">
                责任部门
              </a-col>
              <a-col span="9" class="longText">
                <span class="text">{this.record && this.record.responsibleOrganization ? this.record.responsibleOrganization.name : ''}</span>
              </a-col>
              <a-col span="3" class="title">
                验证人
              </a-col>
              <a-col span="9" class="longText">
                <span class="text">{this.record && this.record.verifier ? this.record.verifier.name : ''}</span>
              </a-col>
            </a-row>
            <a-row class="rows">
              <a-col span="3" class="title">
                关联模型
              </a-col>
              <a-col span="21" class="longText">{<div class="descriptions-item-media">{this.record ? this.record.equipments : ''}</div>}</a-col>
            </a-row>
            <a-row class="rows">
              <a-col span="3" class="title">
                问题描述
              </a-col>
              <a-col span="21" class="longText">{<div class="descriptions-item-media">{this.record ? this.record.content : ''}</div>}</a-col>
            </a-row>
            <a-row class="rows">
              <a-col span="3" class="title">
                整改意见
              </a-col>
              <a-col span="21" class="longText">{<div class="descriptions-item-media">{this.record ? this.record.suggestion : ''}</div>}</a-col>
            </a-row>
            <a-row class={{
              "rows": true,
              "file-row": true,
              "row-bottom": this.record && this.record.qualityProblemRecord && this.record.qualityProblemRecord.length == 0,
            }}>
              <a-col span="3" class="title">
                附件
              </a-col>
              <a-col span="21" class="longText">{<div class="descriptions-item-media">{this.record && this.record.files ? this.fileShow(this.record.files) : null}</div>}</a-col>
            </a-row>
            {this.record ? this.safeProblemReport() : null}
          </div>

          <div class="sm-quality-action-button">
            <span>
              <a-button
                size="small"
                class="close"
                onClick={() => {
                  this.close();
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

      </a-modal>
    );
  },
};
