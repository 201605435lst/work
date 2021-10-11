import Hammer from 'hammerjs';

// 小圆点div
export default  {
  props: {
    barInfo: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      rowHandleHam: null, // 定义一个 hammer 操作对象
    };
  },
  // 挂载的时候操作dom
  mounted() {
    this.initGesture();
  },
  beforeDestroy() {
    if (this.rowHandleHam != null) {
      this.rowHandleHam.destroy();
      this.rowHandleHam=null;
    }
  },
  methods: {
    rowHandleClick(event) {
      this.$emit('rowHandleClick', event, this.barInfo);
    },
    // 初始化行拖拽事件 拖拽小圆点 用来排序
    initGesture() {
      // 找到 rowHandle 的 dom
      const rowHandle = this.$refs.rowHandle;
      // new 一个 hammer 对象
      this.rowHandleHam = new Hammer(rowHandle);
      // 给这个 dom 绑定 hammer 的拖拽 事件(拖拽 时 冒泡到父级)
      this.rowHandleHam.on('press', (event) => this.$emit('gesturePress', event, this.barInfo))
      ;
      // this.rowHandleHam.on('pressup', (event) => {
      //   console.log('放下小圆点');
      //   return this.$emit('gesturePressup', event, this.barInfo);
      // });
    },

  },
  render() {
    // 这是一个小圆点 div
    return <div class='body-row-handle__3YUe clickable__1NNN' ref='rowHandle' onClick={this.rowHandleClick}>
      <i />
    </div>;
  },

};
