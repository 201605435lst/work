import './style/index.less';
import SmStdBasicWorkAttention from '../sm-std-basic-work-attention';


export default {
  name: 'SmStdBasicWorkAttentionModal',
  model: {
    prop: 'visible',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false },
    value: { type: [String, Array], default: null },//已选项
    multiple: { type: Boolean, default: false }, // 是否多选
    repairTagKey: { type: String, default: null },
  },
  data() {
    return {
      selectedWorkAttention: [],
      iVisible: false,
    };
  },

  computed: {},

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

    ok() {
      // 数据提交
      let res = '';
      if (this.selectedWorkAttention.length > 0) {
        this.selectedWorkAttention.map(item => {
          res += item.content + "\r\n";
        });
      }
      this.$emit('ok', res);
      this.close();
    },
    close() {
      this.$emit('change', false);
      this.selectedWorkAttention = [];
    },
  },

  render() {
    return (
      <a-modal
        width={800}
        title="作业注意事项"
        class="sm-basic-selected-work-attention-modal"
        visible={this.iVisible}
        onOk={this.ok}
        onCancel={this.close}
      >
        <div class="selected-work-attention-list">
          <SmStdBasicWorkAttention
            axios={this.axios}
            isSimple={true}
            repairTagKey={this.repairTagKey}
            multiple={this.multiple}
            selected={this.selectedWorkAttention}
            onChange={selected => {
              this.selectedWorkAttention = selected;
            }}
          />
        </div>
      </a-modal>
    );
  },
};
