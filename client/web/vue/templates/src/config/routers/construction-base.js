// 人为制造冲突
import { PageView } from '@/layouts';
import * as permissionResource  from 'snweb-module/es/_permissions/sm-constuction-base';

export default {
  path: 'construction-base',
  name: 'construction-base',
  
  component: PageView,
  meta: { title: '基础数据维护', keepAlive: false, icon: 'hdd', permission: [permissionResource.GroupName] },
  children: [
    {
      path: 'worker',
      name: 'worker',
      component: () => import('@/views/construction-base/Worker'),
      meta: {
        title: '工种管理',
        keepAlive: false,
        permission: [permissionResource.Worker.Default],
        showInMenu: true,
      },
    },
    {
      path: 'equipmentTeam',
      name: 'equipmentTeam',
      component: () => import('@/views/construction-base/EquipmentTeam'),
      meta: {
        title: '设备台班管理',
        keepAlive: false,
        permission: [permissionResource.EquipmentTeam.Default],
        showInMenu: true,
      },
    },
    {
      path: 'constructionBaseMaterial',
      name: 'constructionBaseMaterial',
      component: () => import('@/views/construction-base/Material'),
      meta: {
        title: '工程量管理',
        keepAlive: false,
        permission: [permissionResource.Material.Default],
        showInMenu: true,
      },
    },
    {
      path: 'procedure',
      name: 'procedure',
      component: () => import('@/views/construction-base/Procedure'),
      meta: {
        title: '施工工序管理',
        keepAlive: false,
        permission: [permissionResource.Procedure.Default],
        showInMenu: true,
      },
    },
   
  ],
};
