import { PageView } from '@/layouts';
import { PageState } from '@/common/enums';
import permissionTask from 'snweb-module/es/_permissions/sm-task';

export default {
  path: 'task',
  name: 'task',
  component: PageView,
  meta: {
    title: '任务管理',
    keepAlive: true,
    icon: 'project',
    permission: [permissionTask.GroupName],
  },
  children: [
    {
      path: 'task-tasks',
      name: 'task-tasks',
      component: () => import('@/views/task/Tasks'),
      meta: {
        title: '任务管理',
        keepAlive: false,
        permission: [permissionTask.Tasks.Default],
      },
    },
    {
      path: 'task-calendars',
      name: 'task-calendars',
      component: () => import('@/views/task/TaskCalendars'),
      meta: {
        title: '任务日历',
        keepAlive: false,
        permission: [permissionTask.Tasks.Default],
      },
    },
    {
      path: 'task-task-add',
      name: 'task-task-add',
      component: () => import('@/views/task/TaskPage'),
      meta: {
        title: '添加任务',
        keepAlive: true,
        permission: [permissionTask.Tasks.Create],
      },
      hidden: true,
      props: router => ({
        pageState: PageState.Add,
      }),
    },
    {
      path: 'task-task-edit/:id',
      name: 'task-task-edit',
      component: () => import('@/views/task/TaskPage'),
      meta: {
        title: '任务查看',
        keepAlive: true,
        permission: [permissionTask.Tasks.Detail],
      },
      hidden: true,
      props: router => ({
        pageState: PageState.Edit,
        id: router.params.id,
        creatorId: router.params.creatorId,
        hasParent: router.params.hasParent,
      }),
    },
    {
      path: 'task-task-view/:id',
      name: 'task-task-view',
      component: () => import('@/views/task/TaskPage'),
      meta: {
        title: '任务反馈',
        keepAlive: true,
        permission: [permissionTask.Tasks.Update],
      },
      hidden: true,
      props: router => ({
        pageState: PageState.View,
        id: router.params.id,
      }),
    },
  ],
};