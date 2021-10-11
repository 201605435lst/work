// eslint-disable-next-line
import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts'
import permissionOa from 'snweb-module/es/_permissions/sm-oa';
import { PageState } from '@/common/enums';

export default {
  path: 'oa',
  name: 'oa',
  component: PageView,
  meta: {
    title: 'OA 办公',
    keepAlive: false,
    icon: 'snippets',
    permission: [permissionOa.GroupName],
  },
  children: [
    {
      path: 'duty-schedule',
      name: 'duty-schedule',
      component: () => import('@/views/oa/DutySchedule'),
      meta: {
        title: '值班管理',
        keepAlive: false,
        permission: [permissionOa.DutySchedule.Default],
        showInMenu: true,
      },
    },
    {
      path: 'seal',
      name: 'seal',
      component: () => import('@/views/oa/Seal'),
      meta: {
        title: '签章管理',
        keepAlive: false,
      },
    },
    {
      path: 'contracts',
      name: 'contracts',
      component: () => import('@/views/oa/Contracts'),
      meta: {
        title: '合同管理',
        keepAlive: false,
        permission: [permissionOa.Contracts.Default],
      },
    },
    {
      path: 'oa-contract-add',
      name: 'oa-contract-add',
      component: () => import('@/views/oa/Contract'),
      meta: {
        title: '添加合同',
        keepAlive: true,
        permission: [permissionOa.Contracts.Create],
      },
      hidden: true,
      props: router => ({
        pageState: PageState.Add,
      }),
    },
    {
      path: 'oa-contract-edit/:id',
      name: 'oa-contract-edit',
      component: () => import('@/views/oa/Contract'),
      meta: {
        title: '编辑合同',
        keepAlive: true,
        permission: [permissionOa.Contracts.Update],
      },
      hidden: true,
      props: route => ({
        pageState: PageState.Edit,
        id: route.params.id,
      }),
    },
    {
      path: 'oa-contract-view/:id',
      name: 'oa-contract-view',
      component: () => import('@/views/oa/Contract'),
      meta: {
        title: '合同详情',
        keepAlive: true,
        permission: [permissionOa.Contracts.Detail],
      },
      hidden: true,
      props: route => ({
        pageState: PageState.View,
        id: route.params.id,
      }),
    },
  ],
};
