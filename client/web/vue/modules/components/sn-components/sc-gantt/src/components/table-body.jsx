import TaskMenu from './task-menu';
import Input from '../components/Input';
import RowHandleWrap from '../components/RowHandleWrap';
import { tips as tipsConfig } from '../../../../_utils/config';
import { arrItemReplace } from '../util/array';
import { GanttItemState } from '../../../../_utils/enum';

const indent = 38;
const getCacheData = () => {
  return {
    executor: null,
    name: '',
    content: '',
    startDate: null,
    endDate: null,
    collapsed: false,
    children: [],
    _cacheData: true, // 标识是否属于缓存数据
    _canEdit: false,  // 控制输入框
  };
};




export default {
  components: {
    Input,
    TaskMenu,
    RowHandleWrap,
    // aDropdown,
    // TaskMenu,
    // elDropdown,
    // elDropdownMenu,
    // elDropdownItem,
  },
  inheritAttrs: false,
  props: {
    showSelectionIndicator: { type: Boolean, default: false },
    selectionIndicatorTop: { type: Number, default: 0 },
    rowHeight: { type: Number, default: 28 },
    tableWidth: { type: Number, default: 616 },
    tableHeight: { type: Number, default: 285 },
    dataList: { type: Array, default: () => [] },
    columns: { type: Array, default: () => [] },
    layGesture: { type: Function, default: () => () => { } },
    disableEdit: { type: Boolean, default: false }, //禁用 编辑 ,审批模式用
    showSelection: { type: Boolean, default: true }, // 是否显示选择框
    selectedIds: { type: Array, default: () => [] }, // 选中的ids
  },
  data() {
    return {
      indent,
      cacheIdx: -1,
      cacheRow: null,
      handleIndex: -1, // 拖动条触发索引位置
      iSelectedIds: [],
      targetHandleIndex: -1, // 拖动条移动后所处索引位置
      handleTop: 0,   // 拖动条上下位置
      handleLeft: 61, // 拖动条左右位置
      handleNextIndex: -1, // 拖动条拖动后索引
      handleNextIndent: 0, // 拖动条拖动后缩进值
      selectedRows: [],// 选中的行
    };
  },
  computed: {
    rowDataList() {
      return this.dataList;
    },

  },
  watch: {
    cacheRow(val) {
      if (val) {
        const dataList = this.$parent.dataList;
        let parent = this.cacheRow._parent;
        let children = [];

        if (!parent) {
          children = dataList;
        } else {
          !parent.children && (parent.children = []);
          children = parent.children;
        }

        children.splice(this.cacheIdx, 0, this.cacheRow);
        this.$parent.barList = this.$parent.getBarList();
      }
    },
    selectedIds: {
      handler: function (val, oldVal) {
        this.iSelectedIds = val;
        this.selectedRows = this.selectedRows.filter(a => this.iSelectedIds.includes(a.id));
        this.dataList.forEach(element => {
          element.task._isSelected = this.iSelectedIds.includes(element.task.id);
        });
      },
      immediate: false,
    },

  },
  created() {
  },
  beforeDestroy() {
    // document.body.removeEventListener('mouseup', this.gesturePressup);
    // this.bodyHammer && this.bodyHammer.destroy();
  },
  mounted() {
    // document.body.addEventListener('mouseup', this.gesturePressup);
  },
  methods: {
    getContentReadOnly(barInfo) {
      const task = barInfo.task;
      const readonly = !(task._cacheData || task._canEdit);
      return readonly;
    },
    clearCacheRow() {
      this.cacheIdx = -1;
      this.cacheRow = null;
    },
    /**
     * 插入子任务
     */
    inserChildTask(barInfo) {
      const task = barInfo.task;
      task.collapsed = false;
      this.addCacheTask(task);
    },
    /**
     * 添加数据缓存
     * index 添加数据的位置
     */
    addCacheTask(parent = null) {
      if (this.cacheRow) return;

      this.cacheRow = getCacheData();
      this.cacheRow._parent = parent;

      if (!parent) {
        this.cacheIdx = this.$parent.dataList.length;
      } else {
        this.cacheIdx = (parent.children || []).length;
      }
    },
    /**
     * 完成数据的添加操作
     */
    completeAdd(barInfo, value) {
      const task = this.cacheRow;
      const parent = barInfo._parent;

      // 完成添加操作
      barInfo.label = value;
      task.content = value;
      task._canEdit = false;
      task._cacheData = false;

      this.cacheIdx = -1;
      this.cacheRow = null;

      // 如果值不为空
      if (value) {
        this.$parent.$emit('onTaskCreate', (parent ? { ...parent } : null), { ...task });
        return;
      }

      if (!parent) {
        this.$parent.dataList.splice(this.cacheIdx, 1);
      } else {
        parent.children.splice(this.cacheIdx, 1);
      }


      this.$parent.barList = this.$parent.getBarList();
    },
    /**
     * 完成编辑操作
     */
    completeEdit(barInfo, value) {
      const task = barInfo.task;
      const label = barInfo.label;
      barInfo.label = value;
      task.content = value;
      task._canEdit = false;
      task._cacheData = false;

      if (label !== value) {
        this.$parent.$emit('change', { ...task, ganttItemState: GanttItemState.Edit });
      }
    },
    /**
     * 完成任务添加编辑操作
     * cacheRow 临时添加缓存数据
     * cacheRow 需要添加的位置
     * vlaue 添加输入框编辑的内容
     */
    completeInput(barInfo, value) {
      if (this.cacheRow) {
        this.completeAdd(barInfo, value);
      } else {
        this.completeEdit(barInfo, value);
      }
    },
    /**
     * 对内容触发编辑操作 // 如果是禁止编辑的话就 直接return
     */
    editTaskContent(barInfo) {
      if (this.disableEdit) {
        return;
      }
      const task = barInfo.task;
      task._canEdit = true;
      this.$forceUpdate();
    },
    /**
     * 触发删除操作 (这个函数原来就是空的…… )
     */
    deleteTaskAction() { },
    // 完成对数据的删除
    deleteTask(barInfo) {
      const index = barInfo._index;
      if (barInfo._parent) {
        barInfo._parent.children.splice(index, 1);
      } else {
        this.$parent.dataList.splice(index, 1);
      }

      this.$parent.barList = this.$parent.getBarList();
      this.$parent.$emit('change', { ...barInfo.task, ganttItemState: GanttItemState.Delete });

    },
    /**
     * 插入同级别新任务
     */
    insertTask(barInfo) {
      if (!barInfo._parent) return;
      this.addCacheTask(barInfo._parent);
      this.cacheRow._depth = barInfo._depth;
    },
    /**
     * 向右侧移动任务
     * barInfo 任务信息
     */
    moveRightTask(barInfo) {
      // 必须有上一个兄弟节点
      let index = barInfo._index;
      if (index <= 0) return;

      // 获取坐标与父级
      const parent = barInfo._parent;

      // 获取所有兄弟任务
      let children;
      if (parent) {
        children = parent.children || [];
      } else {
        children = this.$parent.dataList;
      }

      // 获取上一个兄弟任务
      let preSibTask = children[index - 1];
      children.splice(index, 1);

      if (!preSibTask.children)
        preSibTask.children = [];

      // 删除原先位置 挪动到新的位子上去
      preSibTask.children.push(barInfo.task);
      this.$parent.barList = this.$parent.getBarList();
      // barInfo.task 是 已经修改的 任务,preSibTask 是他的新父亲
      let parentId = !!preSibTask ? preSibTask.id : null;
      barInfo.task.parentId = parentId; // 这个得加,不然 后续编辑 的话 他的pid 还是原来 的
      console.log('barInfo._parent.content,老父亲', barInfo._parent.content);
      console.log('新父亲', preSibTask.content);
      this.$parent.$emit('move', barInfo.task);
    },
    /**
     * 向左侧移动任务
     * barInfo 任务信息
     */
    moveLeftTask(barInfo) {
      // 最左侧元素不进行位移
      let depth = barInfo._depth;
      if (depth <= 0) return;

      // 获取父级及爷爷级别的节点
      const parent = barInfo._parent;
      if (!parent) return;
      const ancestor = parent._parent;

      // 获取兄弟节点和父级兄弟节点
      let sibList = parent.children;
      let parentChildren;
      if (ancestor) {
        parentChildren = ancestor.children;
      } else {
        parentChildren = this.$parent.dataList;
      }

      // 开始移动移动元素
      let index = barInfo._index;
      sibList.splice(index, 1);
      parentChildren.push(barInfo.task);
      this.$parent.barList = this.$parent.getBarList();
      let parentId = !!ancestor ? ancestor.id : null;
      barInfo.task.parentId = parentId; // 这个得加,不然 后续编辑 的话 他的pid 还是原来 的
      console.log('barInfo._parent.content', barInfo._parent.content);
      this.$parent.$emit('move', barInfo.task);
    },
    getIndent(depth) {
      return this.indent * depth;
    },
    getShowTrigger(barInfo) {
      return barInfo._childrenCount > 0;
    },
    rowTrigger(barInfo) {
      // console.log('点击折叠小三角');
      this.$emit('onRowOpen', barInfo.task, !barInfo._collapsed);
    },
    handleMouseOver(column, isOver) {
      const type = isOver ? 'mouseOver' : 'mouseLeave';
      this.layGesture(type, column);
    },
    dispatchGesture(type, event) {
      this.layGesture(type, event);
    },

    // 小圆点冒泡到这里 执行 任务item 拖拽操作  _'+"-%^/^^//\////\/\\\\
    gesturePress(event, barInfo) {
      const baseLeft = 56;   // 基础偏移值
      const leftMargin = 61; // 中心点基础偏移值
      const topMargin = 4;   // 垂直方向基础偏移值
      const rowH = 28;       // 行高
      const indentStep = 38; // 行缩进值

      // 手势状态
      this._gestureStatus = 'press';

      // 根据数据下标获取位置
      const getHandleTop = (index) => {
        if (this.dataList.length === 0) return 0;

        const barInfo = this.dataList[index];
        if (!barInfo) {
          index = this.dataList.length - 1;
        }

        return index * rowH + topMargin + rowH;
      };

      // 根据缩进值获取Left值
      const getHandleLeftByIndent = (indent) => {
        return indent * indentStep + leftMargin;
      };

      // 根据鼠标的offsetX获取行缩进值
      const getHandleIndentByOffsetX = (index, offsetX) => {
        if (this.dataList.length === 0) return 0;

        const barInfo = this.dataList[index];
        if (!barInfo) {
          index = this.dataList.length - 1;
        }

        offsetX = (offsetX - baseLeft < 0) ? 0 : offsetX - baseLeft;

        let depth = barInfo._depth;
        let indent = Math.floor(offsetX / indentStep);

        if (indent > depth + 1) {
          indent = depth + 1;
        }

        return indent;
      };

      // 根据行索引获取Left值
      const getHandleLeftByIndex = (index) => {
        const barInfo = this.dataList[index];
        if (this.dataList.length === 0 || !barInfo) return leftMargin;

        return getHandleLeftByIndent(barInfo._depth);
      };

      const getNextHandleIndexByIndent = (index, indent) => {
        const barInfo = this.dataList[index];
        if (this.dataList.length === 0 || !barInfo) return 0;

        let nextIdx = index;
        while (!barInfo._collapsed && nextIdx < this.dataList.length) {
          if (nextIdx === index) {
            nextIdx++;
            continue;
          }

          let barInfo = this.dataList[nextIdx];
          if (barInfo._depth > indent) {
            nextIdx++;
          } else {
            break;
          }
        }

        if (nextIdx !== index) {
          index = nextIdx - 1;
        }

        return index;
      };

      // 根据鼠标位置获取数据下标
      const getHandleBarIndex = (top) => {
        const topMargin = 4;
        const rowH = 28;

        top = top - topMargin < 0 ? 0 : top - topMargin;
        let index = Math.floor(top / rowH);
        if (index >= this.dataList.length) {
          index = this.dataList.length - 1;
        }

        return index;
      };

      // TODO 鼠标移动 适当做节流优化
      const panMove = (event) => {
        // 根据鼠标Y坐标获取所处行索引
        const index = getHandleBarIndex(event.offsetY);
        this.targetHandleIndex = (this.dataList[index] || {})._flattenIndex;

        // 根据行索引及X坐标获取缩进值
        const nextIndent = getHandleIndentByOffsetX(index, event.offsetX);
        this.handleNextIndent = nextIndent;

        // 根据缩进值获取左侧缩进位置
        this.handleLeft = getHandleLeftByIndent(nextIndent);

        // 根据所处行及缩进值获取插入的位置
        const nextIndex = getNextHandleIndexByIndent(index, nextIndent);
        this.handleNextIndex = nextIndex;
        this.handleTop = getHandleTop(nextIndex);
      };

      // 行的折叠状态
      this._handleOpen = !barInfo._collapsed;

      // 如果当前行未折叠 进行折叠操作
      if (this._handleOpen && this.getShowTrigger(barInfo)) this.rowTrigger(barInfo);

      // 记录数据下标
      this.handleIndex = barInfo._flattenIndex;
      this.targetHandleIndex = barInfo._flattenIndex;
      this.handleNextIndex = barInfo._flattenIndex;
      this.handleNextIndent = barInfo._depth;

      // 记录插入数据的位置
      this.handleTop = getHandleTop(this.handleIndex);
      this.handleLeft = getHandleLeftByIndex(this.handleIndex);
      this._panMove = panMove;

    },
    gestureMove(event) {
      // 手势为press状态不进行操作
      if (this._gestureStatus !== 'press') return;

      this._panMove && this._panMove(event);
    },
    gesturePressup() {
      if (this._gestureStatus !== 'press') return;

      // 获取移动的行数数据
      const handleBarInfo = this.rowDataList[this.handleIndex];

      // 恢复展开行的状态
      this._handleOpen && this.getShowTrigger(handleBarInfo) && this.rowTrigger(handleBarInfo);

      // 要拖动数据索引
      const handleIndex = this.handleIndex;
      // 鼠标位置所处的数据索引
      const targetHandleIndex = this.targetHandleIndex;
      // 数据需要移动到位置数据索引
      const handleNextIndex = this.handleNextIndex;
      // 数据缩进的位置
      const handleNextIndent = this.handleNextIndent;

      this.handleIndex = -1;
      delete this._gestureStatus;

      // 获取父级任务
      const getParentTask = (targetHandleIndex, handleNextIndent) => {
        // 视图数据
        const dataList = this.dataList;
        // 鼠标所触发的行数据
        const targetBar = dataList[targetHandleIndex];
        if (!targetBar) return null;

        // 父任务
        const targetTask = targetBar.task;
        // 缩进值
        const depth = targetBar._depth;

        // 同级别缩进值和父子关系
        if (depth === handleNextIndent) {
          return targetTask._parent;
        } else if (depth < handleNextIndent) {
          return targetTask;
        }

        // 向上查找父亲
        let index = targetHandleIndex;
        while (index >= 0 && index < dataList.length) {
          let task = dataList[index].task;

          if (task._depth === handleNextIndent) {
            return task._parent;
          }

          if (task._depth < handleNextIndent) {
            return task;
          }

          index--;
        }

        return null;
      };
      const getTargetTask = (targetHandleIndex) => {
        // 视图数据
        const dataList = this.dataList;
        // 鼠标所触发的行数据
        const targetBar = dataList[targetHandleIndex];
        if (!targetBar) return null;

        return targetBar.task;
      };


      // 获取移动数据
      const getHandleTask = (handleIndex) => {
        // 视图数据
        const dataList = this.dataList;

        // 鼠标所触发的行数据
        const handleBar = dataList[handleIndex];
        if (!handleBar) return null;

        return handleBar.task;
      };

      // 需要移动数据
      const handleTask = getHandleTask(handleIndex);
      // 需要插入的行数据
      const parentTask = getParentTask(targetHandleIndex, handleNextIndent);
      // 鼠标所处位置的行数据
      const targetTask = getTargetTask(targetHandleIndex);

      // 父级原始节点
      const preParent = handleTask._parent;

      // 执行删除数据逻辑
      if (!handleTask) return;
      if (handleTask === targetTask && handleNextIndent >= targetTask._depth) return;

      let children = this.$parent.dataList;
      if (handleTask._parent) {
        children = handleTask._parent.children || [];
      }

      handleTask._parent = parentTask;  // 切换父子关系
      children.splice(handleTask._index, 1);
      let index = 0;

      // 父子关系插入行数据
      if (parentTask === targetTask) {
        parentTask.children.unshift(handleTask);
      } else {
        // 取出子元素
        children = this.$parent.dataList;
        if (parentTask) {
          children = parentTask.children;
        }

        // 根据插入的父子关系 来判断插入位置
        if (handleNextIndent > targetTask._depth) {
          children.unshift(handleTask);
        } else {
          // 根据邻近的兄弟元素 获取插入的位置
          let sibTask = this.dataList[handleNextIndex + 1];
          let splitIdx = sibTask ? sibTask._index : children.length;
          if (!sibTask || sibTask._depth < handleNextIndent) {
            children.push(handleTask);
            index = children.length - 1;
          } else {
            let appendChildren = children.splice(splitIdx);
            children.push(handleTask, ...appendChildren);
            index = splitIdx;
          }
        }
      }

      this.$parent.barList = this.$parent.getBarList();

      // 这个由父级冒泡,冒到调用 gantt-chart-drag 的地方
      // console.log('老父亲',preParent.content, preParent.id);
      // console.log('新父亲',parentTask.content,parentTask.id);
      // console.log('已修改的task',handleTask.content,handleTask.id);
      // console.log('新index',handleTask._index);
      // this.$parent.modifyList.push({...handleTask, parentId: parentTask.id}); //修改列表 添加一条
      // console.log('移动中的taskId', handleTask.id);
      this.$parent.$emit('move', { ...handleTask, parentId: parentTask.id, ganttItemState: GanttItemState.Edit });

    },
    mousemove(event) {
      // console.log('在table-body 上面移动鼠标 ');
      this.$emit('mousemove', event); // 这个冒到 gant 主 jsx 上 , 然后 用来 限流(防抖),暂时不知道干啥用……
      // this.gestureMove(event);
    },
    rowHandleClick(event, barInfo) {
      console.log("小圆点点击已经冒泡到父级了");
      if (this._gestureStatus === 'press') return;
      this.$parent.$emit('onToDetail', barInfo.task); // 这里冒泡不知道是啥意思 ……
    },
  },
  render() {
    let contentColumn = this.columns.find(x => x.name === "name");
    let otherColumns = this.columns.filter(x => x.name !== "name");
    return <div class='scrollable__3FQe'
      onMousemove={this.mousemove}
      onMouseup={this.gesturePressup}
      style={`width: ${this.tableWidth}px; height: ${this.tableHeight}px;`}
    >
      <div
        ref='bodyView'
        class='body__38O5 view-compact__1L78'
        style={`width: ${this.tableWidth}px ; height: ${this.tableHeight}px`}
      >
        {/* 这个可以注释掉 注释掉不影响运行,他是 table-header和 table-body 的交界处的 一小 搓 横杠 */}
        <div class='row__29JV' style='top: 0px; height: 4px;'>
          {this.columns.map((item, key) => {
            return <div
              key={key}
              class={` cell__3xqP resizable__3OIa  ${this.columns[key]._isHandleOver ? 'resize-active__1t-e' : 'resize-default__2DLj'}`}
              style={`width: ${item.width}px;`}>
              <div
                class='handle__cGEN right'
                data-role='handle'
                onMousedown={(event) => {
                  this.dispatchGesture('mouseDown', event);
                }}
                onMouseover={() => {
                  this.handleMouseOver(this.columns[key], true);
                }}
                onMouseout={() => {
                  this.handleMouseOver(this.columns[key], false);
                }}
              />
            </div>;
          })}
        </div>
        {this.rowDataList.map((barInfo, index) => {
          let res = <div
            key={index}
            data-row='robot-guide'
            class={{
              row__29JV: true,
              hovered: barInfo.getHovered(barInfo.translateY, this.selectionIndicatorTop) && this.showSelectionIndicator,

            }}
            style={`top: ${barInfo.translateY - 10}px; height: ${this.rowHeight}px;`}
          >
            {/* 这是第一列的数据 固定 name */}
            {
              <div
                class={`cell__3xqP resizable__3OIa ${contentColumn._isHandleOver ? 'resize-active__1t-e' : 'resize-default__2DLj'}`}
                style={`width: ${contentColumn.width}px;`}
              >
                <div class='row-before__3blm' style='padding-left: 56px;'>
                  {/* 索引 id 1,2,3,4……*/}
                  <div class='row-index__3xNX' style={this.showSelection ? 'width: 48px;' : 'width: 24px;'}>
                    {this.showSelection &&
                      <a-checkbox style="margin-right:5px" checked={barInfo.task._isSelected} onChange={e => {
                        if (e.target.checked) {
                          // 选中
                          this.selectedRows.push(...this.dataList.filter(x => x.task.id == barInfo.task.id).map(x => ({ ...x.task, id: x.task.id, name: x.task.name })));
                        } else {
                          this.selectedRows = this.selectedRows.filter(a => a.id != barInfo.task.id);
                        }
                        // barInfo.task._isSelected= e.target.checked;
                        //this.iSelectedIds.includes(barInfo.task.idid)
                        // let selectIds = this.dataList.filter(x => x.task._isSelected).map(x => ({ ...x.task, id: x.task.id, name: x.task.name }));
                        //this.iSelectedIds = this.dataList.filter(x => x.task._isSelected).map(x => (x.task.id));
                        this.iSelectedIds = this.selectedRows.map(a => a.id);
                        this.$parent.$emit('selectedChange', this.selectedRows); // 跨组件冒泡
                        this.dataList.forEach(element => {
                          element.task._isSelected = this.iSelectedIds.includes(element.task.id);
                        });
                      }} />
                    }
                    {index + 1}
                  </div>
                  <div class='row-indentation__2dHs' style={`background-size: ${this.indent}px; width: ${this.getIndent(barInfo._depth)}px;`} />
                  <div class='row-menu-and-toggler___F4q'>
                    {/*  相当于右键菜单   */}
                    {!this.disableEdit &&
                      <div class='row-menu__QRiG  menu-trigger__2YZ4 '>
                        <a-dropdown trigger={['hover']}>
                          <a class='ant-dropdown-link' onClick={e => e.preventDefault()}>
                            <a-icon type='dash' />
                          </a>
                          <a-menu slot='overlay'>
                            {barInfo._index > 0 ?
                              [<a-menu-item
                                onClick={(e) => {
                                  // 必须有上一个兄弟节点
                                  let index = barInfo._index;
                                  if (index <= 0) return;
                                  this.moveRightTask(barInfo);
                                }}
                              >
                                <a-icon type='double-right' />
                                向右缩进一级</a-menu-item>,
                              <a-menu-divider />,
                              ] : undefined
                            }

                            {barInfo._depth > 1 ?
                              [<a-menu-item
                                onClick={(e) => {
                                  let depth = barInfo._depth;
                                  if (depth <= 0) return;
                                  this.moveLeftTask(barInfo);
                                }}
                              >
                                <a-icon type='double-left' />
                                向左提升一级</a-menu-item>,
                              <a-menu-divider />,
                              ] : undefined
                            }
                            {barInfo._depth !== 0 ? [
                              <a-menu-item
                                onClick={e => {
                                  // 有父级就插入子任务,否则就添加平行的任务
                                  if (!!barInfo.task._parent) {
                                    this.$parent.$refs.ganttModal.addChild(barInfo.task._parent);
                                  } else {
                                    this.$parent.$refs.ganttModal.add();
                                  }
                                }}
                              >
                                <a-icon type='plus' />
                                添加新任务
                              </a-menu-item>,
                              <a-menu-divider />,
                            ] : undefined
                            }

                            <a-menu-item onClick={e => this.$parent.$refs.ganttModal.addChild(barInfo.task)}>
                              <a-icon type='enter' />
                              插入子任务
                            </a-menu-item>
                            <a-menu-divider />
                            <a-menu-item onClick={e => {
                              barInfo.task.disabled = true;
                              this.$parent.$refs.ganttModal.edit(barInfo.task, this.$parent.dataList);
                            }}>
                              <a-icon type="edit" />
                              编辑任务
                            </a-menu-item>
                            <a-menu-divider />
                            <a-menu-item onClick={(e) => {
                              const _this = this;
                              this.$confirm({
                                title: tipsConfig.remove.title,
                                content: h => <div style='color:red;'>{tipsConfig.remove.name}</div>,
                                okType: 'danger',
                                onOk() {
                                  _this.deleteTask(barInfo);
                                },
                              });

                            }}>
                              <a-icon type="delete" />
                              删除
                            </a-menu-item>
                          </a-menu>
                        </a-dropdown>

                      </div>
                    }
                    {this.getShowTrigger(barInfo) && !this.showSelection &&
                      // 折叠小三角形
                      <div onClick={() => this.rowTrigger(barInfo)} class='row-toggler__3rTS'>
                        <div class={`body-row-toggler__wvWq ${barInfo._collapsed ? 'collapsed' : ''}`}>
                          <a-icon type='caret-down' />
                        </div>
                      </div>
                    }

                  </div>
                  {/* 小圆点 */}
                  {!this.disableEdit &&
                    <div class='row-handle__210U'>
                      <RowHandleWrap
                        barInfo={barInfo}
                        onRowHandleClick={this.rowHandleClick}
                        onGesturePress={this.gesturePress}
                        onGesturePressup={(e, barinfo) => {
                          this.gesturePressup();
                        }}
                      >
                        <i />
                      </RowHandleWrap>
                    </div>
                  }
                </div>
                {/* 第一列 的数据  如果属性是只读的话 spcn ,否则是 input 输入框*/}
                <div class='body-cell__OUd5 content'>
                  {this.getContentReadOnly(barInfo) ?
                    <div
                      class='text__aNJc'
                      onClick={() => this.editTaskContent(barInfo)}
                    >
                      <div title={barInfo.label} class='ellipsis hinted'>
                        <span>{barInfo.label}
                          {barInfo.task.isMilestone &&
                            <a-tooltip title='里程碑'>
                              <a-icon type="tags" style={{ fontSize: '12px', color: '#08c', marginLeft: 2 }} />
                            </a-tooltip>
                          }
                        </span>
                      </div>
                    </div>
                    :
                    <Input value={barInfo.label} barInfo={barInfo} />
                  }


                </div>
                {/* 这是一小段竖线,整个大竖线 就是 这一个个小竖线拼接的*/}
                <div
                  class='handle__cGEN right'
                  data-role='handle'
                  onMousedown={(event) => this.dispatchGesture('mouseDown', event)}
                  onMouseover={() => this.handleMouseOver(contentColumn, true)}
                  onMouseout={() => this.handleMouseOver(contentColumn, false)}
                />
              </div>
            }

            {/* 这是除第一列以外的数据 遍历*/}
            {otherColumns.map(x => {
              let taskPropValue = barInfo.task[x.name];
              let span = x.customRender !== undefined // 没有customRender 这个属性就默认用 span 来渲染啦
                ?
                x.customRender(taskPropValue, index, barInfo)
                :
                <span class='text'>{barInfo.task._cacheData ? '' : taskPropValue}</span>;
              return <div
                class={` cell__3xqP resizable__3OIa ${x._isHandleOver ? 'resize-active__1t-e' : 'resize-default__2DLj'}`}
                style={`width: ${x.width}px;`}
              >
                <div class='body-cell__OUd5 date__hqPF hasPermission' style='justify-content: center;'>
                  {span}
                </div>
                {/* 列最右边的竖线*/}
                <div
                  class='handle__cGEN right'
                  data-role='handle'
                  onMousedown={(event) => this.dispatchGesture('mouseDown', event)}
                  onMouseover={() => this.handleMouseOver(x, true)}
                  onMouseout={() => this.handleMouseOver(x, false)}
                />
              </div>;
            },
            )}

          </div>;

          if (this.iSelectedIds.includes(barInfo.task.id) && barInfo.task._isSelected) {
            // barInfo.task._isSelected=false;
          }

          return res;
        })}
        {/* 这是最后一行 用来 添加新任务的 因为我决定用按钮 弹出框来添加新任务,所以这个注释了*/}
        {/*<div class='row__29JV' style={`top: ${(this.rowDataList.length * this.rowHeight + 4)}px; height: 28px;`}>*/}
        {/*  <div class='cell__3xqP resize-default__2DLj resizable__3OIa' style={`width: ${contentColumn.width}px;`}>*/}
        {/*    <div class='row-before__3blm' style='padding-left: 56px;'>*/}
        {/*      <div class='row-index__3xNX' />*/}
        {/*      <div class='row-indentation__2dHs' style='background-size: 38px; width: 0px;' />*/}
        {/*      <div class='row-menu-and-toggler___F4q' />*/}
        {/*    </div>*/}
        {/*    <div class='foot__1A82 gantt-app-outline-foot'>*/}
        {/*      <button*/}
        {/*        class='textbtn-color-blue__361l'*/}
        {/*        type='button'*/}
        {/*        data-role='text-button'*/}
        {/*        onClick={() => this.addCacheTask()}*/}
        {/*      >*/}
        {/*        <i class='next-icon next-icon-plus next-medium'>*/}
        {/*          <svg viewBox='0 0 1024 1024'>*/}
        {/*            /!*<use xlink:href="#at-plus"></use>*!/*/}
        {/*          </svg>*/}
        {/*        </i>*/}
        {/*        <span>添加新任务</span>*/}
        {/*      </button>*/}
        {/*    </div>*/}
        {/*    <div class='handle__cGEN right' data-role='handle' />*/}
        {/*  </div>*/}
        {/*  /!* 添加新任务后面的 空列 *!/*/}
        {/*  {otherColumns.map(x =>*/}
        {/*    <div class='cell__3xqP resize-default__2DLj resizable__3OIa' style={`width: ${x.width}px;`}>*/}
        {/*      <div class='handle__cGEN right' data-role='handle' />*/}
        {/*    </div>,*/}
        {/*  )}*/}
        {/*</div>*/}
        {/* 这是拖拽的时候 显示的 */}
        {this.rowDataList[this.handleIndex] &&
          <div class='mask__3CAH' onMousemove={this.gestureMove} />
        }
        {this.rowDataList[this.handleIndex] &&
          <div class='drag-indicator__1C3_' style={`left: ${this.handleLeft}px; top: ${this.handleTop}px;`} />
        }
      </div>
    </div>;

  },
};
