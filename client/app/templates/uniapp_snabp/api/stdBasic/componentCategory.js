import service from '../../utils/service.js'
let url = '/api/app/stdBasicComponentCategory';

// 获取列表
export async function getByCode(code) {
	const res = await service.request({
		url: `${url}/getByCode`,
		method: 'get',
		data: {
			code
		},
	})
	return res
}
