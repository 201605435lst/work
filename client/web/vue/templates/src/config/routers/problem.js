import { PageView } from '@/layouts';
import permissionProblem from 'snweb-module/es/_permissions/sm-problem';
export default {
  path: 'problem',
  name: 'problem',
  component: PageView,
  meta: { title: '问题库', keepAlive: true, icon: 'question-circle', permission: [permissionProblem.GroupName] },
  children: [
    {
      path: 'problem-categories',
      name: 'problem-categories',
      component: () => import('@/views/problem/ProblemCategories'),
      meta: { title: '问题分类', keepAlive: false, permission: [permissionProblem.Problems.Default] },
    },
    {
      path: 'problems',
      name: 'problems',
      component: () => import('@/views/problem/Problems'),
      meta: { title: '问题管理', keepAlive: false, permission: [permissionProblem.ProblemCategories.Default] },
    },
  ],
};
