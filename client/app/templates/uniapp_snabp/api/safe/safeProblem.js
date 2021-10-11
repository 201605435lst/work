/**
 * 说明：仓库管理接口
 */
import service from '../../utils/service.js';
let url = '/api/app/safeProblem';

// 获取列表
export async function getList(data) {
	const res = await service.request({
		url: `${url}/getList`,
		method: 'get',
		data,
	});
	return res;
}

// 查询待整改数据
export async function getWaitingImproveList(data) {
	const res = await service.request({
		url: `${url}/getWaitingImproveList`,
		method: 'get',
		data,
	});
	return res;
}

// 获取详情
export async function get(id) {
	const res = await service.request({
		url: `${url}/get`,
		method: 'get',
		data: {
			id,
		},
	});
	return res;
}

// 创建记录
export async function createRecord(data) {
	const res = await service.request({
		url: `${url}/createRecord`,
		method: 'post',
		data,
	});
	return res;
}

// 创建记录
export async function create(data) {
	const res = await service.request({
		url: `${url}/create`,
		method: 'post',
		data,
	});
	return res;
}

// 创建记录
export async function update(data) {
	const res = await service.request({
		url: `${url}/update`,
		method: 'post',
		data,
	});
	return res;
}

// 创建出入库记录
export async function getRecordList(id) {
	const res = await service.request({
		url: `${url}/getRecordList`,
		method: 'get',
		data: {
			id,
		},
	});
	return res;
}

// 删除
export async function remove(id) {
	const res = await service.request({
		url: `${url}/delete`,
		method: 'delete',
		header: {
			'content-type': 'application/x-www-form-urlencoded',
		},
		data: {
			id,
		},
	});
	return res;
}
