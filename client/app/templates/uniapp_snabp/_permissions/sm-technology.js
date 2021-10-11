import Common from './common';
const { Create, Reform, Sign, Detail, Dot, Export, Import ,Update,Delete} = Common;
const GroupName = 'AbpTechnology';
const GroupNameConstructInterfaces = GroupName + Dot + 'ConstructInterfaces';

const ConstructInterfaces = {
  Default: GroupNameConstructInterfaces,
  Reform: GroupNameConstructInterfaces + Dot + Reform,
  Sign: GroupNameConstructInterfaces + Dot + Sign,
  Import: GroupNameConstructInterfaces + Dot + Import,
  Detail: GroupNameConstructInterfaces + Dot + Detail,
  Export: GroupNameConstructInterfaces + Dot + Export,
};
const GroupNameComponentRltQRCodes = GroupName + Dot + 'ComponentRltQRCodes';
const ComponentRltQRCodes = {
  Default: GroupNameComponentRltQRCodes,
  ExportCode: GroupNameComponentRltQRCodes + Dot + 'ExportCode',
  GenerateCode: GroupNameComponentRltQRCodes + Dot + 'GenerateCode',
  CodeDetail: GroupNameComponentRltQRCodes + Dot + 'CodeDetail',
};

const GroupNameMaterialPlans = GroupName + Dot + 'MaterialPlans';
const MaterialPlans = {
  Default: GroupNameMaterialPlans,
  Create: GroupNameMaterialPlans + Dot + Create,
  Update: GroupNameMaterialPlans + Dot + Update,
  Delete: GroupNameMaterialPlans + Dot + Delete,
  Detail: GroupNameMaterialPlans + Dot + Detail,
  Export: GroupNameMaterialPlans + Dot + Export,
  Approval: GroupNameMaterialPlans + Dot + 'Approval',
  Submit: GroupNameMaterialPlans + Dot + 'Submit',
};

const GroupNameQuanitys = GroupName + Dot + 'Quanitys';
const Quanitys = {
  Default: GroupNameQuanitys,
  Statistic: GroupNameQuanitys + Dot + 'Statistic',
  GenerateMaterialPlan: GroupNameQuanitys + Dot + 'GenerateMaterialPlan',
  Delete: GroupNameQuanitys + Dot + Delete,
  Export: GroupNameQuanitys + Dot + Export,
};

const GroupNameDiscloses = GroupName + Dot + 'Discloses';
const Discloses = {
  Default: GroupNameDiscloses,
  Update: GroupNameDiscloses + Dot + Update,
  Delete: GroupNameDiscloses + Dot + Delete,
  Detail: GroupNameDiscloses + Dot + Detail,
  Export: GroupNameDiscloses + Dot + Export,
  Upload: GroupNameDiscloses + Dot + 'Upload',
};


export default {
  GroupName,
  ConstructInterfaces,
  ComponentRltQRCodes,
  MaterialPlans,
  Quanitys,
  Discloses,
};
