// 人为制造冲突
import Common from './common';
const {
  Create,
  Update,
  Delete,
  Dot,
  Import,
  Detail,
  Export,
  UpdateEnable,
  Approval,
  Submit,
} = Common;
// 施工计划的权限设置

export const GroupName = 'Construction';
/* 嘟嘟
View
Draw
Approve

 */

// 总体计划
const GroupNameMasterPlan = GroupName + Dot + 'MasterPlan';
export const MasterPlan = {
  Default: GroupNameMasterPlan,
  Create: GroupNameMasterPlan + Dot + Create,
  Update: GroupNameMasterPlan + Dot + Update,
  Delete: GroupNameMasterPlan + Dot + Delete,
  Detail: GroupNameMasterPlan + Dot + Detail,
  View: GroupNameMasterPlan + Dot + 'View',
  Draw: GroupNameMasterPlan + Dot + 'Draw',
  Approve: GroupNameMasterPlan + Dot + 'Approve',
};
// 总体计划详情
const GroupNameMasterPlanContent = GroupName + Dot + 'MasterPlanContent';
export const MasterPlanContent = {
  Default: GroupNameMasterPlanContent,
  Create: GroupNameMasterPlanContent + Dot + Create,
  Update: GroupNameMasterPlanContent + Dot + Update,
  Delete: GroupNameMasterPlanContent + Dot + Delete,
  Detail: GroupNameMasterPlanContent + Dot + Detail,
};

// 施工计划
const GroupNamePlan = GroupName + Dot + 'Plan';
export const Plan = {
  Default: GroupNamePlan,
  Create: GroupNamePlan + Dot + Create,
  Update: GroupNamePlan + Dot + Update,
  Delete: GroupNamePlan + Dot + Delete,
  Detail: GroupNamePlan + Dot + Detail,
  View: GroupNamePlan + Dot + 'View',
  Draw: GroupNamePlan + Dot + 'Draw',
  Approve: GroupNamePlan + Dot + 'Approve',
};
// 施工计划详情
const GroupNamePlanContent = GroupName + Dot + 'PlanContent';
export const PlanContent = {
  Default: GroupNamePlanContent,
  Create: GroupNamePlanContent + Dot + Create,
  Update: GroupNamePlanContent + Dot + Update,
  Delete: GroupNamePlanContent + Dot + Delete,
  Detail: GroupNamePlanContent + Dot + Detail,
};
// 施工计划工程量
const GroupNamePlanMaterial = GroupName + Dot + 'PlanMaterial';
export const PlanMaterial = {
  Default: GroupNamePlanMaterial,
  Create: GroupNamePlanMaterial + Dot + Create,
  Update: GroupNamePlanMaterial + Dot + Update,
  Delete: GroupNamePlanMaterial + Dot + Delete,
  Detail: GroupNamePlanMaterial + Dot + Detail,
};

// 派工单模板
const GroupNameDispatchTemplate = GroupName + Dot + 'DispatchTemplate';
export const DispatchTemplate = {
  Default: GroupNameDispatchTemplate,
  Create: GroupNameDispatchTemplate + Dot + Create,
  Update: GroupNameDispatchTemplate + Dot + Update,
  Delete: GroupNameDispatchTemplate + Dot + Delete,
  Detail: GroupNameDispatchTemplate + Dot + Detail,
};

// 派工单管理
const GroupNameDispatch = GroupName + Dot + 'Dispatch';
export const Dispatch = {
  Default: GroupNameDispatch,
  Create: GroupNameDispatch + Dot + Create,
  Update: GroupNameDispatch + Dot + Update,
  Delete: GroupNameDispatch + Dot + Delete,
  Detail: GroupNameDispatch + Dot + Detail,
  Export: GroupNameDispatch + Dot + Export,
  Submit: GroupNameDispatch + Dot + Submit,
  Approval: GroupNameDispatch + Dot + Approval,
};

// 施工日志
const GroupNameDaily = GroupName + Dot + 'Dailys';
export const Dailys = {
  Default: GroupNameDaily,
  Create: GroupNameDaily + Dot + Create,
  Update: GroupNameDaily + Dot + Update,
  Delete: GroupNameDaily + Dot + Delete,
  Detail: GroupNameDaily + Dot + Detail,
  Submit: GroupNameDaily + Dot + Submit,
  Approve: GroupNameDaily + Dot + 'Approve',
  ApproveFlow: GroupNameDaily + Dot + 'ApproveFlow',
};
// 施工日志模板
const GroupNameDailyTemplate = GroupName + Dot + 'DailyTemplates';
export const DailyTemplates = {
  Default: GroupNameDailyTemplate,
  Create: GroupNameDailyTemplate + Dot + Create,
  Update: GroupNameDailyTemplate + Dot + Update,
  Delete: GroupNameDailyTemplate + Dot + Delete,
  Detail: GroupNameDailyTemplate + Dot + Detail,
  UpdateDefault: GroupNameDailyTemplate + Dot + "UpdateDefault",
};