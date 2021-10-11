import Vue from 'vue';
import store from '@/store/';
import { name as storeApp, mt as mtApp } from '@/store/modules/app/types';
import config from '@/config/default';
import {
  DEFAULT_COLOR,
  DEFAULT_THEME,
  DEFAULT_LAYOUT_MODE,
  DEFAULT_COLOR_WEAK,
  SIDEBAR_VISIBLE,
  DEFAULT_FIXED_HEADER,
  DEFAULT_FIXED_HEADER_HIDDEN,
  DEFAULT_FIXED_SIDEMENU,
  DEFAULT_CONTENT_WIDTH_TYPE,
  DEFAULT_MULTI_TAB,
} from '@/common/consts';

export default function Initializer() {
  console.log(`API_URL: ${process.env.VUE_APP_API_BASE_URL}`);

  store.commit(storeApp + '/' + mtApp.SetSidebarVisible, Vue.ls.get(SIDEBAR_VISIBLE, true));
  store.commit(storeApp + '/' + mtApp.ToggleTheme, Vue.ls.get(DEFAULT_THEME, config.navTheme));
  store.commit(storeApp + '/' + mtApp.ToggleLayoutMode, Vue.ls.get(DEFAULT_LAYOUT_MODE, config.layout));
  store.commit(storeApp + '/' + mtApp.ToggleFixedHeader, Vue.ls.get(DEFAULT_FIXED_HEADER, config.fixedHeader));
  store.commit(storeApp + '/' + mtApp.ToggleFixedSiderbar, Vue.ls.get(DEFAULT_FIXED_SIDEMENU, config.fixedSidebar));
  store.commit(storeApp + '/' + mtApp.ToggleContentWidth, Vue.ls.get(DEFAULT_CONTENT_WIDTH_TYPE, config.contentWidth));
  store.commit(
    storeApp + '/' + mtApp.ToggleFixedHeaderHidden,
    Vue.ls.get(DEFAULT_FIXED_HEADER_HIDDEN, config.autoHideHeader)
  );
  store.commit(storeApp + '/' + mtApp.ToggleWeak, Vue.ls.get(DEFAULT_COLOR_WEAK, config.colorWeak));
  store.commit(storeApp + '/' + mtApp.ToggleColor, Vue.ls.get(DEFAULT_COLOR, config.primaryColor));
  store.commit(storeApp + '/' + mtApp.ToggleMultiTab, Vue.ls.get(DEFAULT_MULTI_TAB, config.multiTab));
  store.commit(storeApp + '/' + mtApp.SetToken, Vue.ls.get(config.tokenKey));

  // last step
}
