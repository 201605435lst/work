import * as permissionConstuctionBase from 'snweb-module/es/_permissions/sm-constuction-base';
import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts';
import basicPermissions from 'snweb-module/es/_permissions/sm-basic.js';
import permissionResource from 'snweb-module/es/_permissions/sm-resource';
export default {
  path: 'basic-data',
  name: 'basic-data',
  component: PageView,
  meta: { title: '基础数据', keepAlive: false, icon: 'hdd', permission: [permissionConstuctionBase.GroupName,basicPermissions.GroupName,permissionResource.GroupName] },
  children: [
    {
      path: 'subItem',
      name: 'subItem',
      component: () => import('@/views/construction-base/SubItem'),
      meta: {
        title: '分部分项管理',
        keepAlive: false,
        permission: [permissionConstuctionBase.SubItem.Default],
        showInMenu: true,
      },
    },
    {
      path: 'standard',
      name: 'standard',
      component: () => import('@/views/construction-base/Standard'),
      meta: {
        title: '工序规范管理',
        keepAlive: false,
        permission: [permissionConstuctionBase.Standard.Default], //todo 这个还没有加权限 ,后面 在加
        showInMenu: true,
      },
    },
    {
      path: 'Section',
      name: 'Section',
      component: () => import('@/views/construction-base/Section'),
      meta: {
        title: '施工区段管理',
        keepAlive: false,
        permission: [permissionConstuctionBase.Section.Default], //todo 这个还没有加权限 ,后面 在加
        showInMenu: true,
      },
    },
    {
      path: 'subItemDraw/:id',
      name: 'subItemDraw',
      hidden: true,
      component: () => import('@/views/construction-base/SubItemDraw'),
      meta: { title: '分布分项编制', keepAlive: false },
      props: route => {
        return {
          id: route.params.id,
        };
      },
    },
    {
        path: 'basic-data-railways',
        name: 'basic-data-railways',
        component: () => import('@/views/basic/Railways'),
        meta: { title: '线路管理', keepAlive: false, permission: [basicPermissions.Railways.Default] },
      },
      {
        path: 'basic-data-stations',
        name: 'basic-data-stations',
        component: () => import('@/views/basic/Stations'),
        meta: { title: '站点管理', keepAlive: false, permission: [basicPermissions.Stations.Default] },
      },
      {
        path: 'basic-data-installation-sites',
        name: 'basic-data-installation-sites',
        component: () => import('@/views/basic/InstallationSites'),
        meta: { title: '机房管理', keepAlive: false, permission: [basicPermissions.InstallationSites.Default] },
      },
      {
        path: 'basic-data-store-house',
        name: 'basic-data-store-house',
        component: () => import('@/views/resource/StoreHouse'),
        meta: {
          title: '仓库管理',
          keepAlive: false,
          permission: [permissionResource.StoreHouse.Default],
          showInMenu: true,
        },
      },
      {
        path: 'basic-data-store-import',
        name: 'basic-data-store-import',
        component: () => import('@/views/resource/StoreImport'),
        meta: {
          title: '设备入库',
          keepAlive: false,
          permission: [permissionResource.StoreEquipmentTransfer.Create],
          showInMenu: true,
        },
      },
      {
        path: 'basic-data-store-export',
        name: 'basic-data-store-export',
        component: () => import('@/views/resource/StoreExport'),
        meta: {
          title: '设备出库',
          keepAlive: false,
          permission: [permissionResource.StoreEquipmentTransfer.Create],
          showInMenu: true,
        },
      },
      {
        path: 'basic-data-store-equipment-transfer',
        name: 'basic-data-store-equipment-transfer',
        component: () => import('@/views/resource/StoreEquipmentTransfer'),
        meta: {
          title: '出入库记录',
          keepAlive: false,
          permission: [permissionResource.StoreEquipmentTransfer.Default],
          showInMenu: true,
        },
      },
      {
        path: 'basic-data-equipment-group',
        name: 'basic-data-equipment-group',
        component: () => import('@/views/resource/EquipmentGroup'),
        meta: {
          title: '设备分组',
          keepAlive: false,
          permission: [permissionResource.EquipmentGroups.Default],
          showInMenu: true,
        },
      },
      {
        path: 'basic-data-equipments',
        name: 'basic-data-equipments',
        component: () => import('@/views/resource/Equipments'),
        meta: {
          title: '设备管理',
          keepAlive: false,
          permission: [permissionResource.Equipments.Default],
          showInMenu: true,
        },
      },
  
      {
        path: 'basic-data-store-equipments',
        name: 'basic-data-store-equipments',
        component: () => import('@/views/resource/StoreEquipments'),
        meta: {
          title: '库存设备管理',
          keepAlive: false,
          permission: [permissionResource.StoreEquipments.Default],
          showInMenu: true,
        },
      },
      {
        path: 'basic-data-code-generation',
        name: 'basic-data-code-generation',
        component: () => import('@/views/resource/CodeGeneration'),
        meta: {
          title: '生成库存设备编码',
          keepAlive: false,
          permission: [permissionResource.StoreEquipments.Create],
          showInMenu: true,
        },
      },
      {
        path: 'basic-data-store-equipments-tests',
        name: 'basic-data-store-equipments-tests',
        component: () => import('@/views/resource/StoreEquipmentsTests'),
        meta: {
          title: '库存设备检测记录',
          keepAlive: true,
          permission: [permissionResource.StoreEquipmentTest.Default],
          showInMenu: true,
        },
      },
      ,
      {
        path: 'basic-data-store-equipments-test',
        name: 'basic-data-store-equipments-test',
        component: () => import('@/views/resource/StoreEquipmentsTest'),
        meta: {
          title: '检测库存设备',
          keepAlive: false,
          permission: [permissionResource.StoreEquipmentTest.Create],
          showInMenu: true,
        },
      },
      {
        path: 'basic-data-store-equipments-test-view/:id',
        name: 'basic-data-store-equipments-test-view',
        component: () => import('@/views/resource/StoreEquipmentsTest'),
        meta: {
          title: '检测单详情',
          keepAlive: true,
          permission: [permissionResource.StoreEquipmentTest.Detail],
          showInMenu: true,
        },
        hidden: true,
        props: route => ({
          id: route.params.id,
        }),
      },
  ],
};
