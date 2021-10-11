const SmQualityProblem = {
  category: 'Modules',
  type: 'Quality',
  title: 'SmQualityProblems',
  subtitle: '质量问题管理',
  demos: [
    {
      path: 'sm-quality-problems',
      component: () => import('@/components/sm-quality/sm-quality-problems/demo/index.vue'),
    },
    {
      path: 'sm-quality-problems-cn',
      component: () => import('@/components/sm-quality/sm-quality-problems/demo/index.vue'),
    },
  ],
};
const SmQualityProblemLibrary = {
  category: 'Modules',
  type: 'Quality',
  title: 'SmQualityProblemLibraries',
  subtitle: '质量问题库管理',
  demos: [
    {
      path: 'sm-quality-problem-libraries',
      component: () => import('@/components/sm-quality/sm-quality-problem-libraries/demo/index.vue'),
    },
    {
      path: 'sm-quality-problem-libraries-cn',
      component: () => import('@/components/sm-quality/sm-quality-problem-libraries/demo/index.vue'),
    },
  ],
};

export const modules = {
  SmQualityProblem,
  SmQualityProblemLibrary,
};
export const demo = [...SmQualityProblem.demos, ...SmQualityProblemLibrary.demos];
