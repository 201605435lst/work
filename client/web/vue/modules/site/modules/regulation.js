const SmRegulationInstitution = {
  category: 'Modules',
  type: 'Regulation',
  title: 'SmRegulationInstitution',
  subtitle: '全部制度',
  demos: [
    {
      path: 'sm-regulation-institution',
      component: () => import('@/components/sm-regulation_/sm-regulation-institution/demo/index.vue'),
    },
    {
      path: 'sm-regulation-institution-cn',
      component: () => import('@/components/sm-regulation_/sm-regulation-institution/demo/index.vue'),
    },
  ],
};

const SmRegulationLabelTreeSelect = {
  category: 'Modules',
  type: 'Regulation',
  title: 'SmRegulationLabelTreeSelect',
  subtitle: '标签选择',
  demos: [
    {
      path: 'sm-regulation-label-tree-select',
      component: () => import('@/components/sm-regulation_/sm-regulation-label-tree-select/demo/index.vue'),
    },
    {
      path: 'sm-regulation-label-tree-select-cn',
      component: () => import('@/components/sm-regulation_/sm-regulation-label-tree-select/demo/index.vue'),
    },
  ],
};

const SmRegulationViewInstitution = {
  category: 'Modules',
  type: 'Regulation',
  title: 'SmRegulationViewInstitution',
  subtitle: '查看制度',
  demos: [
    {
      path: 'sm-regulation-view-institution',
      component: () => import('@/components/sm-regulation_/sm-regulation-view-institution/demo/index.vue'),
    },
    {
      path: 'sm-regulation-view-institution-cn',
      component: () => import('@/components/sm-regulation_/sm-regulation-view-institution/demo/index.vue'),
    },
  ],
};

const SmRegulationAuditedInstitution = {
  category: 'Modules',
  type: 'Regulation',
  title: 'SmRegulationAuditedInstitution',
  subtitle: '制度审核',
  demos: [
    {
      path: 'sm-regulation-audited-institution',
      component: () => import('@/components/sm-regulation_/sm-regulation-audited-institution/demo/index.vue'),
    },
    {
      path: 'sm-regulation-audited-institution-cn',
      component: () => import('@/components/sm-regulation_/sm-regulation-audited-institution/demo/index.vue'),
    },
  ],
};

export const modules = {
  SmRegulationInstitution,
  SmRegulationLabelTreeSelect,
  SmRegulationViewInstitution,
  SmRegulationAuditedInstitution,
};
export const demo = [
  ...SmRegulationInstitution.demos,
  ...SmRegulationLabelTreeSelect.demos,
  ...SmRegulationViewInstitution.demos,
  ...SmRegulationAuditedInstitution.demos,
];
