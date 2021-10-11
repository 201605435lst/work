import Common from './common';
const { Create, Reform, Sign, Detail, Dot,Update, Export,Delete, Import,Verify } = Common;
const GroupName = 'AbpSafe';

const GroupNameProblems = GroupName + Dot + 'Problems';
const SafeProblems = {
  Default: GroupNameProblems,
  Reform: GroupNameProblems + Dot + Reform,
  Sign: GroupNameProblems + Dot + Sign,
  Verify: GroupNameProblems + Dot + Verify,
  Detail: GroupNameProblems + Dot + Detail,
  Export: GroupNameProblems + Dot + Export,
  Update: GroupNameProblems + Dot + Update,
  Position: GroupNameProblems + Dot + 'Position',
};
const GroupNameProblemLibrarys = GroupName + Dot + 'ProblemLibrarys';
const SafeProblemLibrarys = {
  Default: GroupNameProblemLibrarys,
  Create: GroupNameProblemLibrarys + Dot + Create,
  Update: GroupNameProblemLibrarys + Dot + Update,
  Import: GroupNameProblemLibrarys + Dot + Import,
  Delete: GroupNameProblemLibrarys + Dot + Delete,
  Export: GroupNameProblemLibrarys + Dot + Export,
  Detail: GroupNameProblemLibrarys + Dot + Detail,
};
export default {   
  GroupName,
  SafeProblems,
  SafeProblemLibrarys,
};
