/**
 * 接口说明：文件管理服务
 * 作者：easten
 */
import service from '../../utils/service.js';
let url = '/api/app/fileFile';
/**
 * 获取文件上传的签名信息，需要文件的类型生成对应的上传签名地址
 * @param {String} type 文件类型，值传递文件的后缀名即可
 * @returns 文件上传的签名地址
 * @memberof Api
 */
export async function getPresignUrl(params) {
	const res = await service.request({
		url: `${url}/getPresignUrl`,
		method: 'get',
		params: params,
	});
	return res;
}

/**
 * 获取文件版本信息
 * @param {*} id 文件id
 * @returns 文件版本信息
 * @memberof Api
 */
export async function getVersions(id) {
	const res = await service.request({
		url: `${url}/getVersionList`,
		method: 'get',
		params: {id},
	});
	return res;
}
/**
 *新增文件
 * @param {*} data 文件字段对象
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
 * 新增文件版本信息
 * @param {*} data 文件信息
 * @returns bool
 * @memberof Api
 */
export async function createFileVersion(data) {
	const res = await service.request({
		url: `${url}/createFileVersion`,
		method: 'post',
		data,
	});
	return res;
}
/**
 * 选择最新版本
 * @param {*} id 指定的文件id
 * @returns bool
 * @memberof Api
 */
export async function selectNewVersion(data) {
	const res = await service.request({
		url: `${url}/selectNewVersion`,
		method: 'post',
		data,
	});
	return res;
}
/**
 * 更新文件名称
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
 * 删除指定的文件
 * @param {*} id 文件id
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
// 编辑审批状态
export async function updateStatus(data) {
	const res = await service.request({
		url: `${url}/updateStatus`,
		method: 'put',
		data,
	});
	return res;
}
/**
 * 删除指定的文件版本信息
 * @param {*} id 文件版本id
 * @returns bool
 * @memberof Api
 */
export async function deleteFileVersion(id) {
	const res = await service.request({
		url: `${url}/deleteFileVersion`,
		method: 'delete',
		params: {id},
	});
	return res;
}

/**
 * 获取回收站列表
 */
export async function getResourceList(data) {
	const res = await service.request({
		url: `/api/app/fileApproveFileApprove/GetResourceList`,
		method: 'get',
		data,
	});
	return res;
}
