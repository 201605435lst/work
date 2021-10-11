import './style/index.less';
import SmScheduleDiaryLog from '../sm-schedule-diary-log/SmScheduleDiaryLog';

export default {
  name: 'SmScheduleDiaryModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      visible: false,
      id: null,
      record:null,
      preRecored:null,
    };
  },

  watch: {},

  async created() {},

  methods: {
    onClose() {
      this.visible = false;
    },
    view(record) {
      this.visible = true;
      this.record = record;
    },
    preview(data){
      this.visible = true;
      this.preRecored=data;
    },
  },

  render() {
    return (
      <div class="sm-schedule-diary-modal">
        <a-modal
          width={1200}
          class="diary-modal"
          title="日志预览"
          visible={this.visible}
          footer={null}
          onCancel={this.onClose}
        >
          <div class="schedule-diary-modal">
            <SmScheduleDiaryLog axios={this.axios} preRecored={this.preRecored}  record={this.record} onCancel={this.onClose} />
          </div>
        </a-modal>
      </div>
    );
  },
};
