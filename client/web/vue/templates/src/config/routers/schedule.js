import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts';
import { PageState } from '@/common/enums';
import router from '@/router/index';
import permissionSchedule from 'snweb-module/es/_permissions/sm-schedule';

export default {
  path: 'schedule',
  name: 'schedule',
  component: PageView,
  meta: {
    title: '施工进度',
    keepAlive: true,
    icon: 'double-right',
    permission: [permissionSchedule.GroupName],
  },
  children: [
    {
      path: 'schedule-schedules',
      name: 'schedule-schedules',
      component: () => import('@/views/schedule/Schedules'),
      meta: {
        title: '施工计划 ',
        keepAlive: false,
        permission: [permissionSchedule.Schedules.Default],
      },
    },
    {
      path: 'schedule-approvals',
      name: 'schedule-approvals',
      component: () => import('@/views/schedule/Approvals'),
      meta: {
        title: '施工审批管理 ',
        keepAlive: false,
        permission: [permissionSchedule.Approvals.Default],
      },
    },
    {
      path: 'schedule-approval-add',
      name: 'schedule-approval-add',
      component: () => import('@/views/schedule/Approval'),
      hidden: true,
      meta: {
        title: '添加施工审批',
        keepAlive: false,
      },
      props: route => ({
        pageState: PageState.Add,
        id: route.params.id,
      }),
    },
    {
      path: 'schedule-approval-edit/:id',
      name: 'schedule-approval-edit',
      component: () => import('@/views/schedule/Approval'),
      hidden: true,
      meta: {
        title: '编辑施工审批',
        keepAlive: true,
      },
      props: route => {
        return {
          pageState: PageState.Edit,
          id: route.params.id,
        };
      },
    },
    {
      path: 'schedule-diarys',
      name: 'schedule-diarys',
      component: () => import('@/views/schedule/Diarys'),
      meta: {
        title: '施工日志 ',
        keepAlive: false,
        permission: [permissionSchedule.Diarys.Default],
      },
    },
    {
      path: 'schedule-diary-fill/:id',
      name: 'schedule-diary-fill',
      component: () => import('@/views/schedule/Diary'),
      hidden: true,
      meta: {
        title: '日志填报',
        keepAlive: false,
      },
      props: route => ({
        pageState: PageState.Add,
        id: route.params.id,
      }),
    },
    {
      path: 'schedule-diary-edit/:id',
      name: 'schedule-diary-edit',
      component: () => import('@/views/schedule/Diary'),
      hidden: true,
      meta: {
        title: '编辑日志',
        keepAlive: true,
      },
      props: route => {
        return {
          pageState: PageState.Edit,
          id: route.params.id,
        };
      },
    },
    {
      path: 'schedule-diary-logStatistics',
      name: 'schedule-diary-logStatistics',
      component: () => import('@/views/schedule/DiarysStatistics'),
      hidden: true,
      meta: {
        title: '日志统计',
        keepAlive: false,
      },
      props: route => ({
      
      }),
    },
    {
      path: 'schedule-schedule-add',
      name: 'schedule-schedule-add',
      component: () => import('@/views/schedule/Schedule'),
      hidden: true,
      meta: {
        title: '添加计划',
        keepAlive: false,
      },
      props: route => ({
        pageState: PageState.Add,
        id: route.params.id,
      }),
    },
  
    {
      path: 'schedule-schedule-view/:id',
      name: 'schedule-schedule-view',
      component: () => import('@/views/schedule/Schedule'),
      hidden: true,
      meta: {
        title: '查看计划',
        keepAlive: true,
      },
      props: router => ({
        pageState: PageState.View,
        id: router.params.id,
      }),
    },
    {
      path: 'schedule-schedule-edit/:id',
      name: 'schedule-schedule-edit',
      component: () => import('@/views/schedule/Schedule'),
      hidden: true,
      meta: {
        title: '编辑计划',
        keepAlive: true,
      },
      props: route => {
        return {
          pageState: PageState.Edit,
          id: route.params.id,
        };
      },
    },
  ],
};
