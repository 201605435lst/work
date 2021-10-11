const vueSignalR = {
  vm: {},
  // eslint-disable-next-line no-unused-vars
  install(Vue, instance) {
    if (this.installed) {
      return;
    }
    this.installed = true;

    if (!instance) {
      // eslint-disable-next-line no-console
      console.error('You have to install axios');
      return;
    }

    Vue.signalr = instance;

    Object.defineProperties(Vue.prototype, {
      signalr: {
        get: function get() {
          return instance;
        },
      },
    });
  },
};

export { vueSignalR };
