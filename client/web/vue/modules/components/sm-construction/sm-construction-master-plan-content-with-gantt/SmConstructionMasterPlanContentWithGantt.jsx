import './style';
import { findParentIds, flatArr, recModify, requestIsSuccess } from '../../_utils/utils';
import ApiMasterPlanContent from '../../sm-api/sm-construction/ApiMasterPlanContent';
import { GanttItemState } from '../../_utils/enum';
import ApiMasterPlan from '../../sm-api/sm-construction/ApiMasterPlan';
import SmConstructionMasterPlanContent
  from '../../sm-construction/sm-construction-master-plan-content/SmConstructionMasterPlanContent';
import PlanCommonHeader from '../sm-construction-plan-with-gantt/PlanCommonHeader';
import { recModifyProp } from '../sm-construction-plan-with-gantt/planUtil';
import ScGantt from '../../sn-components/sc-gantt/ScGantt';
import dayjs from 'dayjs'; // 导入日期js
let apiMasterPlanContent = new ApiMasterPlanContent();
let apiMasterPlan = new ApiMasterPlan();


export default {
  name: 'SmConstructionMasterPlanContentWithGantt',
  inheritAttrs: false,
  props: {
    axios: { type: Function, default: null },
    isApproval: { type: Boolean, default: false }, // 是否在审批模式
    isSelectName: { type: Boolean, default: false }, //是否选择名称
    inModal: { type: Boolean, default: false }, // 是否在模态框里面
    showSelection: { type: Boolean, default: false }, // 是否显示选择框
    selectedIds: { type: Array, default: () => [] }, // 选中的ids
    masterPlanId: { type: String, default: undefined }, // 总体计划id

  },
  data() {
    return {
      list: [], // table 数据源
      masterPlan: {}, // 任务计划实体
      resolveList: [], // 处理后的 数据源
      loading: false, // table 是否处于加载状态
      iMasterPlanId: undefined, // 施工计划id
      dateFilter: { // 日期筛选
        year: undefined, //年份
        quarter: undefined, //季度
        month: undefined, //月份
        dayStart: undefined, //周数 开始 天
        dayEnd: undefined, //周数 结束 天
        weekIndex: undefined,// 周数索引(标记用)
      },
      yearList: [], // 年份列表
      selectMasterPlanContentIds: [], // 选择的 施工计划详情 ids (选择框模式的时候用)
      selectedEntity: undefined, // 选择的实体(任务计划详情 content )
      dicTypes: [],
      treeDepth: 1, //树深度
      selectedSubItems: [], //已选择分部分项
      importModelVisible: false,
      selectId: undefined, // 单选的id
      clickTimeChange: null, // 单击事件的延时变量
      rowMultiple: false, // table Row 是否是 多选模式
      modalVisible: false, // masterContent 模态框 显示与隐藏
      columns: [
      ],
    };
  },
  computed: {},
  watch: {
    masterPlanId: {
      handler: function (value, oldValue) {
        this.initAxios();
        this.iMasterPlanId = value;
        this.refresh();
      },
      immediate: true,
    },

  },
  async created() {
    this.initAxios();
    !this.isSelectName && await this.getMasterPlan();
    this.refresh();
    this.getYears();
  },
  methods: {
    initAxios() {
      apiMasterPlanContent = new ApiMasterPlanContent(this.axios);
      apiMasterPlan = new ApiMasterPlan(this.axios);
    },
    async moveTask(task) {
      let res = await apiMasterPlanContent.moveTask(task.id, task.parentId);
      if (requestIsSuccess(res)) {
        await this.refresh();
        task.ganttItemState = GanttItemState.Edit;
        await this.ganttItemChange(task);
      }

    },
    // 父组件调用这个方法,用来重置 gantt图里面的数据
    resetByParent() {
      this.iMasterPlanId = undefined;
      this.resolveList = [];
    },
    //重置搜索
    async resetSearch() {
      this.dateFilter = {
        year: undefined, //年份
        quarter: undefined, //季度
        month: undefined, //月份
        dayStart: undefined, //周数 开始 天
        dayEnd: undefined, //周数 结束 天
        weekIndex: undefined,// 周数索引(标记用)
      };
      await this.refresh();
    },
    // 获取 任务计划
    async getMasterPlan() {
      let res = await apiMasterPlan.get(this.iMasterPlanId);
      if (requestIsSuccess(res)) {
        this.masterPlan = res.data;
      }
    },

    // 根据plan id 获取  年份 列表
    async getYears() {
      let res = await apiMasterPlanContent.getYears(this.iMasterPlanId);
      if (requestIsSuccess(res)) {
        this.yearList = res.data;
      }
    },
    // 递归修改父亲
    async recChangeParent(task) {
      let flatList = flatArr(this.resolveList);
      let markIds = findParentIds(flatList, task);
      this.resolveList = recModify(this.resolveList, task, markIds);

      // if (this.resolveList.length !== 0) {
      //   let startDate = this.resolveList[0].startDate;
      //   let endDate = this.resolveList[0].endDate;
      //   // 时间比较
      //   let startNumber = dayjs(startDate).diff(dayjs(dayjs(this.masterPlan.planStartTime).format('YYYY-MM-DD')), 'day');
      //   let endNumber = dayjs(endDate).diff(dayjs(this.masterPlan.planEndTime), 'day');

      //   // 如果 顶级树 结束时间 大于当前 编制的施工计划  结束时间,则将 当前 编制的施工计划结束时间 往上加
      //   if (endNumber != 0 || startNumber != 0) {
      //     // console.log("顶级时间大于计划结束时间");
      //     let res = await apiMasterPlan.changeDateById(this.masterPlan.id, [startDate, endDate]);
      //     if (requestIsSuccess(res)) {
      //       //console.log('修改planContent结束时间顺便也把plan的结束时间修改了');
      //       await this.getMasterPlan();
      //     }
      //   }
      // }
    },
    // 甘特图 Item  变化
    async ganttItemChange(task) {
      // 根据 GanttItemState 执行不同的操作
      // 如果 task 的 开始时间 小于 父亲的 开始时间,不让他动
      if (task._parent) {
        if (dayjs(task._parent.startDate).diff(dayjs(task.startDate), 'days') > 0) {
          this.$message.warn('开始日期不能早于父计划的开始日期');
          task.startDate = task._parent.startDate;
          await this.recChangeParent(task);
          // return await this.refresh();
        }
      }
      switch (task.ganttItemState) {
      case GanttItemState.Edit: // 编辑
        if (dayjs(task._parent.startDate).diff(dayjs(task.startDate), 'days') > 0) {
          task.startDate = task._parent.startDate;
        }
        if (dayjs(task.endDate).diff(dayjs(task._parent.endDate), 'days') > 0) {
          task.endDate = task._parent.endDate;
        }
        await this.recChangeParent(task);
        break;
      case GanttItemState.Delete:
        if (requestIsSuccess(await apiMasterPlanContent.delete(task.id))) {
          // console.log(`删除任务-${task.name}成功`);
          await this.refresh();
        }
        break;
      case GanttItemState.Add:
        if (requestIsSuccess(await apiMasterPlanContent.create({
          ...task,
          masterPlanId: task.parentId ? undefined : task.topLvTreeId,
          masterPlanIdMark: this.masterPlan.id,
        }))) {
          // console.log(`添加任务-${task.name}成功`);
          await this.recChangeParent(task);
        }
        await this.refresh();

        break;
      }
    },
    async refresh() {
      // 刷新表之前先把 数据 弄回初始化
      this.list = [];
      this.selectMasterPlanContentIds = [];
      this.selectedEntity = undefined;

      this.loading = true;
      let res = await apiMasterPlanContent.getSingleTree(this.iMasterPlanId, this.dateFilter);
      if (requestIsSuccess(res) && res.data) {
        this.list = res.data;
        // 递归遍历处理下,符合 gantt 图的数据
        this.resolveList = recModifyProp(res.data, false);
        // console.log('this.resolveList',this.resolveList);
        let flatList = flatArr(this.resolveList);
        let depthArr = flatList.map(x => x.depth);
        this.treeDepth = depthArr.length === 0 ? 0 : Math.max(...depthArr);

      }
      this.loading = false;
    },
    // 甘特图保存的冒泡事件
    async save() {
      if (this.resolveList.length === 0) return;
      let flatList = flatArr(this.resolveList);

      // 处理一下,过滤掉不需要的属性 ,不然 axios 好像会递归 ……
      let solveList = flatList.map(x => ({
        id: x.id,
        name: x.name,
        content: x.content,
        startDate: x.startDate,
        endDate: x.endDate,
        duration: x.duration,
        parentId: x.parentId,
        isMilestone: x.isMilestone,
        ganttItemState: x.ganttItemState,
        preTaskIds: x.preTaskIds && x.preTaskIds.length > 0 ? x.preTaskIds : [],
        masterPlanId: x.depth === 0 ? this.iMasterPlanId : undefined, //深度是0 就是最外面一层,这个是带 masterPlanId 的 ,他的子级不带
      }));
      let res = await apiMasterPlanContent.batchSave(solveList);
      if (requestIsSuccess(res)) {
        this.$message.success('保存成功!');
        this.refresh();
      }
    },
  },
  render() {
    return (
      <div class="sm-construction-master-plan-content-with-gantt">
        <PlanCommonHeader
          axios={this.axios}
          plan={this.masterPlan}
          isSelectName={this.isSelectName}
          selectedPlanId={this.iMasterPlanId}
          yearList={this.yearList}
          dateFilter={this.dateFilter}
          onSelectChange={async id => {
            this.iMasterPlanId = id;
            await this.getMasterPlan();
            await this.getYears();
            await this.refresh();
            //  选择别的 计划后 把已选择的置空
            this.$emit('crossSelectedChange', []);
          }}
          onDateChange={dateFilter => {
            this.dateFilter = dateFilter;
            this.refresh();
          }}
        />

        {!this.inModal &&
          <a-button size='small' type='primary' onClick={() => { this.$emit('back'); }}>返回</a-button>
        }
        <a-button size='small' type='primary' style={this.isSelectName ? '' : 'margin-left: 8px;'} onClick={this.refresh}>搜索</a-button>
        <a-button size='small' type='primary' style='margin-left: 8px;' onClick={this.resetSearch}>重置</a-button>
        {!this.isApproval &&
          [
            <a-button style='margin-left:10px' size='small' type='primary' onClick={this.save}>保存</a-button>,
            <a-button style='margin-left:10px;' size='small' type='primary' disabled={this.resolveList.length === 0} onClick={() => this.modalVisible = true}>引用</a-button>,
            // <a-button style='margin-left:10px;' size='small' type='primary' onClick={() => this.$message.error("没时间研究")}>导入项目文件</a-button>,
          ]
        }
        {/*{this.isApproval&&*/}
        {/*  <a-button style='margin-left:10px;' size='small' type='primary' onClick={() => this.$message.error("没时间研究")}>审批</a-button>*/}
        {/*}*/}

        <ScGantt
          style='margin-top:10px;'
          treeDepth={this.treeDepth}
          showSelection={this.showSelection}
          selectedIds={this.selectedIds}
          data={this.resolveList}
          columns={this.columns}
          disableEdit={this.isApproval}
          topLvTreeId={this.iMasterPlanId}
          onChange={this.ganttItemChange}
          onMove={this.moveTask}
          onSelectedChange={selectedIds => this.$emit('selectedChange', selectedIds)}
        />
        <a-modal
          title='任务计划引用'
          visible={this.modalVisible}
          footer={null}
          width='40%'
          bodyStyle={{ height: '500px', overflowY: 'auto' }}
          onCancel={() => {
            this.refresh();
            this.modalVisible = false;
          }}
          onOk={() => {
            this.refresh();
            this.modalVisible = false;
          }}
        >
          <SmConstructionMasterPlanContent
            axios={this.axios}
            showOperator={false}
            masterPlanId={this.iMasterPlanId}
            isModalState={true}
            showSelectRow={false}
          />
        </a-modal>
      </div>
    );
  },
};
