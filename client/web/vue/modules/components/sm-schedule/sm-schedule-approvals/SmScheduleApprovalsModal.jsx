import SmBmpFlowBase from '../../sm-common/sm-flow-base';

export default {
  name: 'BmpFlowModal',
  props:{
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false }, // 模态框可见性
    width: { type: Number, default: 1100 }, // 模态框宽度
    height: {type: Number, default: 300}, //高度
    nodes: { type: Array, default: () => [] },    //数据
  },
  data() {
    return {
      iValue: null,
      iVisible: false,
      iNodes: [],
    };
  },
  computed: {
  },
  watch: {
    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
      },
      immediate: true,
    },
    nodes:{
      handler: function (value, oldValue) {
        this.iNodes = value;
      },
      immediate: true,
    },
  },
  methods: {
    onClose() {
      this.$emit('change', false);
      this.iNodes = [];
    },
    isClose(v) {
      this.iVisible = v;
      this.onClose();
    },
  },
  render() {
    return (
      <a-modal
        width={this.width}
        height={this.height}
        title="审批流程"
        visible={this.iVisible}
        onCancel={this.onClose}
        destroyOnClose
      >
        <div class="scheduleFlow">
          <SmBmpFlowBase
            nodes={this.iNodes}
            bpmNode={true}
            showDetail={false}
            //scroller={false}
          />
        </div>
        <template slot="footer">
          <a-button onClick={() => this.onClose()}>
            关闭
          </a-button>
        </template>
      </a-modal>
    );
  },
};