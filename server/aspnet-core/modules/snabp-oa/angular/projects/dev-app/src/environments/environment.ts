export const environment = {
  production: false,
  application: {
    name: 'Oa',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44351',
    clientId: 'Oa_ConsoleTestApp',
    dummyClientSecret: '1q2w3e*',
    scope: 'Oa',
    showDebugInformation: true,
    oidc: false,
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44351',
    },
    Oa: {
      url: 'https://localhost:44307',
    },
  },
};
