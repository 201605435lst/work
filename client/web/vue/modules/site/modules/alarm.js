const SmAlarms = {
  category: 'Modules',
  type: 'Alarm',
  title: 'SmAlarms',
  subtitle: '集中告警',
  demos: [
    {
      path: 'sm-alarms',
      component: () => import('@/components/sm-alarm/sm-alarms/demo/index.vue'),
    },
    {
      path: 'sm-alarms-cn',
      component: () => import('@/components/sm-alarm/sm-alarms/demo/index.vue'),
    },
  ],
};

const SmD3Alarms = {
  category: 'Modules',
  type: 'Alarm',
  title: 'SmD3Alarms',
  subtitle: '集中告警',
  demos: [
    {
      path: 'sm-d3-alarms',
      component: () => import('@/components/sm-d3/sm-d3-alarms/demo/index.vue'),
    },
    {
      path: 'sm-d3-alarms-cn',
      component: () => import('@/components/sm-d3/sm-d3-alarms/demo/index.vue'),
    },
  ],
};

export const modules = {
  SmAlarms,
  SmD3Alarms,
};

export const demo = [...SmAlarms.demos, ...SmD3Alarms.demos];
