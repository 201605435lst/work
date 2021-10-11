import Common from './common';
const { Create, Update, Delete, Detail, Dot } = Common;

const GroupName = 'AbpDashboard';

const GroupNameDashboard = GroupName + Dot + 'Dashboard';
const Dashboard = {
  Default: GroupNameDashboard,
  Create: GroupNameDashboard + Dot + Create,
  Update: GroupNameDashboard + Dot + Update,
  Detail: GroupNameDashboard + Dot + Detail,
  Delete: GroupNameDashboard + Dot + Delete,
};

export default {
  Dashboard,
};
