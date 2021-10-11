import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts';
import basicPermissions from 'snweb-module/es/_permissions/sm-basic.js';

export default {
  path: 'basic',
  name: 'basic',
  component: PageView,
  meta: { title: '基础数据', keepAlive: true, icon: 'control', permission: [basicPermissions.GroupName] },
  children: [
    {
      path: 'railways',
      name: 'railways',
      component: () => import('@/views/basic/Railways'),
      meta: { title: '线路管理', keepAlive: false, permission: [basicPermissions.Railways.Default] },
    },
    {
      path: 'stations',
      name: 'stations',
      component: () => import('@/views/basic/Stations'),
      meta: { title: '站点管理', keepAlive: false, permission: [basicPermissions.Stations.Default] },
    },
    {
      path: 'installation-sites',
      name: 'installation-sites',
      component: () => import('@/views/basic/InstallationSites'),
      meta: { title: '机房管理', keepAlive: false, permission: [basicPermissions.InstallationSites.Default] },
    },
  ],
};
