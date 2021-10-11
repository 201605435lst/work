/**
 * 接口说明：文件管理组件接口
 * 作者：easten
 */

import service from '../../utils/service.js';
let url = '/api/app/fileFileManager';
/**
 * 搜索查询
 * @param {*} data
 * @returns 在指定组织或者文件夹下的第一级资源信息
 * @memberof Api
 */
export async function get(params) {
	const res = await service.request({
		url: `${url}/get`,
		method: 'get',
		params,
		paramsSerializer: params => {
			return qs.stringify(params, {indices: false});
		},
	});
	return res;
}
/**
 * @description 获取文件地址前缀
 * @author easten
 * @date 2020-07-16
 * @returns
 * @memberof Api
 */
export async function getEndPoint() {
	const res = await service.request({
		url: `${url}/getEndPoint`,
		method: 'get',
	});
	return res;
}

/**
 * 根据条件获取组织机构的id
 * @param {*} params
 * @returns
 * @memberof Api
 */
export async function getOragniaztionId(params) {
	const res = await service.request({
		url: `${url}/getOrganizationId`,
		method: 'get',
		params,
	});
	return res;
}
/**
 * 获取组织树结构
 * @returns 树结构
 * @memberof Api
 */
export async function getOrganizationTreeList() {
	const res = await service.request({
		url: `${url}/GetOrganizationTreeList`,
		method: 'get',
	});
	return res;
}
/**
 * 获取共享中心目录树列表
 * @returns
 * @memberof Api
 */
export async function getShareCenterTreeList() {
	const res = await service.request({
		url: `${url}/GetShareCenterTreeList`,
		method: 'get',
	});
	return res;
}
/**
 * 获取“我的”文件目录，只获取当前用户上传的文件信息，并且是未公开的
 * @returns
 * @memberof Api
 */
export async function getMineTreeList() {
	const res = await service.request({
		url: `${url}/GetMineTreeList`,
		method: 'get',
	});
	return res;
}
/**
 * 获取“静态”文件目录
 * @returns
 * @memberof Api
 */
export async function getStaticTreeList(staticKey) {
	const res = await service.request({
		url: `${url}/getStaticTreeList`,
		method: 'get',
		params: {staticKey},
	});
	return res;
}
/**
 * 根据节点获取资源信息
 * @param {*} data
 * @returns 资源分页列表
 * @memberof Api
 */
export async function getResourceList(data) {
	const res = await service.request({
		url: `${url}/GetResourceList`,
		method: 'get',
		data,
	});
	return res;
}
/**
 * 获取资源的具体权限，用于反向绑定
 * @param {*} data
 * @returns
 * @memberof Api
 */
export async function getResourcePermission(params) {
	const res = await service.request({
		url: `${url}/GetResourcePermission`,
		method: 'get',
		params,
		paramsSerializer: params => {
			return qs.stringify(params, {indices: false});
		},
	});
	return res;
}
/**
 *获取共享资源的权限信息
 * @param {*} data
 * @returns
 * @memberof Api
 */
export async function getResourceShare(params) {
	const res = await service.request({
		url: `${url}/GetResourceShare`,
		method: 'get',
		params,
		paramsSerializer: params => {
			return qs.stringify(params, {indices: false});
		},
	});
	return res;
}
/**
 * 批量设置资源标签信息
 * @param {*} data
 * @returns
 * @memberof Api
 */
export async function createResourceTag(data) {
	const res = await service.request({
		url: `${url}/createResourceTag`,
		method: 'post',
		data,
	});
	return res;
}
/**
 * 文件移动
 * @param {*} data
 * @returns
 * @memberof Api
 */
export async function createFileMove(data) {
	const res = await service.request({
		url: `${url}/createFileMove`,
		method: 'post',
		data,
	});
	return res;
}
/**
 * 文件复制
 * @param {*} data
 * @returns
 * @memberof Api
 */
export async function createFileCopy(data) {
	const res = await service.request({
		url: `${url}/createFileCopy`,
		method: 'post',
		data,
	});
	return res;
}
/**
 * 资源还原
 * @param {*} data
 * @returns
 * @memberof Api
 */
export async function restore(data) {
	const res = await service.request({
		url: `${url}/restore`,
		method: 'post',
		data,
	});
	return res;
}
/**
 * 设置资源的权限
 * @param {*} data
 * @returns
 * @memberof Api
 */
export async function setResourcePermission(data) {
	const res = await service.request({
		url: `${url}/setResourcePermission`,
		method: 'post',
		data,
	});
	return res;
}
/**
 * 设置资源共享
 * @param {*} data
 * @returns
 * @memberof Api
 */
export async function SetResourceShare(data) {
	const res = await service.request({
		url: `${url}/SetResourceShare`,
		method: 'post',
		data,
	});
	return res;
}
/**
 * @description 发布资源
 * @author easten
 * @date 2020-07-03
 * @param {*} params
 * @returns
 * @memberof Api
 */
export async function PublishResource(data) {
	const res = await service.request({
		url: `${url}/PublishResource`,
		method: 'post',
		data,
	});
	return res;
}
export async function update(data) {}
/**
 * 批量删除资源
 * @param {*} data
 * @returns
 * @memberof Api
 */
export async function delete_(params) {
	const res = await service.request({
		url: `${url}/delete`,
		method: 'delete',
		params,
		paramsSerializer: params => {
			return qs.stringify(params, {indices: false});
		},
	});
	return res;
}

/**
 * @description 清空回收站
 * @author easten
 * @date 2020-07-07
 * @returns boolen
 * @memberof Api
 */
export async function deleteAll() {
	const res = await service.request({
		url: `${url}/deleteAll`,
		method: 'delete',
	});
	return res;
}

/**
 * 删除文件文件夹
 */
export async function deleteFile(val) {
	const res = await service.request({
		url: `/api/app/fileFileManager/delete?${val.type}=${val.id}`,
		method: 'delete',
	});
	return res;
}
