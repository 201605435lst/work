const SmCostmanagementPeopleCost = {
  category: 'Modules',
  type: 'Costmanagement',
  title: 'SmCostmanagementPeopleCost',
  subtitle: '人工成本',
  demos: [
    {
      path: 'sm-costmanagement-people-cost',
      component: () =>
        import('@/components/sm-costmanagement/sm-costmanagement-people-cost/demo/index.vue'),
    },
    {
      path: 'sm-costmanagement-people-cost-cn',
      component: () =>
        import('@/components/sm-costmanagement/sm-costmanagement-people-cost/demo/index.vue'),
    },
  ],
};
const SmCostmanagementOtherCost = {
  category: 'Modules',
  type: 'Costmanagement',
  title: 'SmCostmanagementOtherCost',
  subtitle: '其他成本',
  demos: [
    {
      path: 'sm-costmanagement-other-cost',
      component: () =>
        import('@/components/sm-costmanagement/sm-costmanagement-other-cost/demo/index.vue'),
    },
    {
      path: 'sm-costmanagement-other-cost-cn',
      component: () =>
        import('@/components/sm-costmanagement/sm-costmanagement-other-cost/demo/index.vue'),
    },
  ],
};
const SmCostmanagementMoneyList = {
  category: 'Modules',
  type: 'Costmanagement',
  title: 'SmCostmanagementMoneyList',
  subtitle: '资金列表',
  demos: [
    {
      path: 'sm-costmanagement-money-list',
      component: () =>
        import('@/components/sm-costmanagement/sm-costmanagement-money-list/demo/index.vue'),
    },
    {
      path: 'sm-costmanagement-money-list-cn',
      component: () =>
        import('@/components/sm-costmanagement/sm-costmanagement-money-list/demo/index.vue'),
    },
  ],
};
const SmCostmanagementCapitalReport = {
  category: 'Modules',
  type: 'Costmanagement',
  title: 'SmCostmanagementCapitalReport',
  subtitle: '资金报表',
  demos: [
    {
      path: 'sm-costmanagement-capital-report',
      component: () =>
        import('@/components/sm-costmanagement/sm-costmanagement-capital-report/demo/index.vue'),
    },
    {
      path: 'sm-costmanagement-capital-report-cn',
      component: () =>
        import('@/components/sm-costmanagement/sm-costmanagement-capital-report/demo/index.vue'),
    },
  ],
};
const SmCostmanagementContract = {
  category: 'Modules',
  type: 'Costmanagement',
  title: 'SmCostmanagementContract',
  subtitle: '合同管理',
  demos: [
    {
      path: 'sm-costmanagement-contract',
      component: () =>
        import('@/components/sm-costmanagement/sm-costmanagement-contract/demo/index.vue'),
    },
    {
      path: 'sm-costmanagement-contract-cn',
      component: () =>
        import('@/components/sm-costmanagement/sm-costmanagement-contract/demo/index.vue'),
    },
  ],
};
const SmCostmanagementBreakevenAnalysis = {
  category: 'Modules',
  type: 'Costmanagement',
  title: 'SmCostmanagementBreakevenAnalysis',
  subtitle: '成本盈亏分析',
  demos: [
    {
      path: 'sm-costmanagement-breakeven-analysis',
      component: () =>
        import('@/components/sm-costmanagement/sm-costmanagement-breakeven-analysis/demo/index.vue'),
    },
    {
      path: 'sm-costmanagement-breakeven-analysis-cn',
      component: () =>
        import('@/components/sm-costmanagement/sm-costmanagement-breakeven-analysis/demo/index.vue'),
    },
  ],
};
export const modules = {
  SmCostmanagementPeopleCost,
  SmCostmanagementOtherCost,
  SmCostmanagementMoneyList,
  SmCostmanagementCapitalReport,
  SmCostmanagementContract,
  SmCostmanagementBreakevenAnalysis,

};
export const demo = [
  ...SmCostmanagementMoneyList.demos,
  ...SmCostmanagementPeopleCost.demos,
  ...SmCostmanagementOtherCost.demos,
  ...SmCostmanagementCapitalReport.demos,
  ...SmCostmanagementContract.demos,
  ...SmCostmanagementBreakevenAnalysis.demos,
];
