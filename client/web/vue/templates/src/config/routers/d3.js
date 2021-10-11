import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView, FlowLayout } from '@/layouts';

export default {
  path: 'd3',
  name: 'd3',
  // component: FlowLayout,
  component: () => import('@/views/d3/D3'),
  meta: { title: '3D可视化', keepAlive: false, icon: 'global', permission: [] },
  // children: [
  //   {
  //     path: 'main-d3',
  //     name: 'main-d3',
  //     component: () => import('@/views/d3/D3'),
  //     meta: { title: '3D可视化', keepAlive: false, permission: [] },
  //   },
  // ],
};
