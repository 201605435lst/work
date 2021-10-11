import Common from './common';
const { Create, Update, Delete, Dot, Detail, Export} = Common;

const GroupName = 'AbpReport';


const GroupNameReport = GroupName + Dot + 'Reports';
const Reports = {
  Default: GroupNameReport,
  Create: GroupNameReport + Dot + Create,
  Update: GroupNameReport + Dot + Update,
  Delete: GroupNameReport + Dot + Delete,
  Detail: GroupNameReport + Dot + Detail,
  Export: GroupNameReport + Dot + Export,
};
export default {
  GroupName,
  Reports,
};
