// 递归遍历处理下,符合 gantt 图的数据 格式
import dayjs from 'dayjs';
import { GanttItemState } from '../../_utils/enum';

/**
 *
 * @param list 数据源
 * @param isPlan 是不是plan施工计划,总体计划的话要筛选另外 的值
 * @returns {*}
 */
export  const recModifyProp = (list,isPlan)=> {
  list.forEach(item => {
    item.startDate = dayjs(item.planStartTime).format('YYYY-MM-DD');
    item.endDate = dayjs(item.planEndTime).format('YYYY-MM-DD');
    item.name = item.name;  // desc = content
    item.content = item.content; // content = name
    item.duration = item.period;
    item.collapsed = false;
    let planPreIds = item.antecedents.map(x => (x.frontPlanContentId));
    let masterPlanPreIds = item.antecedents.map(x => (x.frontMasterPlanContentId));
    item.preTaskIds = isPlan===true? planPreIds: masterPlanPreIds;
    item.disabled = false; // 这个是选择树的时候 给 是否禁用
    item.editState = GanttItemState.UnModify; //默认所有元素都是未修改状态
    if (item.children.length !== 0) {
      item.children = recModifyProp(item.children,isPlan);
    }
  });
  return list;
};
export const toPercent = (point) => {
  let str = Number(point * 100).toFixed(1);
  str += '%';
  return str;
};
