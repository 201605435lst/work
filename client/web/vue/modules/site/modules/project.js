const SmProjectProject = {
  category: 'Modules',
  type: 'Project',
  title: 'SmProjectProject',
  subtitle: '项目信息管理单页',
  demos: [
    {
      path: 'sm-project-project',
      component: () => import('@/components/sm-project/sm-project-project/demo/index.vue'),
    },
    {
      path: 'sm-project-project-cn',
      component: () => import('@/components/sm-project/sm-project-project/demo/index.vue'),
    },
  ],
};
const SmProjectProjects = {
  category: 'Modules',
  type: 'Project',
  title: 'SmProjectProjects',
  subtitle: '项目信息管理',
  demos: [
    {
      path: 'sm-project-projects',
      component: () => import('@/components/sm-project/sm-project-projects/demo/index.vue'),
    },
    {
      path: 'sm-project-projects-cn',
      component: () => import('@/components/sm-project/sm-project-projects/demo/index.vue'),
    },
  ],
};
const SmProjectActionTreeSelect = {
  category: 'Modules',
  type: 'Project',
  title: 'SmProjectActionTreeSelect',
  subtitle: '动态添加树',
  demos: [
    {
      path: 'sm-project-action-tree-select',
      component: () =>
        import('@/components/sm-project/sm-project-action-tree-select/demo/index.vue'),
    },
    {
      path: 'sm-project-action-tree-select-cn',
      component: () =>
        import('@/components/sm-project/sm-project-action-tree-select/demo/index.vue'),
    },
  ],
};
const SmProjectArchives = {
  category: 'Modules',
  type: 'Project',
  title: 'SmProjectArchives',
  subtitle: '档案管理',
  demos: [
    {
      path: 'sm-project-archives',
      component: () => import('@/components/sm-project/sm-project-archives/demo/index.vue'),
    },
    {
      path: 'sm-project-archives-cn',
      component: () => import('@/components/sm-project/sm-project-archives/demo/index.vue'),
    },
  ],
};
const SmProjectDossier = {
  category: 'Modules',
  type: 'Project',
  title: 'SmProjectDossier',
  subtitle: '卷宗管理',
  demos: [
    {
      path: 'sm-project-dossier',
      component: () => import('@/components/sm-project/sm-project-dossier/demo/index.vue'),
    },
    {
      path: 'sm-project-dossier-cn',
      component: () => import('@/components/sm-project/sm-project-dossier/demo/index.vue'),
    },
  ],
};
const SmProjectDossierCatrgotyTreeSelect = {
  category: 'Modules',
  type: 'Project',
  title: 'SmProjectDossierCatrgotyTreeSelect',
  subtitle: '卷宗管理选择树',
  demos: [
    {
      path: 'sm-project-dossier-catrgoty-tree-select',
      component: () =>
        import('@/components/sm-project/sm-project-dossier-catrgoty-tree-select/demo/index.vue'),
    },
    {
      path: 'sm-project-dossier-catrgoty-tree-select-cn',
      component: () =>
        import('@/components/sm-project/sm-project-dossier-catrgoty-tree-select/demo/index.vue'),
    },
  ],
};
const SmProjectArchivesCatrgotyTreeSelect = {
  category: 'Modules',
  type: 'Project',
  title: 'SmProjectArchivesCatrgotyTreeSelect',
  subtitle: '档案管理选择树',
  demos: [
    {
      path: 'sm-project-archives-catrgoty-tree-select',
      component: () =>
        import('@/components/sm-project/sm-project-archives-catrgoty-tree-select/demo/index.vue'),
    },
    {
      path: 'sm-project-archives-catrgoty-tree-select-cn',
      component: () =>
        import('@/components/sm-project/sm-project-archives-catrgoty-tree-select/demo/index.vue'),
    },
  ],
};
const SmProjectUploadModal = {
  category: 'Modules',
  type: 'Project',
  title: 'SmProjectUploadModal',
  subtitle: '导入模板选择',
  demos: [
    {
      path: 'sm-project-upload-modal',
      component: () => import('@/components/sm-project/sm-project-upload-modal/demo/index.vue'),
    },
    {
      path: 'sm-project-upload-modal-cn',
      component: () => import('@/components/sm-project/sm-project-upload-modal/demo/index.vue'),
    },
  ],
};
export const modules = {
  SmProjectUploadModal,
  SmProjectProject,
  SmProjectProjects,
  SmProjectArchives,
  SmProjectDossier,
  SmProjectDossierCatrgotyTreeSelect,
  SmProjectArchivesCatrgotyTreeSelect,
  SmProjectActionTreeSelect,
};
export const demo = [
  ...SmProjectProject.demos,
  ...SmProjectProjects.demos,
  ...SmProjectArchives.demos,
  ...SmProjectDossier.demos,
  ...SmProjectDossierCatrgotyTreeSelect.demos,
  ...SmProjectArchivesCatrgotyTreeSelect.demos,
  ...SmProjectActionTreeSelect.demos,
  ...SmProjectUploadModal.demos,
];
