/**
 * 项目默认配置项
 * primaryColor - 默认主题色, 如果修改颜色不生效，请清理 localStorage
 * navTheme - sidebar theme ['dark', 'light'] 两种主题
 * colorWeak - 色盲模式
 * layout - 整体布局方式 ['sidemenu', 'topmenu'] 两种布局
 * fixedHeader - 固定 Header : boolean
 * fixedSidebar - 固定左侧菜单栏 ： boolean
 * autoHideHeader - 向下滚动时，隐藏 Header : boolean
 * contentWidth - 内容区布局： 流式 |  固定
 *
 * storageOptions: {} - Vue-ls 插件配置项 (localStorage/sessionStorage)
 *
 */

export default {
  primaryColor: '#1890ff', //'#52C41A', // primary color of ant design
  navTheme: 'dark', // theme for nav menu
  layout: 'sidemenu', // nav menu position: sidemenu or topmenu
  contentWidth: 'Fluid', // layout of content: Fluid or Fixed, only works when layout is topmenu
  fixedHeader: true, // sticky header
  fixedSidebar: true, // sticky siderbar
  autoHideHeader: false, //  auto hide header
  colorWeak: false,
  multiTab: false,
  production: process.env.NODE_ENV === 'production' && process.env.VUE_APP_PREVIEW !== 'true',
  storageOptions: {
    namespace: 'snabp__', // key prefix
    name: 'ls', // name variable Vue.[ls] or this.[$ls],
    storage: 'local', // storage name session, local, memory
    organizationId: 'OrganizationId',
  },
  tokenKey: 'access_token',
  application: {
    name: 'MyProjectName',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: window.config.apiServiceBaseUrl,
    clientId: 'MyProjectName_App',
    dummyClientSecret: '1q2w3e*',
    scope: 'MyProjectName',
    showDebugInformation: true,
    oidc: false,
    requireHttps: false,
  },
  signalR: {
    url:  window.config.signalRUrl,
  },
  apis: {
    default: {
      url: window.config.apiServiceBaseUrl,
    },
  },
  localization: {
    defaultResourceName: 'MyProjectName',
  },
  ...window.config,
  permissionBlacklist: [],
};
