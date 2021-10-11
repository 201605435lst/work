import service from '../../utils/service.js'
import qs from "qs";
let url = '/api/app/technologyMaterial';

// 获取物资数据列表
export async function getList(params) {
	const res = await service.request({
		url: `${url}/getList`,
		method: 'get',
		data: params
	});
	return res;
}

// 获取所有物资数据列表
export async function getAll() {
	const res = await service.request({
		url: `${url}/getAll`,
		method: 'get'
	});
	return res;
}

// 获取物资详情
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
