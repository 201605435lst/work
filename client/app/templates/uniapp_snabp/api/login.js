import service from '../utils/service.js';
import config from '../config.js';

export async function getOpenIdConfig() {
	const options = {
		url: '/.well-known/openid-configuration',
		method: 'GET',
	};
	const res = await service.request(options);
	return res;
}

export async function getJwks(url) {
	const res = await service.request({
		url,
		method: 'GET',
	});
	return res;
}
export async function getToken(params) {
	const _data = {
		grant_type: 'password',
		scope: config.oAuthConfig.scope,
		...params,
		client_id: config.oAuthConfig.clientId,
		client_secret: config.oAuthConfig.dummyClientSecret,
	};
	const res = await service.request({
		url: uni.getStorageSync('remoteServiceBaseUrl') + '/connect/token',
		method: 'POST',
		header: {
			'content-type': 'application/x-www-form-urlencoded',
		},
		data: _data,
	});
	return res;
}

export async function getApplicationConfiguration() {
	const res = await service.request({
		url: '/api/abp/application-configuration',
		method: 'GET',
	});
	return res;
}

export async function getPermissions() {
	const res = await service.request({
		url: '/api/app/appUser/getUserPermissions',
		method: 'GET',
	});
	return res;
}

// 查询单个用户
export async function findByUsername(username) {
	const res = await service.request({
		url: `/api/app/appUser/findByUsername`,
		method: 'POST',
		header: {
			'content-type': 'application/x-www-form-urlencoded',
		},
		data: {
			username,
		},
	});
	return res;
}

export async function getFileServerEndPoint() {
	let res = await service.request({
		url: '/api/app/fileFileManager/getEndPoint',
		method: 'GET',
	});
	return res;
}

export async function getPresignUrl(params) {
	const res = await service.request({
		url: '/api/app/fileFile/getPresignUrl',
		method: 'GET',
		data: params,
	});
	return res;
}
