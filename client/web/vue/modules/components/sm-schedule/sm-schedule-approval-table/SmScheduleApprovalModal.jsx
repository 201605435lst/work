import './style/index.less';
import SmScheduleApprovalTable from './SmScheduleApprovalTable';


export default {
  name: 'SmScheduleApprovalModal',
  model: {
    prop: 'visible',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false },
    id: { type: String, default: null },
  },
  data() {
    return {
      iVisible: false,
    };
  },

  watch: {
    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
      },
      immediate: true,
    },
  },

  async created() { },

  methods: {
    onClose() {
      this.$emit('cancel',false);
    },
  },

  render() {
    return (
      <a-modal
        width={1000}
        title="日志预览"
        visible={this.iVisible}
        footer={null}
        onCancel={this.onClose}
      >
        <div class="approvalModal">
          <SmScheduleApprovalTable
            axios={this.axios}
            id={this.id}
            onCancel={this.onClose}
          />
        </div>
      </a-modal>
    );
  },
};
