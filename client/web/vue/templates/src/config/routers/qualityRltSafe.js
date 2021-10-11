// eslint-disable-next-line
import { PageView } from '@/layouts'
import permissionQuality from 'snweb-module/es/_permissions/sm-quality';
import permissionSafe from 'snweb-module/es/_permissions/sm-safe';

export default {
  path: 'quality-rlt-safe',
  name: 'quality-rlt-safe',
  component: PageView,
  meta: { title: '质量安全风险管理', keepAlive: false, icon: 'snippets', permission: [permissionQuality.GroupName,permissionSafe.GroupName] },
  children: [
    {
      path: 'quality-rlt-safe-quality-problem',
      name: 'quality-rlt-safe-quality-problem',
      component: () => import('@/views/quality/QualityProblem'),
      meta: {
        title: '质量问题管理',
        keepAlive: false,
        permission: [permissionQuality.QualityProblems.Default],
        showInMenu: true,
      },
    },
   
    {
        path: 'quality-rlt-safe-safe-problem',
        name: 'quality-rlt-safe-safe-problem',
        component: () => import('@/views/safe/SafeProblem'),
        meta: {
          title: '安全问题管理',
          keepAlive: false,
          permission: [permissionSafe.SafeProblems.Default],
          showInMenu: true,
        },
      },
      {
        path: 'quality-rlt-safe-quality-problem-libraries',
        name: 'quality-rlt-safe-quality-problem-libraries',
        component: () => import('@/views/quality/QualityProblemLibraries'),
        meta: {
          title: '质量问题库管理',
          keepAlive: false,
          permission: [permissionQuality.QualityProblemLibraries.Default],
          showInMenu: true,
        },
      },
      {
        path: 'quality-rlt-safe-safe-problem-library',
        name: 'quality-rlt-safe-safe-problem-library',
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
