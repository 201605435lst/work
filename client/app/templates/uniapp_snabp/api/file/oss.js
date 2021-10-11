/**
 * 接口说明：oss 服务管理接口
 * 作者：easten
 */

import service from '../../utils/service.js';
let url = '/api/app/fileOssConfig';
/**
 * 获取oss 配置列表
 * @returns
 * @memberof Api
 */
export async function getList() {
	const res = await service.request({
		url: `${url}/getList`,
		method: 'get',
	});
	return res;
}

/**
 * @description 获取文件管理服务的状态
 * @author easten
 * @date 2020-09-21
 * @param {*} id
 * @returns 状态信息
 * @memberof Api
 */
export async function getOssState(id) {
	const res = await service.request({
		url: `${url}/GetOssState`,
		method: 'get',
		params: {id},
	});
	return res;
}

/**
 * 新增oss 配置
 * @param {*} data
 * @returns
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
 * 启动指定的oss服务
 * @param {*} id
 * @returns
 * @memberof Api
 */
export async function enable(id) {
	const res = await service.request({
		url: `${url}/enable`,
		method: 'post',
		params: {id},
	});
	return res;
}
/**
 * 检测服务状态
 * @param {*} id
 * @returns
 * @memberof Api
 */
export async function check(id) {
	const res = await service.request({
		url: `${url}/check`,
		method: 'post',
		params: {id},
	});
	return res;
}
/**
 * 清空指定的oss 服务数据
 * @param {*} id
 * @returns bool
 * @memberof Api
 */
export async function clear(id) {
	const res = await service.request({
		url: `${url}/clear`,
		method: 'post',
		params: {id},
	});
	return res;
}
/**
 * 更新oss 配置
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
