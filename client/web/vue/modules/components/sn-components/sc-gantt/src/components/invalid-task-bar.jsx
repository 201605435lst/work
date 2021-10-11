// 右侧 没有设置的任务 块块  用来 拖拽时间来设置
export default {
  props: {
    rowH: {
      type: Number,
      default: 28,
    },
    barH: {
      type: Number,
      default: 8,
    },
    translateX: {
      type: Number,
      default: 0,
    },
    top: {
      type: Number,
      default: 0,
    },
    left: {
      type: Number,
      default: 0,
    },
    startXRectBar: {
      type: Function,
      default: () => ({ left: 0, width: 11 }),
    },
    dateTextFormat: {
      type: Function,
      default: () => '',
    },
    setShadowShow: {
      type: Function,
      default: () => {
      },
    },
    setInvalidTaskBar: {
      type: Function,
      default: () => {
      },
    },
  },
  data() {
    const { left, width } = this.startXRectBar(this.translateX);
    return {
      isShow: false,
      barLeft: left,
      barWidth: width,
      startX: 0,
    };
  },
  methods: {
    triggerMove(event) {
      // console.log('鼠标移动');
      const pointerX = this.translateX + (event.clientX - this.startX);
      const { left, width } = this.startXRectBar(pointerX);

      this.barLeft = left;
      this.barWidth = width;
      this.setShadowShow(left, Math.ceil(width), true);
    },
    triggerLeave() {
      // console.log('鼠标移出');
      this.isShow = false;
      this.setShadowShow(0, 0, false);
    },
    triggerDown(event) {
      this.setInvalidTaskBar(this.barLeft, this.barWidth);
      this.$emit('gesturePress', event, 'right');
    },
    triggerEnter() {
      // console.log('鼠标进入');
      this.isShow = true;
      this.startX = this.$refs.trigger.getBoundingClientRect().left;
    },
  },
  render() {
    // TODO  这里 不能触发 鼠标 移入 移出事件,导致无法拖拽 块块 来设置任务 开始 结束 时间
    return <div
      onMouseenter={this.triggerEnter}
      onMouseleave={this.triggerLeave}
      onMousemove={this.triggerMove}
      onMousedown={this.triggerDown}
      class='ifnk'
    >
      <div
        class='task-row-trigger__1Xhy'
        ref='trigger'
        style={`left: ${this.translateX}px; transform: translateY(${this.top - ((this.rowH - this.barH) / 2)}px);`}>
      </div>
      {this.isShow &&
      <div
        class='block__1ibh'
        aria-haspopup='true'
        aria-expanded='false'
        style={`left: ${this.barLeft}px; width: ${Math.ceil(this.barWidth)}px; transform: translateY(${this.top}px); background-color: rgb(149, 221, 255); border-color: rgb(100, 199, 254);`}
      >
        <div class='date__7am6' style={`right: ${Math.ceil(this.barWidth + 6)}px;`}>{this.dateTextFormat(this.barLeft)}</div>
        <div class='date__7am6' style={`left: ${Math.ceil(this.barWidth + 6)}px;`}>{this.dateTextFormat(this.barLeft + this.barWidth)}</div>
      </div>
      }

    </div>;
  },
};
