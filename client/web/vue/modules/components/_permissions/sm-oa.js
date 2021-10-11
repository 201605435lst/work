import Common from './common';
const { Create, Update, Delete, Dot, Detail, Apply, Export, Lock, Efficiency, RestPSW} = Common;

const GroupName = 'AbpOa';

const GroupNameDutySchedule = GroupName + Dot + 'DutySchedule';
const DutySchedule = {
  Default: GroupNameDutySchedule,
  Create: GroupNameDutySchedule + Dot + Create,
  Update: GroupNameDutySchedule + Dot + Update,
  Delete: GroupNameDutySchedule + Dot + Delete,
  Detail: GroupNameDutySchedule + Dot + Detail,
};
const GroupNameContracts = GroupName + Dot + 'Contracts';
const Contracts = {
  Default: GroupNameContracts,
  Create: GroupNameContracts + Dot + Create,
  Update: GroupNameContracts + Dot + Update,
  Delete: GroupNameContracts + Dot + Delete,
  Detail: GroupNameContracts + Dot + Detail,
  Apply: GroupNameContracts + Dot + Apply,
  Export: GroupNameContracts + Dot + Export,
};
const GroupNameSeals = GroupName + Dot + 'Seals';
const Seals = {
  Default: GroupNameSeals,
  Create: GroupNameSeals + Dot + Create,
  Delete: GroupNameSeals + Dot + Delete,
  Lock: GroupNameSeals + Dot + Lock,
  Efficiency: GroupNameSeals + Dot + Efficiency,
  RestPSW: GroupNameSeals + Dot + RestPSW,
};
export default {
  GroupName,
  DutySchedule,
  Contracts,
  Seals,
};
