import service from '../../utils/service.js';
let url = '/api/app/constructionDispatch';

// 查询 派工单列表
export async function getList(params) {
	const res = await service.request({
		url: `${url}/getList`,
		method: 'get',
		data: params,
	});
	return res;
}
// 根据id 查询单个派工单
export async function get(id) {
	const res = await service.request({
		url: `${url}/get`,
		method: 'get',
		data: {id},
	});
	return res;
}

// 添加派工单
export async function create(data) {
	const res = await service.request({
		url: `${url}/create`,
		method: 'post',
		data,
	});
	return res;
}

// 编辑派工单
export async function update(data) {
	const res = await service.request({
		url: `${url}/update`,
		method: 'put',
		data,
	});
	return res;
}

//提交审批
export async function forSubmit(data) {
	const res = await service.request({
		url: `${url}/forSubmit`,
		method: 'post',
		data,
	});
	return res;
}

//审批流程操作
export async function process(data) {
	const res = await service.request({
		url: `${url}/process`,
		method: 'post',
		data,
	});
	return res;
}

// 删除 多个
// export async function delete_(ids) {
//   const res = await service.request({
//     url: `${url}/delete`,
//     method: 'delete',
//     params: { ids },
//   });
//   return res;
// }
