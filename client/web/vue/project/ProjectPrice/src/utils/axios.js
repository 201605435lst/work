import Vue from 'vue';
import store from '@/store';
import axios from 'axios';
import { vueAxios } from './axiosVue';
import defaultConfig from '@/config/default';
import { name as storeApp, at as atApp } from '@/store/modules/app/types';

// 创建 axios 实例
const instance = axios.create({
  baseURL: defaultConfig.apis.default.url,
  // baseURL: '/api',
  timeout: 1000000, // 请求超时时间
});

// 请求拦截器
instance.interceptors.request.use(
  config => {
    const tokenValue = Vue.ls.get(defaultConfig.tokenKey);
    if (tokenValue) {
      config.headers.common['Authorization'] = tokenValue;
      config.headers.common[defaultConfig.storageOptions.organizationId] =
        localStorage.getItem(defaultConfig.storageOptions.organizationId) || '';
    }
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

const vm = new Vue({});
// 相应拦截器
instance.interceptors.response.use(
  response => {
    if (response.request.responseURL.indexOf('ReturnUrl') >= 0) {
      vm.$message.error('未授权，请先登录');
    }
    return response;
  },
  error => {
    if (error.response) {
      const token = Vue.ls.get(defaultConfig.tokenKey);

      let message = '';
      if (
        !!error.response &&
        !!error.response.data.error &&
        !!error.response.data.error.message &&
        error.response.data.error.details
      ) {
        message = error.response.data.error.details;
      } else if (!!error.response && !!error.response.data.error && !!error.response.data.error.message) {
        message = error.response.data.error.message;
      } else if (!error.response) {
        message = '未知错误';
      } else if (error.response.status === 403) {
        message = '未授权';
      } else if (error.response.status === 401 || !token) {
        message = '用户名或密码错误，请重新输入！';
        store.dispatch(storeApp + '/' + atApp.Logout).then(() => {
          setTimeout(() => {
            window.location.reload();
          }, 1500);
        });
      }

      vm.$message.error(message);
    }
    return error.response;
  }
);

// Vue 插件
const installer = {
  vm: {},
  install(Vue) {
    Vue.use(vueAxios, instance);
  },
};

export { installer as VueAxios, instance as axios };
