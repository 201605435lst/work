// 人为制造冲突
import Common from './common';
const { Create, Update, Delete, Dot, Import, Detail, Export, UpdateEnable } = Common;
// 基础施工模块的权限设置

export const GroupName = 'ConstructionBase';

// 工种
const GroupNameWorker = GroupName + Dot + 'Worker';
export const Worker = {
  Default: GroupNameWorker,
  Create : GroupNameWorker + Dot + Create,
  Update : GroupNameWorker + Dot + Update,
  Delete : GroupNameWorker + Dot + Delete,
  Detail : GroupNameWorker + Dot + Detail,
};
// 设备台班
const GroupNameEquipmentTeam = GroupName + Dot + 'EquipmentTeam';
export const EquipmentTeam = {
  Default: GroupNameEquipmentTeam,
  Create : GroupNameEquipmentTeam + Dot + Create,
  Update : GroupNameEquipmentTeam + Dot + Update,
  Delete : GroupNameEquipmentTeam + Dot + Delete,
  Detail : GroupNameEquipmentTeam + Dot + Detail,
};

// 工程量
const GroupNameMaterial = GroupName + Dot + 'Material';
export const Material = {
  Default: GroupNameMaterial,
  Create : GroupNameMaterial + Dot + Create,
  Update : GroupNameMaterial + Dot + Update,
  Delete : GroupNameMaterial + Dot + Delete,
  Detail : GroupNameMaterial + Dot + Detail,
};
// 施工工序
const GroupNameProcedure = GroupName + Dot + 'Procedure';
export const Procedure = {
  Default: GroupNameProcedure,
  Create : GroupNameProcedure + Dot + Create,
  Update : GroupNameProcedure + Dot + Update,
  Delete : GroupNameProcedure + Dot + Delete,
  Detail : GroupNameProcedure + Dot + Detail,
};
// 分部分项
const GroupNameSubItem = GroupName + Dot + 'SubItem';
export const SubItem = {
  Default: GroupNameSubItem,
  Create : GroupNameSubItem + Dot + Create,
  Update : GroupNameSubItem + Dot + Update,
  Delete : GroupNameSubItem + Dot + Delete,
  Detail : GroupNameSubItem + Dot + Detail,
};
// 工序规范
const GroupNameStandard = GroupName + Dot + 'Standard';
export const Standard = {
  Default: GroupNameStandard,
  Create : GroupNameStandard + Dot + Create,
  Update : GroupNameStandard + Dot + Update,
  Delete : GroupNameStandard + Dot + Delete,
  Detail : GroupNameStandard + Dot + Detail,
};
// 施工区段
const GroupNameSection = GroupName + Dot + 'Section';
export const Section = {
  Default: GroupNameSection,
  Create : GroupNameSection + Dot + Create,
  Update : GroupNameSection + Dot + Update,
  Delete : GroupNameSection + Dot + Delete,
  Detail : GroupNameSection + Dot + Detail,
};


