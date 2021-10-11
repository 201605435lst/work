export default {
  props: {
    left: { type: Number, default: 616 },
    right: { type: Number, default: 704 },
    layIsHandleOver: { type: Boolean, default: false },
  },
  data() {
    return {
      upShow:true, //上面显示
      downShow:true, //下面显示
    };
  },
  methods: {
    // 按一下显示最大化的视图模块 ,在按一下均分
    showView() {
      this.$emit('showView');
      if (this.left > 0) {
        this.downShow=false;
      }else{
        this.downShow=true;
      }
    },

    showTable() {
      this.$emit('showTable');
      if (this.right > 20) {
        this.upShow=false;
      }else{
        this.upShow=true;
      }
    },

  },
  render() {
    return <div
      class={{ divider__3vA4: true, 'vertical-disabled__J9XJ': this.layIsHandleOver }}
      style={`left: ${this.left}px;`}>
      {this.upShow &&
      <div onClick={this.showView} class='icon-wrapper__1ugp icon-wrapper__1ugp_left' style='top: 157.5px;'>
        {/*<i class={{ arrow__3Sjh: true, reverse__3bNw: !(this.left > 0) }} />*/}
        {this.left > 0 ? <a-icon type='left' /> : <a-icon type='right' />}
      </div>
      }
      {this.downShow &&
      <div onClick={this.showTable} class='icon-wrapper__1ugp wrapper__1ugp_right' style='top: 200.px;'>
        {this.right > 20 ? <a-icon type='right' /> : <a-icon type='left' />}

      </div>
      }
    </div>


    // 原来的代码
    // <div
    //   class={{ divider__3vA4: true, 'vertical-disabled__J9XJ': this.layIsHandleOver }}
    //   style={`left: ${this.left}px;`}
    //   onClick={this.click}>
    //   <div class='icon-wrapper__1ugp' style='top: 157.5px;'>
    //     <i class={{ arrow__3Sjh: true, reverse__3bNw: !(this.left > 0) }} />
    //   </div>
    // </div>
    ;
  },
};
