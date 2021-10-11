// eslint-disable-next-line
import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts'
import permissionReport from 'snweb-module/es/_permissions/sm-report';
import { PageState } from '@/common/enums';

export default {
  path: 'report',
  name: 'report',
  component: PageView,
  meta: {
    title: '工作汇报',
    keepAlive: false,
    icon: 'snippets',
    permission: [permissionReport.GroupName],
  },
  children: [
    {
      path: 'reports-send',
      name: 'reports-send',
      component: () => import('@/views/report/ReportsSend'),
      meta: {
        title: '我的汇报',
        keepAlive: false,
        permission: [permissionReport.Reports.Default],
      },
    },
    {
      path: 'reports-receive',
      name: 'reports-receive',
      component: () => import('@/views/report/ReportReceive'),
      meta: {
        title: '汇报给我',
        keepAlive: false,
        permission: [permissionReport.Reports.Default],
      },
    },
    {
      path: 'report-report-add',
      name: 'report-report-add',
      component: () => import('@/views/report/Report'),
      meta: {
        title: '新增汇报',
        keepAlive: true,
        permission: [permissionReport.Reports.Create],
      },
      hidden: true,
      props: router => ({
        pageState: PageState.Add,
      }),
    },
    {
      path: 'report-report-edit/:id',
      name: 'report-report-edit',
      component: () => import('@/views/report/Report'),
      meta: {
        title: '编辑汇报',
        keepAlive: true,
        permission: [permissionReport.Reports.Update],
      },
      hidden: true,
      props: route => ({
        pageState: PageState.Edit,
        id: route.params.id,
      }),
    },
    {
      path: 'report-report-view/:id',
      name: 'report-report-view',
      component: () => import('@/views/report/Report'),
      meta: {
        title: '工作详情',
        keepAlive: true,
        permission: [permissionReport.Reports.Detail],
      },
      hidden: true,
      props: route => ({
        pageState: PageState.View,
        id: route.params.id,
      }),
    },
  ],
};
