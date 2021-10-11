// import Vue from 'vue'
import { deviceEnquire, DEVICE_TYPE } from '@/utils/device';
import { mapState } from 'vuex';
import { name as storeApp, mt as mtApp, at as atApp } from '@/store/modules/app/types';

// const mixinsComputed = Vue.config.optionMergeStrategies.computed
// const mixinsMethods = Vue.config.optionMergeStrategies.methods

const mixin = {
  computed: {
    ...mapState({
      layoutMode: state => state.app.layout,
      navTheme: state => state.app.theme,
      primaryColor: state => state.app.color,
      colorWeak: state => state.app.weak,
      fixedHeader: state => state.app.fixedHeader,
      fixedSidebar: state => state.app.fixedSidebar,
      contentWidth: state => state.app.contentWidth,
      autoHideHeader: state => state.app.autoHideHeader,
      sidebarVisible: state => state.app.sidebarVisible,
      multiTab: state => state.app.multiTab,
    }),
  },
  methods: {
    isTopMenu() {
      return this.layoutMode === 'topmenu';
    },
    isSideMenu() {
      return !this.isTopMenu();
    },
  },
};

const mixinDevice = {
  computed: {
    ...mapState({
      device: state => state.app.device,
    }),
  },
  methods: {
    isMobile() {
      return this.device === DEVICE_TYPE.MOBILE;
    },
    isDesktop() {
      return this.device === DEVICE_TYPE.DESKTOP;
    },
    isTablet() {
      return this.device === DEVICE_TYPE.TABLET;
    },
  },
};

const AppDeviceEnquire = {
  mounted() {
    const { $store } = this;
    deviceEnquire(deviceType => {
      switch (deviceType) {
        case DEVICE_TYPE.DESKTOP:
          $store.commit(storeApp + '/' + mtApp.ToggleDevice, 'desktop');
          $store.dispatch(storeApp + '/' + atApp.SetSidebar, true);
          break;
        case DEVICE_TYPE.TABLET:
          $store.commit(storeApp + '/' + mtApp.ToggleDevice, 'tablet');
          $store.dispatch(storeApp + '/' + atApp.SetSidebar, false);
          break;
        case DEVICE_TYPE.MOBILE:
        default:
          $store.commit(storeApp + '/' + mtApp.ToggleDevice, 'mobile');
          $store.dispatch(storeApp + '/' + atApp.SetSidebar, true);
          break;
      }
    });
  },
};

export { mixin, AppDeviceEnquire, mixinDevice };
