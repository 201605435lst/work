import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts';
import { PageState } from '@/common/enums';
import {GroupName,Faults,Plans} from 'snweb-module/es/_permissions/sm-emerg';
import permissionCms from 'snweb-module/es/_permissions/sm-emerg';

export default {
  path: 'emerg',
  name: 'emerg',
  component: PageView,
  meta: {
    title: '应急指挥',
    keepAlive: true,
    icon: 'warning',
    permission: [permissionCms.GroupName],
  },
  children: [
    {
      path: 'emerg-plans',
      name: 'emerg-plans',
      component: () => import('@/views/emerg/EmergPlans'),
      meta: {
        title: '预案管理',
        keepAlive: false,
        permission: [permissionCms.Plans.Default],
      },
    },
    {
      path: 'emerg-plan-add',
      name: 'emerg-plan-add',
      component: () => import('@/views/emerg/EmergPlan'),
      meta: {
        title: '添加预案',
        keepAlive: true,
        permission: [permissionCms.Plans.Create],
      },
      hidden: true,
      props: router => ({
        pageState: PageState.Add,
      }),
    },
    {
      path: 'emerg-plan-edit/:id',
      name: 'emerg-plan-edit',
      component: () => import('@/views/emerg/EmergPlan'),
      meta: {
        title: '编辑预案',
        keepAlive: true,
        permission: [permissionCms.Plans.Update],
      },
      hidden: true,
      props: route => ({
        pageState: PageState.Edit,
        id: route.params.id,
        isApply: false,
      }),
    },
    {
      path: 'emerg-plan-view/:id',
      name: 'emerg-plan-view',
      component: () => import('@/views/emerg/EmergPlan'),
      meta: {
        title: '预案详情',
        keepAlive: true,
        permission: [permissionCms.Plans.Detail],
      },
      hidden: true,
      props: route => ({
        pageState: PageState.View,
        id: route.params.id,
        isApply: route.params.isApply,
        faultId: route.params.faultId,
      }),
    },

    {
      path: 'emerg-plan-view/:faultId/:isApply',
      name: 'emerg-plan-view-fault',
      component: () => import('@/views/emerg/EmergPlan'),
      meta: {
        title: '预案详情',
        keepAlive: true,
        permission: [permissionCms.Plans.Detail],
      },
      hidden: true,
      props: route => ({
        pageState: PageState.View,
        id: route.params.id,
        isApply: route.params.isApply,
        faultId: route.params.faultId,
        isD3:route.params.isD3 ? route.params.isD3 : 'D3',
      }),
    },

    {
      path: 'emerg-faults',
      name: 'emerg-faults',
      component: () => import('@/views/emerg/EmergFaults'),
      meta: {
        title: '故障管理',
        keepAlive: false,
        permission: [permissionCms.Faults.Default],
      },
    },
    {
      path: 'emerg-fault-add-new',
      name: 'emerg-fault-add-new',
      component: () => import('@/views/emerg/EmergFault'),
      meta: {
        title: '添加',
        keepAlive: true,
        permission: [permissionCms.Faults.Create],
      },
      hidden: true,
      props: route => ({
        append: route.params.append,
      }),
    },
    {
      path: 'emerg-fault-add-old',
      name: 'emerg-fault-add-old',
      component: () => import('@/views/emerg/EmergFault'),
      meta: {
        title: '添加故障',
        keepAlive: true,
        permission: [permissionCms.Faults.Create],
      },
      hidden: true,
      props: route => ({
        append: route.params.append,
      }),
    },
    {
      path: 'emerg-fault-edit/:faultId',
      name: 'emerg-fault-edit',
      component: () => import('@/views/emerg/EmergFault'),
      meta: {
        title: '编辑故障',
        keepAlive: true,
        permission: [permissionCms.Faults.Update],
      },
      hidden: true,
      props: route => ({
        faultId: route.params.faultId,
        pageState: PageState.Edit,
        state: route.params.state,
        append: route.params.append,
      }),
    },
    {
      path: 'emerg-fault-view/:faultId',
      name: 'emerg-fault-view',
      component: () => import('@/views/emerg/EmergFault'),
      meta: {
        title: '故障详情',
        keepAlive: true,
        permission: [permissionCms.Faults.Detail],
      },
      hidden: true,
      props: route => ({
        faultId: route.params.faultId,
        pageState: PageState.View,
        isD3:route.params.isD3 ? route.params.isD3 : 'D3',
      }),
    },
  ],
};
