/**
 * 说明：仓库管理接口
 */
import service from '../../utils/service.js'
let url = '/api/app/safeProblemLibrary'

// 获取仓库列表
export async function getList(data) {
	const res = await service.request({
		url: `${url}/getList`,
		method: 'get',
		data,
	})
	return res
}
