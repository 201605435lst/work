// eslint-disable-next-line
import { PageView } from '@/layouts'
import permissionCostmanagement from 'snweb-module/es/_permissions/sm-costmanagement';

export default {
  path: 'costmanagement',
  name: 'costmanagement',
  component: PageView,
  meta: { title: '成本管理', keepAlive: false, icon: 'hdd', permission: [permissionCostmanagement.GroupName] },
  children: [
    {
      path: 'people-cost',
      name: 'people-cost',
      component: () => import('@/views/costmanagement/PeopleCost'),
      meta: {
        title: '人工成本',
        keepAlive: false,
        permission: [permissionCostmanagement.CostPeoples.Default],
        showInMenu: true,
      },
    },
    {
      path: 'other-cost',
      name: 'other-cost',
      component: () => import('@/views/costmanagement/OtherCost'),
      meta: {
        title: '其他成本',
        keepAlive: false,
        permission: [permissionCostmanagement.CostOthers.Default],
        showInMenu: true,
      },
    },
    {
      path: 'costmanagement-contracts',
      name: 'costmanagement-contracts',
      component: () => import('@/views/costmanagement/Contract'),
      meta: {
        title: '合同管理',
        keepAlive: false,
        permission: [permissionCostmanagement.Contracts.Default],
        showInMenu: true,
      },
    },
    {
      path: 'money-list',
      name: 'money-list',
      component: () => import('@/views/costmanagement/MoneyList'),
      meta: {
        title: '资金管理',
        keepAlive: false,
        permission: [permissionCostmanagement.MoneyLists.Default],
        showInMenu: true,
      },
    },
    {
      path: 'capital-report',
      name: 'capital-report',
      component: () => import('@/views/costmanagement/CapitalReport'),
      meta: {
        title: '资金报表',
        keepAlive: false,
        permission: [permissionCostmanagement.MoneyLists.Default],
        showInMenu: true,
      },
    },
    {
      
      path: 'breakeven-analysis',
      name: 'breakeven-analysis',
      component: () => import('@/views/costmanagement/BreakevenAnalysis'),
      meta: {
        title: '盈亏分析',
        keepAlive: false,
        permission: [permissionCostmanagement.MoneyLists.Default],
        showInMenu: true,
      },
    },
  ],
};
