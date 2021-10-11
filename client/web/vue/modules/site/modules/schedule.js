const SmScheduleSchedules = {
  category: 'Modules',
  type: 'Schedule',
  title: 'SmScheduleSchedules',
  subtitle: '施工计划管理',
  demos: [
    {
      path: 'sm-schedule-schedules',
      component: () => import('@/components/sm-schedule/sm-schedule-schedules/demo/index.vue'),
    },
    {
      path: 'sm-schedule-schedules-cn',
      component: () => import('@/components/sm-schedule/sm-schedule-schedules/demo/index.vue'),
    },
  ],
};
const SmScheduleSchedule = {
  category: 'Modules',
  type: 'Schedule',
  title: 'SmScheduleSchedule',
  subtitle: '施工计划管理单页',
  demos: [
    {
      path: 'sm-schedule-schedule',
      component: () => import('@/components/sm-schedule/sm-schedule-schedule/demo/index.vue'),
    },
    {
      path: 'sm-schedule-schedule-cn',
      component: () => import('@/components/sm-schedule/sm-schedule-schedule/demo/index.vue'),
    },
  ],
};

const SmScheduleSchedulesSelect = {
  category: 'Modules',
  type: 'Schedule',
  title: 'SmScheduleSchedulesSelect',
  subtitle: '施工计划选择框',
  demos: [
    {
      path: 'sm-schedule-schedules-select',
      component: () => import('@/components/sm-schedule/sm-schedule-schedules-select/demo/index.vue'),
    },
    {
      path: 'sm-schedule-schedules-select-cn',
      component: () => import('@/components/sm-schedule/sm-schedule-schedules-select/demo/index.vue'),
    },
  ],
};

const SmScheduleApprovals = {
  category: 'Modules',
  type: 'Schedule',
  title: 'SmScheduleApprovals',
  subtitle: '施工审批管理',
  demos: [
    {
      path: 'sm-schedule-approvals',
      component: () => import('@/components/sm-schedule/sm-schedule-approvals/demo/index.vue'),
    },
    {
      path: 'sm-schedule-approvals-cn',
      component: () => import('@/components/sm-schedule/sm-schedule-approvals/demo/index.vue'),
    },
  ],
};

const SmScheduleApproval = {
  category: 'Modules',
  type: 'Schedule',
  title: 'SmScheduleApproval',
  subtitle: '施工审批单页',
  demos: [
    {
      path: 'sm-schedule-approval',
      component: () => import('@/components/sm-schedule/sm-schedule-approval/demo/index.vue'),
    },
    {
      path: 'sm-schedule-approval-cn',
      component: () => import('@/components/sm-schedule/sm-schedule-approval/demo/index.vue'),
    },
  ],
};

const SmScheduleApprovalTable = {
  category: 'Modules',
  type: 'Schedule',
  title: 'SmScheduleApprovalTable',
  subtitle: '施工审批单单页',
  demos: [
    {
      path: 'sm-schedule-approval-table',
      component: () => import('@/components/sm-schedule/sm-schedule-approval-table/demo/index.vue'),
    },
    {
      path: 'sm-schedule-approval-table-cn',
      component: () => import('@/components/sm-schedule/sm-schedule-approval-table/demo/index.vue'),
    },
  ],
};
const SmScheduleDiarys = {
  category: 'Modules',
  type: 'Schedule',
  title: 'SmScheduleDiarys',
  subtitle: '施工日志',
  demos: [
    {
      path: 'sm-schedule-diarys',
      component: () => import('@/components/sm-schedule/sm-schedule-diarys/demo/index.vue'),
    },
    {
      path: 'sm-schedule-diarys-cn',
      component: () => import('@/components/sm-schedule/sm-schedule-diarys/demo/index.vue'),
    },
  ],
};
const SmScheduleDiary = {
  category: 'Modules',
  type: 'Schedule',
  title: 'SmScheduleDiary',
  subtitle: '施工日志填报单页',
  demos: [
    {
      path: 'sm-schedule-diary',
      component: () => import('@/components/sm-schedule/sm-schedule-diary/demo/index.vue'),
    },
    {
      path: 'sm-schedule-diary-cn',
      component: () => import('@/components/sm-schedule/sm-schedule-diary/demo/index.vue'),
    },
  ],
};
const SmScheduleDiaryLog = {
  category: 'Modules',
  type: 'Schedule',
  title: 'SmScheduleDiaryLog',
  subtitle: '施工日志查看',
  demos: [
    {
      path: 'sm-schedule-diary-log',
      component: () => import('@/components/sm-schedule/sm-schedule-diary-log/demo/index.vue'),
    },
    {
      path: 'sm-schedule-diary-log-cn',
      component: () => import('@/components/sm-schedule/sm-schedule-diary-log/demo/index.vue'),
    },
  ],
};
const SmScheduleDiarysStatistics = {
  category: 'Modules',
  type: 'Schedule',
  title: 'SmScheduleDiarysStatistics',
  subtitle: '施工日志统计',
  demos: [
    {
      path: 'sm-schedule-diarys-statistics',
      component: () => import('@/components/sm-schedule/sm-schedule-diarys-statistics/demo/index.vue'),
    },
    {
      path: 'sm-schedule-diarys-statistics-cn',
      component: () => import('@/components/sm-schedule/sm-schedule-diarys-statistics/demo/index.vue'),
    },
  ],
};
export const modules = {
  SmScheduleSchedules,
  SmScheduleSchedule,
  SmScheduleSchedulesSelect,
  SmScheduleApprovals,
  SmScheduleApproval,
  SmScheduleDiarys,
  SmScheduleDiary,
  SmScheduleDiaryLog,
  SmScheduleDiarysStatistics,
  SmScheduleApprovalTable,
};
export const demo = [
  ...SmScheduleSchedules.demos,
  ...SmScheduleSchedule.demos,
  ...SmScheduleDiarys.demos,
  ...SmScheduleDiary.demos,
  ...SmScheduleSchedulesSelect.demos,
  ...SmScheduleApprovals.demos,
  ...SmScheduleApproval.demos,
  ...SmScheduleApprovalTable.demos,
  ...SmScheduleDiaryLog.demos,
  ...SmScheduleDiarysStatistics.demos,

];