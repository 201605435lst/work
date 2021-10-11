import debounce from 'lodash/debounce';
import dayjs from 'dayjs'; // 导入日期js
import moment from 'moment';
import { v4 as uuid } from 'uuid'; // 生成 uuid v4
// const uuidv4 = require("uuid/v4"); // 导入uuid生成插件
import weekOfYear from 'dayjs/plugin/weekOfYear';
import quarterOfYear from 'dayjs/plugin/quarterOfYear';
import isBetween from 'dayjs/plugin/isBetween';
import advancedFormat from 'dayjs/plugin/advancedFormat';
import isLeapYear from 'dayjs/plugin/isLeapYear';
import weekday from 'dayjs/plugin/weekday';

import { dfsCloneNode, flattenDeep } from './src/util/array';
import { addResizeListener } from './src/util/resize-event';
// import weekday from 'dayjs/plugin/weekday';
import TaskBar from './src/components/task-bar';
import InvalidTaskBar from './src/components/invalid-task-bar';
import TaskBarThumb from './src/components/task-bar-thumb';
import TimeIndicator from './src/components/time-indicator';
import TimeAxisScaleSelect from './src/components/time-axis-scale-select';

import Hammer from 'hammerjs';
import TableHeader from './src/components/table-header';
import TableBody from './src/components/table-body';
import SvgSymbol from './src/components/svg-symbol';
import DividerSplit from './src/components/divider-split';

// import '../assets/css/icons.css';
import './src/assets/icons.less';
// import './src/assets/css/cds.css';
import './src/assets/cds.less';
import './src/assets/gantt.less';
import './style/index.less';
import GanttModal from './src/components/GanttModal';
import { GanttItemState } from '../../_utils/enum';
import { interval } from 'rxjs';
import { take } from 'rxjs/operators';
import { flatArr } from '../../_utils/utils';


dayjs.extend(weekday);
dayjs.extend(weekOfYear);
dayjs.extend(quarterOfYear);
dayjs.extend(advancedFormat);
dayjs.extend(isBetween);
dayjs.extend(isLeapYear);

window.dayjs = dayjs;
// console.log(TimeIndicator, '>>>>>>>');
/*
 **************定义字段类型****************
 // 列字段名称
 interface Column {
   minWidth: number,
   prop: string,
   sortable: boolean,
   width: width
 }
 type ColumnList = Column[]
*/

const aTick = ("function" === typeof requestAnimationFrame) ? requestAnimationFrame :
  e => setTimeout(() => e(Date.now()), 1e3 / 60);

// const pxUnitAmp = (60 * 60 * 24 / 30) * 1000;
const topTap = 4;
const headerH = 56; // 这个是 表头的高度
const rowHeight = 28;



// const startDate = dayjs().format('YYYY-MM-DD'); // TODO 先弄成今天,后面在根据 任务条条 来缩放 视图
// const startDate = '2021-07-23'; // TODO 先弄成今天,后面在根据 任务条条 来缩放 视图

// 视图日视图、周视图、月视图、季视图、年视图
const viewTypeList = [
  {
    key: 'day',
    label: '日',
    value: 2880,
  },
  {
    key: 'week',
    label: '周',
    value: 3600,
  },
  {
    key: 'month',
    label: '月',
    value: 14400,
  },
  {
    key: 'quarter',
    label: '季',
    value: 86400,
  },
  {
    key: 'halfYear',
    label: '年',
    value: 115200,
  },
];

const minViewRate = .38;

// const minWidth = 500;
// const minViewWidth = 200;

/** 时间定位相关逻辑 */
const locationModule = {
  locaTimeTranslate(translateX) {
    this.translateX = translateX;
  },

};

/** 排版相关逻辑 */
const layout = {
  data() {
    return {
      layIsHandleOver: false,
      layGesture: () => { }, //这个是 列与列之间 的竖线 当 鼠标挪到竖线的时候 拖拽 ,即可 更改 列的 宽度(挂载mounted 的时候 有 赋值  )
    };
  },
  methods: {
    layUpdate() {
      this.tableWidth = this.iColumns.reduce((width, item) => width + item.width, 0);
      this.viewWidth = this.gantW - this.tableWidth;

      // 表盘宽度不能小于总宽度38%
      if (this.viewWidth < minViewRate * this.gantW) {
        this.viewWidth = minViewRate * this.gantW;
        this.tableWidth = this.gantW - this.viewWidth;
      }

      // 表盘宽度不能小于 200
      if (this.viewWidth < 200) {
        this.viewWidth = 200;
        this.tableWidth = this.gantW - this.viewWidth;
      }

      this.$forceUpdate();
    },
    // 显示右边的视图
    showView() {
      if (this.tableWidth > 0) {
        this.tableWidth = 0;
      } else {
        this.tableWidth = this.iColumns.reduce((width, item) => width + item.width, 0);
      }
      this.viewWidth = this.gantW - this.tableWidth;
    },
    // 显示左边的表格
    showTable() {
      if (this.viewWidth > 20) {
        this.viewWidth = 20;
      } else {
        this.viewWidth = this.gantW - this.iColumns.reduce((width, item) => width + item.width, 0);
      }
      this.tableWidth = this.gantW - this.viewWidth;
    },
  },
  mounted() {
    addResizeListener(this.$refs.gantAppView, () => {
      //todo 这里 每当 销毁的时候 报 Cannot read property 'clientWidth' of undefined 目前还不知道怎么解决
      const width = this.$refs.gantAppView ? this.$refs.gantAppView.clientWidth : this.width;
      // const height = this.$refs.gantAppView.clientHeight;
      const height = this.$refs.gantAppView ? this.$refs.gantAppView.clientHeight : this.height;

      this.gantW = width;
      this.gantH = height;
      this.viewHeight = height - headerH;

      this.tableWidth = this.iColumns.reduce((width, item) => width + item.width, 0);
      this.viewWidth = this.gantW - this.tableWidth;
      // 表盘宽度不能小于总宽度38%
      if (this.viewWidth < minViewRate * width) {
        this.viewWidth = minViewRate * width;
        this.tableWidth = this.gantW - this.viewWidth;
      }

      // 表盘宽度不能小于 200
      if (this.viewWidth < 200) {
        this.viewWidth = 200;
        this.tableWidth = this.gantW - this.viewWidth;
      }
    });
    // 这个是 列与列之间 的竖线 当 鼠标挪到竖线的时候 拖拽 ,即可 更改 列的 宽度
    const layGesture = () => {
      const gantBody = this.$refs.ganttBody;
      const gantBodyH = new Hammer(gantBody);

      let column = null;
      let startWidth = 0;
      let isPress = false;

      // 更新操作
      const update = (event) => {
        if (!column) return;

        let width = startWidth + event.deltaX;
        if (width < column.minWidth) {
          width = column.minWidth;
        }

        column.width = width;
        this.layUpdate();
      };

      // 手指按下
      const mouseOver = (item) => {
        column = item;
        column._isHandleOver = true;
        this.layIsHandleOver = true;
        this.$refs.tableHeader.$forceUpdate();
        this.$refs.tableBody.$forceUpdate();
      };

      // 手指抬起
      const mouseLeave = (column) => {
        if (isPress) return;

        column._isHandleOver = false;
        this.layIsHandleOver = false;
        column = null;
        this.$refs.tableHeader.$forceUpdate();
        this.$refs.tableBody.$forceUpdate();
      };

      // 开始滑动
      const mouseDown = () => {
        isPress = true;
      };

      // 手指移动结束
      const panStart = (event) => {
        if (!column) return;
        if (!isPress) return;

        column._isHandleOver = true;
        this.layIsHandleOver = true;
        startWidth = column.width;
        update(event);
      };
      // 手指移动
      const panMove = (event) => {
        if (!column) return;
        if (!isPress) return;

        aTick(() => {
          update(event);
        });
      };

      const panEnd = (event) => {
        if (!column) return;
        if (!isPress) return;

        update(event);
        isPress = false;
        column._isHandleOver = false;
        this.layIsHandleOver = false;
        column = null;

        this.$refs.tableHeader.$forceUpdate();
        this.$refs.tableBody.$forceUpdate();
      };

      const map = {
        mouseOver,
        mouseLeave,
        mouseDown,
        panStart,
        panMove,
        panEnd,
      };

      gantBodyH.on('panstart', panStart);
      gantBodyH.on('panmove', panMove);
      gantBodyH.on('panend', panEnd);

      // 函数闭包
      return (type, param) => {
        map[type](param);
      };
    };

    this.layGesture = layGesture();
  },

};
const taskEvent = {
  onRowAdd() {
  },
  onRowAddChild() {
  },
  onColumnSort() {
  },
  onTaskCreate() {
  },

  onRowOpen(task, collapsed) {
    // console.log('点击折叠小三角,最终冒泡到父级');
    task.collapsed = collapsed;
    this.barList = this.getBarList();
  },
  onAllRowOpen() {
    this.collapsed = !this.collapsed;
    this.barList.forEach((item) => {
      item.task.collapsed = this.collapsed;
    });

    this.barList = this.getBarList();
    this.$forceUpdate();
  },
  onColumnToggle() {
  },

};

export default {
  name: 'ScGantt',
  components: {
    SvgSymbol,
    DividerSplit,
    TaskBar,
    TableHeader,
    TableBody,
    InvalidTaskBar,
    TimeIndicator,
    TaskBarThumb,
    TimeAxisScaleSelect,
  },
  mixins: [layout],
  inheritAttrs: false,
  props: {
    width: { type: Number, default: 1320 },
    height: { type: Number, default: 200 }, // todo 这个好像不管用,还没有解决
    data: { type: Array, default: () => [] },
    columns: { type: Array, default: () => [] },
    topLvTreeId: { type: String, default: null }, //顶级树id ,一开始的 最上层 有一个(考虑详情表绑定的情况)
    disableEdit: { type: Boolean, default: false }, //禁用 编辑 ,审批模式用
    iModifyList: { type: Array, default: () => [] }, // 修改列表
    showSelection: { type: Boolean, default: false }, // 是否显示选择框
    selectedIds: { type: Array, default: () => [] }, // 选中的ids
    playSpeed: { type: Number, default: 300 }, // 播放速度(毫秒)
    dateArr: { type: Array, default: () => [] }, // 日期数组 (播放的时候用)
  },
  data() {
    const viewTypeObj = viewTypeList[0]; // 日期类型 obj   {key: 'day', label: '日', value: 2880,},{key: 'week', label: '周', value: 3600,},{key: 'month', label: '月', value: 14400,},{key: 'quarter', label: '季', value: 86400,},{key: 'halfYear', label: '年', value: 115200,},
    const translateX = dayjs(this.startDate).valueOf() / (viewTypeObj.value * 1000);
    const gantW = this.width;
    const gantH = this.height;
    const collapsed = this.data.every(bar => bar.collapsed); // 检测 数据是不是都是 折叠的

    return {
      gantW,
      gantH,
      viewWidth: 704, // 右边视图宽度
      viewHeight: this.height,
      tableWidth: 616, // 左标表格宽度
      viewTypeList,
      cellUnit: 30,
      wheelDis: 0,
      translateX,
      viewTypeObj,
      play$: undefined, // 播放流

      selectionIndicatorTop: 0,
      showSelectionIndicator: false,

      activedBarX: 0, // 激活条条的 x坐标
      // 拖拽阴影相关参数
      gestureKeyPress: false,
      shadowGestSide: 'right',
      shadowGestBarLeft: 0,
      shadowGestBarRight: 0,
      showDragToolShadow: false,
      dragToolShadowX: 0,
      dragToolShadowW: 0,
      isShadowGesturePress: false,
      guestureGrantBodyMove: false, // 是否使用 鼠标 让 body 移动
      dataList: dfsCloneNode(this.data),
      modifyList: [], // 修改的数据列表 , 用户 点击 保存 按钮 后 统一把这些 list 提到后端一次性修改
      // 数据部分
      barList: [],
      collapsed,
      // 默认 的 columns
      defaultColumnsOptions: [
        {
          title: '任务名称',
          name: 'name',
          width: 260,
          minWidth: 210,
          visible: true,
          keepVisible: true,
          sortable: true,
          customRender: (text, index, barInfo) => <span>{text}</span>,
        },
        {
          title: '开始时间',
          name: 'startDate',
          width: 110,
          minWidth: 70,
          visible: true,
          keepVisible: true,
          sortable: true,
          customRender: (text, index, barInfo) => {
            let task = barInfo.task;
            return this.disableEdit ? <span>{task.startDate}</span> : <a-date-picker
              allowClear={false}
              size='small'
              // 大于endDate 的时间不能选
              disabledDate={current => current && current > moment(task.endDate).subtract(1, 'days') || (task.parent && current < moment(task._parent.startDate))}
              value={task.startDate ? moment(task.startDate) : null}
              onChange={value => {
                task.startDate = dayjs(value).format('YYYY-MM-DD');
                this.$emit('change', { ...task, ganttItemState: GanttItemState.Edit });
              }} />;
          },
        },
        {
          title: '结束时间',
          name: 'endDate',
          width: 110,
          minWidth: 70,
          visible: true,
          keepVisible: true,
          sortable: true,
          customRender: (text, index, barInfo) => {
            let task = barInfo.task;
            return this.disableEdit ? <span>{dayjs(task.endDate).format('YYYY-MM-DD')}</span> : <a-date-picker
              allowClear={false}
              size='small'
              // 小于startDate 的时间不能选
              disabledDate={current => current && current < moment(task.startDate).subtract(-1, 'days')}
              value={task && task.endDate ? moment(task.endDate).format('YYYY-MM-DD') : null}
              onChange={value => {
                task.endDate = dayjs(value).format('YYYY-MM-DD');
                this.$emit('change', { ...task, ganttItemState: GanttItemState.Edit });
              }}
            />;
          },
        },
        {
          title: '工期',
          name: 'duration',
          width: 70,
          minWidth: 40,
          visible: true,
          keepVisible: true,
          sortable: true,
          // customRender:(text, index, barInfo) => <span class='text'>{barInfo.task._cacheData ? '' : text}</span>,
          // customRender:(text, index, barInfo) => <a-input value={text}
          // onChange={event => {barInfo.task.percent=event.target.value;}}/>,
          customRender: (text, index, barInfo) => {
            let task = barInfo.task;
            return this.disableEdit ?
              <span>{task.duration}</span>
              : <a-input-number
                size='small'
                min={1}
                max={100000}
                value={task.duration}
                precision={0.1}
                parser={value => value.replace(/[^0-9]/g, '')}
                onChange={event => {
                  task.duration = Math.round(event);
                  // console.log('duration',task.duration );
                  task.endDate = dayjs(task.startDate).add(task.duration - 1, 'day').format("YYYY-MM-DD");
                  this.$emit('dateChange', task);// 冒泡给调用的人
                  //todo 更改工期的时候 然后 保存 冒泡会把 masterPlanId 清除导致 所以任务树都没啦 …… 后面 修复下……
                  this.$emit('change', { ...task, ganttItemState: GanttItemState.Edit });

                }} />;
          }
          ,
        },
      ],
    };
  },
  computed: {
    // 内容区滚动高度
    gantBodyH() {
      return this.gantH - headerH;
    },
    startDate() {
      return this.data.length === 0 ? dayjs().format('YYYY-MM-01') : this.data[0].startDate;
    },
    showAddBtn() {
      return this.data.length === 0;
    },
    //内容区滚动区域域高度
    gantBodyScrollH() {
      let height = this.barList.length * rowHeight + topTap;
      if (height < this.gantBodyH) {
        height = this.gantBodyH;
      }

      return height + 2 * rowHeight;
    },
    treeDepth() {
      let flatList = flatArr(this.data);
      let depthArr = flatList.map(x => x.depth);
      return depthArr.length === 0 ? 0 : Math.max(...depthArr);
    },
    // svg 右边视图的高度
    svgViewH() {
      return this.gantBodyScrollH;
    },
    /**
     * 时间起始偏移量
     */
    translateAmp() {
      const translateX = this.translateX;
      const timeamp = this.pxUnitAmp * translateX;
      return timeamp;
    },
    pxUnitAmp() {
      return this.viewTypeObj.value * 1000;
    },
    iColumns() {
      let columns = this.columns;
      if (columns.every(x => !!x.name && !!x.title) === false) return console.error('传进来的columns 列表 item 部分 name 属性 或 title 属性 不存在,请检查!');
      // 遍历 this.columns ,属性重叠的 覆盖,属性不存在的补上
      let defaultColumns = this.defaultColumnsOptions;
      let other = [];
      defaultColumns.forEach(defaultItem => {
        columns.forEach(item => {
          if (item.name === defaultItem.name) {
            let defaultItemProps = Object.keys(defaultItem);
            let itemProps = Object.keys(item);
            defaultItemProps.forEach(x => {
              // 对象 包含 属性 覆盖
              if (itemProps.includes(x)) {
                defaultItem[x] = item[x];
              }
            });
          }
        });
      });
      // 找 传进来的 columns 里面 (defaultColumns 没有的) ,然后 加进来
      this.columns.filter(x => defaultColumns.map(x => x.name).includes(x.name) === false).forEach(item => {
        if (item.width === undefined) item.width = 100; // 宽度
        if (item.minWidth === undefined) item.minWidth = 70; // 最小宽度
        if (item.visible === undefined) item.visible = true; // 可见
        if (item.keepVisible === undefined) item.keepVisible = false; // 缩放时保持 可见
        if (item.sortable === undefined) item.sortable = true; // 可排序
        if (item.customRender === undefined) item.customRender = (text, index, row) => <span>{text}</span>; // 自定义渲染,默认给个span
        other.push(item);
      });
      defaultColumns.push(...other);
      defaultColumns.forEach(item => {
        item._isHandleOver = false;
      });
      // console.log('替换后的columns ',defaultColumns);
      // console.log('other',other);
      defaultColumns.forEach(x => {
        if (x.name === 'name') {
          x.width = 120 + (this.treeDepth + 1) * 40;
        }
      });
      return defaultColumns;
    },
  },

  watch: {
    data: {
      handler(val, oldVal) {
        this.dataList = dfsCloneNode(val); //this.dataList 和 data 长差不多 ,但是给dataList 的 item 加个 未编辑的状态
        this.modifyList = []; //数据源变的时候(肯定是父组件重新请求 数据了)这个时候把 修改列表清空下
      },
      deep: true,
    },
    startDate: {
      handler(val, oldVal) {
        this.translateX = dayjs(val).valueOf() / (this.viewTypeObj.value * 1000);
      },
      immediate: true,
    },

    dataList: { //深度监听，可监听到对象、数组的变化
      handler(val) {
        // console.log('dataList变化', val);
        this.barList = this.getBarList(); // this.barList 是 压扁 后 加 各种属性的 列表
        // console.log("BARLIst",this.barList);
      },
      deep: true, //深度监听
    },
    viewTypeObj() { // 普通的watch监听
      this.translateX = dayjs(this.startDate).valueOf() / (this.viewTypeObj.value * 1000);
      this.barList = this.getBarList();
    },

    iModifyList: {
      handler(val) {
        this.modifyList = val;
      },
      deep: true,
    },
    // iColumns 变化的时候 tableWidth 自适应变化
    iColumns: {
      handler(val) {
        this.tableWidth = val.reduce((width, item) => width + item.width, 0);
      },
      deep: true,
    },
  },

  created() {
    this.deOnMouseMove = debounce(this.onMouseMove, 5); // 这个是用来 对 鼠标移动 事件进行 节流操作的(鼠标移动太频繁)
  },
  mounted() {
    this.barList = this.getBarList();

    this.initGrantBodyGesture();
    const chartView = this.$refs.chartView;
    this.chartHammer = new Hammer(chartView);
    this.chartHammer.options.domEvents = true;
  },
  destroyed() { // 销毁取消订阅
    if (this.play$) {
      this.play$.unsubscribe();
    }
  },
  methods: {
    /**
     * 光标在图表区域滑动 选中行进行移动对应行数据
     */
    onMouseMove(event) {
      if (!this.isPointerPress) {
        this.showSelectionBar(event);
      }
    },
    // 根据 深度 获取 第一列 宽度
    getContentWidth() {
      let depthArr = this.barList.map(x => x._depth);
      if (depthArr.length !== 0) {
        return 80 * Math.max(...depthArr);
      }
      return 80;
    },
    getBarList() {
      let dataTransfer1 = this.dataTransfer(this.dataList);
      // console.log("data 转换", dataTransfer1);
      return dataTransfer1;
    },
    dataTransfer(dataList) {
      const pxUnitAmp = this.pxUnitAmp;
      const minStamp = 11 * pxUnitAmp;
      const height = 8;
      const baseTop = 14;
      const topStep = 28;

      // TODO 后期需优化 增加上周下周等内容
      const dateTextFormat = (startX) => {
        return dayjs(startX * pxUnitAmp).format('YYYY-MM-DD');
      };
      const _dateFormat = (date) => {
        if (!date) return '待设置';
        return dayjs(date).format('YYYY年MM月DD日');
      };

      // 获取鼠标位置所在bar大小及位置
      const startXRectBar = (startX) => {
        let date = dayjs(startX * pxUnitAmp);
        const dayRect = () => {
          const stAmp = date.startOf('day');
          const endAmp = date.endOf('day');
          const left = stAmp / pxUnitAmp;
          const width = (endAmp - stAmp) / pxUnitAmp;

          return {
            left,
            width,
          };
        };
        const weekRect = () => {
          // week 注意周日为每周第一天 ????????
          if (date.$W === 0) {
            date = date.add(-1, 'week');
          }

          const left = date.weekday(1).startOf('day').valueOf() / pxUnitAmp;
          const width = (7 * 24 * 60 * 60 * 1000 - 1000) / pxUnitAmp;

          return {
            left,
            width,
          };
        };
        const monthRect = () => {
          const stAmp = date.startOf('month').valueOf();
          const endAmp = date.endOf('month').valueOf();
          const left = stAmp / pxUnitAmp;
          const width = (endAmp - stAmp) / pxUnitAmp;

          return {
            left,
            width,
          };
        };

        const map = {
          day: dayRect,
          week: weekRect,
          month: weekRect,
          quarter: monthRect,
          halfYear: monthRect,
        };

        return map[this.viewTypeObj.key]();
      };

      // 设置阴影位置
      const setShadowShow = (left, width, isShow) => {
        this.showDragToolShadow = isShow;
        this.shadowGestBarLeft = left;
        this.shadowGestBarRight = left + width;
        this.dragToolShadowX = left;
        this.dragToolShadowW = width;
      };

      // 设置任务
      const setInvalidTaskBar = (barInfo, left, width) => {
        barInfo.translateX = left;
        barInfo.width = width;
        barInfo.invalidDateRange = false;

        this.showDragToolShadow = true;
        this.shadowGestBarLeft = left + width;
        this.shadowGestBarRight = 0;

        this.dragToolShadowX = left;
        this.dragToolShadowW = width;

        barInfo.stepGesture = 'moving';
      };
      /**
       * 根据选中行高度 显示对应条状工具条
       */
      const getHovered = (top, selectionIndicatorTop) => {
        let baseTop = top - (top % rowHeight);
        let isShow = (selectionIndicatorTop >= baseTop && selectionIndicatorTop <= baseTop + rowHeight);
        return isShow;
      };

      // 进行展开扁平
      dataList = flattenDeep(dataList);
      // endDate-startDate 计算下 工期
      dataList.forEach(x => {
        let duration = dayjs(x.endDate).diff(dayjs(x.startDate), 'day') + 1; // 时间相减
        x.duration = isNaN(duration) ? 0 : duration;
      });

      let map1 = dataList.filter(x => x.editState !== GanttItemState.Delete).map((item, index) => {
        let startAmp = dayjs(item.startDate || 0).valueOf();
        let endAmp = dayjs(item.endDate || 0).add(1, 'days').valueOf();

        // 开始结束日期相同默认一天
        if (Math.abs(endAmp - startAmp) < minStamp) {
          startAmp = dayjs(item.startDate || 0).valueOf();
          endAmp = dayjs(item.endDate || 0).add(minStamp, 'millisecond').valueOf();
        }
        let width = (endAmp - startAmp) / pxUnitAmp;
        let translateX = startAmp / pxUnitAmp;
        let translateY = baseTop + index * topStep;
        let _parent = item._parent;

        return {
          task: item,
          translateX,
          translateY,
          width,
          height,
          label: item.name,
          stepGesture: 'end', // start(开始）、moving(移动)、end(结束)
          invalidDateRange: !item.endDate || !item.startDate,  // 是否为有效时间区间
          dateTextFormat,  //TODO 日期格式化函数 后期根据当前时间格式化为上周下周,
          startXRectBar,   // 鼠标位置 获取创建bar位置及大小
          setShadowShow,
          setInvalidTaskBar,
          getHovered,
          _collapsed: item.collapsed,  // 是否折叠
          _depth: item._depth,  // 表示子节点深度
          _index: item._index,  // 任务下标位置
          _flattenIndex: index, // 数据列表下标
          _parent, // 原任务数据
          _childrenCount: !item.children ? 0 : item.children.length, // 子任务
          _dateFormat,
        };
      });
      // console.log("压平后的数据",map1);
      return map1;
    },
    /**
     * 是否显示任务条状图
     */
    getshowTaskBar(width, translateX, timeTranslateX) {
      const rightSide = this.translateX + this.viewWidth;
      const right = translateX;

      return translateX + width < timeTranslateX || right - rightSide > 0;
    },
    /**
     * 手势按下的逻辑
     */
    shadowGesturePress(event, type, barInfo) {
      if (this.disableEdit) return;
      this.gestureKeyPress = true;

      // 移动空隙参数
      const space = 5;
      const { translateX, width } = barInfo;

      const getMoveStep = (isLeft, isShrink, barInfo) => {
        const { translateX, width } = barInfo;
        const startX = isLeft ? translateX : translateX + width;
        const startDate = dayjs(startX * this.pxUnitAmp);

        const getDayStep = () => {
          let endDate = startDate.endOf('day');

          // 左侧收缩
          if (isShrink && isLeft) {
            endDate = startDate.add(1, 'day').startOf('day');
          }

          // 右侧扩展
          if (!isShrink && isLeft) {
            endDate = startDate.startOf('day');
          }

          // 右侧收缩
          if (isShrink && !isLeft) {
            endDate = startDate.add(-1, 'day').endOf('day');
          }

          let step = 24 * 60 * 60 * 1000 / this.pxUnitAmp;
          let diff = Math.abs((endDate.valueOf() - startDate.valueOf()) / this.pxUnitAmp);
          if (diff > space) {
            step = diff;
          }

          return step;
        };

        const getWeekStep = () => {
          let endDate = startDate.weekday(1).hour(0).minute(0).second(0);
          if ((isLeft && isShrink) || (!isLeft && !isShrink)) {
            endDate = endDate.weekday(7).hour(23).minute(59).second(59);
          }

          let step = 7 * 24 * 60 * 60 * 1000 / this.pxUnitAmp;
          let diff = Math.abs(endDate.valueOf() / this.pxUnitAmp - startX);
          if (diff > space) {
            step = diff;
          }

          return step;
        };


        const getMonthStep = () => {
          let month = -1;
          let endDate2 = startDate.startOf('month');
          // 向右侧移动
          if ((isLeft && isShrink) || (!isLeft && !isShrink)) {
            month = 1;
            endDate2 = startDate.endOf('month');
          }

          const endDate = startDate.add(month, 'month');
          let step = Math.abs(endDate.valueOf() / this.pxUnitAmp - startX);

          const diff = Math.abs(endDate2.valueOf() / this.pxUnitAmp - startX);
          if (diff > 5) {
            step = diff;
          }

          return step;
        };

        const map = {
          day() {
            return getDayStep();
          },
          week() {
            return getWeekStep();
          },
          month() {
            return getWeekStep();
          },
          quarter() {
            return getMonthStep();
          },
          halfYear() {
            return getMonthStep();
          },

        };

        const step = map[this.viewTypeObj.key]();
        return step;
      };

      let barLeft = (type === 'left') ? translateX : translateX + width;
      this.dragToolShadowX = translateX;
      this.dragToolShadowW = width;
      this.shadowGestBarLeft = barLeft;
      this.isPointerPress = true;

      const sideType = type;
      const isLeft = sideType === 'left';
      // const step = getMoveStep(isLeft);
      const clientRect = event.target.getBoundingClientRect();
      const startX = isLeft ? clientRect.right : clientRect.left;
      const basePointerX = isLeft ? startX + width : startX - width;

      const setBarShadowPosition = (moveEv) => {
        const pointerX = moveEv.center.x;

        this.showDragToolShadow = true;
        const isShrink = getDragSideShrink(moveEv);
        const isExpand = getDragSideExpand(moveEv);

        const getShadowMoveDis = (baseX, pointerX) => {
          const moveDis = pointerX - baseX;
          return moveDis;
        };

        const moveDis = getShadowMoveDis(startX, pointerX);

        // 每次step可能不一样， 动态计算 如：每月可能30或31天
        const step = getMoveStep(isLeft, isShrink, barInfo);

        if (isShrink) {
          moveShrinkStep(moveDis, step, pointerX);
        }

        if (isExpand) {
          moveExpandStep(moveDis, step, pointerX);
        }
      };

      const getDragSideShrink = (moveEv) => {
        let direction = 0;
        if (moveEv.direction === 2) {
          direction = -1;
        } else if (moveEv.direction === 4) {
          direction = 1;
        }

        return (sideType === 'right' && direction < 0) || (sideType === 'left' && direction > 0);
      };
      const getDragSideExpand = (mouseEv) => {
        let direction = 0;
        if (mouseEv.direction === 2) {
          direction = -1;
        } else if (mouseEv.direction === 4) {
          direction = 1;
        }

        return (sideType === 'right' && direction > 0) || (sideType === 'left' && direction < 0);
      };

      // 跟随鼠标移动搜索阴影
      const moveShrinkStep = (moveDis, step, pointerX) => {
        const isLeft = sideType === 'left';

        let translateX = this.dragToolShadowX;
        let width = this.dragToolShadowW;
        let barLeft = this.shadowGestBarLeft;

        if (isLeft) {
          translateX += step;
          width -= step;
          barLeft = translateX;
        } else {
          width -= step;
          barLeft = translateX + width;
        }

        const pointerDis = Math.abs(pointerX - basePointerX);
        if (pointerDis > width) return;
        if (width <= step) return;

        aTick(() => {
          this.dragToolShadowW = width;
          this.dragToolShadowX = translateX;
          this.shadowGestBarLeft = barLeft;

          barInfo.translateX = translateX;
          barInfo.width = width;
          barInfo.stepGesture = 'moving';
        });
      };
      // 跟随鼠标拖动扩大阴影
      const moveExpandStep = (moveDis, step, pointerX) => {

        let translateX = this.dragToolShadowX;
        let width = this.dragToolShadowW;
        let barLeft = this.shadowGestBarLeft;

        const pointerDis = Math.abs(pointerX - basePointerX);
        if (pointerDis < space || pointerDis < width) return;

        // 测试代码
        if (isLeft) {
          translateX -= step;
          width += step;
          barLeft = translateX;
        } else {
          width += step;
          barLeft = translateX + width;
        }

        aTick(() => {
          this.dragToolShadowW = width;
          this.dragToolShadowX = translateX;
          this.shadowGestBarLeft = barLeft;

          barInfo.translateX = translateX;
          barInfo.width = width;
          barInfo.stepGesture = 'moving';
        });
      };

      const panStart = () => {
        barInfo.stepGesture = 'start';
      };

      const panMove = (event) => {
        setBarShadowPosition(event);
      };

      const panEnd = (event) => {
        setBarShadowPosition(event);
        this.isPointerPress = false;
        this.showDragToolShadow = false;

        this.chartHammer.off('panstart', panStart);
        this.chartHammer.off('panmove', panMove);
        this.chartHammer.off('panend', panEnd);
        this.shadowGestBarLeft = 0;
        this.shadowGestBarRight = 0;
        barInfo.stepGesture = 'end';

        this.updateTaskDate(barInfo);
      };

      this.chartHammer.on('panstart', panStart);
      this.chartHammer.on('panmove', panMove);
      this.chartHammer.on('panend', panEnd);
    },
    // 手指抬起做一些清理操作
    shadowGesturePressup() {
      this.gestureKeyPress = false;
    },
    //  手指按住任务条线触发事件
    shadowGestureBarPress(event, barInfo) {
      if (this.disableEdit) return;
      this.gestureKeyPress = true;

      const step = this.cellUnit;
      let { translateX, width } = barInfo;
      let barLeft = translateX;
      let barRight = translateX + width;

      let startX = 0;
      let pointerX = 0;

      const layoutShadow = (width, translateX, barLeft, barRight) => {
        aTick(() => {
          this.dragToolShadowW = width;
          this.dragToolShadowX = translateX;
          this.shadowGestBarLeft = barLeft;
          this.shadowGestBarRight = barRight;

          barInfo.translateX = translateX;
        });
      };

      const setBarShadowPosition = (event) => {
        pointerX = event.center.x;
        const pointerDis = pointerX - startX;
        const direction = pointerDis > 0 ? 1 : -1;
        const moveX = step * direction;

        if (Math.abs(pointerDis) >= step) {
          translateX = translateX + moveX;
          barLeft = translateX;
          barRight = translateX + width;
          layoutShadow(width, translateX, barLeft, barRight);
          startX = startX + moveX;
          barInfo.stepGesture = 'moving';
        }
      };

      layoutShadow(width, translateX, barLeft, barRight);

      const panStart = (event) => {
        startX = event.center.x;
        barInfo.stepGesture = 'start';
      };

      const panMove = (event) => {
        this.showDragToolShadow = true;
        setBarShadowPosition(event);
      };

      const panEnd = (event) => {
        setBarShadowPosition(event);
        this.showDragToolShadow = false;
        this.chartHammer.off('panstart', panStart);
        this.chartHammer.off('panmove', panMove);
        this.chartHammer.off('panend', panEnd);
        this.shadowGestBarLeft = 0;
        this.shadowGestBarRight = 0;
        barInfo.stepGesture = 'end';

        this.updateTaskDate(barInfo);
      };

      this.chartHammer.on('panstart', panStart);
      this.chartHammer.on('panmove', panMove);
      this.chartHammer.on('panend', panEnd);
    },
    shadowGestureBarPressup(event, barInfo) {
      if (this.disableEdit) return;
      this.gestureKeyPress = false;
      barInfo.stepGesture = 'end';
    },
    // 更新时间
    updateTaskDate(barInfo) {
      const translateX = barInfo.translateX;
      const width = barInfo.width;
      const task = barInfo.task; // 注意 ,这里引用 了 外部 的 barInfo.task 对象,所以 const task 修改以后,外部的东西也会修改
      let startDate = dayjs(translateX * this.pxUnitAmp).format('YYYY-MM-DD');
      let endDate = dayjs((translateX + width) * this.pxUnitAmp).format('YYYY-MM-DD');
      // 开始时间变 ,结束时间也变
      if (barInfo.task.startDate !== startDate && barInfo.task.endDate !== endDate) {
        task.startDate = startDate;
        task.endDate = endDate;
        task.duration = barInfo.task.duration;
        console.log('开始时间变 ,结束时间也变');
      }
      // 开始时间变,结束时间不变
      if (barInfo.task.startDate !== startDate && barInfo.task.endDate === endDate) {
        task.startDate = startDate;
        task.endDate = endDate;
        // 时间相减
        task.duration = dayjs(task.endDate).diff(dayjs(task.startDate), 'day') + 1;
        console.log('开始时间变,结束时间不变');
        console.log('task.duration', task.duration);
      }
      // 开始时间不变,结束时间变
      if (barInfo.task.startDate === startDate && barInfo.task.endDate !== endDate) {
        task.startDate = startDate;
        // 时间右移
        if (dayjs(endDate).diff(dayjs(barInfo.task.endDate), 'day') > 0) {
          task.endDate = dayjs((translateX + width) * this.pxUnitAmp).format('YYYY-MM-DD');
          // 时间相减
          task.duration = dayjs(task.endDate).diff(dayjs(task.startDate), 'day') + 1;
          console.log('开始时间不变,结束时间右移');
          console.log('task.duration', task.duration);
        } else {
          // 时间左移
          task.endDate = endDate;
          // 时间相减
          task.duration = dayjs(task.endDate).diff(dayjs(task.startDate), 'day') + 1;
          console.log('开始时间不变,结束时间左移');
          console.log('task.duration', task.duration);

        }
      }



      this.$emit('change', { ...task, ganttItemState: GanttItemState.Edit });
    },
    //  计算位置
    showSelectionBar(event) {
      const topMargin = 4;
      const rowH = 28;
      const scrollTop = this.$refs.gantMainEl.scrollTop; // 获取滚动的高度
      // 原来的代码 是 event.clientY 是 直接获取 浏览器 0,0 举例鼠标的距离,但是那个组件是全屏的情况下,
      // 现在这个组件不是全屏的了,所以要先获取  ref=father(最外面的div )的坐标,然后拿 e.clientY 减去 father的y轴坐标才行
      // const offsetY = event.clientY - headerH + scrollTop; // 原来vue 的代码
      const offsetY = (event.clientY - this.$refs.father.getBoundingClientRect().top) - headerH + scrollTop; // 改后的代码

      // console.log('father-y', this.$refs.father.getBoundingClientRect().top); // 获取某div 的 y 坐标(相对浏览器)
      // console.log('father-x', this.$refs.father.getBoundingClientRect().left); // 获取某div 的 x 坐标(相对浏览器)

      if (offsetY < topMargin) {
        this.showSelectionIndicator = false;
        return;
      } else {
        let top = Math.floor((offsetY - 4) / rowH) * rowH + 4;
        this.showSelectionIndicator = true;
        this.selectionIndicatorTop = top;
        // console.log(this.selectionIndicatorTop );
      }
    },

    // 去掉默认的滚轮 ,这个滚轮 是 日期 左右 滑动用的
    wheel(event) {
      // event.deltaY 是 y轴 滚了多少  deltaX 应该是 x 轴滚了多少 ,但是这里原版是 deltaX ,导致没有滚动 ,这里改成y
      // console.log('日期滚轮 ',event);
      if (this._wheelTimer) clearTimeout(this._wheelTimer);
      this.guestureGrantBodyMove = true;

      aTick(() => this.translateX += event.deltaY);

      this._wheelTimer = setTimeout(() => {
        this.guestureGrantBodyMove = false;
      }, 100);
    },

    getDurationAmp() {
      const clientWidth = this.viewWidth;
      return this.pxUnitAmp * clientWidth;
    },
    // 跟据 年月日 的类型 返回 右上角 日期 不同的 显示 列表
    getMajorList() {
      const majorFormatMap = {
        'day': 'YYYY年 MM月',
        'week': 'YYYY年 MM月',
        'month': 'YYYY年',
        'quarter': 'YYYY年',
        'halfYear': 'YYYY年',
      };

      const translateAmp = this.translateAmp;
      const endAmp = translateAmp + this.getDurationAmp();
      const key = this.viewTypeObj.key;
      const format = majorFormatMap[key];

      const getNextDate = (start) => {
        if (key === 'day' || key === 'week') {
          return start.add(1, 'month');
        } else {
          return start.add(1, 'year');
        }
      };

      const setStart = (datejs) => {
        if (key === 'day' || key === 'week') {
          return datejs.startOf('month');
        } else {
          return datejs.startOf('year');
        }
      };

      const setEnd = (start) => {
        if (key === 'day' || key === 'week') {
          return start.endOf('month');
        } else {
          return start.endOf('year');
        }
      };

      // 初始化当前时间
      let curDate = dayjs(translateAmp);
      let dateMap = new Map();

      // 对可视区域内的时间进行迭代
      while (curDate.isBetween(translateAmp - 1, endAmp + 1)) {
        let majorKey = curDate.format(format);
        let start = curDate;
        let end = setEnd(start);

        if (dateMap.size !== 0) {
          start = setStart(curDate);
        }

        if (!dateMap.has(majorKey)) {
          dateMap.set(majorKey, {
            label: majorKey,
            startDate: start,
            endDate: end,
          });
        }

        // 获取下次迭代的时间
        start = setStart(curDate);
        curDate = getNextDate(start);
      }

      let majorAmp2Px1 = this.majorAmp2Px([...dateMap.values()]);
      // console.log("日期",majorAmp2Px1);
      return majorAmp2Px1;
    },
    // 修改滑块
    changeSlider(index) {
      if (this.dateArr.length === 0) return;
      if (index === -1) return;
      console.log('index', index);
      console.log('this.dateArr[index]', this.dateArr[index]);
      let dayjs1 = this.dateArr[index].ts;
      this.changeTransLatexAndActivedBarx(dayjs1);
      this.$emit('listenValue', { val: index, text: this.dateArr[index].value });
    },
    // 暂停
    pause() {
      // 取消订阅
      if (this.play$) {
        this.play$.unsubscribe();
      }
    },
    // 播放
    play() {
      if (this.data.length === 0) return;
      this.play$ = interval(this.playSpeed).pipe(
        take(this.dateArr.length),
      ).subscribe(x => {
        if (x === this.dateArr.length - 1) { //最后一个的时候就是播放完成的时候
          this.$emit('playComplete');
        }
        this.$emit('listenValue', { val: x, text: this.dateArr[x].value });
        this.changeTransLatexAndActivedBarx(this.dateArr[x].ts);
      });
    },
    // 修改 视角 x 轴 和 高亮 x 轴
    changeTransLatexAndActivedBarx(dayjs1) {
      // this.translateX = dayjs1.valueOf() / (this.viewTypeObj.value * 1000)-addWidth;
      this.translateX = dayjs1.valueOf() / (this.viewTypeObj.value * 1000) - this.getMinorList()[0].width;
      // console.log('this.translateX',this.translateX);
      // this.activedBarX = Math.round((dayjs1.valueOf() / this.pxUnitAmp) + this.getMinorList()[0].width );
      this.activedBarX = Math.round((dayjs1.valueOf() / this.pxUnitAmp));
    },

    // 根据 年月日 来 遍历 日期  (1日,2日....31日)
    getMinorList() {
      const minorFormatMap = {
        'day': 'YYYY-MM-D',
        'week': 'YYYY-w周',  // format W 不知道为什么不支持周，文档却说支持,
        'month': 'YYYY-MM月',
        'quarter': 'YYYY-第Q季',
        'halfYear': 'YYYY-',
      };
      const fstHalfYear = [0, 1, 2, 3, 4, 5];

      const startAmp = this.translateAmp;
      const endAmp = startAmp + this.getDurationAmp();
      const format = minorFormatMap[this.viewTypeObj.key];

      const getNextDate = (start) => {
        const map = {
          day() {
            return start.add(1, 'day');
          },
          week() {
            return start.add(1, 'week');
          },
          month() {
            return start.add(1, 'month');
          },
          quarter() {
            return start.add(1, 'quarter');
          },
          halfYear() {
            return start.add(6, 'month');
          },

        };

        return (map[this.viewTypeObj.key])();
      };
      const setStart = (datejs) => {
        const map = {
          day() {
            return datejs.startOf('day');
          },
          week() {
            return datejs.weekday(1).hour(0).minute(0).second(0);
          },
          month() {
            return datejs.startOf('month');
          },
          quarter() {
            return datejs.startOf('quarter');
          },
          halfYear() {
            if (fstHalfYear.includes(datejs.month())) {
              return datejs.month(0).startOf('month');
            } else {
              return datejs.month(6).startOf('month');
            }
          },

        };

        return (map[this.viewTypeObj.key])();
      };
      const setEnd = (start) => {
        const map = {
          day() {
            return start.endOf('day');
          },
          week() {
            return start.weekday(7).hour(23).minute(59).second(59);
          },
          month() {
            return start.endOf('month');
          },
          quarter() {
            return start.endOf('quarter');
          },
          halfYear() {
            if (fstHalfYear.includes(start.month())) {
              return start.month(5).endOf('month');
            } else {
              return start.month(11).endOf('month');
            }
          },

        };

        return (map[this.viewTypeObj.key])();
      };
      const getMinorKey = (datejs) => {
        if (this.viewTypeObj.key === 'halfYear') {
          return datejs.format(format) + (fstHalfYear.includes(datejs.month()) ? '上半年' : '下半年');
        }

        return datejs.format(format);
      };

      // 初始化当前时间
      let curDate = dayjs(startAmp);
      let dateMap = new Map();

      while (curDate.isBetween(startAmp - 1, endAmp + 1)) {
        let minorKey = getMinorKey(curDate);

        let start = setStart(curDate);
        let end = setEnd(start);
        if (!dateMap.has(minorKey)) {
          dateMap.set(minorKey, {
            label: minorKey.split('-').pop(),
            startDate: start,
            endDate: end,
            key: end,
          });
        }

        curDate = getNextDate(start);
      }
      // dateMap 是 这个 viewWidth 宽度下的 一段时间(比如 7.01~7.18)
      //  px 是 这个 viewWidth 宽度下的 一段时间 图形属性 (条条宽,条条偏移)
      let px = this.minorAmp2Px([...dateMap.values()]);
      return px;
    },
    majorAmp2Px(ampList) {
      const pxUnitAmp = this.pxUnitAmp;
      // for(let i = 0)
      const list = ampList.map(item => {
        const startDate = item.startDate;
        const endDate = item.endDate;
        const label = item.label;

        let left = (startDate.valueOf() / pxUnitAmp);
        let width = (endDate.valueOf() - startDate.valueOf()) / pxUnitAmp;

        return {
          label,
          left,
          width,
        };
      });

      return list;
    },
    minorAmp2Px(ampList) {
      const pxUnitAmp = this.pxUnitAmp;
      const curDate = dayjs().startOf('day');
      let highlightW = 4;
      let highlightX = curDate.valueOf() / pxUnitAmp;
      if (this.viewTypeObj.key === 'day') {
        highlightW = 8;
        highlightX = highlightX + 11;
      }

      const list = ampList.map(item => {
        let startDate = item.startDate.hour(0).minute(0).second(0); // 时间分隔成段 的 起始
        let endDate = item.endDate.hour(23).minute(59).second(59);// 时间分隔成段 的 结束

        let label = item.label;
        let left = Math.ceil(startDate.valueOf() / pxUnitAmp);
        let width = Math.ceil((endDate.valueOf() - startDate.valueOf()) / pxUnitAmp);
        let isWeek = false;
        if (this.viewTypeObj.key === 'day') {
          isWeek = [0, 6].includes(startDate.$W);
        }

        let isHighlight = false;
        let curSt = curDate.valueOf();
        let startSt = startDate.valueOf();
        let endSt = endDate.valueOf();
        if (curSt >= startSt && curSt <= endSt) {
          isHighlight = true;
        }

        return {
          label,
          left,// 条条x 距离
          width, //  条条宽度
          isWeek, // 是否是周末 (周末条条用别的颜色标记下)
          isHighlight, // 是否是今天(高亮)
          highlightW, //条条 高亮 宽度
          highlightX, // 条条x 距离 (从1970年开始算)
        };
      });

      return list;
    },
    ...locationModule,
    ...taskEvent,
    initGrantBodyGesture() {
      const timeAxisRender = this.$refs.timeAxisRender;
      const chartView = this.$refs.chartView;

      const timeAxisRenderH = new Hammer(timeAxisRender);
      const chartViewH = new Hammer(chartView);
      let translateX = this.translateX;

      const panStart = () => {
        if (this.gestureKeyPress) return;

        this.guestureGrantBodyMove = true;
        translateX = this.translateX;
      };

      const panMove = (event) => {
        if (this.gestureKeyPress) return;
        aTick(() => this.translateX = translateX - event.deltaX);
      };

      const panEnd = (event) => {
        if (this.gestureKeyPress) return;
        this.guestureGrantBodyMove = false;
        this.translateX = translateX - event.deltaX;
      };

      timeAxisRenderH.on('panstart', panStart);
      timeAxisRenderH.on('panmove', panMove);
      timeAxisRenderH.on('panend', panEnd);
      chartViewH.on('panstart', panStart);
      chartViewH.on('panmove', panMove);
      chartViewH.on('panend', panEnd);
    },
  },


  // beforeDestroy() {
  //   // this.ha.destroy()
  //   // this.$af.destroy();
  // },
  render() {
    return <div class='sc-gantt ' style='width: 100%;'>
      <div ref='father' style='width:100%;height:100%;min-height:60px;display:flex;flex-flow:column;overflow:hidden;'>
        <div class='gantt-app-view' ref='gantAppView' >
          <SvgSymbol />
          <GanttModal
            ref='ganttModal'
            selectTree={this.dataList}
            onAdd={value => {
              value = { id: uuid(), ...value, ganttItemState: GanttItemState.Add };
              if (!!value.parentId) { // children 添加数组
                // 递归根据 parentId 找到 item,给他的children push value
                dfsCloneNode(this.dataList, (item, flatItem) => {
                  if (item.id === value.parentId) {
                    item.children.push(value);
                  }
                });
              } else { // 根部 添加 数组
                this.dataList.push(value);
              }
              this.modifyList.push(value);
              this.$emit('change', value);
            }}
            onEdit={(editId, value) => {
              // console.log(value);
              value = { id: editId, ...value, ganttItemState: GanttItemState.Edit };
              const recEdit = (id, list) => { // 定义一个 action(没有返回值的委托)
                list.forEach((x, index, arr) => {
                  if (x.id === id) {
                    arr[index] = { ...value, children: x.children };
                    this.$emit('change', { ...value });
                    return;
                  }
                  if (x.children !== undefined && x.children.length > 0) {
                    recEdit(id, x.children);
                  }
                });

              };
              recEdit(editId, this.dataList);
              this.barList = this.getBarList(); // 得加这行,不然 修改 了数据 页面 不显示……
            }}
          />

          <div class='container__1PWP' style={` box-sizing: content-box; height:${this.svgViewH}px; `}>
            <div
              ref='ganttBody'
              id='gantt-body'
              class='body__3LBc gantt__3Xim'
              width={this.gantW}
              height={this.gantH}>
              <div
                class={`scroll-indicator__3aij ${this.guestureGrantBodyMove ? 'scrolling__1B1k' : ''}`}
                style={`left: ${this.tableWidth - 8}px; width: 8px;`}
              >
              </div>
              <header>
                <TableHeader
                  ref='tableHeader'
                  showAddBtn={this.showAddBtn}
                  collapsed={this.collapsed}
                  width={this.tableWidth}
                  disableEdit={this.disableEdit}
                  columns={this.iColumns}
                  layGesture={this.layGesture}
                  onSaveBtnPress={() => {
                    let flattenIds = this.barList.map(x => x.task.id); //把树压平后的 ids
                    this.modifyList.forEach(x => {
                      x.preTaskIds.forEach(p => {
                        if (!flattenIds.includes(p)) { // 前置任务列表里面有 barList 不存在 的 id
                          let notExistIndex = x.preTaskIds.findIndex(y => y === p); //找到不存在的index
                          if (notExistIndex !== -1) { //找到的话 就删除
                            x.preTaskIds.splice(notExistIndex, 1);
                          }
                        }
                      });
                    });
                    // return console.log("保存冒泡前",this.modifyList);

                    this.$emit('save', this.modifyList);
                  }}
                  onOnAllRowOpen={this.onAllRowOpen}
                  onCreateTask={() => {
                    this.$refs.ganttModal.add(this.topLvTreeId);// header 创建任务冒泡到这里来打开模态框
                  }}
                />
                {/* 这是 右边视图 上方 的 日期 div */}
                <div
                  ref='timeAxisRender'
                  onWheel={event => {
                    event.preventDefault();
                    this.wheel(event);
                  }}
                  class='time-axis__3meF'
                  style={`left: ${this.tableWidth}px; width: ${this.viewWidth}px;`}
                >
                  <div class='render-chunk__28qu' style={`transform: translateX(-${this.translateX}px;`}>
                    {this.getMajorList().map((item, key) => {
                      return <div key={key} class='major__2rd6' style={`width: ${item.width}px; left: ${item.left}px;`}>
                        <div class='label__RLd1'>{item.label}</div>
                      </div>;
                    })}

                    {this.getMinorList().map((dayItem) => {
                      return <div
                        class={{
                          minor__11Xd: true,
                          weekends__1EmY: dayItem.isWeek,
                          highlight__3NdW: dayItem.isHighlight && this.viewTypeObj.key === 'day',
                        }}
                        key={dayItem.key}
                        style={`width: ${dayItem.width}px; left:${dayItem.left}px;`}
                      >
                        <div class='label__RLd1'>{dayItem.label}</div>
                        {dayItem.isHighlight && this.viewTypeObj.key === 'day' &&
                          <div class='highlight-bg__1mPp' />
                        }
                      </div>;
                    })}
                  </div>
                </div>
              </header>
              <main ref='gantMainEl'>
                {this.showSelectionIndicator &&
                  <div
                    class='selection-indicator__3rr6'
                    style={`height: 28px; top: ${this.selectionIndicatorTop}px;`}
                  />
                }
                <TableBody
                  showSelection={this.showSelection}
                  selectedIds={this.selectedIds}
                  ref='tableBody'
                  showSelectionIndicator={this.showSelectionIndicator}
                  selectionIndicatorTop={this.selectionIndicatorTop}
                  table-width={this.tableWidth}
                  table-height={this.svgViewH}
                  disableEdit={this.disableEdit}
                  dataList={this.barList}
                  columns={this.iColumns}
                  layGesture={this.layGesture}
                  onMousemove={this.deOnMouseMove}
                  onOnRowOpen={this.onRowOpen}
                />
                <div
                  ref='chartView'
                  onWheel={event => {
                    event.preventDefault();
                    this.wheel(event);
                  }}
                  onMouseup={this.shadowGesturePressup}
                  onMousemove={this.deOnMouseMove}
                  onMouseleave={() => this.showSelectionIndicator = false}
                  class='chart__3nGi'
                  style={`left:${this.tableWidth}px;height:${this.svgViewH}px;width:${this.viewWidth}px;`}
                >
                  <svg width={this.viewWidth} height={this.svgViewH} viewBox={`${this.translateX} 0 ${this.viewWidth} ${this.svgViewH}`} class='chart-svg-renderer__7ors' xmlns='http://www.w3.org/2000/svg' version='1.1'>
                    <rect class='actived_bar' fill='#87d7ff' opacity='1' strokeWidth='0' x={this.viewTypeObj.key === 'day' ? this.activedBarX + 11 : this.activedBarX} y='0' width={this.viewTypeObj.key === 'day' ? 4 : 2} height={this.svgViewH} />
                    {this.getMinorList().map((item) => {
                      // 高亮条条
                      let highLightBar = item.isHighlight && <rect fill='#FFA941' opacity='0.3' strokeWidth='0' x={item.highlightX} y='0' width={item.highlightW} height={this.svgViewH} />;
                      return item.isWeek ?
                        <g key={item.key} stroke='#f0f0f0'>
                          <path d={`M${item.left}.5,0 L${item.left},${this.svgViewH}`} />
                          <rect fill='#F7F7F7' opacity='0.5' strokeWidth='0' x={item.left} y='0' width={item.width} height={this.svgViewH} />
                          {highLightBar}
                        </g>
                        :
                        <g key={item.key} stroke='#f0f0f0'>
                          <path d={`M${item.left}.5,0 L${item.left},${this.svgViewH}`} />
                          {highLightBar}
                        </g>;
                    })}

                    {this.showDragToolShadow &&
                      <g
                        fill='rgba(204, 236, 255, 0.3)'
                        stroke='#87D2FF'
                      >
                        {this.shadowGestBarLeft &&
                          <path d={`M${this.shadowGestBarLeft},0 L${this.shadowGestBarLeft},${this.svgViewH}`} />
                        }
                        <rect x={this.dragToolShadowX} y='0' width={this.dragToolShadowW} height={this.svgViewH} strokeWidth='0' />
                        {this.shadowGestBarRight &&
                          <path d={`M${this.shadowGestBarRight},0 L${this.shadowGestBarRight},${this.svgViewH}`} />
                        }
                      </g>
                    }

                  </svg>
                  <div class='render-chunk__22Ez' style={`height: ${this.svgViewH}px; transform:translateX(-${this.translateX}px;`}>
                    {this.barList.map((bar, index) => {
                      // console.log(bar.translateY);
                      return this.getshowTaskBar(bar.width, bar.translateX, this.translateX) && !bar.invalidDateRange ?
                        <TaskBarThumb
                          key={index}
                          label={bar.label}
                          viewWidth={this.viewWidth}
                          viewTranslateX={this.translateX}
                          translateX={bar.translateX}
                          translateY={bar.translateY}
                          width={bar.width}
                          onTimeTranslateLocation={this.locaTimeTranslate}
                        />
                        :
                        <TaskBar
                          key={index}
                          label={bar.label}
                          barInfo={bar}
                          width={bar.width}
                          translateX={bar.translateX}
                          translateY={bar.translateY}
                          stepGesture={bar.stepGesture}
                          invalidDateRange={bar.invalidDateRange}
                          dateTextFormat={bar.dateTextFormat}
                          showDragBar={bar.getHovered(bar.translateY, this.selectionIndicatorTop)}
                          // 块块 左右长按 拉长
                          onGesturePress={(event, type) => this.shadowGesturePress(event, type, bar)}
                          onGestureBarPress={(event) => this.shadowGestureBarPress(event, bar)}
                          onGestureBarPressup={(event) => this.shadowGestureBarPressup(event, bar)}
                        />;
                    })}

                    {this.barList.filter(item => item.invalidDateRange).map((bar, key) => {
                      return <InvalidTaskBar
                        key={this.barList.length + key}
                        translateX={this.translateX}
                        top={bar.translateY}
                        left={bar.translateX}
                        startXRectBar={bar.startXRectBar}
                        dateTextFormat={bar.dateTextFormat}
                        setShadowShow={bar.setShadowShow}
                        setInvalidTaskBar={(left, width) => bar.setInvalidTaskBar(bar, left, width)}
                        onGesturePress={(event, type) => this.shadowGesturePress(event, type, bar)}
                      />;
                    })}
                  </div>
                </div>
              </main>
              <DividerSplit
                onShowView={this.showView}
                onShowTable={this.showTable}
                layIsHandleOver={this.layIsHandleOver}
                left={this.tableWidth}
                right={this.viewWidth}
              />
              <TimeIndicator
                guestureGrantBodyMove={this.guestureGrantBodyMove}
                viewTranslateX={this.translateX}
                tableWidth={this.tableWidth}
                viewWidth={this.viewWidth}
                pxUnitAmp={this.pxUnitAmp}
                onTimeTranslateLocation={this.locaTimeTranslate}
              />
              <TimeAxisScaleSelect
                value={this.viewTypeObj}
                guestureGrantBodyMove={this.guestureGrantBodyMove}
                viewWidth={this.viewWidth}
                viewTypeList={this.viewTypeList}
                defaultValue={this.viewTypeObj}
                onChange={selectItem => {
                  this.$emit('dateTypeChange', selectItem); // 向上冒泡 日期类型 (年月日周)
                  return this.viewTypeObj = selectItem;
                }}
              />
            </div>
          </div>
        </div>
      </div>
    </div>;
  },
};
