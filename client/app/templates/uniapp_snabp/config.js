export default {
	// 接口服务地址
	appName: '铁路通信设备维护管理系统',
	remoteServiceBaseUrl: uni.getStorageSync('remoteServiceBaseUrl'),
	oAuthConfig: {
		clientId: 'MyProjectName_App',
		dummyClientSecret: '1q2w3e*',
		scope: 'MyProjectName',
		showDebugInformation: true,
		oidc: false,
		requireHttps: false,
	},
	// token
	Token: 'token',
};
