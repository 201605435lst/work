import Common from './common';
const { Create, Update, Delete, Dot} = Common;

const GroupName = 'AbpCostManagement';


const GroupNameCostPeople = GroupName + Dot + 'PeopleCosts';
const CostPeoples = {
  Default: GroupNameCostPeople,
  Create: GroupNameCostPeople + Dot + Create,
  Update: GroupNameCostPeople + Dot + Update,
  Delete: GroupNameCostPeople + Dot + Delete,
};

const GroupNameCostOther = GroupName + Dot + 'CostOthers';
const CostOthers = {
  Default: GroupNameCostOther,
  Create: GroupNameCostOther + Dot + Create,
  Update: GroupNameCostOther + Dot + Update,
  Delete: GroupNameCostOther + Dot + Delete,
};

const GroupNameMoneyList = GroupName + Dot + 'MoneyLists';
const MoneyLists = {
  Default: GroupNameMoneyList,
  Create: GroupNameMoneyList + Dot + Create,
  Update: GroupNameMoneyList + Dot + Update,
  Delete: GroupNameMoneyList + Dot + Delete,
};
const GroupNameContract = GroupName + Dot + 'Contracts';
const Contracts = {
  Default: GroupNameContract,
  Create: GroupNameContract + Dot + Create,
  Update: GroupNameContract + Dot + Update,
  Delete: GroupNameContract + Dot + Delete,
};
export default {
  GroupName,
  CostOthers,
  CostPeoples,
  MoneyLists,
  Contracts,
};
