const SmProblemProblemCategories = {
  category: 'Modules',
  type: 'Problem',
  title: 'SmProblemProblemCategories',
  subtitle: '问题分类',
  demos: [
    {
      path: 'sm-problem-problem-categories',
      component: () =>
        import('@/components/sm-problem/sm-problem-problem-categories/demo/index.vue'),
    },
    {
      path: 'sm-problem-problem-categories-cn',
      component: () =>
        import('@/components/sm-problem/sm-problem-problem-categories/demo/index.vue'),
    },
  ],
};
const SmProblemProblemCategoryTreeSelect = {
  category: 'Modules',
  type: 'Problem',
  title: 'SmProblemProblemCategoryTreeSelect',
  subtitle: '问题分类选择框',
  demos: [
    {
      path: 'sm-problem-problem-category-tree-select',
      component: () =>
        import('@/components/sm-problem/sm-problem-problem-category-tree-select/demo/index.vue'),
    },
    {
      path: 'sm-problem-problem-category-tree-select-cn',
      component: () =>
        import('@/components/sm-problem/sm-problem-problem-category-tree-select/demo/index.vue'),
    },
  ],
};
const SmProblemProblems = {
  category: 'Modules',
  type: 'Problem',
  title: 'SmProblemProblems',
  subtitle: '问题管理',
  demos: [
    {
      path: 'sm-problem-problems',
      component: () => import('@/components/sm-problem/sm-problem-problems/demo/index.vue'),
    },
    {
      path: 'sm-problem-problems-cn',
      component: () => import('@/components/sm-problem/sm-problem-problems/demo/index.vue'),
    },
  ],
};

export const modules = {
  SmProblemProblemCategories,
  SmProblemProblemCategoryTreeSelect,
  SmProblemProblems,
};
export const demo = [
  ...SmProblemProblemCategories.demos,
  ...SmProblemProblemCategoryTreeSelect.demos,
  ...SmProblemProblems.demos,
];
