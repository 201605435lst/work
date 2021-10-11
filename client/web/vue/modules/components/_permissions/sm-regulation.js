import Common from './common';
const { Create, Update, Delete, Dot, Detail, Export, Import, Authoriry, Audit } = Common;

const GroupName = 'AbpRegulation';

const GroupNameInstitutions = GroupName + Dot + 'Institutions';
const Institutions = {
  Default: GroupNameInstitutions,
  Create: GroupNameInstitutions + Dot + Create,
  Update: GroupNameInstitutions + Dot + Update,
  Delete: GroupNameInstitutions + Dot + Delete,
  Detail: GroupNameInstitutions + Dot + Detail,
  Import: GroupNameInstitutions + Dot + Import,
  Export: GroupNameInstitutions + Dot + Export,
  Authoriry: GroupNameInstitutions + Dot + Authoriry,
  Audit: GroupNameInstitutions + Dot + Audit,
};

const GroupNameLabels = GroupName + Dot + 'Labels';
const Labels = {
  Default: GroupNameLabels,
  Create: GroupNameLabels + Dot + Create,
  Update: GroupNameLabels + Dot + Update,
  Delete: GroupNameLabels + Dot + Delete,
  Detail: GroupNameLabels + Dot + Detail,
  Import: GroupNameLabels + Dot + Import,
  Export: GroupNameLabels + Dot + Export,
};
export default {
  GroupName,
  Institutions,
  Labels,
};
