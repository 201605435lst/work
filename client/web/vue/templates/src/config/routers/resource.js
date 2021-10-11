// eslint-disable-next-line
import { PageView } from '@/layouts'
import permissionResource from 'snweb-module/es/_permissions/sm-resource';

export default {
  path: 'resource',
  name: 'resource',
  component: PageView,
  meta: { title: '资源管理', keepAlive: false, icon: 'hdd', permission: [permissionResource.GroupName] },
  children: [
    {
      path: 'store-house',
      name: 'store-house',
      component: () => import('@/views/resource/StoreHouse'),
      meta: {
        title: '仓库管理',
        keepAlive: false,
        permission: [permissionResource.StoreHouse.Default],
        showInMenu: true,
      },
    },
    {
      path: 'store-import',
      name: 'store-import',
      component: () => import('@/views/resource/StoreImport'),
      meta: {
        title: '设备入库',
        keepAlive: false,
        permission: [permissionResource.StoreEquipmentTransfer.Create],
        showInMenu: true,
      },
    },
    {
      path: 'store-export',
      name: 'store-export',
      component: () => import('@/views/resource/StoreExport'),
      meta: {
        title: '设备出库',
        keepAlive: false,
        permission: [permissionResource.StoreEquipmentTransfer.Create],
        showInMenu: true,
      },
    },
    {
      path: 'store-equipment-transfer',
      name: 'store-equipment-transfer',
      component: () => import('@/views/resource/StoreEquipmentTransfer'),
      meta: {
        title: '出入库记录',
        keepAlive: false,
        permission: [permissionResource.StoreEquipmentTransfer.Default],
        showInMenu: true,
      },
    },
    {
      path: 'equipment-group',
      name: 'equipment-group',
      component: () => import('@/views/resource/EquipmentGroup'),
      meta: {
        title: '设备分组',
        keepAlive: false,
        permission: [permissionResource.EquipmentGroups.Default],
        showInMenu: true,
      },
    },
    {
      path: 'equipments',
      name: 'equipments',
      component: () => import('@/views/resource/Equipments'),
      meta: {
        title: '设备管理',
        keepAlive: false,
        permission: [permissionResource.Equipments.Default],
        showInMenu: true,
      },
    },

    {
      path: 'store-equipments',
      name: 'store-equipments',
      component: () => import('@/views/resource/StoreEquipments'),
      meta: {
        title: '库存设备管理',
        keepAlive: false,
        permission: [permissionResource.StoreEquipments.Default],
        showInMenu: true,
      },
    },
    {
      path: 'code-generation',
      name: 'code-generation',
      component: () => import('@/views/resource/CodeGeneration'),
      meta: {
        title: '生成库存设备编码',
        keepAlive: false,
        permission: [permissionResource.StoreEquipments.Create],
        showInMenu: true,
      },
    },
    {
      path: 'store-equipments-tests',
      name: 'store-equipments-tests',
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
      path: 'store-equipments-test',
      name: 'store-equipments-test',
      component: () => import('@/views/resource/StoreEquipmentsTest'),
      meta: {
        title: '检测库存设备',
        keepAlive: false,
        permission: [permissionResource.StoreEquipmentTest.Create],
        showInMenu: true,
      },
    },
    {
      path: 'store-equipments-test-view/:id',
      name: 'store-equipments-test-view',
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
