export default   {
  props: {
    value: { type: String, default: '' },
    barInfo: { type: Object, default: () => ({}) },
  },
  methods: {
    /**
     * 触发失焦事件完成任务添加
     */
    onBlur() {
      // console.log('失去焦点');
      let value = this.$refs.input.value.trim();
      this.$parent.completeInput(this.barInfo, value);
    },
    /**
     * 按键事件触发
     * todo 这里不能触发  onKeyPress 和 onKeyDown
     */
    keydown($event) {
      // enter 按键
      if (
        !(
          !$event.type.indexOf("key") &&
          this._k($event.keyCode, "enter", 13, $event.key, "Enter")
        )
      ) {
        // return this.onKeydownEnter($event); // 原来回车是 连续添加 新任务
        //改成 完成创建
        let value = this.$refs.input.value.trim();
        this.$parent.completeInput(this.barInfo, value);
      }
      // //这些先注释掉,用鼠标来确定 任务的升级 降级 吧……
      // // shift + tab 组合按键
      // if (
      //   !(
      //     !$event.type.indexOf("key") &&
      //     this._k($event.keyCode, "tab", 9, $event.key, "Tab")
      //   ) &&
      //   $event.shiftKey
      // ) {
      //   return this.onKeyShiftTab($event);
      // }
      //
      // // Tab 按键
      // if (
      //   !(
      //     !$event.type.indexOf("key") &&
      //     this._k($event.keyCode, "tab", 9, $event.key, "Tab")
      //   )
      // ) {
      //   return this.onKeyTab($event);
      // }
      //
    },
    /**
     * enter 事件按下并且 添加新纪录 连续添加任务
     */
    onKeydownEnter() {
      const input = this.$refs.input;
      const value = input.value;
      if (!value.trim()) return;
      input.blur();
      this.$parent.$nextTick(() => {
        this.$parent.addCacheTask(this.barInfo._parent);
      });
    },
    /**
     * Tab 和 Shift 组合键 按下
     */
    onKeyShiftTab($event) {
      $event.preventDefault();

      // 处在任务添加状态排除
      if (this.$parent.cacheRow) return;

      this.$parent.moveLeftTask(this.barInfo);
    },
    /**
     * tab按键按下
     */
    onKeyTab($event) {
      $event.preventDefault();

      // 处在任务添加状态排除
      if (this.$parent.cacheRow) return;

      // 开始向右侧移动
      this.$parent.moveRightTask(this.barInfo);
    },

  },
  mounted() {
    const input = this.$refs.input;
    input.setSelectionRange(this.value.length, this.value.length);
    input.focus();
  },
  render() {
    return <input
      class='input__Q-pw input-hover__35mM input-size-normal__3P2H input__3gEb gantt-app-outline-content-input'
      placeholder={'输入任务标题'}
      value={this.value}
      onBlur={this.onBlur}
      onKeydown={this.keydown}
      ref='input'
    />;
  },

};
