export default {
  name: 'TaskNode',
  components: {},
  props: {
    node: { type: Object, default: null },
  },
  data() {
    return {};
  },
  computed: {},
  watch: {},
  created() {},
  methods: {},
  render() {
    return (
      <div class="sm-node">
        <div class="node-container">
          <div class="node-title">
            <p class="main-title">{this.node.title}</p>
            <p class="sub-title">审批人:{this.node.subTitle}</p>
          </div>
          <div class="node-right">
            <span class={`process task-type-${this.node.type}`}></span>
            <span class="number">{this.node.perc}%</span>
          </div>
          <div class="node-root"></div>
        </div>
      </div>
    );
  },
};
