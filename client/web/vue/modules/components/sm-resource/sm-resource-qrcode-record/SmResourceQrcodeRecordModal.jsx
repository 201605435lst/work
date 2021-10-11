import { ModalStatus } from '../../_utils/enum';
import SmResourceQrcodeRecord from '../sm-resource-qrcode-record/SmResourceQrcodeRecord';
export default {
  name: 'SmResourceQrcodeRecordModal',
  props: {
    axios: { type: Function, default: null },
    value: { type: Array, default: null },
  },
  data() {
    return {
      disabled: true,
      record: null,
      iValue: null,
      titleName: '',
      iVisible: false,
      status: ModalStatus.Hide, // 模态框状态
      confirmLoading: false, //确定按钮加载状态
    };
  },
  computed: {
    visible() {
      // 计算模态框的显示变量k
      return this.status !== ModalStatus.Hide;
    },
  },
  watch: {
  },
  methods: {
    // 关闭模态框
    close() {
      this.status = ModalStatus.Hide;
      this.confirmLoading = false;
    },
    view(record) {
      this.titleName = record.name;
      this.record = record;
      this.status = ModalStatus.View;
      this.iValue = record.id;
    },
  },
  render() {
    return (
      <a-modal
        width={600}
        title={this.titleName + '构件跟踪二维码履历'}
        visible={this.visible}
        onCancel={this.close}
        onOk={this.close}
        destroyOnClose={true}
        confirmLoading={this.confirmLoading}
      >
        <SmResourceQrcodeRecord
          value={this.iValue}
          axios={this.axios}
          iconParameter={2}
          showPerson={this.disabled}
          // showInfo={this.disabled}
          bordered={false}
        />
      </a-modal>
    );
  },
};
