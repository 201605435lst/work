import Vue from 'vue';
import Vuex from 'vuex';

import app from './modules/app';
import router from './modules/router';
// import router from './modules/router_async'

import getters from './getters';

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    app,
    router,
  },
  state: {},
  mutations: {},
  actions: {},
  getters,
});
