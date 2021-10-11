/**
 * 接口说明：文件夹接口
 * 作者：easten
 */

import service from '../../utils/service.js';
let url = '/api/app/fileFolder';
/**
 * 根据id获取详细的文件夹信息
 * @param {*} id
 * @returns 文件夹实体
 * @memberof Api
 */
export async function get(id) {
	const res = await service.request({
		url: `${url}/get`,
		method: 'get',
		params: {id},
	});
	return res;
}

/**
 * @description 文件下载列表获取
 * @author easten
 * @date 2020-07-13
 * @param {*} id
 * @returns
 * @memberof Api
 */
export async function getFiles(id) {
	const res = await service.request({
		url: `${url}/getDownloadFile`,
		method: 'get',
		params: {id},
	});
	return res;
}
/**
 * 创建一个文件夹
 * @param {*} data 文件夹创建对象
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
 * 更新文件夹的名字
 * @param {*} data
 * @returns
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
 * 删除指定的文件夹
 * @param {*} id 文件夹id
 * @returns bool
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
