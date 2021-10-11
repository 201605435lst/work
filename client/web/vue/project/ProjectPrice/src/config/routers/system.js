import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts';
import permissionSystem from 'snweb-module/es/_permissions/sm-system';

export default {
  path: 'system',
  name: 'system',
  component: PageView,
  meta: { title: '系统管理', keepAlive: true, icon: 'setting', permission: [permissionSystem.GroupName] },
  children: [
    {
      path: 'data-dictionaries',
      name: 'data-dictionaries',
      component: () => import('@/views/system/DataDictionaries'),
      meta: { title: '数据字典', keepAlive: false, permission: [permissionSystem.DataDictionary.Default] },
    },
    {
      path: 'organizations',
      name: 'organizations',
      component: () => import('@/views/system/Organizations'),
      meta: { title: '组织单元', keepAlive: false, permission: [permissionSystem.Organization.Default] },
    },
    {
      path: 'roles',
      name: 'roles',
      component: () => import('@/views/system/Roles'),
      meta: { title: '角色权限', keepAlive: false, permission: [permissionSystem.Roles.Default] },
    },
    {
      path: 'users',
      name: 'users',
      component: () => import('@/views/system/Users'),
      meta: { title: '用户管理', keepAlive: false, permission: [permissionSystem.Users.Default] },
    },
  ],
};
