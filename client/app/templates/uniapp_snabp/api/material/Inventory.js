/**
 * 说明：仓库管理接口
 */
import service from '../../utils/service.js'
let url = '/api/app/inventory'

// 获取仓库列表
export async function getList(data) {
	const res = await service.request({
		url: `${url}/getList`,
		method: 'get',
		data,
	})
	return res
}

// 根据材料id获取仓库列表
export async function getListByMaterialId(materialId) {
	const res = await service.request({
		url: `${url}/getListByMaterialId`,
		method: 'get',
		data: {
			materialId
		},
	})
	return res
}

// 库存材料详情获取
export async function get(id) {
	const res = await service.request({
		url: `${url}/get`,
		method: 'get',
		data: {
			id
		}
	})
	return res
}
