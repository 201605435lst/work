const _window = window as any

export default {
  storageOptions: {
    namespace: 'snabp__', // key prefix
    name: 'ls', // name variable Vue.[ls] or this.[$ls],
    storage: 'local', // storage name session, local, memory
    organizationId: 'OrganizationId'
  },
  tokenKey: 'access_token',
  application: {
    name: 'MyProjectName',
    logoUrl: ''
  },
  oAuthConfig: {
    issuer: _window.config.apiServiceBaseUrl,
    clientId: 'MyProjectName_App',
    dummyClientSecret: '1q2w3e*',
    scope: 'MyProjectName',
    showDebugInformation: true,
    oidc: false,
    requireHttps: false
  },
  signalR: {
    url: _window.config.signalRUrl
  },
  apis: {
    default: {
      url: _window.config.apiServiceBaseUrl
    }
  },
  localization: {
    defaultResourceName: 'MyProjectName'
  },
  permissionBlacklist: []
}
