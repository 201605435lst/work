import * as utils from '../../../_utils/utils';
import { ModalStatus, InterfaceFlagType } from '../../../_utils/enum';
import ApiConstructInterfaceInfo from '../../../sm-api/sm-technology/ConstructInterfaceInfo';
let apiConstructInterfaceInfo = new ApiConstructInterfaceInfo();
import moment from 'moment';
import '../style/index';
import { requestIsSuccess, getMarkType, getFileUrl, getConstructType } from '../../../_utils/utils';
import SmVideo from '../../../sm-common/sm-video/SmVideo';
import SmFileImageView from '../../../sm-file/sm-file-manage/src/component/SmFileImageView';
import SmFileDocumentView from '../../../sm-file/sm-file-manage/src/component/SmFileDocumentView';
export default {
  name: 'SmTechnologyTimeLine',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      record: {}, // 表单绑定的对象,
      data: [],
      imgtypes: ['.jpg', '.png', '.tif', 'gif', '.JPG', '.PNG', '.GIF', '.jpeg', '.JPEG'],
      dataSource: [],
      type: null, //记录单类型
      loading: false, //确定按钮加载状态
    };
  },

  computed: {
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
      apiConstructInterfaceInfo = new ApiConstructInterfaceInfo(this.axios);
    },
    view(record, type) {
      this.type = type;
      this.record = record;
      this.refresh();
      this.status = ModalStatus.View;
    },
    async refresh() {
      let data = {
        constructInterfaceId: this.record ? this.record.id : null,
      };
      let response = await apiConstructInterfaceInfo.getInterfanceReform({
        ...data,
      });

      if (requestIsSuccess(response)) {
        this.dataSource = response.data;
        this.dataSource.forEach(item => {
          let _fileList = [];
          _fileList = item.markFiles;
          _fileList.forEach(files => {
            let _file = files.markFile;
            if (this.imgtypes.includes(_file.type)) {
              files.url = getFileUrl(_file.url);
            }
          });
        });
        console.log(this.dataSource);
      }
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
    // 关闭模态框
    close() {
      this.status = ModalStatus.Hide;
    },
  },
  render() {
    let _dataSource = [];
    this.dataSource.map(item => {
      _dataSource.push(
        <a-timeline-item color="green" class="timeLine">
          <div class="timeline-item-detail">
            <div class="item-detail-content">
              <span class="item">检查人员：</span>
              <span>{item.marker ? item.marker.name : null}</span>
            </div>
            <div class="item-detail-content">
              <span class="item">土建单位：</span>
              <span>{item.builder ? item.builder.name : null}</span>
            </div>
            <div class="item-detail-content">
              <span class="item">接口状况：</span>
              <span>{item.markType ? getMarkType(item.markType) : null}</span>
            </div>
            <div class="item-detail-content">
              <span class="item">状况原因：</span>
              <span>{item.reason}</span>
            </div>
            <div class="item-detail-content imgs">
              <span class="item">检查图片：</span>
              {item.markFiles.map(item => {
                if (item.type == InterfaceFlagType.InterfaceFlag) {
                  let _name = item.markFile ? item.markFile.name : null;
                  console.log(_name);
                  return (
                    <span
                      onClick={() => {
                        this.play(item.markFile);
                      }}
                    >
                      <img src={item.url} alt={`${_name}`} class="picture" />
                    </span>
                  );
                }
              })}
            </div>
            {this.type == 'report' && item.reformer && item.reformDate ? (
              <div>
                <div class="item-detail-content">
                  <span class="item">整改人：</span>
                  <span>{item.reformer ? item.reformer.name : null}</span>
                </div>
                <div class="item-detail-content">
                  <span class="item">整改时间：</span>
                  <span>
                    {' '}
                    {item.markDate ? moment(item.reformDate).format('YYYY-MM-DD HH:mm:ss') : null}
                  </span>
                </div>
                <div class="item-detail-content">
                  <span class="item">整改情况：</span>
                  <span>{item.reformExplain ? getMarkType(item.reformExplain) : null}</span>
                </div>
                <div class="item-detail-content imgs">
                  <span class="item">整改图片：</span>
                  {item.markFiles.map(item => {
                    if (item.type == InterfaceFlagType.InterfaceFlag) {
                      let _name = item.markFile ? item.markFile.name : null;
                      return (
                        <span
                          onClick={() => {
                            this.play(item.markFile);
                          }}
                        >
                          <img src={item.url} alt={`${_name}`} class="picture" />
                        </span>
                      );
                    }
                  })}
                </div>
              </div>
            ) : null}
          </div>
        </a-timeline-item>,
        <a-timeline-item>
          <a-icon slot="dot" type="clock-circle-o" style="font-size: 16px;" />
          {item.markDate ? moment(item.markDate).format('YYYY-MM-DD HH:mm:ss') : null}
        </a-timeline-item>,
      );
    });

    return (
      <div class="sm-technology-time-line">
        <a-modal
          class="sm-technology-time-line-modal"
          title={this.type == 'report' ? '接口报告' : '接口标记'}
          visible={this.visible}
          onCancel={this.close}
          confirmLoading={this.loading}
          destroyOnClose={true}
          footer={null}
          width={800}
        >
          <div class="timeline-detail">
            <div>
              {/* <a-timeline mode="alternate">{_dataSource}</a-timeline> */}
              {this.dataSource.length > 0 ? (
                <a-timeline>{_dataSource}</a-timeline>
              ) : (
                <a-empty
                  description={this.type == 'report' ? '暂无接口报告数据' : '暂无接口标记数据'}
                />
              )}
            </div>
          </div>
        </a-modal>
        <SmVideo axios={this.axios} ref="SmVideo" />{/* 图片类预览组件 */}
        <SmFileImageView axios={this.axios} ref="SmFileImageView" />{/* 文档浏览组件 */}
        <SmFileDocumentView axios={this.axios} ref="SmFileDocumentView" />
      </div>
    );
  },
};
