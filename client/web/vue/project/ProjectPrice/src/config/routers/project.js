import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts';
// import permissionFile from 'snweb-module/es/_permissions/sm-file';

export default {
  path: 'projects',
  name: 'projects',
  component: () => import('@/views/project/Projects'),
  meta: { title: '项目管理', keepAlive: true, icon: 'profile', permission: [] },
};
