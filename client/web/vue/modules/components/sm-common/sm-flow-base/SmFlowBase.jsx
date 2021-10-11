import { Graph, Node, Point } from '@antv/x6';
import dagre from 'dagre';
import TaskNode from './components/TaskNode';
import BpmNode from './components/BpmNode';
import '@antv/x6-vue-shape';
import './style';
Graph.registerEdge(
  'org-edge',
  {
    zIndex: -1,
    attrs: {
      line: {
        stroke: '#585858',
        strokeWidth: 2,
        sourceMarker: null,
        targetMarker: null,
      },
    },
  },
  true,
);
export default {
  name: 'SmFlowBase',
  components: {TaskNode,BpmNode},
  props: {
    nodes: { type: Array, default: () => [] },
    height: { type: Number, default: () => 600 },
    width: { type: Number, default: () => 900 },
    scroller: { type: Boolean, default: () => true },
    grid: { type: Boolean, default: () => true },
    filedTrans: { type: Object, default: () => new Object() },
    bpmNode: { type: Boolean, default: false }, // 是否为工作流的节点
    showDetail: { type: Boolean, default: false },// 是否显示工作流的详情
    isView: { type: Boolean, default: false },// 是否为查看模式，只显示节点名称
  },
  data() {
    return {
      graph: null,
      iNodes: [],
      testData: 555,
      iView: false,
    };
  },
  computed: {},
  watch: {
    isView: {
      handler(n, o) {
        this.iView = n;
      },
      immediate: true,
    },
    nodes: {
      handler(n, o) {
        this.iNodes = n;
      },
      immediate: true,
    },
  },
  mounted() {
    this.init();
  },
  created() {
  },
  methods: {
    init() {
      this.graph = new Graph({
        container: this.$refs.container,
        grid: this.grid,
        height: this.height,
        width: this.width,
        scroller: this.scroller,
        snapline: true,
        interacting: false,
      });
      this.build();
      this.graph.centerContent();
    },
    // 创建节点
    createNode(data,iView,showDetail) {
      let _this = this;
      return this.graph.addNode({
        width: 180,
        height: 90,
        shape: "vue-shape",
        component:{
          render(){
            if(_this.bpmNode){
              return <BpmNode isView={iView} node={data} showDetail={showDetail} />;
            }else{
              data.title =_this.filedTrans['title'] === undefined ? data.title : data[_this.filedTrans['title']];
              data.subTitle = _this.filedTrans['subTitle'] === undefined? data.subTitle: data[_this.filedTrans['subTitle']];
              data.perc =  _this.filedTrans['perc'] === undefined ? data.perc : data[_this.filedTrans['perc']];
              data.type =_this.filedTrans['type'] === undefined ? data.type : data[_this.filedTrans['type']];
              return <TaskNode node={data}/>;
            }
          },
        },
      });
    },
    createChildrenNode(nodes, parentNode, allNodes, allEgde) {
      if (nodes) {
        nodes.forEach(item => {
          if (item.children == undefined) {
            item.children = [];
          }
          let node = this.createNode(item,this.iView,this.showDetail);
          let egde = this.createEdge(parentNode, node);
          if (item.children.length > 0) {
            this.createChildrenNode(item.children, node, allNodes, allEgde);
          }
          allNodes.push(node);
          allEgde.push(egde);
        });
      }
    },
    // 构建节点信息
    build() {
      let allNodes = [];
      let allEgde = [];
      if (this.iNodes.length > 0) {
        this.iNodes.forEach(item => {
          if (item.children == undefined) {
            item.children = [];
          }
          let node = this.createNode(item,this.iView,this.showDetail);
          if (item.children.length > 0) {
            this.createChildrenNode(item.children, node, allNodes, allEgde);
          }
          allNodes.push(node);
        });
        this.graph.resetCells([...allNodes, ...allEgde]);
        this.layout();
        this.graph.zoomTo(0.9);
        this.graph.centerContent();
      }
    },
    // 自动布局
    layout() {
      const dir =this.bpmNode?'LR': 'TB'; // LR RL TB BT
      const nodes = this.graph.getNodes();
      const edges = this.graph.getEdges();
      const g = new dagre.graphlib.Graph();
      g.setGraph({ rankdir: dir, nodesep: 16, ranksep: 16 });
      g.setDefaultEdgeLabel(() => ({}));
      const width = 180;
      const height = 70;
      nodes.forEach(node => {
        g.setNode(node.id, { width, height });
      });

      edges.forEach(edge => {
        const source = edge.getSource();
        const target = edge.getTarget();

        g.setEdge(source.cell, target.cell);
      });

      dagre.layout(g);
      this.graph.freeze();
      g.nodes().forEach(id => {
        const node = this.graph.getCell(id);
        if (node) {
          const pos = g.node(id);
          node.position(pos.x, pos.y);
        }
      });

      edges.forEach(edge => {
        const source = edge.getSourceNode();
        const target = edge.getTargetNode();
        const sourceBBox = source.getBBox();
        const targetBBox = target.getBBox();

        if ((dir === 'LR' || dir === 'RL') && sourceBBox.y !== targetBBox.y) {
          const gap =
            dir === 'LR'
              ? targetBBox.x - sourceBBox.x - sourceBBox.width
              : -sourceBBox.x + targetBBox.x + targetBBox.width;
          const fix = dir === 'LR' ? sourceBBox.width : 0;
          const x = sourceBBox.x + fix + gap / 2;
          edge.setVertices([
            { x, y: sourceBBox.center.y },
            { x, y: targetBBox.center.y },
          ]);
        } else if (dir === 'TB' || dir === 'BT') {
          const gap =
            dir === 'TB'
              ? targetBBox.y - sourceBBox.y - sourceBBox.height - 20
              : -sourceBBox.y + targetBBox.y + targetBBox.height;
          const fix = dir === 'TB' ? sourceBBox.height : 0;
          const y = sourceBBox.y + fix + gap / 2;
          edge.setVertices([
            { x: sourceBBox.center.x, y },
            { x: targetBBox.center.x, y },
          ]);
        } else {
          edge.setVertices([]);
        }
      });

      this.graph.unfreeze();
    },
    // 创建节点
    createEdge(source, target) {
      return this.graph.createEdge({
        shape: 'org-edge',
        source: { cell: source.id },
        target: { cell: target.id },
      });
    },
  },
  componentDidMount() {},
  render() {
    return (
      <div>
        <div ref="container">{this.$slots.test}</div>
      </div>
    );
  },
};
