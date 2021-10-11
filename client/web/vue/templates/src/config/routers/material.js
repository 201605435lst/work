import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView, FlowLayout } from '@/layouts';
import { PageState } from '@/common/enums';
import router from '@/router/index';
import permissionMaterial from 'snweb-module/es/_permissions/sm-material';

export default {
  path: 'material',
  name: 'material',
  component: PageView,
  meta: {
    title: '物资管理',
    keepAlive: true,
    icon: 'gold',
    permission: [permissionMaterial.GroupName],
  },
  children: [
    {
      path: 'material-plan-view',
      name: 'material-plan-view',
      component: () => import('@/views/material/MaterialPlanView'),
      meta: {
        title: '物资需求计划',
        keepAlive: false,
        showInMenu: true,
         permission: [permissionMaterial.MaterialPlans.Default],
      },
    },
    {
      path: 'purchase-plan',
      name: 'purchase-plan',
      component: FlowLayout,
      meta: { title: '采购计划管理', keepAlive: false, permission: [permissionMaterial.GroupName] },
      children:[
        {
          path: 'purchase-plan-plan',
          name: 'purchase-plan-plan',
          component: () => import('@/views/material/PurchasePlan'),
          meta: {
            title: '采购计划清单',
            keepAlive: false,
            permission: [permissionMaterial.PurchasePlans.Default],
            showInMenu: true,
          },
        },
        {
          path: 'purchase-plan-approval',
          name: 'purchase-plan-approval',
          component: () => import('@/views/material/PurchasePlanApproval'),
          meta: {
            title: '采购计划审批',
            keepAlive: false,
            permission: [permissionMaterial.PurchasePlans.Default],
            showInMenu: true,
          },
        },
      ],
    },
    {
      path: 'purchase-list',
      name: 'purchase-list',
      component: FlowLayout,
      meta: { title: '采购定单管理', keepAlive: false, permission: [permissionMaterial.GroupName] },
      children:[
        {
          path: 'purchase-list-plan',
          name: 'purchase-list-plan',
          component: () => import('@/views/material/PurchaseList'),
          meta: {
            title: '采购单',
            keepAlive: false,
            permission: [permissionMaterial.PurchaseLists.Default],
            showInMenu: true,
          },
        },
        {
          path: 'purchase-list-approval',
          name: 'purchase-list-approval',
          component: () => import('@/views/material/PurchaseListApproval'),
          meta: {
            title: '采购单审批',
            keepAlive: false,
            permission: [permissionMaterial.PurchaseLists.Default],
            showInMenu: true,
          },
        },
      ],
    },
    {
      path: 'material-acceptance',
      name: 'material-acceptance',
      component: () => import('@/views/material/Acceptance'),
      meta: {
        title: '物资验收',
        keepAlive: false,
        permission: [permissionMaterial.Acceptances.Default],
      },
    },
    {
      path: 'entry-records',
      name: 'entry-records',
      component: () => import('@/views/material/EntryRecords'),
      meta: {
        title: '物资入库管理',
        keepAlive: false,
        permission: [permissionMaterial.EntryRecords.Default],
      },
    },
    {
      path: 'out-records',
      name: 'out-records',
      component: () => import('@/views/material/OutRecords'),
      meta: {
        title: '物资出库管理',
        keepAlive: false,
        permission: [permissionMaterial.OutRecords.Default],
      },
    },
    {
      path: 'material-of-bill',
      name: 'material-of-bill',
      component: () => import('@/views/material/MaterialOfBill'),
      meta: {
        title: '领料单管理',
        keepAlive: false,
        // permission: [permissionMaterial.Acceptances.Default],
      },
    },
    {
      path: 'stock',
      name: 'stock',
      component: FlowLayout,
      meta: { title: '库存管理', keepAlive: false, permission: [permissionMaterial.GroupName] },
      children:[
        {
          path: 'material-inventory',
          name: 'material-inventory',
          component: () => import('@/views/material/Inventory'),
          meta: {
            title: '库存台账',
            keepAlive: false,
            permission: [permissionMaterial.Suppliers.Default],
            showInMenu: true,
          },
        },
      ],
    },
    {
      path: 'data-maintenance',
      name: 'data-maintenance',
      component: FlowLayout,
      meta: { title: '数据维护', keepAlive: false, permission: [permissionMaterial.GroupName] },
      children:[
        {
          path: 'partital',
          name: 'partital',
          component: () => import('@/views/material/Partital'),
          meta: {
            title: '仓库管理',
            keepAlive: false,
            //  permission: [permissionMaterial.Inventorys.Default],
            showInMenu: true,
          },
        },
        {
          path: 'material-materials',
          name: 'material-materials',
          component: () => import('@/views/material/Material'),
          meta: {
            title: '材料维护',
            keepAlive: false,
            // permission: [permissionMaterial.Materials.Default], //要改成Materials
          },
        },
        {
          path: 'material-suppliers',
          name: 'material-suppliers',
          component: () => import('@/views/material/Suppliers'),
          meta: {
            title: '供应商管理',
            keepAlive: false,
            permission: [permissionMaterial.Suppliers.Default],
          },
        },
        {
          path: 'material-supplier-add',
          name: 'material-supplier-add',
          component: () => import('@/views/material/Supplier'),
          hidden: true,
          meta: {
            title: '添加供应商',
            keepAlive: false,
          },
          props: router => ({
            pageState: PageState.Add,
          }),
        },
        {
          path: 'material-supplier-view/:id',
          name: 'material-supplier-view',
          component: () => import('@/views/material/Supplier'),
          hidden: true,
          meta: {
            title: '查看供应商',
            keepAlive: true,
          },
          props: router => ({
            pageState: PageState.View,
            id: router.params.id,
          }),
        },
        {
          path: 'material-supplier-edit/:id',
          name: 'material-supplier-edit',
          component: () => import('@/views/material/Supplier'),
          hidden: true,
          meta: {
            title: '编辑供应商',
            keepAlive: true,
          },
          props: route => {
            return {
              pageState: PageState.Edit,
              id: route.params.id,
            };
          },
        },
      ],
    },
    
    // {
    //   path: 'construction-section',
    //   name: 'construction-section',
    //   component: () => import('@/views/material/ConstructionSection'),
    //   meta: {
    //     title: '施工区段管理',
    //     keepAlive: false,
    //     permission: [permissionMaterial.ConstructionSections.Default],
    //     showInMenu: true,
    //   },
    // },
    // {
    //   path: 'construction-team',
    //   name: 'construction-team',
    //   component: () => import('@/views/material/ConstructionTeam'),
    //   meta: {
    //     title: '施工队管理',
    //     keepAlive: false,
    //     permission: [permissionMaterial.ConstructionTeams.Default],
    //     showInMenu: true,
    //   },
    // },
    // {
    //   path: 'contract',
    //   name: 'contract',
    //   component: () => import('@/views/material/Contract'),
    //   meta: {
    //     title: '物资合同',
    //     keepAlive: false,
    //     // permission: [permissionMaterial.ConstructionTeams.Default],
    //     showInMenu: true,
    //   },
    // },
    // {
    //   path: 'inquire',
    //   name: 'inquire',
    //   component: () => import('@/views/material/Inquire'),
    //   meta: {
    //     title: '物资查询',
    //     keepAlive: false,
    //     permission: [permissionMaterial.Inquires.Default],
    //     showInMenu: true,
    //   },
    // },
   
   
  ],
};
