const ScTest = {
  category: 'Modules',
  type: 'Components',
  title: 'ScTest',
  subtitle: '数据搜索框',
  demos: [
    {
      path: 'sc-test',
      component: () => import('@/components/sn-components/sc-test/demo/index.vue'),
    },
    {
      path: 'sc-test-cn',
      component: () => import('@/components/sn-components/sc-test/demo/index.vue'),
    },
  ],
};

const SnMapAmap = {
  category: 'Modules',
  type: 'Components',
  title: 'SnMapAmap',
  subtitle: '高德地图',
  demos: [
    {
      path: 'sn-map-amap',
      component: () => import('@/components/sn-components/sn-map-amap/demo/index.vue'),
    },
    {
      path: 'sn-map-amap-cn',
      component: () => import('@/components/sn-components/sn-map-amap/demo/index.vue'),
    },
  ],
};
const ScGantt = {
  category: 'Modules',
  type: 'Components',
  title: 'ScGantt',
  subtitle: '甘特图',
  demos: [
    { path: 'sc-gantt', component: () => import('@/components/sn-components/sc-gantt/demo/index.vue') },
    { path: 'sc-gantt-cn', component: () => import('@/components/sn-components/sc-gantt/demo/index.vue') },
  ],
};
const ScPlayerBar = {
  category: 'Modules',
  type: 'Components',
  title: 'ScPlayerBar',
  subtitle: '播放条',
  demos: [
    { path: 'sc-player-bar', component: () => import('../../components/sn-components/sc-player-bar/demo/index.vue') },
    { path: 'sc-player-bar-cn', component: () => import('../../components/sn-components/sc-player-bar/demo/index.vue') },
  ],
};

export const modules = {
  ScTest,
  SnMapAmap,
  ScGantt,
  ScPlayerBar,
};

export const demo = [
  ...ScTest.demos,
  ...SnMapAmap.demos,
  ...ScGantt.demos,
  ...ScPlayerBar.demos,
];
