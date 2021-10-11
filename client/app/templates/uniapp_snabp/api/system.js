import service from '../utils/service.js';
import qs from 'qs';
// 通过values值查询
export async function getValues(params) {
	const res = await service.request({
		url: '/api/app/appDataDictionary/getValues',
		method: 'get',
		data: params,
	});
	return res;
}

//组织机构
export async function getOrganizationList(params) {
	const res = await service.request({
		url: `/api/app/appOrganization/getList`,
		method: 'get',
		data: params,
	});
	return res;
}

export async function getOrganization(id) {
	const res = await service.request({
		url: `/api/app/appOrganization/get`,
		method: 'get',
		data: {
			id,
		},
	});
	return res;
}

// 获取用户数据列表
export async function getUserList(params) {
	const res = await service.request({
		url: `/api/app/appUser/getList`,
		method: 'get',
		data: params,
	});
	return res;
}
// 获取用户
export async function get(id) {
	const res = await service.request({
		url: `/api/app/appUser/get`,
		method: 'get',
		data: {id},
	});
	return res;
}

//查询单个用户
export async function get_(data) {
	const res = await service.request({
		url: `/api/app/appUser/get`,
		method: 'get',
		data,
	});
	return res;
}

// 用户管理编辑
export async function update(id, data) {
	console.log(id, data);
	const res = await service.request({
		url: `/api/app/appUser/update?id=${id}`,
		method: 'put',
		data,
	});
	return res;
}

export async function getCurrentUserOrganizations(data) {
	const res = await service.request({
		url: `/api/app/appOrganization/getCurrentUserOrganizations`,
		method: 'get',
		data,
		paramsSerializer: data => {
			return qs.stringify(data, {
				arrayFormat: 'repeat',
			});
		},
	});
	return res;
}
// 查询当前用户所在组织机构的根级组织
export async function getLoginUserOrganizationRootTag(id) {
	const res = await service.request({
		url: `/api/app/appOrganization/getLoginUserOrganizationRootTag`,
		method: 'get',
		data: {id},
	});
	return res;
}
