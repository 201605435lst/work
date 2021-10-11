import service from '../../utils/service.js';
let url = '/api/app/section';
// 获取施日志批数据列表
export async function getTreeList(params) {
	const res = await service.request({
		url: `${url}/getTreeList`,
		method: 'get',
		data: params,
	});
	return res;
}
