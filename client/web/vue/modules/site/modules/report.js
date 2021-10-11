const SmReport = {
  category: 'Modules',
  type: 'Report',
  title: 'SmReport',
  subtitle: '工作汇报单页',
  demos: [
    {
      path: 'sm-report',
      component: () => import('@/components/sm-report/sm-report/demo/index.vue'),
    },
    {
      path: 'sm-report-cn',
      component: () => import('@/components/sm-report/sm-report/demo/index.vue'),
    },
  ],
};
const SmReports = {
  category: 'Modules',
  type: 'Report',
  title: 'SmReports',
  subtitle: '工作汇报',
  demos: [
    {
      path: 'sm-reports',
      component: () => import('@/components/sm-report/sm-reports/demo/index.vue'),
    },
    {
      path: 'sm-reports-cn',
      component: () => import('@/components/sm-report/sm-reports/demo/index.vue'),
    },
  ],
};

export const modules = {
   
  SmReport,
  SmReports,
  
};
export const demo = [
   
  ...SmReport.demos,
  ...SmReports.demos,
   
];
  