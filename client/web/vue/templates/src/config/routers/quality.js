// eslint-disable-next-line
import { PageView } from '@/layouts'
import permissionQuality from 'snweb-module/es/_permissions/sm-quality';

export default {
  path: 'quality',
  name: 'quality',
  component: PageView,
  meta: { title: '质量问题', keepAlive: false, icon: 'snippets', permission: [permissionQuality.GroupName] },
  children: [
    {
      path: 'quality-problem',
      name: 'quality-problem',
      component: () => import('@/views/quality/QualityProblem'),
      meta: {
        title: '质量问题管理',
        keepAlive: false,
        permission: [permissionQuality.QualityProblems.Default],
        showInMenu: true,
      },
    },
    {
      path: 'quality-problem-libraries',
      name: 'quality-problem-libraries',
      component: () => import('@/views/quality/QualityProblemLibraries'),
      meta: {
        title: '质量问题库管理',
        keepAlive: false,
        permission: [permissionQuality.QualityProblemLibraries.Default],
        showInMenu: true,
      },
    },
  ],
};
