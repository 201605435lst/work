import {
  ModalStatus,
  StationsType,
  DateReportType,
  RepairType,
  StoreHouseEnable,
  RepairValType,
  RepairPeriodUnit,
  SelectablePlanType,
  YearMonthPlanState,
  PlanType,
  WorkContentType,
  PlanState,
  RepairLevel,
  MaterialAcceptanceTestStatus,
  OrderState,
  TestingResultType,
  StoreEquipmentTransferTypeEnable,
  WorkflowState,
  WorkflowStepState,
  UserWorkflowGroup,
  SkylightType,
  CategoryEnable,
  InstallationSiteType,
  RunningState,
  ConstructType,
  RailwayType,
  QuestionType,
  State,
  Source,
  ServiceLifeUnit,
  Group,
  StoreEquipmentState,
  RailwayDirection,
  InstallationSiteState,
  InstallationSiteLocationType,
  InstallationSiteUseType,
  StoreEquipmentTestState,
  EquipmentServiceRecordType,
  ScopeType,
  CableLayType,
  CableCoreType,
  SealType,
  ProjectState,
  PageState,
  AlarmLevel,
  MemberType,
  BuildUnitType,
  ReportType,
  TaskGroup,
  PriorityType,
  StateType,
  SecurityType,
  WorkType,
  ScheduleState,
  SupplierType,
  SupplierLevel,
  SupplierProperty,
  ProcessType,
  ComputerCodeType,
  Category,
  ModelDetailLevel,
  ApprovalIdea,
  MarkType,
  MaterialAcceptanceTypeEnable,
  WorkTicketRltCooperationUnitState,
  SafeProblemState,
  NodeType,
  ComponentTrackTypeType,
  QualityProblemLevel,
  QualityProblemType,
  SafetyRiskLevel,
  PurchaseListTypeStatus,
  PurchaseState,
  PurchaseListType,
  GanttItemState,
  DispatchState,
  UnplannedTaskType,
  SafetyMeasure,
  ControlType,
} from './enum';
import moment from 'moment';
import dayjs from 'dayjs';

/**
 * 获取模态框标题
 * @param {状态} status
 */
export function getProjectStateTitle(status) {
  let title = '';
  switch (status) {
  case ProjectState.Building:
    title = '在建';
    break;
  case ProjectState.Finshed:
    title = '竣工';
    break;
  case ProjectState.Acceptance:
    title = '验收';
    break;
  case ProjectState.Stop:
    title = '终止';
    break;
  case ProjectState.WaitSurvey:
    title = '待勘察';
    break;
  case ProjectState.NoStart:
    title = '未开始';
    break;
  case ProjectState.Start:
    title = '开始';
    break;
  default:
    break;
  }
  return title;
}
// //材料检测类型
// export const MaterialAcceptanceTypeEnable={
//   Inspect:1,//送检
//   SelfInspection:2,//自检
// };
export function getMaterialAcceptanceTypeEnable(status) {
  let title = '';
  switch (status) {
  case MaterialAcceptanceTypeEnable.Inspect:
    title = '送检';
    break;
  case MaterialAcceptanceTypeEnable.SelfInspection:
    title = '自检';
    break;
  default:
    title = undefined;
    break;
  }
  return title;
}
// WaitingImprove: 1, //待整改
// WaitingVerifie: 2, //待审查
// Improved: 3, //已整改
export function getSafeProblemState(status) {
  let title = '';
  switch (status) {
  case SafeProblemState.WaitingImprove:
    title = '待整改';
    break;
  case SafeProblemState.WaitingVerifie:
    title = '待审查';
    break;
  case SafeProblemState.Improved:
    title = '已整改';
    break;
  default:
    title = undefined;
    break;
  }
  return title;
}
// //采购方式
// export const PurchaseListTypeStatus = {
//   MonthPurchasing: 1, //按月采购
//   CentralPurchasing: 2, //集中采购
//   MinorPurchasing: 3, //零星采购
// };
export function getPurchaseListTypeStatus(status) {
  let title = '';
  switch (status) {
  case PurchaseListTypeStatus.MonthPurchasing:
    title = '按月采购';
    break;
  case PurchaseListTypeStatus.CentralPurchasing:
    title = '集中采购';
    break;
  case PurchaseListTypeStatus.MinorPurchasing:
    title = '零星采购';
    break;
  }
  return title;
}
//任务类型
export function getUnplannedTaskType(status) {
  let title = '';
  switch (status) {
  case UnplannedTaskType.TemporaryDuty:
    title = '临时任务';
    break;
  case UnplannedTaskType.OtherDuty:
    title = '其他任务';
    break;
  }
  return title;
}
export function getMaterialAcceptanceTestStatus(status) {
  let title = '';
  switch (status) {
  case MaterialAcceptanceTestStatus.ForAcceptance:
    title = '待验收';
    break;
  case MaterialAcceptanceTestStatus.Approved:
    title = '已验收';
    break;
  }
  return title;
}
export function getBuildTypeTitle(status) {
  let title = '';
  switch (status) {
  case BuildUnitType.Supplier:
    title = '供应商';
    break;
  case BuildUnitType.Owner:
    title = '业主';
    break;
  default:
    break;
  }
  return title;
}
//构件跟踪
export function getNodeTypeTitle(status) {
  let title = '';
  switch (status) {
  case NodeType.CheckOut:
    title = '检验';
    break;
  case NodeType.PutStorage:
    title = '入库';
    break;
  case NodeType.OutStorage:
    title = '出库';
    break;
  case NodeType.ToTest:
    title = '到场检验';
    break;
  case NodeType.Install:
    title = '安装';
    break;
  case NodeType.Alignment:
    title = '调试';
  default:
    break;
  }
  return title;
}
/* 接口检查情况 */
export function getMarkType(status) {
  let title = '';
  switch (status) {
  case MarkType.NoCheck:
    title = '未检查';
    break;
  case MarkType.Qualified:
    title = '合格';
    break;
  case MarkType.NoQualified:
    title = '不合格';
    break;
  }
  return title;
}
// //接口检查情况
// export const MarkType = {
//   NoCheck: 1, //未检查
//   Qualified: 2, //合格
//   NoQualified: 3, //不合格
// };
// export const ConstructType = {
//   Civil: 1, //接口标记
//   Electric: 2, //接口整改
// };
export function getConstructType(status) {
  let title = '';
  switch (status) {
  case ConstructType.Civil:
    title = '土建工程';
    break;
  case ConstructType.Electric:
    title = '四电工程';
    break;
  }
  return title;
}
export function getModalTitle(status) {
  let title = '';
  switch (status) {
  case ModalStatus.Add:
    title = '添加';
    break;
  case ModalStatus.Edit:
    title = '编辑';
    break;
  case ModalStatus.View:
    title = '查看';
    // case ModalStatus.Detail:
    //     title = '详情';
    break;
  }
  return title;
}
//机密类型
export function getSecurityTypeTitle(status) {
  let title = '';
  switch (status) {
  case SecurityType.secret:
    title = '绝密';
    break;
  case SecurityType.confidential:
    title = '机密';
    break;
  case SecurityType.common:
    title = '普通';
    break;
  }
  return title;
}

export function getStationTypeTitle(status) {
  let title = '';
  switch (status) {
  case StationsType.Station:
    title = '车站';
    break;
  case StationsType.Section:
    title = '区间';
    break;
  default:
    title = '未定义';
  }
  return title;
}
//工作汇报类型
export function getReportTypeTitle(status) {
  let title = '';
  switch (status) {
  case ReportType.DayReport:
    title = '日报';
    break;
  case ReportType.WeekReport:
    title = '周报';
    break;
  case ReportType.MonthReport:
    title = '月报';
    break;
  }
  return title;
}
//线路类型
export function getRailwayTypeTitle(status) {
  let title = '';
  switch (status) {
  case RailwayType.Uniline:
    title = '单线';
    break;
  case RailwayType.DoubleLine:
    title = '复线';
    break;
  default:
    title = '未定义';
  }
  return title;
}
//构件跟踪预设状态
export function getComponentTrackTypeTypeTitle(status) {
  let title = '';
  switch (status) {
  case ComponentTrackTypeType.Reserved:
    title = '已预设';
    break;
  case ComponentTrackTypeType.NoReserved:
    title = '未预设';
    break;
  }
  return title;
}
//线路方向
export function getRailwayDirectionTitle(status) {
  let title = '';
  switch (status) {
  case RailwayDirection.Up:
    title = '上行';
    break;
  case RailwayDirection.Down:
    title = '下行';
    break;
  case RailwayDirection.UpAndDown:
    title = '上下行';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//机房状态
export function getInstallationSiteStateTitle(status) {
  let title = '';
  switch (status) {
  case InstallationSiteState.Using:
    title = '在用';
    break;
  case InstallationSiteState.Building:
    title = '在建';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//位置类型
export function getInstallationSiteLocationTypeTitle(status) {
  let title = '';
  switch (status) {
  case InstallationSiteLocationType.RailwayOuter:
    title = '非沿线';
    break;
  case InstallationSiteLocationType.SectionInner:
    title = '沿线区间';
    break;
  case InstallationSiteLocationType.StationInner:
    title = '沿线站内';
    break;
  case InstallationSiteLocationType.Other:
    title = '其它';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//使用类别
export function getInstallationSiteUseTypeTitle(status) {
  let title = '';
  switch (status) {
  case InstallationSiteUseType.Private:
    title = '独用';
    break;
  case InstallationSiteUseType.Share:
    title = '共用';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//年月表类型
export function getDateReportTypeTitle(status) {
  let title = '';
  switch (status) {
  case DateReportType.Year:
    title = '年表';
    break;
  case DateReportType.Month:
    title = '月表';
    break;
  default:
    title = '未定义';
  }
  return title;
}
//仓库状态类型
export function getStoreHouseEnableOption(status) {
  let title = '';
  switch (status) {
  case StoreHouseEnable.All:
    title = '所有';
    break;
  case StoreHouseEnable.Enable:
    title = '启用';
    break;
  case StoreHouseEnable.Unable:
    title = '停用';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//维修类别
export function getRepairTypeTitle(status) {
  let title = '';
  switch (status) {
  case RepairType.Daily:
    title = '日常检修';
    break;
  case RepairType.Muster:
    title = '集中检修';
    break;
  case RepairType.Key:
    title = '重点检修';
    break;
  case RepairType.Other:
    title = '其他';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//维修类别
export function getRepairValTypeTitle(status) {
  let title = '';
  switch (status) {
  case RepairValType.Number:
    title = '数字';
    break;
  case RepairValType.Char:
    title = '字符';
    break;
  case RepairValType.Table:
    title = '表格';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//维修周期单位
export function getRepairPeriodUnit(unit) {
  let title = '';
  switch (unit) {
  case RepairPeriodUnit.Other:
    title = '其他';
    break;
  case RepairPeriodUnit.Year:
    title = '年';
    break;
  case RepairPeriodUnit.Month:
    title = '月';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//添加待选计划类型
export function getSelectablePlanType(status) {
  let title = '';
  switch (status) {
  case SelectablePlanType.Year:
    title = '年';
    break;
  case SelectablePlanType.HalfYaer:
    title = '半年';
    break;
  case SelectablePlanType.QuarterYear:
    title = '季度';
    break;
  case SelectablePlanType.Month:
    title = '月';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//年月表状态
export function getYearMonthPlanStateType(status) {
  let title = '';
  switch (status) {
  case YearMonthPlanState.UnCommit:
    title = '未提交';
    break;
  case YearMonthPlanState.UnCheck:
    title = '待审核';
    break;
  case YearMonthPlanState.Checking:
    title = '审核中';
    break;
  case YearMonthPlanState.Passed:
    title = '审核通过';
    break;
  case YearMonthPlanState.UnPassed:
    title = '审核驳回';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//计划类型
export function getPlanTypeTitle(status) {
  let title = '';
  switch (status) {
  case PlanType.Vertical:
    title = '垂直天窗';
    break;
  case PlanType.General:
    title = '综合天窗';
    break;
  case PlanType.OutOf:
    title = '天窗点外';
    break;
  case PlanType.All:
    title = '全部';
    break;
  default:
    title = '';
  }
  return title;
}

//计划内容类型
export function getWorkContentTypeTitle(status) {
  let title = '';
  switch (status) {
  case WorkContentType.MonthYearPlan:
    title = '年月计划';
    break;
  case WorkContentType.OtherPlan:
    title = '其他计划';
    break;
  default:
    title = '';
  }
  return title;
}

//天窗类型
export function getSkyligetTypeTitle(status) {
  let title = '';
  switch (status) {
  case SkylightType.Vertical:
    title = '垂直天窗';
    break;
  case SkylightType.General:
    title = '综合天窗';
    break;
  case SkylightType.OutOf:
    title = '天窗点外';
    break;
  case SkylightType.All:
    title = '全部';
    break;
  case SkylightType.Other:
    title = '其他';
    break;
  default:
    title = '';
  }
  return title;
}

//计划状态
export function getPlanState(status) {
  let title = '';
  switch (status) {
  case PlanState.UnDispatching:
    title = '待派工';
    break;
  case PlanState.Dispatching:
    title = '已派工';
    break;
  case PlanState.NotIssued:
    title = '未下发';
    break;
  case PlanState.Issued:
    title = '已下发';
    break;
  case PlanState.Complete:
    title = '已完成';
    break;
    // case PlanState.Submited:
    //   title = '已提交';
    //   break;
  case PlanState.Waitting:
    title = '审批中';
    break;
  case PlanState.UnSubmited:
    title = '待提交';
    break;
  case PlanState.Revoke:
    title = '已撤销';
    break;
  case PlanState.Adopted:
    title = '已批复';
    break;
  case PlanState.UnAdopted:
    title = '未批复';
    break;
  case PlanState.Backed:
    title = '已退回';
    break;
  case PlanState.OrderCancel:
    title = '命令号取消';
    break;
  case PlanState.NaturalDisasterCancel:
    title = '自然灾害取消';
    break;
  case PlanState.OtherReasonCancel:
    title = '其他原因取消';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//维修级别
export function getRepairLevelTitle(status) {
  let title = '';
  switch (status) {
  case RepairLevel.LevelI:
    title = '天窗点内I级维修';
    break;
  case RepairLevel.LevelII:
    title = '天窗点内II级维修';
    break;
  case RepairLevel.LevelIII:
    title = '天窗点外I级维修';
    break;
  case RepairLevel.LevelIv:
    title = '天窗点外II级维修';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//派工单状态
export function getOrderStateTitle(status) {
  let title = '';
  switch (status) {
  case OrderState.UnFinished:
    title = '未完成';
    break;
  case OrderState.Complete:
    title = '已完成';
    break;
  case OrderState.Acceptance:
    title = '已验收';
    break;
  case OrderState.NaturalDisasterCancel:
    title = '自然灾害取消';
    break;
  case OrderState.OrderCancel:
    title = '命令号取消';
    break;
  case OrderState.OtherReasonCancel:
    title = '其他原因取消';
    break;
  default:
    title = '未定义';
  }
  return title;
}

// 工作流状态
export function getWorkflowState(state) {
  let title = '';
  switch (state) {
  case WorkflowState.All:
    title = '全部';
    break;
  case WorkflowState.Finished:
    title = '已完成';
    break;
  case WorkflowState.Waiting:
    title = '待审批';
    break;
  case WorkflowState.Rejected:
    title = '已退回';
    break;
  case WorkflowState.Stopped:
    title = '已终止';
    break;
  default:
    title = '未定义';
  }
  return title;
}
//工作流节点状态
export function getWorkflowStepState(state) {
  let title = '';
  switch (state) {
  case WorkflowStepState.Approved:
    title = '通过';
    break;
  case WorkflowStepState.Rejected:
    title = '退回';
    break;
  case WorkflowStepState.Stopped:
    title = '终止';
    break;
  default:
    title = '发起';
  }
  return title;
}

// 用户工作流群组
export function getUserWorkflowGroup(group) {
  let title = '';
  switch (group) {
  case UserWorkflowGroup.All:
    title = '所有';
    break;
  case UserWorkflowGroup.Initial:
    title = '我发起的';
    break;
  case UserWorkflowGroup.Waiting:
    title = '待我审批';
    break;
  case UserWorkflowGroup.Approved:
    title = '我已审批';
    break;
  case UserWorkflowGroup.Cc:
    title = '抄送我的';
    break;
  default:
    title = '未定义';
  }
  return title;
}

// 栏目是否启用
export function getCategoryEnable(status) {
  let title = '';
  switch (status) {
  case CategoryEnable.All:
    title = '所有';
    break;
  case CategoryEnable.Enable:
    title = '是';
    break;
  case CategoryEnable.Unable:
    title = '否';
    break;
  default:
    title = '未定义';
  }
  return title;
}
// 设备是否出库
export function getStoreEquipmentTransferTypeEnable(status) {
  let title = '';
  switch (status) {
  case StoreEquipmentTransferTypeEnable.Import:
    title = '入库';
    break;
  case StoreEquipmentTransferTypeEnable.Export:
    title = '出库';
    break;
  default:
    title = '未定义';
  }
  return title;
}
// 设备安装位置类型
export function getInstallationSiteType(status) {
  let title = '';
  switch (status) {
  case InstallationSiteType.Station:
    title = '车站';
    break;
  case InstallationSiteType.Section:
    title = '区间';
    break;
  case InstallationSiteType.Other:
    title = '其他';
    break;
  default:
    title = '未定义';
  }
  return title;
}
//电算代号类型
export function getComputerCodeTypeTitle(status) {
  let title = '';
  switch (status) {
  case ComputerCodeType.Artificial:
    title = '人工';
    break;
  case ComputerCodeType.Mechanics:
    title = '机械';
    break;
  case ComputerCodeType.Material:
    title = '材料';
    break;
  default:
    break;
  }
  return title;
}
// 设备 运行状态类型
export function getRunningState(status) {
  let title = '';
  switch (status) {
  case RunningState.PrimaryUse:
    title = '主用';
    break;
  case RunningState.SealUp:
    title = '封存';
    break;
  case RunningState.Standby:
    title = '备用';
    break;
  case RunningState.Scrap:
    title = '报废';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//题库 题目类型
export function getQuestionType(status) {
  let title = '';
  switch (status) {
  case QuestionType.SingleChoice:
    title = '单选';
    break;
  case QuestionType.MultipleChoice:
    title = '多选';
    break;
  case QuestionType.TrueOrFalseQuestions:
    title = '判断';
    break;
  case QuestionType.GapFilling:
    title = '填空';
    break;
  case QuestionType.ShortAnswerQuestion:
    title = '简答';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//故障案例 故障状态
export function getState(status) {
  let title = '';
  switch (status) {
  case State.UnSubmitted:
    title = '待提交';
    break;
  case State.Submitted:
    title = '已提交';
    break;
  case State.Pending:
    title = '待处理';
    break;
  case State.UnChecked:
    title = '未销记';
    break;
  case State.CheckedOut:
    title = '已销记';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//故障案例 来源
export function getSource(status) {
  let title = '';
  switch (status) {
  case Source.History:
    title = '历史记录';
    break;
  case Source.System:
    title = '系统登记';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//故障案例 来源
export function getServiceLifeUnit(status) {
  let title = '';
  switch (status) {
  case ServiceLifeUnit.Year:
    title = '年';
    break;
  case ServiceLifeUnit.Month:
    title = '月';
    break;
  case ServiceLifeUnit.Day:
    title = '天';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//应急预案关联人员 处理状态
export function getGroup(status) {
  let title = '';
  switch (status) {
  case Group.Launched:
    title = '由我发起';
    break;
  case Group.Waiting:
    title = '待我处理';
    break;
  case Group.Handled:
    title = '我已处理';
    break;
  case Group.Cc:
    title = '抄送给我';
    break;
  default:
    title = '未定义';
  }
  return title;
}
//库存设备 设备状态
export function getStoreEquipmentState(status) {
  let title = '';
  switch (status) {
  case StoreEquipmentState.UnActived:
    title = '未激活';
    break;
  case StoreEquipmentState.OnService:
    title = '已安装';
    break;
  case StoreEquipmentState.WaitForTest:
    title = '待检测';
    break;
  case StoreEquipmentState.Spare:
    title = '备用';
    break;
  case StoreEquipmentState.Scrap:
    title = '报废';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//检测单 设备状态
export function getStoreEquipmentTestPassed(status) {
  let title = '';
  switch (status) {
  case StoreEquipmentTestState.Unqualified:
    title = '不合格';
    break;
  case StoreEquipmentTestState.Qualified:
    title = '合格';
    break;
  case StoreEquipmentTestState.All:
    title = '所有';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//设备 设备上下道
export function getEquipmentServiceRecordType(status) {
  let title = '';
  switch (status) {
  case EquipmentServiceRecordType.Install:
    title = '安装';
    break;
  case EquipmentServiceRecordType.UnInstall:
    title = '拆除';
    break;
  default:
    title = '未定义';
  }
  return title;
}
//检测结果
export function getTestingResultType(status) {
  let title = '';
  switch (status) {
  case TestingResultType.Qualified:
    title = '合格';
    break;
  case TestingResultType.Disqualification:
    title = '不合格';
    break;
  default:
  }
  return title;
}

//电缆铺设类型
export function getCableLayTypeTile(status) {
  let title = '';
  switch (status) {
  case CableLayType.Conduit:
    title = '管道';
    break;
  case CableLayType.Overhead:
    title = '架空';
    break;
  case CableLayType.Bury:
    title = '直埋';
    break;
  case CableLayType.InnerChannelFlow:
    title = '室内槽道及竖井';
    break;
  case CableLayType.OuterChannelFlow:
    title = '室外槽道';
    break;
  default:
    title = '未定义';
  }
  return title;
}

// 电缆类型
export function getCableCoreType(type) {
  let name = '';
  switch (type) {
  case CableCoreType.Electric:
    name = '电缆芯';
    break;
  case CableCoreType.Optical:
    name = '光缆芯';
    break;
  default:
    name = '未定义';
  }
  return name;
}

//签章类型
export function getSealType(type) {
  let name = '';
  switch (type) {
  case SealType.Personal:
    name = '个人签名';
    break;
  case SealType.Company:
    name = '单位印章';
    break;
  default:
    name = '未定义';
  }
  return name;
}

//成员类型
export function getMemberType(type) {
  let name = null;
  switch (type) {
  case MemberType.Role:
    name = MemberType.Role;
    break;
  case MemberType.User:
    name = MemberType.User;
    break;
  case MemberType.Organization:
    name = MemberType.Organization;
    break;
  case MemberType.DynamicType:
    name = MemberType.DynamicType;
    break;
  case MemberType.DynamicOrgLevel1:
    name = MemberType.DynamicOrgLevel1;
    break;
  case MemberType.DynamicOrgLevel2:
    name = MemberType.DynamicOrgLevel2;
    break;
  case MemberType.DynamicOrgLevel3:
    name = MemberType.DynamicOrgLevel3;
    break;
  case MemberType.DynamicOrgLevel4:
    name = MemberType.DynamicOrgLevel4;
    break;
  case MemberType.DynamicOrgLevel5:
    name = MemberType.DynamicOrgLevel4;
    break;
  case MemberType.DynamicOrgLevel6:
    name = MemberType.DynamicOrgLevel6;
    break;
  case MemberType.DynamicOrgLevel7:
    name = MemberType.DynamicOrgLevel7;
    break;
  case MemberType.DynamicOrgLevel8:
    name = MemberType.DynamicOrgLevel8;
    break;
  case MemberType.DynamicOrgLevel9:
    name = MemberType.DynamicOrgLevel9;
    break;
  default:
    name = '未定义';
  }
  return name;
}

// 电缆类型
export function getAlarmLevel(level) {
  let name = '';
  switch (level) {
  case AlarmLevel.PreAlarm:
    name = '预警告警';
    break;
  case AlarmLevel.Normal:
    name = '一般告警';
    break;
  case AlarmLevel.Important:
    name = '重要告警';
    break;
  case AlarmLevel.Emergency:
    name = '紧急告警';
    break;
  default:
    name = '未定义';
  }
  return name;
}

export function getAlarmColor(level, d3 = false) {
  let color = null;
  switch (level) {
  case AlarmLevel.PreAlarm:
    color = d3 ? '#00BFFF' : '#5093FF';
    break;
  case AlarmLevel.Normal:
    color = d3 ? '#FFFF00' : '#F2D519';
    break;
  case AlarmLevel.Important:
    color = d3 ? '#FF5E00' : '#FF9100';
    break;
  case AlarmLevel.Emergency:
    color = d3 ? '#FF0000' : '#FF4D4F';
    break;
  default:
    color = null;
  }
  return color;
}
// Processing: 1, //建设中
// Receive: 2, //待接收
// Finshed: 3, //已完成
// Refused: 4, //已拒绝
// NoStart: 5, //未启动
// Stop: 6, //已暂停
export function getTaskColor(state) {
  let color = null;
  switch (state) {
  case 1:
    color = '#32CD32';
    break;
  case 2:
    color = '#FFFF00';
    break;
  case 3:
    color = '#0000FF';
    break;
  case 4:
    color = '#800000';
    break;
  case 5:
    color = '#1E90FF';
    break;
  case 6:
    color = '#FF0000';
    break;
  default:
    color = null;
  }
  return color;
}

export function getTaskGroup(type) {
  let state = null;
  switch (type) {
  case TaskGroup.Initial:
    state = '我发起的';
    break;
  case TaskGroup.Manage:
    state = '我负责的';
    break;
  case TaskGroup.Cc:
    state = '我参与的';
    break;
  default:
    state = '未定义';
  }
  return state;
}

export function getPriorityType(type) {
  let state = null;
  switch (type) {
  case PriorityType.ImportantUrgent:
    state = '重要紧急';
    break;
  case PriorityType.ImportantNoUrgent:
    state = '重要不紧急';
    break;
  case PriorityType.NoImportantUrgent:
    state = '不重要紧急';
    break;
  case PriorityType.NoImportantNoUrgent:
    state = '不重要不紧急';
    break;
  default:
    state = '未定义';
  }
  return state;
}

export function getStateType(type) {
  let state = null;
  switch (type) {
  case StateType.Processing:
    state = 'processing';
    break;
  case StateType.Receive:
    state = 'receive';
    break;
  case StateType.Finshed:
    state = 'finshed';
    break;
  case StateType.Refused:
    state = 'refused';
    break;
  case StateType.NoStart:
    state = 'noStart';
    break;
  case StateType.Stop:
    state = 'stop';
    break;
  default:
    state = '未定义';
  }
  return state;
}

export function getWorkType(type) {
  let state = null;
  switch (type) {
  case WorkType.AutoCompute:
    state = '自动计算';
    break;
  case WorkType.Milestone:
    state = '里程碑';
    break;
  case WorkType.Important:
    state = '关键任务';
    break;
  case WorkType.Unimportant:
    state = '非关键任务';
    break;
  default:
    state = '未定义';
  }
  return state;
}

export function getScheduleState(type) {
  let state = null;
  switch (type) {
  case ScheduleState.Processing:
    state = '进行中';
    break;
  case ScheduleState.Finshed:
    state = '已完成';
    break;
  case ScheduleState.Refused:
    state = '已拒绝';
    break;
  case ScheduleState.NoStart:
    state = '未启动';
    break;
  case ScheduleState.Stop:
    state = '已暂停';
    break;
  default:
    state = '未定义';
  }
  return state;
}

//供应商类型
export function getSupplierType(type) {
  let state = null;
  switch (type) {
  case SupplierType.Supplier:
    state = '供应商';
    break;
  case SupplierType.Proprietor:
    state = '业主';
    break;
  case SupplierType.ConstructionTeam:
    state = '施工队';
    break;
  default:
    state = '未定义';
  }
  return state;
}

//供应商级别
export function getSupplierLevel(type) {
  let state = null;
  switch (type) {
  case SupplierLevel.LevelI:
    state = '一级供应商';
    break;
  case SupplierLevel.LevelII:
    state = '二级供应商';
    break;
  case SupplierLevel.LevelIII:
    state = '三级供应商';
    break;
  default:
    state = '未定义';
  }
  return state;
}

//供应商性质
export function getSupplierProperty(type) {
  let state = null;
  switch (type) {
  case SupplierProperty.Unit:
    state = '单位';
    break;
  case SupplierProperty.Personal:
    state = '个人';
    break;
  default:
    state = '未定义';
  }
  return state;
}

export function getCategory(type) {
  let category = null;
  switch (type) {
  case Category.Auxiliary:
    category = '辅助材料';
    break;
  case Category.Appliance:
    category = '器具';
    break;
  case Category.Mechanical:
    category = '机械';
    break;
  case Category.SafetyArticle:
    category = '安全防护用品';
    break;
  default:
    category = '未定义';
  }
  return category;
}

export function getApprovalIdea(type) {
  let idea = null;
  switch (type) {
  case ApprovalIdea.Consent:
    idea = '同意';
    break;
  case ApprovalIdea.ConsentApply:
    idea = '同意申请';
    break;
  case ApprovalIdea.Refuse:
    idea = '拒绝';
    break;
  case ApprovalIdea.RefuseApply:
    idea = '拒绝申请';
    break;
  default:
    idea = '未定义';
  }
  return idea;
}

export function getWorkTicketRltCooperationUnitState(type) {
  let state = null;
  switch (type) {
  case WorkTicketRltCooperationUnitState.Finish:
    state = '已完成';
    break;
  case WorkTicketRltCooperationUnitState.UnFinish:
    state = '未完成';
    break;
  default:
    state = '未定义';
    break;
  }
  return state;
}

/**
 * 过滤一个 Object 的特定属性
 * @param {*} object
 * @param {*} props ['name', 'age',...]
 * @param {*} deep
 */
export function objFilterProps(object, props, deep = false) {
  let _obj = {};
  for (let prop of props) {
    if (object.hasOwnProperty(prop)) {
      _obj[prop] = object[prop];
    }
  }
  return _obj;
}

/**
 * 判断一个请求是否成功
 * @param {*} response
 */
export function requestIsSuccess(response) {
  if (response.request.responseURL.indexOf('ReturnUrl') >= 0) {
    return false;
  }

  return (
    response && (response.status === 200 || response.status === 201 || response.status === 204)
  );
}

/**
 * 浮点数加法
 * @param {*} arg1
 * @param {*} arg2
 */
export function addNum(arg1, arg2) {
  let r1, r2, m, c;
  try {
    r1 = arg1.toString().split('.')[1].length;
  } catch (e) {
    r1 = 0;
  }
  try {
    r2 = arg2.toString().split('.')[1].length;
  } catch (e) {
    r2 = 0;
  }
  c = Math.abs(r1 - r2);
  m = Math.pow(10, Math.max(r1, r2));
  if (c > 0) {
    let cm = Math.pow(10, c);
    if (r1 > r2) {
      arg1 = Number(arg1.toString().replace('.', ''));
      arg2 = Number(arg2.toString().replace('.', '')) * cm;
    } else {
      arg1 = Number(arg1.toString().replace('.', '')) * cm;
      arg2 = Number(arg2.toString().replace('.', ''));
    }
  } else {
    arg1 = Number(arg1.toString().replace('.', ''));
    arg2 = Number(arg2.toString().replace('.', ''));
  }
  return (arg1 + arg2) / m;
}

/**
 * 浮点数减法
 * @param {*} arg1
 * @param {*} arg2
 */
export function subNum(arg1, arg2) {
  let r1, r2, m, n;
  try {
    r1 = arg1.toString().split('.')[1].length;
  } catch (e) {
    r1 = 0;
  }
  try {
    r2 = arg2.toString().split('.')[1].length;
  } catch (e) {
    r2 = 0;
  }
  m = Math.pow(10, Math.max(r1, r2)); //last modify by deeka //动态控制精度长度
  n = r1 >= r2 ? r1 : r2;
  return parseFloat(((arg1 * m - arg2 * m) / m).toFixed(n));
}

/**
 * 浮点数乘法
 * @param {*} arg1
 * @param {*} arg2
 */
export function mulNum(arg1, arg2) {
  let m = 0,
    s1 = arg1.toString(),
    s2 = arg2.toString();
  try {
    m += s1.split('.')[1].length;
  } catch (e) {}
  try {
    m += s2.split('.')[1].length;
  } catch (e) {}
  return (Number(s1.replace('.', '')) * Number(s2.replace('.', ''))) / Math.pow(10, m);
}

/**
 * 浮点数除法
 * @param {*} arg1
 * @param {*} arg2
 */
export function divNum(arg1, arg2) {
  let t1 = 0,
    t2 = 0,
    r1,
    r2;
  try {
    t1 = arg1.toString().split('.')[1].length;
  } catch (e) {}
  try {
    t2 = arg2.toString().split('.')[1].length;
  } catch (e) {}
  r1 = Number(arg1.toString().replace('.', ''));
  r2 = Number(arg2.toString().replace('.', ''));
  return (r1 / r2) * pow(10, t2 - t1);
}

export function getFileUrl(url) {
  let fileServerEndPoint = localStorage.getItem('fileServerEndPoint');
  return url && url.indexOf('http') > -1 ? url : fileServerEndPoint + url;
}

export function dateFormat(date, fmt) {
  let nowDate;
  if (typeof date == 'string') {
    date = date.replace(/-/g, '/');
  }
  if (!fmt) {
    fmt = 'yyyy-MM-dd HH:mm:ss';
  }
  if (date) {
    nowDate = new Date(date);
  } else {
    nowDate = new Date();
  }

  if (nowDate.getFullYear() == 1) return null;

  let o = {
    'M+': nowDate.getMonth() + 1, //月份
    'd+': nowDate.getDate(), //日
    'H+': nowDate.getHours(), //小时
    'm+': nowDate.getMinutes(), //分
    's+': nowDate.getSeconds(), //秒
    'q+': Math.floor((nowDate.getMonth() + 3) / 3), //季度
    S: nowDate.getMilliseconds(), //毫秒
  };
  if (/(y+)/.test(fmt))
    fmt = fmt.replace(RegExp.$1, (nowDate.getFullYear() + '').substr(4 - RegExp.$1.length));
  for (let k in o)
    if (new RegExp('(' + k + ')').test(fmt))
      fmt = fmt.replace(
        RegExp.$1,
        RegExp.$1.length == 1 ? o[k] : ('00' + o[k]).substr(('' + o[k]).length),
      );
  return fmt;
}

// 生产 Guid
export function CreateGuid() {
  function S4() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
  }
  return S4() + S4() + '-' + S4() + '-' + S4() + '-' + S4() + '-' + S4() + S4() + S4();
}

// 获取设备范围类型 1@name1@id1.2@name2@id2
export function parseScope(scopeString) {
  if (!scopeString) return null;

  let scopes = scopeString.split('.');
  let last = scopes[scopes.length - 1];
  let lastArray = last.split('@');

  let organizationScopeCode = scopes.length ? scopes[0] : null;
  let organizationScope = null;
  if (organizationScopeCode) {
    let [type, name, id] = organizationScopeCode.split('@');
    organizationScope = {
      type: parseInt(type),
      name,
      id,
    };
  }

  let railwayScopeCode = scopes.length ? scopes[1] : null;
  let railwayScope = null;
  if (railwayScopeCode) {
    let [type, name, id] = railwayScopeCode.split('@');
    railwayScope = {
      type: parseInt(type),
      name,
      id,
    };
  }

  return {
    _scopes: scopeString,
    type: parseInt(lastArray[0]),
    name: lastArray[1],
    id: lastArray[2],
    scope: last,
    scopeCode: last,
    organizationScope,
    railwayScope,
  };
}

/**
 * 获取范围编码
 * @param {Guid} id id
 * @param {ScopeType} type 范围类型
 * @param {String} name 名称
 */
export function stringfyScope(id, type, name) {
  if (!id || !type) {
    return null;
  }
  return `${type}@${name || ''}@${id}`;
}

/**
 * A 是否包含 B Scope
 * @param {string} scopeCodeA A
 * @param {string} scopeCodeB B
 */
export function scopeIsContains(scopeCodeA, scopeCodeB) {
  // scopeCodes:[
  //   '1@长阳线路所@39f7a383-e548-826a-cd73-f93a621e4023',
  //   '1@长阳线路所@39f7a383-e548-826a-cd73-f93a621e4023.2@长阳-北京@b424f56a-b5b9-4b33-819e-e07a677c3760',
  //   '1@长阳线路所@39f7a383-e548-826a-cd73-f93a621e4023.2@长阳-北京@b424f56a-b5b9-4b33-819e-e07a677c3760.3@长阳站@99074cc2-3d6e-445d-b45c-5e4ee887bbe3',
  //   '1@长阳线路所@39f7a383-e548-826a-cd73-f93a621e4023.2@长阳-北京@b424f56a-b5b9-4b33-819e-e07a677c3760.3@长阳站@99074cc2-3d6e-445d-b45c-5e4ee887bbe3.4@原继电器室@5a0abe2f-06dc-482a-883e-b26b12154e2c',
  // ]

  let a = parseScope(scopeCodeA);
  let b = parseScope(scopeCodeB);

  if (a && b && (a.scope === b.scope || a.id === b.id)) {
    return true;
  }

  if (scopeCodeB.indexOf(scopeCodeA) > -1) {
    return true;
  }

  return false;
}

/**
 *
 * @param {VNode} component vue 组件或者 dom
 * @param {Array} allPermissions 所有权限数组
 * @param {String} permission 当前权限组
 */
export function vPermission(component, allPermissions, permission) {
  if (permission instanceof Array) {
    for (let item of permission) {
      if (allPermissions.find(x => x === item) != null) {
        return component;
      }
    }
  } else if (typeof permission === 'string') {
    return allPermissions.find(x => x === permission) ? component : undefined;
  }
  return undefined;
}

/**
 * 是否拥有全新
 * @param {Array} allPermissions 所有权限数组
 * @param {String} permission 当前权限组
 */
export function vP(allPermissions, permission) {
  if (permission instanceof Array) {
    for (let item of permission) {
      if (allPermissions.find(x => x === item) != null) {
        return true;
      }
    }
  } else if (typeof permission === 'string') {
    if (allPermissions.find(x => x === permission) != null) {
      return true;
    }
  }
  return false;
}

/**
 * 条件是否成立
 * @param {*} component 组件
 * @param {*} bool 条件
 */
export function vIf(component, bool) {
  return bool ? component : undefined;
}

export function isArray(value) {
  return value instanceof Array;
}

export function isString(value) {
  return typeof value === 'string';
}

//工序类型
export function getProcessTypeOption(status) {
  let title = '';
  switch (status) {
  case ProcessType.ManagemenetTask:
    title = '管理任务';
    break;
  case ProcessType.ConstructionTask:
    title = '施工任务';
    break;
  default:
    title = '未定义';
  }
  return title;
}

//模型精细等级
export function getModelDetailLevel(status) {
  let title = '';
  switch (status) {
  case ModelDetailLevel.GradeI:
    title = 'G1.0';
    break;
  case ModelDetailLevel.GradeII:
    title = 'G2.0';
    break;
  case ModelDetailLevel.GradeIII:
    title = 'G3.0';
    break;
  case ModelDetailLevel.GradeIv:
    title = 'G4.0';
    break;
  }
  return title;
}

export function convertKeysToLowerCase(obj) {
  if (!obj || typeof obj !== 'object') return null;

  if (obj instanceof Array) {
    return obj.forEach(function(value) {
      return convertKeysToLowerCase(value);
    });
  }

  for (let key in obj) {
    let value = obj[key];
    delete obj[key];
    key = key.charAt(0).toLowerCase() + key.slice(1);
    obj[key] = value;
    convertKeysToLowerCase(value);
  }

  return obj;
}

export function firstToLowerCase(string) {
  if (string != null && string != undefined && string.length > 0) {
    return string.charAt(0).toLowerCase() + string.slice(1);
  }
  return string;
}

export function d3Pair2Path(groupName, name) {
  return `${groupName}/${name}`;
}

export function d3Path2Pair(path) {
  //'a/b/c'
  let arr = path.split('/');
  let name = arr.pop();

  let groupName = arr.join('/');
  return { groupName, name };
}

/**
 * 返回值
 * {
 *    "key": "string",
 *    "count": 0,
 *    "index": 0,
 *    "progress": 0,
 *    "isDone": true,
 *    "hasError": true,
 *    "message": "string"
 * }
 *
 * @param {*} taskKey
 * @param {*} axios
 * @param {*} options {onProgress：进度回调, onSuccess：成功回调, onFailure：失败回调, requestUrl: 任务请求地址, interval：轮训周期}
 */
export async function initBackgroundTask(
  taskKey,
  axios,
  options = {
    onProgress: null,
    onSuccess: null,
    onFailure: null,
    requestUrl: null,
    interval: 1000,
  },
) {
  let response;
  let onProgress = options.onProgress;
  let onSuccess = options.onSuccess;
  let onFailure = options.onFailure;
  let interval = options.interval || 1000;
  let requestUrl = options.requestUrl || '/api/app/commonBackgroundTask/get';

  let handle = async () => {
    response = await axios({
      url: requestUrl,
      method: 'get',
      params: { key: taskKey },
    });

    let isDoneEmit = false;

    if (requestIsSuccess(response)) {
      let rst = response.data;

      if (!rst) {
        clearInterval(timer);
        return;
      }

      console.log(rst);

      onProgress && onProgress(rst);

      if (rst.isDone) {
        setTimeout(() => clearInterval(timer), 10000);

        if (!isDoneEmit) {
          onSuccess && onSuccess();
        }
      }

      if (rst.hasError) {
        onFailure && onFailure(rst);
      }
    }
  };
  let timer = setInterval(handle, interval);

  return this;
}
//问题标记类型
export function getQualityProblemType(status) {
  let title = '';
  switch (status) {
  case QualityProblemType.A:
    title = 'A类';
    break;
  case QualityProblemType.B:
    title = 'B类';
    break;
  case QualityProblemType.C:
    title = 'C类';
    break;
  default:
    break;
  }
  return title;
}
//问题标记等级
export function getQualityProblemLevel(status) {
  let title = '';
  switch (status) {
  case QualityProblemLevel.Great:
    title = '重大质量事故';
    break;
  case QualityProblemLevel.General:
    title = '一般质量事故';
    break;
  case QualityProblemLevel.Minor:
    title = '质量问题';
    break;
  default:
    break;
  }
  return title;
}
//安全问题库风险等级
export function getSafetyRiskLevel(status) {
  let title = '';
  switch (status) {
  case SafetyRiskLevel.Especially:
    title = '特别重大事故';
    break;
  case SafetyRiskLevel.Great:
    title = '重大事故';
    break;
  case SafetyRiskLevel.Larger:
    title = '较大事故';
    break;
  case SafetyRiskLevel.General:
    title = '一般事故';
    break;
  default:
    break;
  }
  return title;
}
//采购清单状态
export function getPurchaseState(status) {
  let title = '';
  switch (status) {
  case PurchaseState.ToSubmit:
    title = '待提交';
    break;
  case PurchaseState.OnReview:
    title = '审核中';
    break;
  case PurchaseState.Pass:
    title = '审核通过';
    break;
  case PurchaseState.UnPass:
    title = '审核未通过';
    break;
  default:
    break;
  }
  return title;
}
//采购类型
export function getPurchaseListType(status) {
  let title = '';
  switch (status) {
  case PurchaseListType.ToSubmit:
    title = '按月采购';
    break;
  case PurchaseListType.OnReview:
    title = '集中采购';
    break;
  case PurchaseListType.Pass:
    title = '零星采购';
    break;
  default:
    break;
  }
  return title;
}

/**
 * 根据 年月 获取 指定月份 的周 时间段
 * @param year
 * @param month
 * @returns {*[]}
 */
export function getWeek(year, month) {
  month -= 1;
  const weeks = [];
  const firstDay = new Date(year, month, 1);
  const lastDay = new Date(year, month + 1, 0);
  const daysInMonth = lastDay.getDate();
  let dayOfWeek = firstDay.getDay();
  let start;
  let oldstart;
  let end;

  for (let i = 1; i < daysInMonth + 1; i++) {
    if (dayOfWeek === 1 || i === 1) {
      start = i;
    }

    if (dayOfWeek === 0 || i === daysInMonth) {
      end = i;

      if (start) {
        weeks.push({
          start: start,
          end: end,
        });
        start = null;
      }
    }

    dayOfWeek = new Date(year, month, i + 1).getDay();
  }

  return weeks;
}
// 递归展开数组 (树)
export function flatArr(treeArr) {
  const flatDeep = (arr, depth = 0) => {
    return arr.reduce((flat, item) => {
      item.depth = depth; // 给加一个深度的属性(后面要用)
      return flat.concat(item, item.children ? flatDeep(item.children, depth + 1) : []);
    }, []);
  };
  return flatDeep(treeArr);
}
// 根据对象搜索树元素的父亲及以上 (根据传进来的小明对象找到 小明的爸爸爷爷...及以上的祖先的ids)
export const findParentIds = (flatList, taskObj) => {
  let markIds = [];
  if (taskObj.parentId ) {
    let parentId = taskObj.parentId;
    markIds.push(parentId);
    let parent = flatList.find(x => x.id === parentId);
    return markIds.concat(findParentIds(flatList, parent));
  }
  return markIds;
};

/**
 * 根据条件递归修改
 * @param treArr 树元素
 * @param task 传进来的条件 树元素
 * @param markIds 要修改的 树元素 ids
 * @returns {*}
 */
export const recModify = (treArr, task, markIds) => {
  const deep = item => {
    if (!item) return;
    let newNode = { ...item };
    if (markIds.includes(newNode.id)) {
      //改父亲爷爷 的 时间
      // 如果 任务的日期大于父亲爷爷... 的 日期,则将父亲爷爷...的日期都修改了
      if (dayjs(task.endDate).diff(dayjs(newNode.endDate), 'day') >= 0) {
        newNode.endDate = task.endDate;
        newNode.ganttItemState = GanttItemState.Edit;
        newNode.duration = dayjs(newNode.endDate).diff(dayjs(newNode.startDate), 'day');
        if (newNode.workDay && newNode.workDay !== 0) {
          newNode.workerNumber = Math.round(newNode.workDay / newNode.duration);
          newNode.workerNumber = isNaN(newNode.workerNumber) ?0:newNode.workerNumber;
          console.log('newNode.workerNumber',newNode.workerNumber);
        }
      }
    }
    // 改了父亲爷爷也要把自己改了
    if (item.id === task.id) {
      newNode.name = task.name;
      newNode.content = task.content;
      newNode.startDate = task.startDate;
      newNode.endDate = task.endDate;
      newNode.ganttItemState = GanttItemState.Edit;
      newNode.duration = dayjs(task.endDate).diff(dayjs(task.startDate), 'day');
      newNode.parentId = task.parentId || newNode.parentId;
      newNode.preTaskIds = task.preTaskIds;
      newNode.isMilestone = task.isMilestone;
      if (newNode.workDay) {
        newNode.workDay = task.workDay || newNode.workDay;
      }
      if (newNode.workerNumber) {
        let workerNumber =
          task.workDay && task.parentId
            ? Math.round(task.workDay / task.duration)
            : Math.round(newNode.workDay / newNode.duration);
        newNode.workerNumber = workerNumber === 0 ? 1 : workerNumber;
        newNode.workerNumber = isNaN(newNode.workerNumber) ?0:newNode.workerNumber;
        // console.log('newNode.workerNumber',newNode.workerNumber);
      }
    }

    // || 运算符 左边为null或者 undefined 用右边,否则用左边(有点像c# 的 ??)
    newNode.children = (newNode.children || []).map(node => deep(node));
    return newNode;
  };
  return treArr.map(x => deep(x));
};
/* 将数组按指定元素排序 */
const groupBy = (array, f) => {
  let groups = {};
  array.forEach(function(o) {
    let group = JSON.stringify(f(o));
    groups[group] = groups[group] || [];
    groups[group].push(o);
  });
  return Object.keys(groups).map(function(group) {
    return groups[group];
  });
};
export function arrayGroupBy(list, groupId) {
  let sorted = groupBy(list, function(item) {
    return [item[groupId]];
  });
  return sorted;
}
//派工单审批状态
export function getDispatchState(status) {
  let title = '';
  switch (status) {
  case DispatchState.UnSubmit:
    title = '待提交';
    break;
  case DispatchState.OnReview:
    title = '审核中';
    break;
  case DispatchState.Pass:
    title = '已通过';
    break;
  case DispatchState.UnPass:
    title = '已驳回';
    break;
  default:
    break;
  }
  return title;
}

//安全防护措施
export function getSafetyMeasure(status) {
  let title = '';
  switch (status) {
  case SafetyMeasure.InternalTraining:
    title = '内部培训';
    break;
  case SafetyMeasure.Ownar:
    title = '自身装备';
    break;
  case SafetyMeasure.NoSafetyRisk:
    title = '现场环境无安全隐患';
    break;
  default:
    break;
  }
  return title;
}

//安全防护措施
export function getControlType(status) {
  let title = '';
  switch (status) {
  case ControlType.KeyProcess:
    title = '关键工序';
    break;
  case ControlType.GeneralProcess:
    title = '一般工序';
    break;
  case ControlType.HideProcess:
    title = '隐蔽';
    break;
  case ControlType.SideProcess:
    title = '旁站';
    break;
  default:
    break;
  }
  return title;
}

/**
 * 两个时间进行比较
 * @param {String} dateTime1 时间1
 * @param {String} dateTime2 时间2
 * @param {String} TimeCompare 时间2 YYYY-MM-DD-HH-mm-ss
 */
export function compareDate(dateTime1, dateTime2, TimeCompare) {
  let date1 = moment(dateTime1).format(TimeCompare || 'YYYY-MM-DD');
  let date2 = moment(dateTime2).format(TimeCompare || 'YYYY-MM-DD');
  let formatDate1 = new Date(date1);
  let formatDate2 = new Date(date2);
  if (formatDate1 > formatDate2) {
    return '>';
  } else if (formatDate1 == formatDate2) {
    return '=';
  } else {
    return '<';
  }
}
