import Vue from 'vue';
import App from './App';
import store from './store';
import {checkToken, conformToRegular} from '@/utils/util.js';
import uniLoading from '@/components/uniLoading.vue';
import wybTable from '@/components/wyb-table/wyb-table.vue';
import * as util from '@/utils/util.js';

Vue.config.productionTip = false;
Vue.prototype.$store = store;
Vue.prototype.$util = util;
Vue.prototype.$checkToken = checkToken;
Vue.prototype.$conformToRegular = conformToRegular;
Vue.component('uniLoading', uniLoading);
Vue.component('wybTable', wybTable);
App.mpType = 'app';

/**
 * @app app项目打包时选择配置
 * @param {boolean} true BIM+GIS城市轨道交通信息集成管理云平台 Android包名：gm.njdt
 * @param {boolean} false BIM接触网数字孪生管理应用平台 Android包名：gm.jcw
 * 打包时注意更改发行时:Android包名,包名一样就是更新
 */
const $packagingMode = false; //是打包模式，非开发模式
const $appBIMGIS = false; //是BIM+GIS城市轨道交通信息集成管理云平台
Vue.prototype.$appBIMGIS = $appBIMGIS;
Vue.prototype.$packagingMode = $packagingMode;

const app = new Vue({
	...App,
	store,
	util,
});
app.$mount();
