/**
 * 说明：入库管理接口
 */
import service from '../../utils/service.js'
let url = '/api/app/materialEntryRecord'

// 获取列表
export async function getList(data) {
	const res = await service.request({
		url: `${url}/getList`,
		method: 'get',
		data,
	})
	return res
}

// 获取详情
export async function get(id) {
	const res = await service.request({
		url: `${url}/get`,
		method: 'get',
		data: {
			id
		},
	})
	return res
}

// 创建记录
export async function create(data) {
	const res = await service.request({
		url: `${url}/create`,
		method: 'post',
		data,
	})
	return res
}

