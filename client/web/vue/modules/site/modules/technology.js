const SmTechnologyDisclose = {
  category: 'Modules',
  type: 'Technology',
  title: 'SmTechnologyDisclose',
  subtitle: '技术交底',
  demos: [
    {
      path: 'sm-technology-disclose',
      component: () => import('@/components/sm-technology/sm-technology-disclose/demo/index.vue'),
    },
    {
      path: 'sm-technology-disclose-cn',
      component: () => import('@/components/sm-technology/sm-technology-disclose/demo/index.vue'),
    },
  ],
};
const SmTechnologyInterfaceFlag = {
  category: 'Modules',
  type: 'Technology',
  title: 'SmTechnologyInterfaceFlag',
  subtitle: '接口标记',
  demos: [
    {
      path: 'sm-technology-interface-flag',
      component: () => import('@/components/sm-technology/sm-technology-interface-flag/demo/index.vue'),
    },
    {
      path: 'sm-technology-interface-flag-cn',
      component: () => import('@/components/sm-technology/sm-technology-interface-flag/demo/index.vue'),
    },
  ],
};
const SmTechnologyInterfaceReport = {
  category: 'Modules',
  type: 'Technology',
  title: 'SmTechnologyInterfaceReport',
  subtitle: '接口报告',
  demos: [
    {
      path: 'sm-technology-interface-report',
      component: () => import('@/components/sm-technology/sm-technology-interface-report/demo/index.vue'),
    },
    {
      path: 'sm-technology-interface-report-cn',
      component: () => import('@/components/sm-technology/sm-technology-interface-report/demo/index.vue'),
    },
  ],
};
const SmTechnologyInterfaceListing = {
  category: 'Modules',
  type: 'Technology',
  title: 'SmTechnologyInterfaceListing',
  subtitle: '接口清单',
  demos: [
    {
      path: 'sm-technology-interface-listing',
      component: () => import('@/components/sm-technology/sm-technology-interface-listing/demo/index.vue'),
    },
    {
      path: 'sm-technology-interface-listing-cn',
      component: () => import('@/components/sm-technology/sm-technology-interface-listing/demo/index.vue'),
    },
  ],
};
const SmTechnologyQuantity = {
  category: 'Modules',
  type: 'Technology',
  title: 'SmTechnologyQuantity',
  subtitle: '工程量清单',
  demos: [
    {
      path: 'sm-technology-quantity',
      component: () => import('@/components/sm-technology/sm-technology-quantity/demo/index.vue'),
    },
    {
      path: 'sm-technology-quantity-cn',
      component: () => import('@/components/sm-technology/sm-technology-quantity/demo/index.vue'),
    },
  ],
};
const SmTechnologyMaterialPlan = {
  category: 'Modules',
  type: 'Technology',
  title: 'SmTechnologyMaterialPlan',
  subtitle: '用料计划',
  demos: [
    {
      path: 'sm-technology-material-plan',
      component: () => import('@/components/sm-technology/sm-technology-material-plan/demo/index.vue'),
    },
    {
      path: 'sm-technology-material-plan-cn',
      component: () => import('@/components/sm-technology/sm-technology-material-plan/demo/index.vue'),
    },
  ],
};
export const modules = {
  SmTechnologyDisclose,
  SmTechnologyInterfaceFlag,
  SmTechnologyInterfaceListing,
  SmTechnologyInterfaceReport,
  SmTechnologyQuantity,
  SmTechnologyMaterialPlan,
};
export const demo = [
  ...SmTechnologyDisclose.demos,
  ...SmTechnologyInterfaceFlag.demos,
  ...SmTechnologyInterfaceListing.demos,
  ...SmTechnologyInterfaceReport.demos,
  ...SmTechnologyQuantity.demos,
  ...SmTechnologyMaterialPlan.demos,

];
