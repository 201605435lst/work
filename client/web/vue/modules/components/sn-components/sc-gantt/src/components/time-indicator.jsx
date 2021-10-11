
import dayjs from "dayjs"; // 导入日期js
export default {
  // 这个组件是今天按钮
  props: {
    viewTranslateX: {
      type: Number,
      default: 554780,
    },
    tableWidth: {
      type: Number,
      default: 0,
    },
    viewWidth: {
      type: Number,
      default: 1322,
    },
    pxUnitAmp: {
      type: Number,
      default: 0,
    },
    guestureGrantBodyMove: {
      type: Boolean,
      default: false,
    },
  },
  computed: {
    curTranslateX() {
      return Math.floor(dayjs(new Date().valueOf()).hour(0).minute(0).second(0).valueOf() / this.pxUnitAmp);
    },
    type() {
      let type = 'right';
      if (this.curTranslateX < this.viewTranslateX) {
        type = 'left';
      }
      return type;
    },
    left() {
      return this.type=== 'left' ? `${this.tableWidth}px` : 'unset';
    },
    right() {
      return this.type === 'right' ? 111 + 'px' : 'unset';
    },
    display() {
      let isOverLeft = this.curTranslateX < this.viewTranslateX;
      let isOverRight = this.curTranslateX > this.viewTranslateX + this.viewWidth;
      return (isOverLeft || isOverRight) ? '' : 'display: none';
    },
  },
  methods: {
    timeTranslateLocation() {
      const translateX = this.curTranslateX - (this.viewWidth / 2);
      this.$emit('timeTranslateLocation', translateX);
    },
  },
  render() {
    return <button
      onClick={this.timeTranslateLocation}
      class={`btn-type-secondary__h-_i move-to-today__uxgP btn-size-smaller__zaJ7 ${this.guestureGrantBodyMove ? 'scrolling__3E94':''}`}
      type='button'
      data-role='button'
      style={`left: ${this.left}; right: ${this.right};${this.display}`}>
      {this.type === 'left' &&
      <a-icon type='left-circle' />
      }
      <span>今天</span>
      {this. type === 'right' &&
      <a-icon type="right-circle" />
      }
    </button>;

  },
};
