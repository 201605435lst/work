// 人为制造冲突
import { PageView ,FlowLayout} from '@/layouts';
import { PageState } from '@/common/enums';
import * as permission  from 'snweb-module/es/_permissions/sm-construction';
import * as permissionResource  from 'snweb-module/es/_permissions/sm-constuction-base';
export default {
  path: 'construction',
  name: 'construction',
  component: PageView,
  meta: { title: '进度管理', keepAlive: false, icon: 'audit', permission: [permission.GroupName] },
  children: [

    {
      path: 'master-plan',
      name: 'master-plan',
      component: FlowLayout,
      meta: { title: '总体计划管理', keepAlive: false, permission: [permission.GroupName] },
      children:[
        {
          path: 'masterPlanDraw',
          name: 'masterPlanDraw',
          component: () => import('@/views/construction/MasterPlanDraw'),
          meta: {
            title: '总体计划编制',
            keepAlive: false,
            permission: [permission.MasterPlan.Draw],
            showInMenu: true,
          },
        },
        {
          path: 'MasterPlanContentWithGantt/:id',
          name: 'MasterPlanContentWithGantt',
          hidden: true,
          component: () => import('@/views/construction/MasterPlanContentWithGantt'),
          meta: { title: '计划编制', keepAlive: false},
          props: route => {
            return {
              id: route.params.id,
            };
          },
        },
        {
          path: 'masterPlanApprove',
          name: 'masterPlanApprove',
          component: () => import('@/views/construction/MasterPlanApprove'),
          meta: {
            title: '总体计划审批',
            keepAlive: false,
            permission: [permission.MasterPlan.Approve],
            showInMenu: true,
          },
        },
        {
          path: 'masterPlanView',
          name: 'masterPlanView',
          component: () => import('@/views/construction/MasterPlanView'),
          meta: {
            title: '总体计划查询',
            keepAlive: false,
            permission: [permission.MasterPlan.View],
            showInMenu: true,
          },
        },
      ],
    },
    {
      path: 'plan',
      name: 'plan',
      component: FlowLayout,
      meta: { title: '施工计划管理', keepAlive: false, permission: [permission.GroupName] },
      children:[
        {
          path: 'PlanDraw',
          name: 'PlanDraw',
          component: () => import('@/views/construction/PlanDraw'),
          meta: {
            title: '施工计划编制',
            keepAlive: false,
            permission: [permission.Plan.Draw],
            showInMenu: true,
          },
        },
        {
          path: 'PlanContentWithGantt/:id',
          name: 'PlanContentWithGantt',
          hidden: true,
          component: () => import('@/views/construction/PlanContentWithGantt'),
          meta: { title: '计划编制', keepAlive: false},
          props: route => {
            return {
              id: route.params.id,
            };
          },
        },
        {
          path: 'PlanApprove',
          name: 'PlanApprove',
          component: () => import('@/views/construction/PlanApprove'),
          meta: {
            title: '施工计划审批',
            keepAlive: false,
            permission: [permission.Plan.Approve],
            showInMenu: true,
          },
        },
        {
          path: 'PlanView',
          name: 'PlanView',
          component: () => import('@/views/construction/PlanView'),
          meta: {
            title: '施工计划查询',
            keepAlive: false,
            permission: [permission.Plan.View],
            showInMenu: true,
          },
        },
    
      ],
    },
    {
      path: 'dispatch',
      name: 'dispatch',
      component: FlowLayout,
      meta: { title: '派工管理', keepAlive: false, permission: [permission.GroupName] },
      children:[
        {
          path: 'dispatch-templates',
          name: 'dispatch-templates',
          component: () => import('@/views/construction/DispatchTemplates'),
          meta: {
            title: '派工单模板管理',
            keepAlive: false,
            permission: [permission.DispatchTemplate.Default],
            showInMenu: true,
          },
        },
        {
          path: 'dispatchs',
          name: 'dispatchs',
          component: () => import('@/views/construction/Dispatchs'),
          meta: {
            title: '派工管理',
            keepAlive: false,
            permission: [permission.Dispatch.Default],
            showInMenu: true,
          },
        },
        {
          path: 'dispatch-add',
          name: 'dispatch-add',
          hidden:true,
          component: () => import('@/views/construction/Dispatch'),
          meta: {
            title: '添加派工单',
            keepAlive: false,
            permission: [permission.Dispatch.Create],
            showInMenu: true,
          },
          props: route => ({
            pageState: PageState.Add,
          }),
        },
        {
          path: 'dispatch-edit',
          name: 'dispatch-edit',
          hidden:true,
          component: () => import('@/views/construction/Dispatch'),
          meta: {
            title: '编辑派工单',
            keepAlive: false,
            permission: [permission.Dispatch.Update],
            showInMenu: true,
          },
          props: route => ({
            pageState: PageState.Edit,
            id: route.params.id,
          }),
        },
        {
          path: 'dispatch-view',
          name: 'dispatch-view',
          hidden:true,
          component: () => import('@/views/construction/Dispatch'),
          meta: {
            title: '查看派工单',
            keepAlive: false,
            permission: [permission.Dispatch.Detail],
            showInMenu: true,
          },
          props: route => ({
            pageState: PageState.View,
            id: route.params.id,
          }),
        },
        {
          path: 'dispatch-approval',
          name: 'dispatch-approval',
          hidden:true,
          component: () => import('@/views/construction/Dispatch'),
          meta: {
            title: '派工单审批',
            keepAlive: false,
            permission: [permission.Dispatch.Approval],
            showInMenu: true,
          },
          props: route => ({
            pageState: PageState.View,
            id: route.params.id,
            isApprove:true,
          }),
        },
        {
          path: 'dispatchs-approval',
          name: 'dispatchs-approval',
          component: () => import('@/views/construction/DispatchApproval'),
          meta: {
            title: '派工审批',
            keepAlive: false,
            permission: [permission.Dispatch.Default], //todo 这个还没有加权限 ,后面 在加
            showInMenu: true,
          },
        },
        {
          path: 'dispatch-filter',
          name: 'dispatch-filter',
          component: () => import('@/views/construction/DispatchFilter'),
          meta: {
            title: '派工查询',
            keepAlive: false,
            permission: [permission.Dispatch.Default], //todo 这个还没有加权限 ,后面 在加
            showInMenu: true,
          },
        },
      ],
    },
    {
      path: 'dailys',
      name: 'dailys',
      component: FlowLayout,
      meta: { title: '施工日志管理',  keepAlive: false, permission: [permission.GroupName] },
      children:[
        {
          path: 'construction-dailys-template',
          name: 'construction-dailys-template',
          component: () => import('@/views/construction/DailyTemplate'),
          meta: {
            title: '施工日志模板管理 ',
             keepAlive: false,
            permission: [permission.DailyTemplates.Default],
          },
        },
        {
          path: 'construction-dailys',
          name: 'construction-diarys',
          component: () => import('@/views/construction/Dailys'),
          meta: {
            title: '施工日志管理 ',
             keepAlive: false,
            permission: [permission.Dailys.Default],
          },
        },
        {
          path: 'construction-dailys-approval',
          name: 'construction-dailys-approval',
          component: () => import('@/views/construction/DailyApproval'),
          meta: {
            title: '施工日志审批 ',
            keepAlive: false,
            permission: [permission.Dailys.Default],
          },
        },
        {
          path: 'construction-dailys-add',
          name: 'construction-dailys-add',
          component: () => import('@/views/construction/Daily'),
          meta: {
            title: '日志填报',
            keepAlive: false,
            permission: [permission.Dailys.Create],
          },
          hidden: true,
          props: router => ({
            pageState: PageState.Add,
          }),
        },
        {
          path: 'construction-dailys-edit/:id',
          name: 'construction-dailys-edit',
          component: () => import('@/views/construction/Daily'),
          hidden: true,
          meta: {
            title: '编辑日志',
            keepAlive: false,
            permission: [permission.Dailys.Update],
          },
          props: route => {
            return {
              pageState: PageState.Edit,
              id: route.params.id,
            };
          },
        },
        {
          path: 'construction-dailys-approval-approval/:id',
          name: 'construction-dailys-approval-approval',
          component: () => import('@/views/construction/Daily'),
          hidden: true,
          meta: {
            title: '日志审批',
            keepAlive: false,
            permission: [permission.Dailys.Default],
          },
          props: route => ({
            pageState: PageState.Approval,
            id: route.params.id,
          }),
        },
        {
          path: 'construction-dailys-view/:id',
          name: 'construction-dailys-view',
          component: () => import('@/views/construction/Daily'),
          hidden: true,
          meta: {
            title: '日志详情',
            keepAlive: false,
            permission: [permission.Dailys.Detail],
          },
          props: route => ({
            pageState: PageState.View,
            id: route.params.id,
          }),
        },
      ],
    },
    // {
    //   path: 'process',
    //   name: 'process',
    //   component: FlowLayout,
    //   meta: { title: '施工进度', keepAlive: false, permission: [permission.GroupName] },
    //   children:[
    //   ],
    // },
   
  ],
};
