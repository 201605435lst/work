
export default {
  // 右侧的任务小块块 - 缩略图
  props: {
    viewWidth: {
      type: Number,
      default: 1332,
    },
    viewTranslateX: {
      type: Number,
      default: 552410,
    },
    label: {
      type: String,
      default: '',
    },
    translateX: {
      type: Number,
      default: 554810,
    },
    translateY: {
      type: Number,
      default: 554810,
    },
    width: {
      type: Number,
      default: 30,
    },
  },
  computed: {
    left() {
      let left = this.viewTranslateX + 2;

      if (this.type === 'right') {
        left = this.viewTranslateX + this.viewWidth - 2;
      }

      return left;
    },
    top() {
      return this.translateY - 10;
    },
    type() {
      return this.getThumbType();
    },
    sideClass() {
      return this.type === 'left' ? ['left__3u5u'] : ['right__3MHd'];
    },

  },
  methods: {
    /**
     *  获取浮动文字的类型
     */
    getThumbType() {
      const rightSide = this.viewTranslateX + this.viewWidth;
      const right = this.translateX;

      return right - rightSide > 0 ? 'right' : 'left';
    },
    timeTranslateLocation() {
      let translateX1 = this.viewTranslateX + (this.viewWidth / 2);
      let translateX2 = this.translateX + this.width;

      let diffX = Math.abs(translateX2 - translateX1);
      let translateX = this.viewTranslateX + diffX;

      if (this.type === 'left') {
        translateX = this.viewTranslateX - diffX;
      }

      this.$emit('timeTranslateLocation', translateX);
    },
  },
  render(){
    return <div
      onClick={event => event.stopPropagation()}
      class={`task-bar-thumb__2suT ${this.sideClass}`}
      style={`top: ${this.top}px; left: ${this.left}px;`}
    >
      <button class='btn-type-subtle__1YDY btn-size-smaller__zaJ7' onClick={this.timeTranslateLocation} type='button' data-role='button'>
        {this.type === 'right' &&
        <div class='label__fdzT' style='left: 0px;'>{this.label}</div>
        }
        {this.type === 'left' &&
        // <i class='icon__YImk icon dls-icon icon-triangle-right-s' data-role='icon'/>
        <a-icon class='icon__YImk icon dls-icon icon-triangle-right-s' type="caret-right" />
        }
        {this.type === 'right' &&
        <i class='icon__YImk icon dls-icon icon-triangle-left-s' data-role='icon'/>
        }
        {this.type === 'left' &&
        <div class='label__fdzT' style='left: 0px;'>{this.label}</div>
        }
      </button>
    </div>;
  },
};
