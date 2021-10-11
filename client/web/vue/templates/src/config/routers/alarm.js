import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts';
import { PageState } from '@/common/enums';

export default {
  path: 'alarm',
  name: 'alarm',
  component: PageView,
  meta: {
    title: '集中告警',
    keepAlive: true,
    icon: 'alert',
    permission: [],
  },
  children: [
    {
      path: 'alarms',
      name: 'alarms',
      component: () => import('@/views/alarm/Alarms'),
      meta: {
        title: '告警管理',
        keepAlive: false,
      },
    },
  ],
};
