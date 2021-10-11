// with polyfills
import 'core-js/stable';
import 'regenerator-runtime/runtime';

import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store/';
import { VueAxios } from './utils/axios';
import { VueSignalR } from './utils/signalr';

import VueCompositionAPI from '@vue/composition-api';
Vue.use(VueCompositionAPI);

// mock
// WARNING: `mockjs` NOT SUPPORT `IE` PLEASE DO NOT USE IN `production` ENV.
// import './mock'

import bootstrap from './core/bootstrap';
import './core/lazy_use';
import './utils/filter'; // global filter
import './components/global.less';
// import { Dialog } from '@/components';

import snWebIcon from 'snweb-icon';
import 'snweb-icon/dist/snweb-icon.css';
Vue.use(snWebIcon);

import smwebComponent from 'snweb-component';
import 'snweb-component/dist/snweb-component.css';
Vue.use(smwebComponent);

// import smwebModule from 'snweb-module';
import 'snweb-module/dist/snweb-module.css';
import 'snweb-module/es/sm-basic/sm-basic-railways/style/index.less';
// Vue.use(smwebModule);

Vue.config.productionTip = false;

// mount axios Vue.$http and this.$http
Vue.use(VueAxios);
Vue.use(VueSignalR);
// Vue.use(Dialog);

import vuedraggable from 'vuedraggable';
Vue.use(vuedraggable);

new Vue({
  router,
  store,
  created: bootstrap,
  render: h => h(App),
}).$mount('#app');
