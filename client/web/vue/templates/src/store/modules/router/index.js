import { asyncRouterMap, constantRouterMap } from '@/config/router';
import { mt, at } from './types';

/**
 * 过滤账户是否拥有某一个权限，并将菜单从加载列表移除
 *
 * @param permissions
 * @param route
 * @returns {boolean}
 */
function hasPermission(permissions, route) {
  if (route.meta && route.meta.permission && route.meta.permission.length) {
    let flag = false;
    for (let i = 0, len = permissions.length; i < len; i++) {
      flag = route.meta.permission.includes(permissions[i]);
      if (flag) {
        return true;
      }
    }
    return false;
  }
  return true;
}

/**
 * 单账户多角色时，使用该方法可过滤角色不存在的菜单
 *
 * @param roles
 * @param route
 * @returns {*}
 */
// eslint-disable-next-line
function hasRole(roles, route) {
  if (route.meta && route.meta.roles) {
    return route.meta.roles.includes(roles.id);
  } else {
    return true;
  }
}

function filterAsyncRouter(routerMap, permissions) {
  const accessedRouters = routerMap.filter(route => {
    // console.log(route);
    if (hasPermission(permissions, route)) {
      if (route.children && route.children.length) {
        route.children = filterAsyncRouter(route.children, permissions);
      }
      return true;
    }
    return false;
  });
  return accessedRouters;
}

// function filterAsyncRouter (routerMap, roles) {
//   const accessedRouters = routerMap.filter(route => {
//     if (hasPermission(roles.permissionList, route)) {
//       if (route.children && route.children.length) {
//         route.children = filterAsyncRouter(route.children, roles)
//       }
//       return true
//     }
//     return false
//   })
//   return accessedRouters
// }

const router = {
  namespaced: true,
  state: {
    routers: constantRouterMap,
    addRouters: [],
  },
  mutations: {
    [mt.SET_ROUTERS]: (state, routers) => {
      state.addRouters = routers;
      state.routers = constantRouterMap.concat(routers);
    },
  },
  actions: {
    [at.GenerateRoutes]({ commit }, permissions) {
      commit(mt.SET_ROUTERS, filterAsyncRouter(asyncRouterMap, permissions));
    },
  },
};

export default router;
