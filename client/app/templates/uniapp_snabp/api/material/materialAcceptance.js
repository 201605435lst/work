import service from '../../utils/service.js';
let url = '/api/app/materialAcceptance';

// 查询id
export async function get(id) {
	const res = await service.request({
		url: `${url}/get`,
		method: 'get',
		header: {
			'content-type': 'application/x-www-form-urlencoded',
		},
		data: {id},
	});
	return res;
}
// 查询列表
export async function getList(data) {
	const res = await service.request({
		url: `${url}/getList`,
		method: 'get',
		data,
	});
	return res;
}

// 创建
export async function create(data) {
	const res = await service.request({
		url: `${url}/create`,
		method: 'post',
		data,
	});
	return res;
}

// 更新
export async function update(data) {
	const res = await service.request({
		url: `${url}/update`,
		method: 'put',
		data,
	});
	return res;
}
// 二维码数据提交
export async function CreateAcceptanceQRCode(data) {
	const res = await service.request({
		url: `${url}/createAcceptanceQRCode`,
		method: 'post',
		data,
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
		data: {id},
	});
	return res;
}
