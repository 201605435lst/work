/**
 * 接口说明：标签管理服务接口
 * 作者：easten
 */

import service from '../../utils/service.js';
let url = '/api/app/fileTag';
/**
 * 根据组织id获取标签信息
 * @param {*} id 组织结构id
 * @returns tag model
 * @memberof Api
 */
export async function getList(id) {
	const res = await service.request({
		url: `${url}/getList`,
		method: 'get',
		params: {id},
	});
	return res;
}
/**
 * 新增一个标签信息
 * @param {*} data
 * @returns bool
 * @memberof Api
 */
export async function create(data) {
	const res = await service.request({
		url: `${url}/create`,
		method: 'post',
		data,
	});
	return res;
}
/**
 * 更新标签名称
 * @param {*} data
 * @returns bool
 * @memberof Api
 */
export async function update(data) {
	const res = await service.request({
		url: `${url}/update`,
		method: 'put',
		data,
	});
	return res;
}
/**
 * 删除标签信息
 * @param {*} id
 * @returns
 * @memberof Api
 */
export async function delete_(id) {
	const res = await service.request({
		url: `${url}/delete`,
		method: 'delete',
		params: {id},
	});
	return res;
}

export async function getTagIds(params) {
	const res = await service.request({
		url: `${url}/getTagIds`,
		method: 'get',
		params,
	});
	return res;
}
