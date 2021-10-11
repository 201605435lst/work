const SmDashboard = {
  category: 'Modules',
  type: 'Dashboard',
  title: 'SmDashboard',
  subtitle: '工作看板',
  demos: [
    {
      path: 'sm-dashboard',
      component: () => import('@/components/sm-dashboard/sm-dashboard/demo/index.vue'),
    },
    {
      path: 'sm-dashboard-cn',
      component: () => import('@/components/sm-dashboard/sm-dashboard/demo/index.vue'),
    },
  ],
};
const SmDashboardListCard = {
  category: 'Modules',
  type: 'Dashboard',
  title: 'SmDashboardListCard',
  subtitle: '面板模块',
  demos: [
    {
      path: 'sm-dashboard-list-card',
      component: () => import('@/components/sm-dashboard/sm-dashboard-list-card/demo/index.vue'),
    },
    {
      path: 'sm-dashboard-list-card-cn',
      component: () => import('@/components/sm-dashboard/sm-dashboard-list-card/demo/index.vue'),
    },
  ],
};

// const SmProblemProblems = {
//   category: 'Modules',
//   type: 'Problem',
//   title: 'SmProblemProblems',
//   subtitle: '问题管理',
//   demos: [
//     {
//       path: 'sm-problem-problems',
//       component: () => import('@/components/sm-problem/sm-problem-problems/demo/index.vue'),
//     },
//     {
//       path: 'sm-problem-problems-cn',
//       component: () => import('@/components/sm-problem/sm-problem-problems/demo/index.vue'),
//     },
//   ],
// };

export const modules = {
  SmDashboard,
  SmDashboardListCard,
};
export const demo = [...SmDashboard.demos, ...SmDashboardListCard.demos];
