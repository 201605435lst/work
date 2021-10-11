import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts';
import permissionCms from 'snweb-module/es/_permissions/sm-cms';
export default {
  path: 'cms',
  name: 'cms',
  component: PageView,
  meta: { title: '栏目文章', keepAlive: true, icon: 'file-text', permission: [permissionCms.GroupName] },
  children: [
    {
      path: 'categories',
      name: 'categories',
      component: () => import('@/views/cms/Categories'),
      meta: { title: '栏目管理', keepAlive: false, permission: [permissionCms.Categories.Default] },
    },
    {
      path: 'category-preview',
      name: 'category-preview',
      hidden: true,
      component: () => import('@/views/cms/Category'),
      meta: { title: '栏目预览', keepAlive: false, permission: [permissionCms.Categories.Detail] },
      props: route => {
        return {
          categoryCode: route.params.categoryCode,
        };
      },
    },
    {
      path: 'articles',
      name: 'articles',
      component: () => import('@/views/cms/Articles'),
      meta: { title: '文章管理', keepAlive: false, permission: [permissionCms.Articles.Default] },
    },
    {
      path: 'article-preview',
      name: 'article-preview',
      hidden: true,
      component: () => import('@/views/cms/Article'),
      meta: { title: '文章预览', keepAlive: false, permission: [permissionCms.Articles.Detail] },
      props: route => {
        return {
          id: route.params.id,
        };
      },
    },
    {
      path: 'categories-rlt-articles',
      name: 'categories-rlt-articles',
      component: () => import('@/views/cms/CategoryRltArticles'),
      meta: { title: '栏目文章管理', keepAlive: false, permission: [permissionCms.CategoriesRltArticles.Default] },
    },
  ],
};
