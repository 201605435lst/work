import './style';
import { findParentIds, flatArr, recModify, requestIsSuccess } from '../../_utils/utils';
import ApiPlanContent from '../../sm-api/sm-construction/ApiPlanContent';
import ApiPlan from '../../sm-api/sm-construction/ApiPlan';
import { GanttItemState } from '../../_utils/enum';
import dayjs from 'dayjs';
import Moment from 'moment';

import SmConstructionPlanContent from '../sm-construction-plan-content/SmConstructionPlanContent';
import SmConstructionMasterPlan from '../sm-construction-master-plan/SmConstructionMasterPlan';
import SmConstructionPlanMaterial from '../sm-construction-plan-material/SmConstructionPlanMaterial';
import PlanCommonHeader from './PlanCommonHeader';
import { recModifyProp, toPercent } from './planUtil';
import ScGantt from '../../sn-components/sc-gantt/ScGantt';
import ScPlayerBar from '../../sn-components/sc-player-bar/ScPlayerBar';
import { extendMoment } from 'moment-range';

const moment = extendMoment(Moment); // 引用 moment 的 range 插件 好算 日期之间的数组

let apiPlanContent = new ApiPlanContent();
let apiPlan = new ApiPlan();

export default {
  name: 'SmConstructionPlanWithGantt',
  props: {
    axios: { type: Function, default: null },
    isApproval: { type: Boolean, default: false }, // 是否在审批模式
    inModal: { type: Boolean, default: false }, // 是否在模态框里面
    isSelectName: { type: Boolean, default: false }, //是否选择名称
    showSelection: { type: Boolean, default: false }, // 是否显示选择框
    selectedIds: { type: Array, default: () => [] }, // 选中的ids
    planId: { type: String, default: undefined }, // 总体计划id
    single: { type: Boolean, default: false },// 是否单一显示甘特图
  },
  data() {
    return {
      list: [], // table 数据源
      plan: {}, // 任务计划实体
      iPlanId: undefined, // 施工 计划 Id
      resolveList: [], // 处理后的 数据源
      startDate: undefined, // 开始日期
      playBarValue: 0, // 播放条的值
      toolTipText: '', //  播放条的文字(toolTip)
      duration: 0, // 总任务的任务周期(天)
      loading: false, // table 是否处于加载状态
      treeDepth: 1, //树深度
      yearList: [], // 年份列表
      dateFilter: { // 日期筛选
        year: undefined, //年份
        quarter: undefined, //季度
        month: undefined, //月份
        dayStart: undefined, //周数 开始 天
        dayEnd: undefined, //周数 结束 天
        weekIndex: undefined,// 周数索引(标记用)
      },
      // 日期类型 默认 'day'
      dateType: { key: 'day', label: '日', value: 2880 },
      dateArr: [], // 日期数组
      selectPlanContentIds: [], // 选择的 施工计划详情 ids (选择框模式的时候用)
      selectedEntity: undefined, // 选择的实体(任务计划详情 content )
      dicTypes: [],
      selectedSubItems: [], //已选择分部分项
      importModelVisible: false,
      selectId: undefined, // 单选的id
      clickTimeChange: null, // 单击事件的延时变量
      rowMultiple: false, // table Row 是否是 多选模式
      subItemModalVisible: false, // 分布分项 模态框 显示与隐藏
      masterPlanModalVisible: false, // 总体计划 模态框 显示与隐藏
      planMaterialModalVisible: false, // 工程量信息 模态框 显示与隐藏
      selectMasterPlanIds: [],
      planContentId: undefined, // 施工计划详情id
      modifyList: [],// 修改的列表
      confirmLoading: false,
      columns: this.inModal ? [
        {
          title: '工日', name: 'workDay', width: 60, customRender: (text, index, barInfo) => {
            return <span>{text}</span>;
          },
        },
        {
          title: '人工', name: 'workerNumber', width: 60, customRender: (text, index, barInfo) => {
            return <span>{text}</span>;
          },
        },
        {
          title: '进度', name: 'allProgress', width: 60, customRender: (text, index, barInfo) => {
            return <span>{toPercent(text)}</span>;
          },
        },
      ] : [
        {
          title: '工程量', name: 'material', width: 90, customRender: (text, index, barInfo) => {
            return <span color="blue" onClick={event => {
              this.planContentId = barInfo.task.id;
              this.planMaterialModalVisible = true;
            }}
            >
              设置工程量
            </span>;
          },
        },
        {
          title: '工日', name: 'workDay', width: 60, customRender: (text, index, barInfo) => {
            return <span>{text}</span>;
          },
        },
        {
          title: '人工', name: 'workerNumber', width: 60, customRender: (text, index, barInfo) => {
            return <span>{text}</span>;
          },
        },
        {
          title: '进度', name: 'allProgress', width: 60, customRender: (text, index, barInfo) => {
            return <span>{toPercent(text)}</span>;
          },
        },
      ],
      initEquipment: false, // 是否查询了设备
    };
  },
  computed: {

  },
  watch: {
    planId: {
      handler: function (value, oldValue) {
        this.initAxios();
        this.iPlanId = value;
        this.refresh();
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    !this.isSelectName && await this.getPlan();
    await this.refresh();
    await this.getYears();
    this.changeDateType(this.dateType);
    this.playIndex = 0;
  },
  methods: {
    initAxios() {
      apiPlanContent = new ApiPlanContent(this.axios);
      apiPlan = new ApiPlan(this.axios);
    },
    // 修改日期类型
    changeDateType(dateType) {
      this.dateType = dateType;
      if (this.resolveList.length === 0) return;
      let start = this.resolveList[0].startDate;
      let end = dayjs(this.resolveList[0].endDate).add(-1, 'days').format('YYYY-MM-DD');
      let range = moment.range(start, end);
      let map;
      switch (dateType.key) {
      case 'day': // 获取 日期数组
        // value 是 值 ，ts 是时间戳
        map = Array.from(range.by('days')).map(x => ({ value: x.format('YYYY-MM-DD'), ts: x.valueOf() }));
        break;
      case 'week':
        map = Array.from(range.by('weeks')).map(x => ({ value: x.week(), ts: x.valueOf() }));
        break;
      case 'month':
        map = Array.from(range.by('months')).map(x => ({ value: x.format('YYYY-MM'), ts: x.valueOf() }));
        break;
      case 'quarter':
        map = Array.from(range.by('quarters')).map(x => ({ value: x.quarter(), ts: x.valueOf() }));
        break;
      case 'halfYear':
        map = Array.from(range.by('years')).map(x => ({ value: x.format('YYYY'), ts: x.valueOf() }));
        break;
      }
      this.dateArr = map;
    },
    async moveTask(task) {
      let res = await apiPlanContent.moveTask(task.id, task.parentId);
      if (requestIsSuccess(res)) {
        await this.refresh();
        task.ganttItemState = GanttItemState.Edit;
        await this.ganttItemChange(task);
      }

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
    // 根据plan id 获取  年份 列表
    async getYears() {
      let res = await apiPlanContent.getYears(this.iPlanId);
      if (requestIsSuccess(res)) {
        this.yearList = res.data;
      }
    },
    // 获取 任务计划
    async getPlan() {
      if (this.iPlanId == null) return;
      let res = await apiPlan.get(this.iPlanId);
      if (requestIsSuccess(res)) {
        this.plan = res.data;
      }
    },
    // 递归修改父亲
    async recChangeParent(task) {
      let flatList = flatArr(this.resolveList);
      let markIds = findParentIds(flatList, task);
      this.resolveList = recModify(this.resolveList, task, markIds);
      if (this.resolveList.length !== 0) {
        let startDate = this.resolveList[0].startDate;
        let endDate = this.resolveList[0].endDate;
        // 时间比较
        let startNumber = dayjs(startDate).diff(dayjs(dayjs(this.plan.planStartTime).format('YYYY-MM-DD')), 'day');
        let endNumber = dayjs(endDate).diff(dayjs(this.plan.planEndTime), 'day');
        // 如果 顶级树 结束时间 大于当前 编制的施工计划  结束时间,则将 当前 编制的施工计划结束时间 往上加
        if (endNumber != 0 || startNumber != 0) {
          // let res = await apiPlan.changeDateById(this.plan.id, [startDate, endDate]);
          // if (requestIsSuccess(res)) {
          //   // console.log('修改planContent结束时间顺便也把plan的结束时间修改了');
          //   await this.getPlan();
          // }
        }
      }
    },
    // 甘特图 Item  变化
    async ganttItemChange(task) {
      console.log(task);
      // 根据 GanttItemState 执行不同的操作
      // 如果 task 的 开始时间 小于 父亲的 开始时间,不让他动
      if (task._parent) {
        if (dayjs(task._parent.startDate).diff(dayjs(task.startDate), 'days') > 0) {
          this.$message.warn('开始日期不能早于父计划的开始日期');
          task.startDate = task._parent.startDate;
          await this.recChangeParent(task);
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
      case GanttItemState.Delete: // 删除
        if (requestIsSuccess(await apiPlanContent.delete(task.id))) {
          await this.refresh();
        }
        break;
      case GanttItemState.Add: // 添加
        if (requestIsSuccess(await apiPlanContent.create({
          ...task,
          planId: task.parentId ? undefined : task.topLvTreeId,
          planIdMark: this.plan.id,
        }))) {
          await this.recChangeParent(task);
        }
        await this.refresh();
        break;
      }
      // 修改完成后记得 把年份 更新下
      await this.getYears();
    },
    // 引用总体计划里面的 content 树
    async importMasterPlan() {
      if (this.selectMasterPlanIds.length === 0) {
        return this.$message.error('未选择计划!');
      }

      let res = await apiPlanContent.importMasterPlan(this.iPlanId, this.selectMasterPlanIds[0]);
      if (requestIsSuccess(res)) {
        this.$message.success('导入成功!');
        this.refresh();
      }


    },
    async refresh() {
      // 刷新表之前先把 数据 弄回初始化
      this.list = [];
      this.selectPlanContentIds = [];
      this.selectedEntity = undefined;

      this.loading = true;
      let res = await apiPlanContent.getSingleTree(this.iPlanId, this.dateFilter);
      if (requestIsSuccess(res) && res.data) {
        this.list = res.data;
        this.resolveList = recModifyProp(res.data, true);
        this.startDate = this.resolveList.length === 0 ? dayjs().format('YYYY-MM-01') : this.resolveList[0].startDate;
        this.duration = this.resolveList.length === 0 ? 0 : this.resolveList[0].duration;
        let flatList = flatArr(this.resolveList);
        let depthArr = flatList.map(x => x.depth);
        this.treeDepth = depthArr.length === 0 ? 0 : Math.max(...depthArr);
        this.changeDateType(this.dateType);
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
        workerNumber: x.workerNumber,
        preTaskIds: x.preTaskIds,
        planId: x.depth === 0 ? this.iPlanId : undefined, //深度是0 就是最外面一层,这个是带 planId 的 ,他的子级不带
      }));
      let res = await apiPlanContent.batchSave(solveList);
      if (requestIsSuccess(res)) {
        this.$message.success('保存成功!');
        await this.refresh();
      }
    },
  },
  render() {
    return (
      <div class="sm-construction-plan-with-gantt">

        {!this.single && <PlanCommonHeader
          isUseByPlan={true}
          axios={this.axios}
          plan={this.plan}
          size={this.inModal ? 'small' : 'default'}
          isSelectName={this.isSelectName}
          selectedPlanId={this.iPlanId}
          yearList={this.yearList}
          dateFilter={this.dateFilter}
          onSelectChange={async id => {
            this.iPlanId = id;
            await this.getPlan();
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
        }
        <div style="margin-bottom:10px;">
          {(!this.inModal) &&
            !this.single && <a-button size='small' type='primary' onClick={() => { this.$emit('back'); }}>返回</a-button>
          }
          {(!this.single) &&
            <a-button size='small' type='primary' style={this.isSelectName ? '' : 'margin-left: 8px;'} onClick={this.refresh}>搜索</a-button>}
          {!this.single && <a-button size='small' type='primary' style='margin-left: 8px;' onClick={this.resetSearch}>重置</a-button>}
          {!this.isApproval &&
            !this.single &&
            [
              <a-button size='small' style='margin-left: 8px;' type='primary' onClick={this.save}>保存</a-button>,
              <a-dropdown style='margin-left: 8px;'>
                <a-menu slot="overlay" click="handleMenuClick">
                  <a-menu-item key="1" onClick={() => {
                    if (this.list.length > 0) {
                      return this.$message.warn('已经有数据,不能总体计划引用!');
                    }
                    this.masterPlanModalVisible = true;
                  }} > 总体计划引用</a-menu-item>
                  <a-menu-item key="3" disabled={this.resolveList.length === 0} onClick={() => this.subItemModalVisible = true}> 分部分项引用</a-menu-item>
                </a-menu>
                <a-button type='primary' size='small' style="margin-left: 8px">引用<a-icon type="down" /></a-button>
              </a-dropdown>,
              // <a-button style='margin-left:10px;' size='small' type='primary' onClick={() => this.$message.error("没时间研究")}>导入项目文件</a-button>,
            ]
          }
          {this.isApproval &&
            !this.single &&
            !this.showSelection &&
            <a-button style='margin-left:10px;' size='small' type='primary' onClick={() => this.$message.error("没时间研究")}>审批</a-button>
          }
        </div>

        {/*TODO播放条先注释,后端还没有写……*/}
        {this.single ? <ScPlayerBar
          style='margin-top:10px;'
          ref='player'
          max={this.dateArr ? this.dateArr.length - 1 : 0}
          value={this.playBarValue}
          toolTipText={this.toolTipText}
          onPlay={async () => {
            // 第一次查询设备数据，查询成功后执行播放
            if (this.initEquipment) {
              this.$refs.gantt.play();
            } else {
              let response = await apiPlanContent.getAllRelationEquipment(this.planId);
              if (requestIsSuccess(response)) {
                this.$emit("init", response.data);
                // 两秒后执行
                setTimeout(() => {
                  this.$refs.gantt.play();
                }, 2000);
              }
            }
          }}
          onPause={() => this.$refs.gantt.pause()}
          onChange={val => this.$refs.gantt.changeSlider(val - 1)}
        /> : null}

        <ScGantt
          ref='gantt'
          treeDepth={this.treeDepth}
          showSelection={this.showSelection}
          selectedIds={this.selectedIds}
          style='margin-top:0px;'
          data={this.resolveList}
          dateArr={this.dateArr}
          columns={this.columns}
          disableEdit={this.isApproval}
          topLvTreeId={this.iPlanId}
          startDate={this.startDate}
          onChange={this.ganttItemChange}
          onMove={this.moveTask}
          onSelectedChange={selectedIds => { this.$emit('selectedChange', selectedIds); }}
          // 进度模拟
          onListenValue={async val => {
            let date = "";
            let speed = 0;
            switch (this.dateType.key) {
            case 'day': // 获取 日期数组
              this.playBarValue = val.val;
              this.toolTipText = val.text;
              date = val.text;
              break;
            case 'week':
              this.playBarValue = val.val;
              this.toolTipText = `第${val.text}周`;
              speed = val.text;
              break;
            case 'month':
              this.playBarValue = val.val;
              this.toolTipText = `第${val.text}月`;
              date = val.text;
              break;
            case 'quarter':
              this.playBarValue = val.val;
              this.toolTipText = `第${val.text}季度`;
              speed = val.text;
              break;
            case 'halfYear':
              this.playBarValue = val.val;
              this.toolTipText = `第${val.text}年`;
              speed = val.text;
              break;
            }
            //调用后端接口，查询时间段内的设备信息
            let response = await apiPlanContent.getAnalogData({
              planId: this.planId,
              type: this.dateType.key,
              speed: speed,
              date: date,
            });
            if (requestIsSuccess(response)) {
              this.$emit("change", response.data);
            };
          }}
          onPlayComplete={() => this.$refs.player.playComplete()}
          onDateTypeChange={this.changeDateType}
        />

        <a-modal
          title='任务计划引用'
          visible={this.subItemModalVisible}
          footer={null}
          width='40%'
          bodyStyle={{ height: '500px', overflowY: 'auto' }}
          onCancel={() => {
            this.refresh();
            this.subItemModalVisible = false;
          }}
          onOk={() => {
            this.refresh();
            this.subItemModalVisible = false;
          }}
        >
          <SmConstructionPlanContent
            axios={this.axios}
            showOperator={false}
            planId={this.iPlanId}
            isModalState={true}
            showSelectRow={false}
          />
        </a-modal>
        <a-modal
          title='总体计划引用'
          visible={this.masterPlanModalVisible}
          width='40%'
          bodyStyle={{ height: '500px', overflowY: 'auto' }}
          onCancel={() => {
            this.refresh();
            this.masterPlanModalVisible = false;
            this.selectMasterPlanIds = [];
          }}
          onOk={async () => {
            await this.importMasterPlan();
            await this.refresh();
            this.masterPlanModalVisible = false;
            this.selectMasterPlanIds = [];
          }}
        >
          <SmConstructionMasterPlan
            axios={this.axios}
            showOperator={false}
            onlyQuery={true}
            isModalState={true}
            showSelectRow={true}
            isSimpleColumn={true}
            selectIds={this.selectMasterPlanIds}
            onSelectedChange={selectIds => {
              this.selectMasterPlanIds = selectIds;
            }}

          />
        </a-modal>


        <a-modal
          title='工程量信息'
          visible={this.planMaterialModalVisible}
          width={800}
          forceRender={true}
          bodyStyle={{ height: '400px', overflowY: 'auto' }}
          // confirmLoading={this.confirmLoading}
          onCancel={() => {
            this.refresh();
            this.planContentId = undefined;
            this.planMaterialModalVisible = false;
            this.selectMasterPlanIds = [];
          }}
        // onOk={async () => {
        //   await this.refresh();
        //   this.planContentId = undefined;
        //   this.planMaterialModalVisible = false;
        //   this.selectMasterPlanIds = [];
        // }}
        >
          <template slot="footer">
            <a-button
              type="primary"
              loading={this.confirmLoading}
              onClick={async () => {
                await this.refresh();
                this.planContentId = undefined;
                this.planMaterialModalVisible = false;
                this.selectMasterPlanIds = [];
              }}
            >
              返回
            </a-button>
          </template>
          <SmConstructionPlanMaterial
            axios={this.axios}
            isApproval={this.isApproval}
            showOperator={!this.single}
            planContentId={this.planContentId}
            onChange={value => this.confirmLoading = value}
          />
        </a-modal >



      </div >
    );
  },
};
