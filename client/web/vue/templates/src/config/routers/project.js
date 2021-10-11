import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts';
import { PageState } from '@/common/enums';
import router from '@/router/index';
import permissionProject from 'snweb-module/es/_permissions/sm-project';
export default {
  path: 'project',
  name: 'project',
  component: PageView,
  meta: {
    title: '项目管理',
    keepAlive: true,
    icon: 'project',
    permission: [permissionProject.GroupName],
  },
  children: [
    {
      path: 'project-projects',
      name: 'project-projects',
      component: () => import('@/views/project/Projects'),
      meta: {
        title: '项目台账',
        keepAlive: false,
        // permission: [permissionProject.Projects.Default],
      },
    },
    
    {
      path: 'project-project-add',
      name: 'project-project-add',
      component: () => import('@/views/project/Project'),
      hidden: true,
      meta: {
        title: '添加项目',
        keepAlive: false,
      },
      props: router => ({
        pageState: PageState.Add,
      }),
    },
    {
      path: 'project-project-view/:id',
      name: 'project-project-view',
      component: () => import('@/views/project/Project'),
      hidden: true,
      meta: {
        title: '查看项目详情',
        keepAlive: true,
      },
      props: router => ({
        pageState: PageState.View,
        id: router.params.id,
      }),
    },
    {
      path: 'project-project-edit/:id',
      name: 'project-project-edit',
      component: () => import('@/views/project/Project'),
      hidden: true,
      meta: {
        title: '编辑项目',
        keepAlive: true,
      },
      props: router => ({
        pageState: PageState.Edit,
        id: router.params.id,
      }),
    },
    {
      path: 'archives',
      name: 'archives',
      component: () => import('@/views/project/Archives'),
      meta: {
        title: '卷宗管理',
        keepAlive: false,
        permission: [permissionProject.Archivess.Default],
        showInMenu: true,
      },
    },
    {
      path: 'dossier',
      name: 'dossier',
      component: () => import('@/views/project/Dossier'),
      meta: {
        title: '档案管理',
        keepAlive: false,
        permission: [permissionProject.Dossiers.Default],
        showInMenu: true,
      },
    },
  ],
};
