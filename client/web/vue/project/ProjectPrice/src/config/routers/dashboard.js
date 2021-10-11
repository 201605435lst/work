import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView, FlowLayout } from '@/layouts';

export default {
  path: 'dashboard',
  name: 'dashboard',
  // component: FlowLayout,
  component: () => import('@/views/dashboard/Dashboard'),
  meta: { title: '工作看板', keepAlive: false, icon: 'dashboard', permission: [] },
  // children: [
  //   {
  //     path: 'main-d3',
  //     name: 'main-d3',
  //     component: () => import('@/views/d3/D3'),
  //     meta: { title: '3D可视化', keepAlive: false, permission: [] },
  //   },
  // ],
};
