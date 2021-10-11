import { PageView, BlankLayout } from '@/layouts';
import permissionCrPlan from 'snweb-module/es/_permissions/sm-cr-plan';
import { getGroupWithRepairTag, repairTagKeyConfigs } from './_util';

let routes = [
  {
    path: 'sending-works',
    name: 'sending-works',
    component: () => import('@/views/cr-plan/SendingWorks'),
    meta: { title: '派工作业', keepAlive: false, permission: [permissionCrPlan.SendingWork.Default] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },
  {
    path: 'other-works',
    name: 'other-works',
    component: () => import('@/views/cr-plan/OtherWorks'),
    meta: { title: '其他作业', keepAlive: false, permission: [permissionCrPlan.OtherWorks.Default] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },

  {
    path: 'sending-work/:operatorType/:sendingWorkId',
    name: 'sending-work',
    component: () => import('@/views/cr-plan/SendingWork'),
    hidden: true,
    meta: { title: '派工作业单页', keepAlive: false, permission: [permissionCrPlan.SendingWork.Default] },
    props: (route, repairTagKey) => {
      return {
        operatorType: route.params.operatorType,
        id: route.params.id,
        orderState: route.params.orderState,
        repairTagKey,
      };
    },
  },
  {
    path: 'maintenance-records',
    name: 'maintenance-records',
    component: () => import('@/views/cr-plan/MaintenanceRecords'),
    meta: { title: '检修记录', keepAlive: false, permission: [permissionCrPlan.MaintenanceRecord.Default] },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },
  {
    path: 'maintenance-record-view/:organizationId/:repairGroupId',
    name: 'maintenance-record-view',
    component: () => import('@/views/cr-plan/MaintenanceRecord'),
    hidden: true,
    meta: {
      title: '查看设备检修记录',
      keepAlive: false,
      permission: [permissionCrPlan.MaintenanceRecord.Detail],
    },
    props: (route, repairTagKey) => {
      return {
        organizationId: route.params.organizationId,
        equipmentId: route.params.equipmentId,
        repairGroupId: route.params.repairGroupId,
        equipType: route.params.equipType,
        equipName: route.params.equipName,
        equipModelNumber: route.params.equipModelNumber,
        equipModelCode: route.params.equipModelCode,
        installationSite: route.params.installationSite,
        workOrderIds: route.params.workOrderIds,
        repairTagKey,
      };
    },
  },
];

export default {
  path: 'cr-plan-works',
  name: 'cr-plan-works',
  component: PageView,
  meta: {
    title: '作业管理',
    keepAlive: false,
    icon: 'calendar',
    permission: [permissionCrPlan.CrPlanWorkGroupName],
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
