import * as utils from '../../_utils/utils';
import { ModalStatus, InterfaceFlagType } from '../../_utils/enum';
import ApiComponentQrCode from '../../sm-api/sm-componenttrack/componentQrCode';
import { requestIsSuccess, getNodeTypeTitle } from '../../_utils/utils';
import moment from 'moment';
import './style';
import { color } from 'echarts/lib/export';
let apiComponentQrCode = new ApiComponentQrCode();
export default {
  name: 'SmComponentQrCodeTimeLineModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      record: {}, // 表单绑定的对象,
      id: null,
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
      apiComponentQrCode = new ApiComponentQrCode(this.axios);
    },
    view(record) {
      this.record = record;
      this.refresh();
      this.status = ModalStatus.View;
    },
    async refresh() {
      try {
        let data = {
          componentTrackId: this.record ? this.record.id : '',
          trackProcessId: this.record ? this.record.trackProcessId : '',
        };
        let response = await apiComponentQrCode.getRecord(data);
        if (requestIsSuccess(response)) {
          this.dataSource = response.data;
          console.log(this.dataSource);
        }
      } catch (error) {
        this.$message.warning(error.message);
      }
    },
    // 关闭模态框
    close() {
      this.status = ModalStatus.Hide;
    },
  },
  render() {
    let _dataSource = [];
    this.dataSource && this.dataSource.nodes ? this.dataSource.nodes.map(item => {
      _dataSource.push(
        <div>
          <a-timeline-item>
            <p>
              发起人：{item.user.name}</p>
            <p>跟踪状态：<span style={{color:item.componentTrackRecord?'#5bcd6b':''}}>{getNodeTypeTitle(item.nodeType)}</span></p>
            <p>跟踪时间：{item.componentTrackRecord && item.componentTrackRecord.time ? moment(item.componentTrackRecord.time).format('YYYY-MM-DD HH:mm:ss') : ''}</p>
            <a-icon slot="dot" style="font-size: 16px;" type="check-circle" theme="twoTone" two-tone-color={!item.componentTrackRecord?'#A9A9A9':'#1890ff'} />
          </a-timeline-item>
        </div>,
      );
    }) : '';
    return (
      <div class="sm-component-track-time-line">
        <a-modal
          class="sm-component-track-time-line-modal"
          title={'跟踪计划查看'}
          visible={this.visible}
          onCancel={this.close}
          confirmLoading={this.loading}
          destroyOnClose={true}
          footer={null}
          width={800}
        >
          <div class="timeline-detail">
            <div>
              {this.dataSource && this.dataSource.nodes && this.dataSource.nodes.length > 0 ? (
                <a-timeline>{_dataSource}</a-timeline>
              ) : (
                <a-empty
                  description={'暂无数据，请刷新页面重新尝试'}
                />
              )}
            </div>
          </div>
        </a-modal>
      </div>
    );
  },
};
