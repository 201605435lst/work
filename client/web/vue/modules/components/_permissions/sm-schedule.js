import Common from './common';
const { Create, Update, Delete, Dot, Import, Detail, Export, SetFlow } = Common;
const GroupName = 'Schedule';

const GroupNameSchedules = GroupName + Dot + 'Schedule';
const Schedules = {
  Default: GroupNameSchedules,
  Create: GroupNameSchedules + Dot + Create,
  Update: GroupNameSchedules + Dot + Update,
  Delete: GroupNameSchedules + Dot + Delete,
  Detail: GroupNameSchedules + Dot + Detail,
  Import: GroupNameSchedules + Dot + Import,
  Export: GroupNameSchedules + Dot + Export,
  SetFlow: GroupNameSchedules + Dot + SetFlow,
};

const GroupNameApprovals = GroupName + Dot + 'Approval';
const Approvals = {
  Default: GroupNameApprovals,
  Create: GroupNameApprovals + Dot + Create,
  Update: GroupNameApprovals + Dot + Update,
  Delete: GroupNameApprovals + Dot + Delete,
  Detail: GroupNameApprovals + Dot + Detail,
  Export: GroupNameApprovals + Dot + Export,
};
const GroupNameDiarys = GroupName + Dot + 'Diary';
const Diarys = {
  Default: GroupNameDiarys,
  Fill: GroupNameDiarys + Dot + "Fill",
  View: GroupNameDiarys + Dot + "View",
  PdfExport: GroupNameDiarys + Dot + "PdfExport",
  Update: GroupNameDiarys + Dot + Update,
  ExcelExport: GroupNameDiarys + Dot + "ExcelExport",
  LogStatistics: GroupNameDiarys + Dot + "LogStatistics",
  Examination: GroupNameDiarys + Dot + "Examination",
};
export default {
  GroupName,
  Schedules,
  Approvals,
  Diarys,
};