// eslint-disable-next-line
import { PageView,FlowLayout } from '@/layouts'
import permissionTechnology from 'snweb-module/es/_permissions/sm-technology';
import permissionResource from 'snweb-module/es/_permissions/sm-resource';

export default {
  path: 'interface-manage',
  name: 'interface-manage',
  component: PageView,
  meta: { title: '接口管理', keepAlive: false, icon: 'snippets', permission: [permissionTechnology.GroupName] },
  children: [
    {
      path: 'interface-manage-flag',
      name: 'interface-manage-flag',
      component: () => import('@/views/technology/InterfaceFlag'),
      meta: {
        title: '接口标记',
        keepAlive: false,
        permission: [permissionTechnology.ConstructInterfaces.Default],
        showInMenu: true,
      },
    },
    {
      path: 'interface-manage-listing',
      name: 'interface-manage-listing',
      component: () => import('@/views/technology/InterfaceListing'),
      meta: {
        title: '接口清单',
        keepAlive: false,
        permission: [permissionTechnology.ConstructInterfaces.Default],
        showInMenu: true,
      },
    },
    {
      path: 'interface-manage-report',
      name: 'interface-manage-report',
      component: () => import('@/views/technology/InterfaceReport'),
      meta: {
        title: '接口报告',
        keepAlive: false,
        permission: [permissionTechnology.ConstructInterfaces.Default],
        showInMenu: true,
      },
    },
  ],
};
