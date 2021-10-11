import { PageView, BlankLayout } from '@/layouts';
import { PageState, RepairTags } from '@/common/enums';
import permissionCrPlan from 'snweb-module/es/_permissions/sm-cr-plan';
import { getGroupWithRepairTag, repairTagKeyConfigs } from './_util';

let routes = [
  {
    path: 'dashboard',
    name: 'dashboard',
    component: () => import('@/views/cr-statistics/Dashboard'),
    meta: {
      title: '统计图表',
      keepAlive: false,
      permission: [
        // permissionCrPlan.StatisticalCharts,
      ],
    },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },
  {
    path: 'planState',
    name: 'planState',
    component: () => import('@/views/cr-statistics/PlanState'),
    meta: {
      title: '完成情况',
      keepAlive: false,
      permission: [
        // permissionCrPlan.PlanCompletion,
      ],
    },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },
  {
    path: 'planTrack/:organizationId/:planTime',
    name: 'planTrack',
    component: () => import('@/views/cr-statistics/PlanTrack'),
    meta: {
      title: '计划追踪',
      keepAlive: false,
      permission: [
        // permissionCrPlan.PlanStateTracking,
      ],
    },
    props: (route, repairTagKey) => {
      return {
        organizationId: route.params.organizationId,
        planTime: route.params.planTime,
        planType: route.params.planType,
        number: route.params.number,
        repairTagKey,
      };
    },
  },
];

export default {
  path: 'cr-statistics',
  name: 'cr-statistics',
  component: PageView,
  meta: {
    title: '智能报表',
    keepAlive: false,
    icon: 'bar-chart',
    permission: [
      // permissionCrPlan.CrPlanStatisticsGroupName,
      // permissionCrPlan.StatisticalCharts,
      // permissionCrPlan.PlanCompletion,
      // permissionCrPlan.PlanStateTracking,
    ],
  },

  children: [
    ...getGroupWithRepairTag(routes),
    ...repairTagKeyConfigs.map(item => {
      return {
        path: item.routePrefix,
        name: item.routePrefix,
        component: BlankLayout,
        meta: { ...item.routeMeta },
        children: getGroupWithRepairTag(routes, item.tag, item.routePrefix),
      };
    }),
  ],
};
