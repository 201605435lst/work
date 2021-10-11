const SmConstructionMasterPlan = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionMasterPlan',
  subtitle: '总体计划',
  demos: [
    {
      path: 'sm-construction-master-plan',
      component: () =>
        import('@/components/sm-construction/sm-construction-master-plan/demo/index.vue'),
    },
    {
      path: 'sm-construction-master-plan-cn',
      component: () =>
        import('@/components/sm-construction/sm-construction-master-plan/demo/index.vue'),
    },
  ],
};
// const SmConstructionMasterPlanContent = {
//   category: 'Modules',
//   type: 'Construction',
//   title: 'SmConstructionMasterPlanContent',
//   subtitle: '总体计划详情',
//   demos: [
//     {
//       path: 'sm-construction-master-plan-content',
//       component: () =>
//         import('@/components/sm-construction/sm-construction-master-plan-content/demo/index.vue'),
//     },
//     {
//       path: 'sm-construction-master-plan-content-cn',
//       component: () =>
//         import('@/components/sm-construction/sm-construction-master-plan-content/demo/index.vue'),
//     },
//   ],
// };
const SmConstructionMasterPlanContentWithGantt = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionMasterPlanContentWithGantt',
  subtitle: '总体计划详情结合甘特图',
  demos: [
    { path: 'sm-construction-master-plan-content-with-gantt', component: () => import( '@/components/sm-construction/sm-construction-master-plan-content-with-gantt/demo/index.vue' ) },
    { path: 'sm-construction-master-plan-content-with-gantt-cn', component: () => import( '@/components/sm-construction/sm-construction-master-plan-content-with-gantt/demo/index.vue' ) },
  ],
};
const SmConstructionMasterPlanContentSelect = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionMasterPlanContentSelect',
  subtitle: '总体计划详情选择框',
  demos: [
    { path: 'sm-construction-master-plan-content-select', component: () => import( '../../components/sm-construction/sm-construction-master-plan-content-select/demo/index.vue' ) },
    { path: 'sm-construction-master-plan-content-select-cn', component: () => import( '../../components/sm-construction/sm-construction-master-plan-content-select/demo/index.vue' ) },
  ],
};
const SmConstructionPlan = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionPlan',
  subtitle: '施工计划',
  demos: [
    {
      path: 'sm-construction-plan',
      component: () => import('@/components/sm-construction/sm-construction-plan/demo/index.vue'),
    },
    {
      path: 'sm-construction-plan-cn',
      component: () => import('@/components/sm-construction/sm-construction-plan/demo/index.vue'),
    },
  ],
};
const SmConstructionDispatchSelect = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionDispatchSelect',
  subtitle: '派工单选择框',
  demos: [
    {
      path: 'sm-construction-dispatch-select',
      component: () => import('@/components/sm-construction/sm-construction-dispatch-select/demo/index.vue'),
    },
    {
      path: 'sm-construction-dispatch-select-cn',
      component: () => import('@/components/sm-construction/sm-construction-dispatch-select/demo/index.vue'),
    },
  ],
};
// const SmConstructionPlanContent = {
//   category: 'Modules',
//   type: 'Construction',
//   title: 'SmConstructionPlanContent',
//   subtitle: '施工计划详情',
//   demos: [
//     {
//       path: 'sm-construction-plan-content',
//       component: () =>
//         import('@/components/sm-construction/sm-construction-plan-content/demo/index.vue'),
//     },
//     {
//       path: 'sm-construction-plan-content-cn',
//       component: () =>
//         import('@/components/sm-construction/sm-construction-plan-content/demo/index.vue'),
//     },
//   ],
// };
const SmConstructionPlanWithGantt = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionPlanWithGantt',
  subtitle: '施工计划详情结合甘特图',
  demos: [
    {
      path: 'sm-construction-plan-with-gantt',
      component: () =>
        import('@/components/sm-construction/sm-construction-plan-with-gantt/demo/index.vue'),
    },
    {
      path: 'sm-construction-plan-with-gantt-cn',
      component: () =>
        import('@/components/sm-construction/sm-construction-plan-with-gantt/demo/index.vue'),
    },
  ],
};
const SmConstructionDispatchTemplate = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionDispatchTemplate',
  subtitle: '派工单模板管理',
  demos: [
    {
      path: 'sm-construction-dispatch-template',
      component: () =>
        import('@/components/sm-construction/sm-construction-dispatch-template/demo/index.vue'),
    },
    {
      path: 'sm-construction-dispatch-template-cn',
      component: () =>
        import('@/components/sm-construction/sm-construction-dispatch-template/demo/index.vue'),
    },
  ],
};

const SmConstructionPlanContentSelect = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionPlanContentSelect',
  subtitle: '施工计划详情选择框',
  demos: [
    { path: 'sm-construction-plan-content-select', component: () => import( '../../components/sm-construction/sm-construction-plan-content-select/demo/index.vue' ) },
    { path: 'sm-construction-plan-content-select-cn', component: () => import( '../../components/sm-construction/sm-construction-plan-content-select/demo/index.vue' ) },
  ],
};
const SmConstructionPlanContentSelectModal = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionPlanContentSelectModal',
  subtitle: '施工计划详情选择模态框',
  demos: [
    { path: 'sm-construction-plan-content-select-modal', component: () => import( '../../components/sm-construction/sm-construction-plan-content-select-modal/demo/index.vue' ) },
    { path: 'sm-construction-plan-content-select-modal-cn', component: () => import( '../../components/sm-construction/sm-construction-plan-content-select-modal/demo/index.vue' ) },
  ],
};
const SmConstructionDispatchs = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionDispatchs',
  subtitle: '派工管理',
  demos: [
    {
      path: 'sm-construction-dispatchs',
      component: () =>
        import('@/components/sm-construction/sm-construction-dispatchs/demo/index.vue'),
    },
    {
      path: 'sm-construction-dispatchs-cn',
      component: () =>
        import('@/components/sm-construction/sm-construction-dispatchs/demo/index.vue'),
    },
  ],
};

const SmConstructionDispatch = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionDispatch',
  subtitle: '派工管理单页',
  demos: [
    {
      path: 'sm-construction-dispatch',
      component: () =>
        import('@/components/sm-construction/sm-construction-dispatch/demo/index.vue'),
    },
    {
      path: 'sm-construction-dispatch-cn',
      component: () =>
        import('@/components/sm-construction/sm-construction-dispatch/demo/index.vue'),
    },
  ],
};

// const SmConstructionPlanMaterial = {
//   category: 'Modules',
//   type: 'Construction',
//   title: 'SmConstructionPlanMaterial',
//   subtitle: '施工计划工程量',
//   demos: [
//     {
//       path: 'sm-construction-plan-material',
//       component: () => import('../../components/sm-construction/sm-construction-plan-material/demo/index.vue'),
//     },
//     {
//       path: 'sm-construction-plan-material-cn',
//       component: () => import('../../components/sm-construction/sm-construction-plan-material/demo/index.vue'),
//     },
//   ],
// };
const SmConstructionPlanMaterial = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionPlanMaterial',
  subtitle: '施工计划工程量',
  demos: [
    {
      path: 'sm-construction-plan-material',
      component: () => import('../../components/sm-construction/sm-construction-plan-material/demo/index.vue'),
    },
    {
      path: 'sm-construction-plan-material-cn',
      component: () => import('../../components/sm-construction/sm-construction-plan-material/demo/index.vue'),
    },
  ],
};
const SmConstructionDaily = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionDaily',
  subtitle: '施工日志单页',
  demos: [
    {
      path: 'sm-construction-daily',
      component: () => import('../../components/sm-construction/sm-construction-daily/demo/index.vue'),
    },
    {
      path: 'sm-construction-daily-cn',
      component: () => import('../../components/sm-construction/sm-construction-daily/demo/index.vue'),
    },
  ],
};

const SmConstructionDailyTemplate = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionDailyTemplate',
  subtitle: '施工日志模板管理',
  demos: [
    {
      path: 'sm-construction-daily-template',
      component: () =>
        import('@/components/sm-construction/sm-construction-daily-template/demo/index.vue'),
    },
    {
      path: 'sm-construction-daily-template-cn',
      component: () =>
        import('@/components/sm-construction/sm-construction-daily-template/demo/index.vue'),
    },
  ],
};
const SmConstructionDailys = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionDailys',
  subtitle: '施工日志管理',
  demos: [
    {
      path: 'sm-construction-dailys',
      component: () => import('../../components/sm-construction/sm-construction-dailys/demo/index.vue'),
    },
    {
      path: 'sm-construction-dailys-cn',
      component: () => import('../../components/sm-construction/sm-construction-dailys/demo/index.vue'),
    },
  ],
};
const SmConstructionDailyView = {
  category: 'Modules',
  type: 'Construction',
  title: 'SmConstructionDailyView',
  subtitle: '施工日志详情',
  demos: [
    {
      path: 'sm-construction-daily-view',
      component: () => import('../../components/sm-construction/sm-construction-daily-view/demo/index.vue'),
    },
    {
      path: 'sm-construction-daily-view-cn',
      component: () => import('../../components/sm-construction/sm-construction-daily-view/demo/index.vue'),
    },
  ],
};

export const modules = {
  SmConstructionMasterPlan,
  SmConstructionMasterPlanContentSelect,
  // SmConstructionMasterPlanContent,
  SmConstructionMasterPlanContentWithGantt,
  SmConstructionPlan,
  // SmConstructionPlanContent,
  SmConstructionPlanWithGantt,
  SmConstructionPlanContentSelect,
  SmConstructionPlanContentSelectModal,
  // SmConstructionPlanMaterial,
  SmConstructionDispatch,
  SmConstructionDispatchs,
  SmConstructionDailys,
  SmConstructionDaily,
  SmConstructionDispatchTemplate,
  SmConstructionDispatchSelect,
  SmConstructionDailyView,
  SmConstructionDailyTemplate,
};
export const demo = [
  ...SmConstructionMasterPlan.demos,
  ...SmConstructionMasterPlanContentSelect.demos,
  // ...SmConstructionMasterPlanContent.demos,
  ...SmConstructionMasterPlanContentWithGantt.demos,
  ...SmConstructionPlan.demos,
  // ...SmConstructionPlanContent.demos,
  ...SmConstructionPlanWithGantt.demos,
  ...SmConstructionPlanContentSelect.demos,
  ...SmConstructionPlanContentSelectModal.demos,
  // ...SmConstructionPlanMaterial.demos,
  ...SmConstructionDispatch.demos,
  ...SmConstructionDispatchs.demos,
  ...SmConstructionDailys.demos,
  ...SmConstructionDaily.demos,
  ...SmConstructionDispatchTemplate.demos,
  ...SmConstructionDispatchSelect.demos,
  ...SmConstructionDailyView.demos,
  ...SmConstructionDailyTemplate.demos,
];
