const getters = {
  device: state => state.app.device,
  info: state => state.app.info,
  theme: state => state.app.theme,
  color: state => state.app.color,
  token: state => state.app.token,
  avatar: state => state.app.avatar,
  nickname: state => state.app.name,
  welcome: state => state.app.welcome,
  roles: state => state.app.roles,
  userInfo: state => state.app.info,
  permissions: state => state.app.permissions,
  projectIds: state => state.app.projectIds,
  addRouters: state => state.router.addRouters,
  multiTab: state => state.app.multiTab,
  // lang: state => state.app.lang
};

export default getters;
