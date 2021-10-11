import './style/index.less';
import SmScheduleApprovals from '../sm-schedule-approvals/SmScheduleApprovals';

export default {
  name: 'SmScheduleDiaryAuthorizationModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      visible: false,
      id: null,
      dataSource:[],
    };
  },

  watch: {
  },

  async created() {},

  methods: {
    onClose() {
      this.visible = false;
    },
    infor(data) {
      this.visible = true;
      this.dataSource=data;
    },
  },

  render() {
    return (
      <div class="sm-schedule-diary-authorization-modal">
        <a-modal
          width={1200}
          title="审批单"
          class="schedule-diary-authorization-modal"
          visible={this.visible}
          footer={null}
          onCancel={this.onClose}
        >
          <div class="authorization-modal">
            <SmScheduleApprovals 
              axios={this.axios} 
              data={this.dataSource}
              isAuthorizationModal={true}
              onCancel={this.onClose} />
          </div>
        </a-modal>
      </div>
    );
  },
};
