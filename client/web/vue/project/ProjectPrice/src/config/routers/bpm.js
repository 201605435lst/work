// eslint-disable-next-line
import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts'
import { PageState, UserWorkflowGroup, WorkflowState } from '@/common/enums';
import bpmPermissions from 'snweb-module/es/_permissions/sm-bpm.js';
export default {
  path: 'bpm',
  name: 'bpm',
  component: PageView,
  meta: { title: '流程审批', keepAlive: true, icon: 'branches', permission: [bpmPermissions.GroupName] },
  children: [
    {
      path: 'workflow-template-add',
      name: 'workflow-template-add',
      component: () => import('@/views/bpm/WorkflowTemplate'),
      meta: {
        title: '添加流程',
        keepAlive: true,
        permission: [bpmPermissions.WorkflowTemplate.Create],
        icon: 'plus',
      },
      props: route => ({ pageState: PageState.Add }),
    },
    {
      // path 不能相同
      path: 'workflow-template-edit/:id',
      // name 不能相同
      name: 'workflow-template-edit',
      component: () => import('@/views/bpm/WorkflowTemplate'),
      // hidden 为true，表示在左侧导航栏不显示
      hidden: true,
      meta: { title: '编辑流程', keepAlive: true, permission: [] },
      /**
       *  props 自动映射路由参数为组件的 props 属性
       *  其中：
       *  pageState: PageState.Edit 为路由固有参数，
       *  id等其他为路由动态参数，动态参数需要在跳转的时候传入，
       *  this.$router.push({
            name: 'workflow-edit',
            params: {
              id,
            },
          });
       */
      props: route => ({ pageState: PageState.Edit, id: route.params.id }),
    },
    {
      path: 'workflow-template-view/:id',
      name: 'workflow-template-view',
      component: () => import('@/views/bpm/WorkflowTemplate'),
      hidden: true,
      meta: { title: '查看流程', keepAlive: false, permission: [] },
      props: route => ({ pageState: PageState.View, id: route.params.id }),
    },
    {
      path: 'workflow-templates',
      name: 'workflow-templates',
      component: () => import('@/views/bpm/WorkflowTemplates'),
      meta: {
        title: '流程管理',
        keepAlive: false,
        permission: [bpmPermissions.WorkflowTemplate.Default],
        icon: 'unordered-list',
      },
    },
    {
      path: 'workflows',
      name: 'workflows',
      component: () => import('@/views/bpm/WorkflowTemplates'),
      meta: { title: '待我发起', keepAlive: false, permission: [], icon: 'appstore' },
      props: route => ({ forCurrentUser: true }),
    },
    {
      path: 'workflows-initial',
      name: 'workflows-initial',
      component: () => import('@/views/bpm/Workflows'),
      meta: { title: '我发起的', keepAlive: false, permission: [], icon: 'alert' },
      props: route => ({ group: UserWorkflowGroup.Initial }),
    },
    {
      path: 'workflows-waiting',
      name: 'workflows-waiting',
      component: () => import('@/views/bpm/Workflows'),
      meta: { title: '待我审批', keepAlive: false, permission: [], icon: 'clock-circle' },
      props: route => ({ group: UserWorkflowGroup.Waiting }),
    },
    {
      path: 'workflows-approved',
      name: 'workflows-approved',
      component: () => import('@/views/bpm/Workflows'),
      meta: { title: '我已审批', keepAlive: false, permission: [], icon: 'check' },
      props: route => ({ group: UserWorkflowGroup.Approved }),
    },
    {
      path: 'workflows-cc',
      name: 'workflows-cc',
      component: () => import('@/views/bpm/Workflows'),
      meta: { title: '抄送我的', keepAlive: false, permission: [], icon: 'message' },
      props: route => ({ group: UserWorkflowGroup.Cc }),
    },
  ],
};
