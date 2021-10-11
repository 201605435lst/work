import Vue from 'vue';
import { mt, at } from './types';
import * as apiApp from '@/api/app';
import ApiUser from 'snweb-module/es/sm-api/sm-system/User';
import ApiOrganization from 'snweb-module/es/sm-api/sm-system/Organization';
import { axios } from '@/utils/axios';
import config from '@/config/default';
import { requestIsSuccess } from '@/utils/util';
import defaultConfig from '@/config/default';
import router from '@/router';

let apiUser = new ApiUser(axios);
let apiOrganization = new ApiOrganization(axios);

import {
  SIDEBAR_VISIBLE,
  DEFAULT_THEME,
  DEFAULT_LAYOUT_MODE,
  DEFAULT_COLOR,
  DEFAULT_COLOR_WEAK,
  DEFAULT_FIXED_HEADER,
  DEFAULT_FIXED_SIDEMENU,
  DEFAULT_FIXED_HEADER_HIDDEN,
  DEFAULT_CONTENT_WIDTH_TYPE,
  DEFAULT_MULTI_TAB,
} from '@/common/consts';

const app = {
  namespaced: true,
  state: {
    // 前端配置
    sidebarVisible: true,
    device: 'desktop',
    themeDrawerVisible: false,
    theme: '',
    layout: '',
    contentWidth: '',
    fixedHeader: false,
    fixedSidebar: false,
    autoHideHeader: false,
    color: null,
    weak: false,
    multiTab: true,

    // abp 配置
    info: null,

    // template
    token: '',
    name: '',
    welcome: '',
    avatar: '',
    roles: [],
    permissions: [],
    projectIds: [],
    scope: null,
  },
  mutations: {
    [mt.SetSidebarVisible]: (state, visible) => {
      state.sidebarVisible = visible;
      Vue.ls.set(SIDEBAR_VISIBLE, visible);
    },

    [mt.CloseSidebar]: state => {
      Vue.ls.set(SIDEBAR_VISIBLE, true);
      state.sidebarVisible = false;
    },

    [mt.ToggleDevice]: (state, device) => {
      state.device = device;
    },

    [mt.ToggleThemeDrawerVisible]: (state, visible) => {
      window.dispatchEvent(new Event('resize'));
      state.themeDrawerVisible = visible;
    },

    [mt.ToggleTheme]: (state, theme) => {
      Vue.ls.set(DEFAULT_THEME, theme);
      state.theme = theme;
    },

    [mt.ToggleLayoutMode]: (state, layout) => {
      Vue.ls.set(DEFAULT_LAYOUT_MODE, layout);
      state.layout = layout;
    },

    [mt.ToggleFixedHeader]: (state, fixed) => {
      Vue.ls.set(DEFAULT_FIXED_HEADER, fixed);
      state.fixedHeader = fixed;
    },

    [mt.ToggleFixedSiderbar]: (state, fixed) => {
      Vue.ls.set(DEFAULT_FIXED_SIDEMENU, fixed);
      state.fixedSidebar = fixed;
    },

    [mt.ToggleFixedHeaderHidden]: (state, show) => {
      Vue.ls.set(DEFAULT_FIXED_HEADER_HIDDEN, show);
      state.autoHideHeader = show;
    },

    [mt.ToggleContentWidth]: (state, type) => {
      Vue.ls.set(DEFAULT_CONTENT_WIDTH_TYPE, type);
      state.contentWidth = type;
    },

    [mt.ToggleColor]: (state, color) => {
      Vue.ls.set(DEFAULT_COLOR, color);
      state.color = color;
    },

    [mt.ToggleWeak]: (state, flag) => {
      Vue.ls.set(DEFAULT_COLOR_WEAK, flag);
      state.weak = flag;
    },

    [mt.ToggleMultiTab]: (state, bool) => {
      Vue.ls.set(DEFAULT_MULTI_TAB, bool);
      state.multiTab = bool;
    },

    [mt.SetToken]: (state, token) => {
      state.token = token;
    },

    [mt.SetName]: (state, { name, welcome }) => {
      state.name = name;
      state.welcome = welcome;
    },

    [mt.SetAvatar]: (state, avatar) => {
      state.avatar = avatar;
    },

    [mt.SetRoles]: (state, roles) => {
      state.roles = roles;
    },

    [mt.SetPermissions]: (state, permissions) => {
      state.permissions = permissions;
    },

    [mt.SetProjectIds]: (state, projectIds) => {
      state.projectIds = projectIds;
    },

    [mt.SetInfo]: (state, info) => {
      state.info = info;
    },

    [mt.SetScope]: (state, scope) => {
      state.scope = scope;
    },
  },
  actions: {
    [at.SetSidebar]({ commit }, visible) {
      commit(mt.SetSidebarVisible, visible);
    },

    [at.CloseSidebar]({ commit }) {
      commit(mt.CloseSidebar);
    },

    [at.ToggleDevice]({ commit }, device) {
      commit(mt.ToggleDevice, device);
    },

    [at.ToggleTheme]({ commit }, theme) {
      commit(mt.ToggleTheme, theme);
    },

    [at.ToggleLayoutMode]({ commit }, mode) {
      commit(mt.ToggleLayoutMode, mode);
    },

    [at.ToggleFixedHeader]({ commit }, fixedHeader) {
      if (!fixedHeader) {
        commit(mt.ToggleFixedHeaderHidden, false);
      }
      commit(mt.ToggleFixedHeader, fixedHeader);
    },

    [at.ToggleFixSiderbar]({ commit }, fixedSidebar) {
      commit(mt.ToggleFixedSiderbar, fixedSidebar);
    },

    [at.ToggleFixedHeaderHidden]({ commit }, show) {
      commit(mt.ToggleFixedHeaderHidden, show);
    },

    [at.ToggleContentWidth]({ commit }, type) {
      commit(mt.ToggleContentWidth, type);
    },

    [at.ToggleColor]({ commit }, color) {
      commit(mt.ToggleColor, color);
    },

    [at.ToggleWeak]({ commit }, weakFlag) {
      commit(mt.ToggleWeak, weakFlag);
    },

    [at.ToggleMultiTab]({ commit }, bool) {
      commit(mt.ToggleMultiTab, bool);
    },

    async [at.Login]({ commit }, params) {
      const response = await apiApp.getToken(params);
      if (response && response.status === 200 && response.data && response.data.access_token) {
        const data = response.data;
        const tokenValue = data.token_type + ' ' + data.access_token;
        Vue.ls.set(config.tokenKey, tokenValue, data.expires_in);
        commit(mt.SetToken, tokenValue);
      }
      return response;
    },

    async [at.GetInfo]({ commit }) {
      let response = await apiApp.getApplicationConfiguration();
      if (requestIsSuccess(response)) {
        let configuration = response.data;
        let _this = this;
        // 获取当前用户所在组织机构权限
        response = await apiApp.getPermissions();
        if (requestIsSuccess(response)) {
          // 获取用户名
          let responseUser = await apiUser.findByUsername(configuration.currentUser.userName);
          if (requestIsSuccess(responseUser)) {
            configuration.currentUser.name = responseUser.data.name;
            localStorage.setItem('currentUserName', responseUser.data.name);

            if (responseUser.data.projectIds && responseUser.data.projectIds.length > 0) {
              commit(mt.SetProjectIds, responseUser.data.projectIds);
            }
          }

          let responseOrgRoot = await apiOrganization.getLoginUserOrganizationRootTag(
            localStorage.getItem(defaultConfig.storageOptions.organizationId)
          );
          if (requestIsSuccess(responseOrgRoot) && responseOrgRoot.data) {
            localStorage.setItem(defaultConfig.storageOptions.organizationRootTagId, responseOrgRoot.data);
          }

          commit(mt.SetInfo, configuration);
          commit(mt.SetPermissions, response.data);

          // 获取文件服务器端点
          response = await apiApp.getFileServerEndPoint();
          if (requestIsSuccess(response)) {
            localStorage.setItem('fileServerEndPoint', response.data);
          }
        }
      }

      return response;
    },

    [at.Logout]({ commit, state }) {
      // apiApp.logout()
      commit(mt.SetToken, '');
      commit(mt.SetRoles, []);
      // Vue.ls.remove(config.tokenKey);
      localStorage.clear();
    },
  },
};

export default app;
