import Common from './common';
const { Create, Update, Delete, Detail, Improve, Export, Import, Verify, Dot } = Common;

const GroupName = 'AbpQuality';

const GroupNameQualityProblems = GroupName + Dot + 'QualityProblems';
const QualityProblems = {
  Default: GroupNameQualityProblems,
  Create: GroupNameQualityProblems + Dot + Create,
  Improve: GroupNameQualityProblems + Dot + Improve,
  Update: GroupNameQualityProblems + Dot + Update,
  Verify: GroupNameQualityProblems + Dot + Verify,
  Delete: GroupNameQualityProblems + Dot + Delete,
  Detail: GroupNameQualityProblems + Dot + Detail,
  Export: GroupNameQualityProblems + Dot + Export,
  Position: GroupNameQualityProblems + Dot + 'Position',
};

const GroupNameQualityProblemLibraries = GroupName + Dot + 'QualityProblemLibraries';
const QualityProblemLibraries = {
  Default: GroupNameQualityProblemLibraries,
  Create: GroupNameQualityProblemLibraries + Dot + Create,
  Update: GroupNameQualityProblemLibraries + Dot + Update,
  Delete: GroupNameQualityProblemLibraries + Dot + Delete,
  Detail: GroupNameQualityProblemLibraries + Dot + Detail,
  Export: GroupNameQualityProblemLibraries + Dot + Export,
  Import: GroupNameQualityProblemLibraries + Dot + Import,
};

export default {
  GroupName,
  QualityProblems,
  QualityProblemLibraries,
};
