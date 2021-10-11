const SmTaskTasks = {
  category: 'Modules',
  type: 'Task',
  title: 'SmTaskTasks',
  subtitle: '任务管理',
  demos: [
    {
      path: 'sm-task-tasks',
      component: () => import('@/components/sm-task/sm-task-tasks/demo/index.vue'),
    },
    {
      path: 'sm-task-tasks-cn',
      component: () => import('@/components/sm-task/sm-task-tasks/demo/index.vue'),
    },
  ],
};
const SmTaskTask = {
  category: 'Modules',
  type: 'Task',
  title: 'SmTaskTask',
  subtitle: '任务管理单页',
  demos: [
    {
      path: 'sm-task-task',
      component: () => import('@/components/sm-task/sm-task-task/demo/index.vue'),
    },
    {
      path: 'sm-task-task-cn',
      component: () => import('@/components/sm-task/sm-task-task/demo/index.vue'),
    },
  ],
};
const SmTaskCalendars = {
  category: 'Modules',
  type: 'Task',
  title: 'SmTaskCalendars',
  subtitle: '任务日历',
  demos: [
    {
      path: 'sm-task-calendars',
      component: () => import('@/components/sm-task/sm-task-calendars/demo/index.vue'),
    },
    {
      path: 'sm-task-calendars-cn',
      component: () => import('@/components/sm-task/sm-task-calendars/demo/index.vue'),
    },
  ],
};
const SmTaskCalendar = {
  category: 'Modules',
  type: 'Task',
  title: 'SmTaskCalendar',
  subtitle: '任务日历单页',
  demos: [
    {
      path: 'sm-task-calendar',
      component: () => import('@/components/sm-task/sm-task-calendar/demo/index.vue'),
    },
    {
      path: 'sm-task-calendar-cn',
      component: () => import('@/components/sm-task/sm-task-calendar/demo/index.vue'),
    },
  ],
};
export const modules = {
  SmTaskTasks,
  SmTaskTask,
  SmTaskCalendar,
  SmTaskCalendars,
};
export const demo = [
  ...SmTaskTasks.demos,
  ...SmTaskTask.demos,
  ...SmTaskCalendar.demos,
  ...SmTaskCalendars.demos,
];