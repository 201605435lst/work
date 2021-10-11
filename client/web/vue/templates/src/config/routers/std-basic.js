import { getGroupWithRepairTag, repairTagKeyConfigs } from './cr-plan/_util';

// eslint-disable-next-line
import { PageView, BlankLayout } from '@/layouts';
import permissionStdBasic from 'snweb-module/es/_permissions/sm-std-basic';

let routes = [
  {
    path: 'repair-items',
    name: 'repair-items',
    component: () => import('@/views/std-basic/RepairItems'),
    meta: {
      title: '维修项管理',
      keepAlive: false,
      permission: [
        permissionStdBasic.RepairItems.Default,
        permissionStdBasic.RepairTestItems.Default,
        permissionStdBasic.RepairGroup.Default,
      ],
      showInMenu: true,
    },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },
  {
    path: 'influenceranges',
    name: 'influenceranges',
    component: () => import('@/views/std-basic/InfluenceRanges'),
    meta: {
      title: '影响范围管理',
      keepAlive: false,
      permission: [permissionStdBasic.InfluenceRanges.Default],
      showInMenu: true,
    },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },
  {
    path: 'work-attention',
    name: 'work-attention',
    component: () => import('@/views/std-basic/WorkAttention'),
    meta: {
      title: '作业注意事项管理',
      keepAlive: false,
      permission: [permissionStdBasic.WorkAttention.Default],
      showInMenu: true,
    },
    props: (route, repairTagKey) => {
      return {
        repairTagKey,
      };
    },
  },
];

export default {
  path: 'std-basic',
  name: 'std-basic',
  component: PageView,
  meta: { title: '标准库管理', keepAlive: false, icon: 'compass', permission: [permissionStdBasic.GroupName] },
  children: [
    {
      path: 'product-category',
      name: 'product-category',
      component: () => import('@/views/std-basic/ProductCategory'),
      meta: {
        title: '产品分类',
        keepAlive: false,
        permission: [permissionStdBasic.ProductCategories.Default],
        showInMenu: true,
      },
    },
    {
      path: 'component-category',
      name: 'component-category',
      component: () => import('@/views/std-basic/ComponentCategory'),
      meta: {
        title: '构件分类',
        keepAlive: false,
        permission: [permissionStdBasic.ComponentCategories.Default],
        showInMenu: true,
      },
    },
    {
      path: 'manufacturers',
      name: 'manufacturers',
      component: () => import('@/views/std-basic/Manufacturers'),
      meta: {
        title: '厂家管理',
        keepAlive: false,
        permission: [permissionStdBasic.Manufactures.Default],
        showInMenu: true,
      },
    },
    {
      path: 'standard-equipments',
      name: 'standard-equipments',
      component: () => import('@/views/std-basic/StandardEquipments'),
      meta: {
        title: '产品管理',
        keepAlive: false,
        permission: [permissionStdBasic.StandardEquipments.Default],
        showInMenu: true,
      },
    },
    {
      path: 'mvd-category',
      name: 'mvd-category',
      component: () => import('@/views/std-basic/MvdCategory'),
      meta: {
        title: '信息交换模板分类管理',
        keepAlive: false,
        permission: [permissionStdBasic.MVDCategory.Default],
        showInMenu: true,
      },
    },
    {
      path: 'mvd-property',
      name: 'mvd-property',
      component: () => import('@/views/std-basic/MvdProperty'),
      meta: {
        title: '信息交换模板属性管理',
        keepAlive: false,
        permission: [permissionStdBasic.MVDProperty.Default],
        showInMenu: true,
      },
    },
    {
      path: 'computer-code',
      name: 'computer-code',
      component: () => import('@/views/std-basic/ComputerCode'),
      meta: {
        title: '电算代号管理',
        keepAlive: false,
        permission: [permissionStdBasic.ComputerCodes.Default],
        showInMenu: true,
      },
    },
    {
      path: 'individual-project',
      name: 'individual-project',
      component: () => import('@/views/std-basic/IndividualProject'),
      meta: {
        title: '单项工程管理',
        keepAlive: false,
        permission: [permissionStdBasic.IndividualProjects.Default],
        showInMenu: true,
      },
    },
    {
      path: 'process-template',
      name: 'process-template',
      component: () => import('@/views/std-basic/ProcessTemplate'),
      meta: {
        title: '工序模板管理',
        keepAlive: false,
        permission: [permissionStdBasic.ProcessTemplates.Default],
        showInMenu: true,
      },
    },
    {
      path: 'project-item',
      name: 'project-item',
      component: () => import('@/views/std-basic/ProjectItem'),
      meta: {
        title: '工程工项管理',
        keepAlive: false,
        permission: [permissionStdBasic.ProjectItems.Default],
        showInMenu: true,
      },
    },
    {
      path: 'quota',
      name: 'quota',
      component: () => import('@/views/std-basic/Quota'),
      meta: {
        title: '定额清单管理',
        keepAlive: false,
        permission: [permissionStdBasic.Quotas.Default],
        showInMenu: true,
      },
    },
    {
      path: 'quota-category',
      name: 'quota-category',
      component: () => import('@/views/std-basic/QuotaCategory'),
      meta: {
        title: '定额分类管理',
        keepAlive: false,
        permission: [permissionStdBasic.QuotaCategorys.Default],
        showInMenu: true,
      },
    },
    ...getGroupWithRepairTag(routes),
  ],
};
