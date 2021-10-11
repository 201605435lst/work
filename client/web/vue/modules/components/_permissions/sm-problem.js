import Common from './common';
const { Create, Update, Delete, Detail, Dot } = Common;

const GroupName = 'AbpProblem';

const GroupNameProblems = GroupName + Dot + 'Problems';
const Problems = {
  Default: GroupNameProblems,
  Create: GroupNameProblems + Dot + Create,
  Update: GroupNameProblems + Dot + Update,
  Detail: GroupNameProblems + Dot + Detail,
  Delete: GroupNameProblems + Dot + Delete,
};

const GroupNameProblemCategories = GroupName + Dot + 'ProblemCategories';
const ProblemCategories = {
  Default: GroupNameProblemCategories,
  Create: GroupNameProblemCategories + Dot + Create,
  Update: GroupNameProblemCategories + Dot + Update,
  Detail: GroupNameProblemCategories + Dot + Detail,
  Delete: GroupNameProblemCategories + Dot + Delete,
};

export default {
  GroupName,
  Problems,
  ProblemCategories,
};
