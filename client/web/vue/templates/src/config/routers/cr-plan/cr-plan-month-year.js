/**
 * 年月表管理
 */
import { PageView, BlankLayout } from '@/layouts';
import { PageState, RepairTags } from '@/common/enums';
import permissionCrPlan from 'snweb-module/es/_permissions/sm-cr-plan.js';
import { getGroupWithRepairTag, repairTagKeyConfigs } from './_util';

let routes = [
  {
    path: 'year-plan',
    name: 'year-plan',
    component: () => import('@/views/cr-plan/YearPlan'),
    meta: { title: '年表管理', keepAlive: false, permission: [permissionCrPlan.YearPlan.Default] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },

  {
    path: 'month-plan',
    name: 'month-plan',
    component: () => import('@/views/cr-plan/MonthPlan'),
    meta: { title: '月表管理', keepAlive: false, permission: [permissionCrPlan.MonthPlan.Default] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },

  {
    path: 'month-of-year-plan',
    name: 'month-of-year-plan',
    component: () => import('@/views/cr-plan/PlanMonthOfYear'),
    meta: { title: '年表月度管理', keepAlive: false, permission: [permissionCrPlan.MonthOfYearPlan.Default] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },

  {
    path: 'year-month-change/:planType/:orgId/:orgName',
    name: 'year-month-change',
    hidden: true,
    component: () => import('@/views/cr-plan/PlanYearMonthChange'),
    meta: {
      title: '年月表变更',
      keepAlive: false,
      permission: [permissionCrPlan.YearPlan.Alter, permissionCrPlan.MonthPlan.Alter],
    },
    props: (route, repairTagKey) => {
      return {
        planType: route.params.planType,
        orgId: route.params.orgId,
        orgName: route.params.orgName,
        repairTagKey,
      };
    },
  },

  {
    path: 'plan-changes',
    name: 'plan-changes',
    component: () => import('@/views/cr-plan/PlanChanges'),
    meta: { title: '未完成计划变更', keepAlive: false, permission: [permissionCrPlan.PlanChange.Default] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },
  {
    path: 'plan-change-add',
    name: 'plan-change-add',
    component: () => import('@/views/cr-plan/PlanChange'),
    hidden: true,
    meta: { title: '添加计划变更', keepAlive: false, permission: [permissionCrPlan.PlanChange.Create] },
    props: (route, repairTagKey) => {
      return {
        pageState: PageState.Add,
        organizationId: route.params.organizationId,
        repairTagKey,
      };
    },
  },
  {
    path: 'plan-change-edit/:id/:organizationId',
    name: 'plan-change-edit',
    component: () => import('@/views/cr-plan/PlanChange'),
    hidden: true,
    meta: { title: '编辑计划变更', keepAlive: false, permission: [permissionCrPlan.PlanChange.Update] },
    props: (route, repairTagKey) => {
      return {
        pageState: PageState.Edit,
        id: route.params.id,
        organizationId: route.params.organizationId,
        repairTagKey,
      };
    },
  },
  {
    path: 'plan-change-view/:id',
    name: 'plan-change-view',
    component: () => import('@/views/cr-plan/PlanChange'),
    hidden: true,
    meta: { title: '查看计划变更', keepAlive: false, permission: [permissionCrPlan.PlanChange.Detail] },
    props: (route, repairTagKey) => {
      return {
        pageState: PageState.View,
        id: route.params.id,
        repairTagKey,
      };
    },
  },
  {
    path: 'year-month-alter-record/:planType/:orgId/',
    name: 'year-month-alter-record',
    component: () => import('@/views/cr-plan/YearMonthAlterRecord'),
    hidden: false,
    meta: {
      title: '年月表变更记录',
      keepAlive: false,
    },
    props: (route, repairTagKey) => {
      return {
        planType: route.params.planType,
        orgId: route.params.orgId,
        orgName: route.params.orgName,
        alterRecordId: route.params.alterRecordId == undefined ? null : route.params.alterRecordId,
        isCreateRecord: route.params.isCreateRecord == undefined ? false : route.params.isCreateRecord,
        repairTagKey: route.params.repairTagKey,
      };
    },
  },
  {
    path: 'year-plan-records',
    name: 'year-plan-records',
    component: () => import('@/views/cr-plan/YearPlanRecords'),
    meta: { title: '年表记录', keepAlive: false, permission: [] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },
  {
    path: 'month-plan-records',
    name: 'month-plan-records',
    component: () => import('@/views/cr-plan/MonthPlanRecords'),
    meta: { title: '月表记录', keepAlive: false, permission: [] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },
];

export default {
  path: 'cr-plan-month-year',
  name: 'cr-plan-month-year',
  component: PageView,
  meta: {
    title: '年月表管理',
    keepAlive: false,
    icon: 'calendar',
    permission: [permissionCrPlan.CrPlanGroupName],
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
