export default {
  props: {
    collapsed: { type: Boolean, default: false },
    showAddBtn: { type: Boolean, default: true }, //显示 添加按钮
    width: { type: Number, default: 660 }, // 整个 table header 的宽度
    height: { type: Number, default: 56 },
    columns: { type: Array, default: () => [] },
    isHandleOver: { type: Boolean, default: false },
    layGesture: { type: Function, default: () => () => {} },
    disableEdit:{type:Boolean,default:false} , //禁用 编辑 ,审批模式用
  },
  data() {
    return {
      hovered: '',
    };
  },
  mounted() {
    this._gesture = this.layGesture();
  },
  methods: {
    onAllRowOpen() {
      // console.log('鼠标点击');
      this.$emit('onAllRowOpen');
    },
    handleMouseOver(column, isOver) {
      // console.log('鼠标出去');
      const type = isOver ? 'mouseOver' : 'mouseLeave';
      this.layGesture(type, column);
    },
    dispatchGesture(type, event) {
      // console.log('鼠标进来');
      this.layGesture(type, event);
    },
  },
  render() {
    const contentColumn = this.columns.find(x=>x.name==="name"); // 找到 任务名称 列的属性
    const otherColumns = this.columns.filter(x=>x.name!=="name"); // 除了 任务名称以外的 columns
    return <div
      class='scrollable__3FQe'
      style={`width: ${this.width}px; height: 56px;`}
    >
      <div class='head__NLQw' style={`width: ${this.width}px; height: 56px;`}>
        <div class='row__29JV' style='height: 56px;'>
          {/* 第一列 永远是 任务名称 */}
          {
            <div
              class={` cell__3xqP resizable__3OIa ${contentColumn._isHandleOver ? 'resize-active__1t-e' : 'resize-default__2DLj'}`}
              style={`width: ${contentColumn.width}px;`}
            >
              {this.showAddBtn&&  <div class='config__3_dc'>
                <div class='head-row-index__24Rh'>
                  <i class='next-icon next-icon-gear next-medium'>
                    {!this.disableEdit&&
                    <a-tooltip title='创建新任务'>
                      <a-icon onClick={e => {
                        this.$emit('createTask');
                      }}
                      type='plus' style={{ fontSize: '22px', color: '#08c' }} />
                    </a-tooltip>
                    }
                  </i>
                </div>
              </div> }
              {/*<div class='config__3_dc'>*/}
              {/*  <div class='head-row-index__24Rh'>*/}
              {/*    <i class='next-icon next-icon-gear next-medium'>*/}
              {/*      {!this.disableEdit&&*/}
              {/*      <a-tooltip title='保存更改'>*/}
              {/*        <a-icon onClick={e=>{*/}
              {/*          this.$emit('saveBtnPress');}}*/}
              {/*        type='save' style={{ fontSize: '22px',color:'#08c'}}/>*/}
              {/*      </a-tooltip>*/}
              {/*      }*/}

              {/*    </i>*/}
              {/*  </div>*/}
              {/*</div>*/}
              <div class='head-cell__cL1U'>
                <span class='ellipsis__315v sortable___z3_'>{contentColumn.title}</span>
              </div>
              <div class='suffix__1j4D' onClick={this.onAllRowOpen} aria-haspopup='true' aria-expanded='false'>
                <i
                  class={` next-icon next-medium ${this.collapsed ? 'next-icon-chevrons-expand-vertical' : 'next-icon-chevrons-collapsed-vertical'} `}>
                  <a-icon type='vertical-align-middle' />
                </i>
              </div>
              <div
                class='handle__cGEN right'
                data-role='handle'
                onMousedown={(event) => this.dispatchGesture('mouseDown', event)}
                onMouseover={() => this.handleMouseOver(contentColumn, true)}
                onMouseout={() => this.handleMouseOver(contentColumn, false)}
              />
            </div>
          }
          {
            otherColumns.map(column =>
              <div
                class={` cell__3xqP resizable__3OIa  ${column._isHandleOver ? 'resize-active__1t-e' : 'resize-default__2DLj'}`}
                style={`width: ${column.width}px;`}>
                <div class='head-cell__cL1U'>
                  <span class='ellipsis__315v sortable___z3_'>{column.title}</span>
                </div>
                <div
                  class='handle__cGEN right'
                  data-role='handle'
                  onMousedown={(event) => this.dispatchGesture('mouseDown', event)}
                  onMouseover={() => this.handleMouseOver(column, true)}
                  onMouseout={() => this.handleMouseOver(column, false)}
                />
              </div>,
            )
          }
        </div>
      </div>
    </div>;
  },
};
