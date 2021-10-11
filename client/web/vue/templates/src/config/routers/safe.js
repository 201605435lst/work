// eslint-disable-next-line
import { PageView } from '@/layouts'
import permissionSafe from 'snweb-module/es/_permissions/sm-safe';

export default {
  path: 'safe',
  name: 'safe',
  component: PageView,
  meta: { title: '安全问题', keepAlive: false, icon: 'hdd', permission: [permissionSafe.GroupName] },
  children: [
    {
      path: 'safe-problem',
      name: 'safe-problem',
      component: () => import('@/views/safe/SafeProblem'),
      meta: {
        title: '安全问题管理',
        keepAlive: false,
        permission: [permissionSafe.SafeProblems.Default],
        showInMenu: true,
      },
    },
    {
      path: 'safe-problem-library',
      name: 'safe-problem-library',
      component: () => import('@/views/safe/SafeProblemLibrary'),
      meta: {
        title: '安全问题库管理',
        keepAlive: false,
        permission: [permissionSafe.SafeProblemLibrarys.Default],
        showInMenu: true,
      },
    },
  ],
};
