import service from '../../utils/service.js'
import qs from "qs";
let url = '/api/app/costManagementContract'
// 获取施工审批数据列表
export async function getList(params) {
	const res = await service.request({
		url: `${url}/getList`,
		method: 'get',
		data: params
	});
	return res;
}

// 获取施工审批详情
export async function get(id) {
	const res = await service.request({
		url: `${url}/get`,
		method: 'get',
		data: {
			id
		}
	});
	return res;
}

export async function create(data) {
	const res = await service.request({
		url: `${url}/create`,
		method: 'post',
		data
	});
	return res;
}

export async function update(data) {
	const res = await service.request({
		url: `${url}/update`,
		method: 'put',
		data
	});
	return res;
}

// 删除
export async function remove(ids) {
	const res = await service.request({
		url: `${url}/delete`,
		method: 'delete',
		header: {
			'content-type': 'application/x-www-form-urlencoded',
		},
		data: {
			ids
		},
		paramsSerializer: data => {
			return qs.stringify(data, {
				arrayFormat: 'repeat',
			});
		},
	});
	return res;
}
