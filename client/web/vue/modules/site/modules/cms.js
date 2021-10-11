const SmCmsCategories = {
  category: 'Modules',
  type: 'Cms',
  title: 'SmCmsCategories',
  subtitle: '栏目管理',
  demos: [
    {
      path: 'sm-cms-categories',
      component: () => import('@/components/sm-cms/sm-cms-categories/demo/index.vue'),
    },
    {
      path: 'sm-cms-categories-cn',
      component: () => import('@/components/sm-cms/sm-cms-categories/demo/index.vue'),
    },
  ],
};
const SmCmsCategory = {
  category: 'Modules',
  type: 'Cms',
  title: 'SmCmsCategory',
  subtitle: '栏目预览',
  demos: [
    {
      path: 'sm-cms-category',
      component: () => import('@/components/sm-cms/sm-cms-category/demo/index.vue'),
    },
    {
      path: 'sm-cms-category-cn',
      component: () => import('@/components/sm-cms/sm-cms-category/demo/index.vue'),
    },
  ],
};
const SmCmsArticles = {
  category: 'Modules',
  type: 'Cms',
  title: 'SmCmsArticles',
  subtitle: '文章管理',
  demos: [
    {
      path: 'sm-cms-articles',
      component: () => import('@/components/sm-cms/sm-cms-articles/demo/index.vue'),
    },
    {
      path: 'sm-cms-articles-cn',
      component: () => import('@/components/sm-cms/sm-cms-articles/demo/index.vue'),
    },
  ],
};
const SmCmsArticle = {
  category: 'Modules',
  type: 'Cms',
  title: 'SmCmsArticle',
  subtitle: '文章预览',
  demos: [
    {
      path: 'sm-cms-article',
      component: () => import('@/components/sm-cms/sm-cms-article/demo/index.vue'),
    },
    {
      path: 'sm-cms-article-cn',
      component: () => import('@/components/sm-cms/sm-cms-article/demo/index.vue'),
    },
  ],
};
const SmCmsCategoryRltArticles = {
  category: 'Modules',
  type: 'Cms',
  title: 'SmCmsCategoryRltArticles',
  subtitle: '栏目文章',
  demos: [
    {
      path: 'sm-cms-category-rlt-articles',
      component: () => import('@/components/sm-cms/sm-cms-category-rlt-articles/demo/index.vue'),
    },
    {
      path: 'sm-cms-category-rlt-articles-cn',
      component: () => import('@/components/sm-cms/sm-cms-category-rlt-articles/demo/index.vue'),
    },
  ],
};

export const modules = {
  SmCmsCategories,
  SmCmsCategory,
  SmCmsArticles,
  SmCmsArticle,
  SmCmsCategoryRltArticles,
};
export const demo = [
  ...SmCmsCategories.demos,
  ...SmCmsCategory.demos,
  ...SmCmsArticles.demos,
  ...SmCmsArticle.demos,
  ...SmCmsCategoryRltArticles.demos,
];
