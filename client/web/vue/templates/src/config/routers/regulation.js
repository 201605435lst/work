// eslint-disable-next-line
import { PageView } from '@/layouts'
import permissionOa from 'snweb-module/es/_permissions/sm-oa';
import { PageState } from '@/common/enums';

export default {
  path: 'regulation',
  name: 'regulation',
  component: PageView,
  meta: {
    title: '制度管理',
    keepAlive: false,
    icon: 'snippets',
    // permission: [permissionOa.GroupName],
  },
  children: [
    {
      path: 'institution',
      name: 'institution',
      component: () => import('@/views/regulation/Institution'),
      meta: {
        title: '全部制度',
        keepAlive: false,
      },
    },
    {
      path: 'regulation-institution-view/:id',
      name: 'regulation-institution-view',
      component: () => import('@/views/regulation/ViewInstitution'),
      meta: {
        title: '查看制度',
        keepAlive: true,
        // permission: [permissionOa.Contracts.Create],
      },
      hidden: true,
      props: route => ({
        pageState: PageState.View,
        id: route.params.id,
      }),
    },
    {
      path: 'regulation-institution-audit/:id',
      name: 'regulation-institution-audit',
      component: () => import('@/views/regulation/AuditedInstitution'),
      meta: {
        title: '制度审核',
        keepAlive: true,
        permission: [permissionOa.Contracts.Update],
      },
      hidden: true,
      props: route => ({
        // pageState: PageState.Edit,
        id: route.params.id,
      }),
    },
  ],
};
