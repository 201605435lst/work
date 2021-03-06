// eslint-disable-next-line
import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts'
import system from './routers/system';
import file from './routers/file';
import bpm from './routers/bpm';
import basic from './routers/basic';
import resource from './routers/resource';
import crPlanMonthYear from './routers/cr-plan/cr-plan-month-year';
import crPlanManage from './routers/cr-plan/cr-plan-manage';
import crPlanWorks from './routers/cr-plan/cr-plan-works';
import crPlanStatistics from './routers/cr-plan/cr-plan-statistics';
import report from './routers/report';
import cms from './routers/cms';
import emerg from './routers/emerg';
import d3 from './routers/d3';
import oa from './routers/oa';
import config from './default';
import dashboard from './routers/dashboard';
import stdBasic from './routers/std-basic';
import alarm from './routers/alarm';
import problem from './routers/problem';
import regulation from './routers/regulation';
import project from './routers/project';
import task from './routers/task';
import material from './routers/material';
import schedule from './routers/schedule';
import technology from './routers/technology';
import costmanagement from './routers/costmanagement';
import safe from './routers/safe';
import quality from './routers/quality';
import constructionBase from './routers/construction-base';
import componenttrack from './routers/componenttrack';
import construction from './routers/construction';
import basicData from './routers/basicData';
import interfaceManage from './routers/interfaceManage';
import qualityRltSafe from './routers/qualityRltSafe';

let routers = [
  d3,
  dashboard,
  system,
  file,
  oa,
  report,
  bpm,
  cms,
  // basic,
  // resource,
  stdBasic,
  crPlanMonthYear,
  crPlanManage,
  crPlanWorks,
  crPlanStatistics,
  emerg,
  project,
  alarm,
  problem,
  regulation,
  task,
  material,
  costmanagement,
  technology,
  // safe,
  // quality,
  construction,
  basicData,
  interfaceManage,
  qualityRltSafe,
];

export const asyncRouterMap = [
  //  {
  //    path: '/changPassWord',
  //    name: 'changPassWord',
  //    component: () => import('@/views/system/ChangePassWord'),
  //    hidden: true,
  //    meta: {
  //      title: '????????????',
  //      keepAlive: false,
  //      permission: [],
  //    },
  //  },
  {
    path: '/',
    name: 'index',
    component: BasicLayout,
    meta: { title: '??????' },
    redirect: { path: config.homePath }, //users
    children: [
      ...routers,
      // // dashboard
      // {
      //   path: 'dashboard',
      //   name: 'dashboard',
      //   redirect: '/dashboard/workplace',
      //   component: RouteView,
      //   meta: { title: '?????????', keepAlive: , icon: bxAnaalyse, permission: ['dashboard'] },
      //   children: [
      //     {
      //       path: 'analysis/:pageNo([1-9]\\d*)?',
      //       name: 'Analysis',
      //       component: () => import('@/views/dashboard/Analysis'),
      //       meta: { title: '?????????', keepAlive: false, permission: ['dashboard'] },
      //     },
      //     // ????????????
      //     {
      //       path: 'https://www.baidu.com/',
      //       name: 'Monitor',
      //       meta: { title: '?????????????????????', target: '_blank' },
      //     },
      //     {
      //       path: 'workplace',
      //       name: 'Workplace',
      //       component: () => import('@/views/dashboard/Workplace'),
      //       meta: { title: '?????????', keepAlive: true, permission: ['dashboard'] },
      //     },
      //     {
      //       path: 'test-work',
      //       name: 'TestWork',
      //       component: () => import('@/views/dashboard/TestWork'),
      //       meta: { title: '????????????', keepAlive: true, permission: ['dashboard'] },
      //     },
      //   ],
      // },

      // // develop
      // {
      //   path: '/develop',
      //   redirect: '/develop',
      //   component: PageView,
      //   meta: { title: '??????', icon: 'form' },
      //   children: [
      //     {
      //       path: 'test',
      //       name: 'test',
      //       component: () => import('@/views/develop/Test'),
      //       meta: { title: '??????', keepAlive: true },
      //     },
      //     {
      //       path: 'testJsx',
      //       name: 'testJsx',
      //       component: () => import('@/views/develop/TestJsx.jsx'),
      //       meta: { title: '??????Jsx', keepAlive: true },
      //     },
      //   ],
      // },

      // // forms
      // {
      //   path: '/form',
      //   redirect: '/form/base-form',
      //   component: PageView,
      //   meta: { title: '?????????', icon: 'form', permission: ['form'] },
      //   children: [
      //     {
      //       path: '/form/base-form',
      //       name: 'BaseForm',
      //       component: () => import('@/views/form/BasicForm'),
      //       meta: { title: '????????????', keepAlive: true, permission: ['form'] },
      //     },
      //     {
      //       path: '/form/step-form',
      //       name: 'StepForm',
      //       component: () => import('@/views/form/stepForm/StepForm'),
      //       meta: { title: '????????????', keepAlive: true, permission: ['form'] },
      //     },
      //     {
      //       path: '/form/advanced-form',
      //       name: 'AdvanceForm',
      //       component: () => import('@/views/form/advancedForm/AdvancedForm'),
      //       meta: { title: '????????????', keepAlive: true, permission: ['form'] },
      //     },
      //   ],
      // },

      // // list
      // {
      //   path: '/list',
      //   name: 'list',
      //   component: PageView,
      //   redirect: '/list/table-list',
      //   meta: { title: '?????????', icon: 'table', permission: ['table'] },
      //   children: [
      //     {
      //       path: '/list/table-list/:pageNo([1-9]\\d*)?',
      //       name: 'TableListWrapper',
      //       hideChildrenInMenu: true, // ???????????? MenuItem ????????? SubMenu
      //       component: () => import('@/views/list/TableList'),
      //       meta: { title: '????????????', keepAlive: true, permission: ['table'] },
      //     },
      //     {
      //       path: '/list/basic-list',
      //       name: 'BasicList',
      //       component: () => import('@/views/list/StandardList'),
      //       meta: { title: '????????????', keepAlive: true, permission: ['table'] },
      //     },
      //     {
      //       path: '/list/card',
      //       name: 'CardList',
      //       component: () => import('@/views/list/CardList'),
      //       meta: { title: '????????????', keepAlive: true, permission: ['table'] },
      //     },
      //     {
      //       path: '/list/search',
      //       name: 'SearchList',
      //       component: () => import('@/views/list/search/SearchLayout'),
      //       redirect: '/list/search/article',
      //       meta: { title: '????????????', keepAlive: true, permission: ['table'] },
      //       children: [
      //         {
      //           path: '/list/search/article',
      //           name: 'SearchArticles',
      //           component: () => import('../views/list/search/Article'),
      //           meta: { title: '????????????????????????', permission: ['table'] },
      //         },
      //         {
      //           path: '/list/search/project',
      //           name: 'SearchProjects',
      //           component: () => import('../views/list/search/Projects'),
      //           meta: { title: '????????????????????????', permission: ['table'] },
      //         },
      //         {
      //           path: '/list/search/application',
      //           name: 'SearchApplications',
      //           component: () => import('../views/list/search/Applications'),
      //           meta: { title: '????????????????????????', permission: ['table'] },
      //         },
      //       ],
      //     },
      //   ],
      // },

      // // profile
      // {
      //   path: '/profile',
      //   name: 'profile',
      //   component: RouteView,
      //   redirect: '/profile/basic',
      //   meta: { title: '?????????', icon: 'profile', permission: ['profile'] },
      //   children: [
      //     {
      //       path: '/profile/basic',
      //       name: 'ProfileBasic',
      //       component: () => import('@/views/profile/basic/Index'),
      //       meta: { title: '???????????????', permission: ['profile'] },
      //     },
      //     {
      //       path: '/profile/advanced',
      //       name: 'ProfileAdvanced',
      //       component: () => import('@/views/profile/advanced/Advanced'),
      //       meta: { title: '???????????????', permission: ['profile'] },
      //     },
      //   ],
      // },

      // // result
      // {
      //   path: '/result',
      //   name: 'result',
      //   component: PageView,
      //   redirect: '/result/success',
      //   meta: { title: '?????????', icon: 'check-circle-o', permission: ['result'] },
      //   children: [
      //     {
      //       path: '/result/success',
      //       name: 'ResultSuccess',
      //       component: () => import(/* webpackChunkName: "result" */ '@/views/result/Success'),
      //       meta: { title: '??????', keepAlive: false, hiddenHeaderContent: true, permission: ['result'] },
      //     },
      //     {
      //       path: '/result/fail',
      //       name: 'ResultFail',
      //       component: () => import(/* webpackChunkName: "result" */ '@/views/result/Error'),
      //       meta: { title: '??????', keepAlive: false, hiddenHeaderContent: true, permission: ['result'] },
      //     },
      //   ],
      // },

      // // Exception
      // {
      //   path: '/exception',
      //   name: 'exception',
      //   component: RouteView,
      //   redirect: '/exception/403',
      //   meta: { title: '?????????', icon: 'warning', permission: ['exception'] },
      //   children: [
      //     {
      //       path: '/exception/403',
      //       name: 'Exception403',
      //       component: () => import(/* webpackChunkName: "fail" */ '@/views/exception/403'),
      //       meta: { title: '403', permission: ['exception'] },
      //     },
      //     {
      //       path: '/exception/404',
      //       name: 'Exception404',
      //       component: () => import(/* webpackChunkName: "fail" */ '@/views/exception/404'),
      //       meta: { title: '404', permission: ['exception'] },
      //     },
      //     {
      //       path: '/exception/500',
      //       name: 'Exception500',
      //       component: () => import(/* webpackChunkName: "fail" */ '@/views/exception/500'),
      //       meta: { title: '500', permission: ['exception'] },
      //     },
      //   ],
      // },

      // // account
      // {
      //   path: '/account',
      //   component: RouteView,
      //   redirect: '/account/center',
      //   name: 'account',
      //   meta: { title: '?????????', icon: 'user', keepAlive: true, permission: ['user'] },
      //   children: [
      //     {
      //       path: '/account/center',
      //       name: 'center',
      //       component: () => import('@/views/account/center/Index'),
      //       meta: { title: '????????????', keepAlive: true, permission: ['user'] },
      //     },
      //     {
      //       path: '/account/settings',
      //       name: 'settings',
      //       component: () => import('@/views/account/settings/Index'),
      //       meta: { title: '????????????', hideHeader: true, permission: ['user'] },
      //       redirect: '/account/settings/base',
      //       hideChildrenInMenu: true,
      //       children: [
      //         {
      //           path: '/account/settings/base',
      //           name: 'BaseSettings',
      //           component: () => import('@/views/account/settings/BaseSetting'),
      //           meta: { title: '????????????', hidden: true, permission: ['user'] },
      //         },
      //         {
      //           path: '/account/settings/security',
      //           name: 'SecuritySettings',
      //           component: () => import('@/views/account/settings/Security'),
      //           meta: { title: '????????????', hidden: true, keepAlive: true, permission: ['user'] },
      //         },
      //         {
      //           path: '/account/settings/custom',
      //           name: 'CustomSettings',
      //           component: () => import('@/views/account/settings/Custom'),
      //           meta: { title: '???????????????', hidden: true, keepAlive: true, permission: ['user'] },
      //         },
      //         {
      //           path: '/account/settings/binding',
      //           name: 'BindingSettings',
      //           component: () => import('@/views/account/settings/Binding'),
      //           meta: { title: '????????????', hidden: true, keepAlive: true, permission: ['user'] },
      //         },
      //         {
      //           path: '/account/settings/notification',
      //           name: 'NotificationSettings',
      //           component: () => import('@/views/account/settings/Notification'),
      //           meta: { title: '???????????????', hidden: true, keepAlive: true, permission: ['user'] },
      //         },
      //       ],
      //     },
      //   ],
      // },

      // // other
      // {
      //   path: '/other',
      //   name: 'otherPage',
      //   component: PageView,
      //   meta: { title: '????????????', icon: 'slack', permission: ['dashboard'] },
      //   redirect: '/other/icon-selector',
      //   children: [
      //     {
      //       path: '/other/icon-selector',
      //       name: 'TestIconSelect',
      //       component: () => import('@/views/other/IconSelectorView'),
      //       meta: { title: 'IconSelector', icon: 'tool', keepAlive: true, permission: ['dashboard'] },
      //     },
      //     {
      //       path: '/other/list',
      //       component: RouteView,
      //       meta: { title: '????????????', icon: 'layout', permission: ['support'] },
      //       redirect: '/other/list/tree-list',
      //       children: [
      //         {
      //           path: '/other/list/tree-list',
      //           name: 'TreeList',
      //           component: () => import('@/views/other/TreeList'),
      //           meta: { title: '???????????????', keepAlive: true },
      //         },
      //         {
      //           path: '/other/list/edit-table',
      //           name: 'EditList',
      //           component: () => import('@/views/other/TableInnerEditList'),
      //           meta: { title: '??????????????????', keepAlive: true },
      //         },
      //         {
      //           path: '/other/list/user-list',
      //           name: 'UserList',
      //           component: () => import('@/views/other/UserList'),
      //           meta: { title: '????????????', keepAlive: true },
      //         },
      //         {
      //           path: '/other/list/role-list',
      //           name: 'RoleList',
      //           component: () => import('@/views/other/RoleList'),
      //           meta: { title: '????????????', keepAlive: true },
      //         },
      //         {
      //           path: '/other/list/system-role',
      //           name: 'SystemRole',
      //           component: () => import('@/views/role/RoleList'),
      //           meta: { title: '????????????2', keepAlive: true },
      //         },
      //         {
      //           path: '/other/list/permission-list',
      //           name: 'PermissionList',
      //           component: () => import('@/views/other/PermissionList'),
      //           meta: { title: '????????????', keepAlive: true },
      //         },
      //       ],
      //     },
      //   ],
      // },
    ],
  },
  {
    path: '*',
    redirect: { path: config.homePath },
    hidden: true,
  },
];

/**
 * ????????????
 * @type { *[] }
 */
export const constantRouterMap = [
         {
           path: '/user',
           component: UserLayout,
           redirect: '/user/login',
           hidden: true,
           children: [
             {
               path: 'login/:username?/:password?', //  path: 'login',path: 'login/:username?/:password?',
               name: 'login',
               component: () => import(/* webpackChunkName: "user" */ '@/views/user/Login'),
             },
             {
               path: 'register',
               name: 'register',
               component: () => import(/* webpackChunkName: "user" */ '@/views/user/Register'),
             },
             {
               path: 'register-result',
               name: 'registerResult',
               component: () => import(/* webpackChunkName: "user" */ '@/views/user/RegisterResult'),
             },
             {
               path: 'recover',
               name: 'recover',
               component: undefined,
             },
           ],
         },
         {
           path: '/share',
           component: BlankLayout,
           //  redirect: '/share/share_BZT_YXK',
           hidden: true,
           children: [
             {
               path: '/share/share_BZT', //  path: 'login',path: 'login/:username?/:password?',
               name: 'share_BZT',
               component: () => import(/* webpackChunkName: "user" */ '@/views/share/ShareBztYxk'),
             },
             {
               path: '/share/share_AllStatistic', //  path: 'login',path: 'login/:username?/:password?',
               name: 'share_AllStatistic',
               component: () => import(/* webpackChunkName: "user" */ '@/views/share/SahreAllStatistics'),
             },
             {
               path: '/share/share_BZT_GTK', //  path: 'login',path: 'login/:username?/:password?',
               name: 'share_BZT_GTK',
               component: () => import(/* webpackChunkName: "user" */ '@/views/share/ShareBztYxk'),
             },
           ],
         },
         {
           path: '/404',
           component: () => import(/* webpackChunkName: "fail" */ '@/views/exception/404'),
         },
       ];
