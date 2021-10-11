import './style/index';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmSafeProblem from '../../sm-safe/sm-safe-problem';
export default {
  name: 'SmD3Quality',
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false }, //面板是否弹出
    position: {
      type: Object,
      default: () => {
        return { left: '280px', bottom: '20px' };
      },
    },
    height: { type: String, default: '60%' },
    width: { type: String, default: '840px' },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      iVisible: false,
      shwoInfo: false,
      loading: false,
      record: null,
    };
  },

  computed: {},

  watch: {
    visible: {
      handler: function(value, oldValue) {
        this.iVisible = value;
      },
      immediate: true,
    },
  },

  async created() {
  },

  mounted() {},

  methods: {
  },
  render() {
    return (
      <sc-panel
        class="sm-d3-safe"
        bordered
        borderedRadius
        visible={this.iVisible}
        position={this.position}
        bodyFlex
        height={this.height}
        width={this.width}
        animate="bottomEnter"
        forceRender
        icon="alert"
        resizable
        title="安全问题"
        onClose={visible => {
          this.iVisible = visible;
          this.$emit('close', this.iVisible);
        }}
      >
        <a-icon slot="icon" type="interaction" />
        <SmSafeProblem
          permissions={this.permissions}
          isD3={true}
          axios={this.axios}
          onFlyTo={(data) => {
            console.log("data",data);
            this.$emit('flyTo', data);
          }}
        />
      </sc-panel>
    );
  },
};
