import Common from './common';
const { Create, Update, Delete, Detail, Dot, Export, Import, Encrypt, Download, Apply } = Common;
const GroupName = 'AbpProject';

const GroupNameProjects = GroupName + Dot + 'Project';

const Projects = {
  Default: GroupNameProjects,
  Create: GroupNameProjects + Dot + Create,
  Update: GroupNameProjects + Dot + Update,
  Delete: GroupNameProjects + Dot + Delete,
  Detail: GroupNameProjects + Dot + Detail,
  Export: GroupNameProjects + Dot + Export,
};
const GroupNameArchives = GroupName + Dot + 'Archives';
const Archivess = {
  Default: GroupNameArchives,
  Create: GroupNameArchives + Dot + Create,
  Update: GroupNameArchives + Dot + Update,
  Delete: GroupNameArchives + Dot + Delete,
  Import: GroupNameArchives + Dot + Import,
  Apply: GroupNameArchives + Dot + Apply,
  Export: GroupNameArchives + Dot + Export,
};
const GroupNameArchivesCategory = GroupName + Dot + 'ArchivesCategory';

const ArchivesCategorys = {
  Default: GroupNameArchivesCategory,
  Create: GroupNameArchivesCategory + Dot + Create,
  Update: GroupNameArchivesCategory + Dot + Update,
  Delete: GroupNameArchivesCategory + Dot + Delete,
  Encrypt: GroupNameArchivesCategory + Dot + Encrypt,
};
const GroupNameDossier = GroupName + Dot + 'Dossier';
const Dossiers = {
  Default: GroupNameDossier,
  Create: GroupNameDossier + Dot + Create,
  Update: GroupNameDossier + Dot + Update,
  Delete: GroupNameDossier + Dot + Delete,
  Import: GroupNameDossier + Dot + Import,
  Detail: GroupNameDossier + Dot + Detail,
  Download: GroupNameDossier + Dot + Download,
  Export: GroupNameDossier + Dot + Export,
};
export default {
  GroupName,
  Projects,
  Archivess,
  Dossiers,
  ArchivesCategorys,
};
