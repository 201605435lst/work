import { PageView, BlankLayout } from '@/layouts';
import permissionCrPlan from 'snweb-module/es/_permissions/sm-cr-plan';
import { PageState, RepairTags } from 'snweb-module/es/_utils/enum';
import { getGroupWithRepairTag, repairTagKeyConfigs } from './_util';

let routes = [
  {
    path: 'maintenance-work',
    name: 'maintenance-work',
    component: () => import('@/views/cr-plan/MaintenanceWork'),
    meta: { title: '维修作业', keepAlive: false, permission: [permissionCrPlan.MaintenanceWork.Default] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },
  {
    path: 'maintenance-work-check',
    name: 'maintenance-work-check',
    component: () => import('@/views/cr-plan/MaintenanceWorkCheck'),
    meta: { title: '维修作业复验', keepAlive: false, permission: [permissionCrPlan.MaintenanceWorkChcek.Default] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },
  {
    path: 'vertical-skylight-plans',
    name: 'vertical-skylight-plans',
    component: () => import('@/views/cr-plan/VerticalSkylightPlans'),
    meta: { title: '垂直天窗计划', keepAlive: false, permission: [permissionCrPlan.SkylightPlan.Default] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },
  {
    path: 'skylight-plan-add/:organizationId/:planTime',
    name: 'skylight-plan-add',
    component: () => import('@/views/cr-plan/VerticalSkylightPlan'),
    hidden: true,
    meta: { title: '添加计划', keepAlive: false, permission: [permissionCrPlan.SkylightPlan.Create] },
    props: (route, repairTagKey) => {
      return {
        pageState: PageState.Add,
        organizationId: route.params.organizationId,
        planTime: route.params.planTime,
        planType: route.params.planType,
        repairTagKey,
      };
    },
  },
  {
    path: 'skylight-plan-edit/:id/:organizationId/:planTime',
    name: 'skylight-plan-edit',
    component: () => import('@/views/cr-plan/VerticalSkylightPlan'),
    hidden: true,
    meta: { title: '编辑计划', keepAlive: false, permission: [permissionCrPlan.SkylightPlan.Update] },
    props: (route, repairTagKey) => {
      return {
        pageState: PageState.Edit,
        id: route.params.id,
        organizationId: route.params.organizationId,
        planTime: route.params.planTime,
        planType: route.params.planType,
        isChange: route.params.isChange,
        repairTagKey,
      };
    },
  },
  {
    path: 'skylight-plan-view/:id/:organizationId',
    name: 'skylight-plan-view',
    component: () => import('@/views/cr-plan/VerticalSkylightPlan'),
    hidden: true,
    meta: { title: '查看计划', keepAlive: false, permission: permissionCrPlan.SkylightPlan.Detail },
    props: (route, repairTagKey) => {
      return {
        pageState: PageState.View,
        id: route.params.id,
        organizationId: route.params.organizationId,
        planTime: route.params.planTime,
        planType: route.params.planType,
        repairTagKey,
      };
    },
  },

  {
    path: 'general-skylight-plans',
    name: 'general-skylight-plans',
    component: () => import('@/views/cr-plan/GeneralSkylightPlans'),
    meta: { title: '综合天窗计划', keepAlive: false, permission: [permissionCrPlan.SkylightPlan.Default] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },

  {
    path: 'out-of-skylight-plans',
    name: 'out-of-skylight-plans',
    component: () => import('@/views/cr-plan/OutOfSkylightPlans'),
    meta: { title: '天窗点外计划', keepAlive: false, permission: [permissionCrPlan.SkylightPlan.Default] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },

  {
    path: 'other-plans',
    name: 'other-plans',
    component: () => import('@/views/cr-plan/OtherPlans'),
    meta: { title: '其他计划', keepAlive: false, permission: [permissionCrPlan.OtherPlan.Default] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },
  {
    path: 'vertical-other-plan-add/:organizationId/:planTime',
    name: 'vertical-other-plan-add',
    component: () => import('@/views/cr-plan/OtherPlan'),
    hidden: true,
    meta: { title: '添加计划', keepAlive: false, permission: [permissionCrPlan.OtherPlan.Create] },
    props: (route, repairTagKey) => {
      return {  
        pageState: PageState.Add,
        organizationId: route.params.organizationId,
        planTime: route.params.planTime,
        repairTagKey,
        workAreaId: route.params.workAreaId,
      };
    },
  },
  {
    path: 'vertical-other-plan-edit/:id/:organizationId/:planTime',
    name: 'vertical-other-plan-edit',
    component: () => import('@/views/cr-plan/OtherPlan'),
    hidden: true,
    meta: { title: '编辑计划', keepAlive: false, permission: [permissionCrPlan.OtherPlan.Update] },
    props: (route, repairTagKey) => {
      return {
        pageState: PageState.Edit,
        id: route.params.id,
        organizationId: route.params.organizationId,
        planTime: route.params.planTime,
        isChange:route.params.isChange,
        repairTagKey,
      };
    },
  },
  {
    path: 'vertical-other-plan-view/:id/:organizationId/:planTime',
    name: 'vertical-other-plan-view',
    component: () => import('@/views/cr-plan/OtherPlan'),
    hidden: true,
    meta: { title: '查看计划', keepAlive: false, permission: [permissionCrPlan.OtherPlan.Detail] },
    props: (route, repairTagKey) => {
      return {
        pageState: PageState.View,
        id: route.params.id,
        organizationId: route.params.organizationId,
        planTime: route.params.planTime,
        repairTagKey,
      };
    },
  },
];

export default {
  path: 'cr-plan-manage',
  name: 'cr-plan-manage',
  component: PageView,
  meta: {
    title: '计划管理',
    keepAlive: false,
    icon: 'calendar',
    permission: [permissionCrPlan.CrPlanPlanGroupName],
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
