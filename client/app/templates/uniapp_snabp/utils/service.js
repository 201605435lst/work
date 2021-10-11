import uniRequest from './uni-request.js';
import config from '@/config.js';
const service = new uniRequest({
	baseUrl: uni.getStorageSync('remoteServiceBaseUrl'),
});
service.interceptors.request = requestConfig => {
	// console.log("请求-前-拦截器")
	var token = uni.getStorageSync(config.Token);
	if (token) {
		requestConfig.header['Authorization'] = 'Bearer ' + token;
	}

	return requestConfig;
};

service.interceptors.response = (error, response) => {
	// console.log("请求-后-拦截器")
	let errMsg = '';
	if (error) {
		errMsg = error;
	} else if (error && error.response) {
		const token = '';
		uni.getStorage({
			key: 'token',
			success: function (res) {
				token = res.data;
			},
		});
		if (!!error.response && !!error.response.data.error && !!error.response.data.error.message && error.response.data.error.details) {
			errMsg = error.response.data.error.details;
		} else if (!!error.response && !!error.response.data.error && !!error.response.data.error.message) {
			errMsg = error.response.data.error.message;
		} else if (!error.response) {
			errMsg = '未知错误';
		} else if (error.response.status === 403) {
			errMsg = '未授权';
		} else if (error.response.status === 401 || !token) {
			errMsg = '用户名或密码错误，请重新输入！';
		}
	}

	if (errMsg) {
		uni.navigateTo({
			url: '../login/login',
		});
		// uni.showToast({
		// 	title: `错误：${errMsg && errMsg.errMsg ? '错误请求！' : errMsg}`,
		// 	icon: 'none',
		// 	mask: true,
		// 	duration: 2000,
		// });
		return;
	}

	if (response && response.statusCode === 403 && response.data && response.data.error && response.data.error.message) {
		// uni.showToast({
		// 	title: response.data.error.message,
		// 	icon: 'none',
		// 	mask: true,
		// 	duration: 2000,
		// });
		return;
	}

	if (response.request && response.request.responseURL && response.request.responseURL.indexOf('ReturnUrl') >= 0) {
		uni.showToast({
			title: '未授权，请先登录',
			icon: 'none',
			mask: true,
			duration: 2000,
		});
		uni.navigateTo({
			url: '../login/login',
		});
	}

	return response;
};

export default service;
