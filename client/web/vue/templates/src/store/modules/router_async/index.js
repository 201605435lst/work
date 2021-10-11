/**
 * 向后端请求用户的菜单，动态生成路由
 */
import { constantRouterMap } from '@/store/modules/router_async/@/config/router';
import { generatorDynamicRouter } from '@/store/modules/router_async/@/router/generator-routers';
import { mt, at } from './types';

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
    [at.GenerateRoutes]({ commit }, data) {
      return new Promise(resolve => {
        const { token } = data;
        generatorDynamicRouter(token).then(routers => {
          commit(mt.SET_ROUTERS, routers);
          resolve();
        });
      });
    },
  },
};

export default router;
