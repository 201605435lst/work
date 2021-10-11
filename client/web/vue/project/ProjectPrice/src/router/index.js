import Vue from 'vue';
import Router from 'vue-router';

import { constantRouterMap } from '@/config/router';
import store from '@/store';
import NProgress from 'nprogress'; // progress bar
import '@/components/NProgress/nprogress.less'; // progress bar custom style
import notification from 'ant-design-vue/es/notification';
import { setDocumentTitle, domTitle } from '@/utils/domUtil';
import config from '@/config/default';
import { name as storeApp, at as atApp } from '@/store/modules/app/types';
import { name as storeRouter, at as atRouter } from '@/store/modules/router/types';
import { requestIsSuccess } from '@/utils/util';

Vue.use(Router);

// hack router push callback
const originalPush = Router.prototype.push;
Router.prototype.push = function push(location, onResolve, onReject) {
  if (onResolve || onReject) return originalPush.call(this, location, onResolve, onReject);
  return originalPush.call(this, location).catch(err => err);
};

// NProgress 配置
NProgress.configure({ showSpinner: false });

// 路由白名单
const whiteList = ['login', 'register', 'registerResult'];
const defaultRoutePath = '/dashboard/workplace';

const router = new Router({
  // mode: 'history',
  base: process.env.BASE_URL,
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRouterMap,
});

router.beforeEach(async (to, from, next) => {
  // 进度条开启载入
  NProgress.start();

  // 设置浏览器标题
  to.meta && typeof to.meta.title !== 'undefined' && setDocumentTitle(`${to.meta.title} - ${domTitle}`);

  // 路由跳转逻辑处理
  if (Vue.ls.get(config.tokenKey)) {
    /* has token */
    if (to.path === '/user/login') {
      next({ path: defaultRoutePath });
      NProgress.done();
    } else {
      if (!store.getters.info) {
        const response = await store.dispatch(storeApp + '/' + atApp.GetInfo);

        if (requestIsSuccess(response)) {
     
          // 根据权限表增加路由表
          store.dispatch(storeRouter + '/' + atRouter.GenerateRoutes, store.getters.permissions);
          router.addRoutes(store.getters.addRouters);

          // 请求带有 redirect 重定向时，登录自动重定向到该地址
          const redirect = decodeURIComponent(from.query.redirect || to.path);
          if (to.path === redirect) {
            // hack方法 确保addRoutes已完成 ,set the replace: true so the navigation will not leave a history record
            next({ ...to, replace: true });
          } else {
            // 跳转到目的路由
            next({ path: redirect });
          }
        } else {
          notification.error({
            message: '错误',
            description: '初始化应用失败，请重试',
          });

          await store.dispatch(storeApp + '/' + atApp.Logout);
          next({ path: '/user/login', query: { redirect: to.fullPath } });
        }

        //  store
        //   .dispatch(storeApp + '/' + atApp.GetInfo)
        //   .then(res => {
        //     const roles = res.data.result && res.data.result.role

        //     store.dispatch(storeRouter + '/' + atRouter.GenerateRoutes, { roles }).then(() => {
        //       // 根据roles权限生成可访问的路由表
        //       // 动态添加可访问路由表
        //       router.addRoutes(store.getters.addRouters)

        //       // 请求带有 redirect 重定向时，登录自动重定向到该地址
        //       const redirect = decodeURIComponent(from.query.redirect || to.path)
        //       if (to.path === redirect) {
        //         // hack方法 确保addRoutes已完成 ,set the replace: true so the navigation will not leave a history record
        //         next({ ...to, replace: true })
        //       } else {
        //         // 跳转到目的路由
        //         next({ path: redirect })
        //       }
        //     })
        //   })
        //   .catch(() => {
        //     notification.error({
        //       message: '错误',
        //       description: '请求用户信息失败，请重试'
        //     })
        //     store.dispatch(storeApp + '/' + atApp.Logout).then(() => {
        //       next({ path: '/user/login', query: { redirect: to.fullPath } })
        //     })
        //   })
      } else {
        next();
      }
    }
  } else {
    if (whiteList.includes(to.name)) {
      // 在免登录白名单，直接进入
      next();
    } else {
      next({ path: '/user/login', query: { redirect: to.fullPath } });
      NProgress.done(); // if current page is login will not trigger afterEach hook, so manually handle it
    }
  }
});

router.afterEach(() => {
  NProgress.done();
});

export default router;
