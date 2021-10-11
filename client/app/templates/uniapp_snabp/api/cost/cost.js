import service from '../../utils/service.js';
let url = '/api/app/costManagementContract';

// 获取统计信息
export async function getStatistics(code) {
	const res = await service.request({
		url: `${url}/getStatistics`,
		method: 'get',
		data: {
			code,
		},
	});
	return res;
}
