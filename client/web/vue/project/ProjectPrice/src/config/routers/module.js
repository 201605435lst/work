import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts';
// import permissionFile from 'snweb-module/es/_permissions/sm-file';

export default {
  path: 'module',
  name: 'module',
  component: () => import('@/views/module/Modules'),
  meta: { title: '模块管理', keepAlive: true, icon: 'wallet', permission: [] },
};
