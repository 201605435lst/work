/**
 * 说明：项目管理接口
 */
import service from '../../utils/service.js';
let url = '/api/app/projectManager';

// 获取列表
export async function getList(data) {
	const res = await service.request({
		url: `${url}/getList`,
		method: 'get',
		data,
	});
	return res;
}
// 根据id获取列表
export async function getListByIds(data) {
	const res = await service.request({
		url: `${url}/getListByIds`,
		method: 'get',
		data,
	});
	return res;
}
