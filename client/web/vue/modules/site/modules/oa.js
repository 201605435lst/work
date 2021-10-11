const SmOaDutySchedule = {
  category: 'Modules',
  type: 'Oa',
  title: 'SmOaDutySchedule',
  subtitle: '值班管理',
  demos: [
    {
      path: 'sm-oa-duty-schedule',
      component: () => import('@/components/sm-oa/sm-oa-duty-schedule/demo/index.vue'),
    },
    {
      path: 'sm-oa-duty-schedule-cn',
      component: () => import('@/components/sm-oa/sm-oa-duty-schedule/demo/index.vue'),
    },
  ],
};

const SmOaContract = {
  category: 'Modules',
  type: 'Oa',
  title: 'SmOaContract',
  subtitle: '合同管理单页',
  demos: [
    {
      path: 'sm-oa-contract',
      component: () => import('@/components/sm-oa/sm-oa-contract/demo/index.vue'),
    },
    {
      path: 'sm-oa-contract-cn',
      component: () => import('@/components/sm-oa/sm-oa-contract/demo/index.vue'),
    },
  ],
};
const SmOaContracts = {
  category: 'Modules',
  type: 'Oa',
  title: 'SmOaContracts',
  subtitle: '合同管理',
  demos: [
    {
      path: 'sm-oa-contracts',
      component: () => import('@/components/sm-oa/sm-oa-contracts/demo/index.vue'),
    },
    {
      path: 'sm-oa-contracts-cn',
      component: () => import('@/components/sm-oa/sm-oa-contracts/demo/index.vue'),
    },
  ],
};
const SmOaSeals = {
  category: 'Modules',
  type: 'Oa',
  title: 'SmOaSeals',
  subtitle: '签章管理',
  demos: [
    {
      path: 'sm-oa-seals',
      component: () => import('@/components/sm-oa/sm-oa-seals/demo/index.vue'),
    },
    {
      path: 'sm-oa-seals-cn',
      component: () => import('@/components/sm-oa/sm-oa-seals/demo/index.vue'),
    },
  ],
};
const SmOaSealsSelect = {
  category: 'Modules',
  type: 'Oa',
  title: 'SmOaSealsSelect',
  subtitle: '签章选择',
  demos: [
    {
      path: 'sm-oa-seals-select',
      component: () => import('@/components/sm-oa/sm-oa-seals-select/demo/index.vue'),
    },
    {
      path: 'sm-oa-seals-select-cn',
      component: () => import('@/components/sm-oa/sm-oa-seals-select/demo/index.vue'),
    },
  ],
};

export const modules = {
  SmOaDutySchedule,
  SmOaContract,
  SmOaContracts,
  SmOaSeals,
  SmOaSealsSelect,
};
export const demo = [
  ...SmOaDutySchedule.demos,
  ...SmOaContract.demos,
  ...SmOaContracts.demos,
  
  ...SmOaSeals.demos,
  ...SmOaSealsSelect.demos,
];
