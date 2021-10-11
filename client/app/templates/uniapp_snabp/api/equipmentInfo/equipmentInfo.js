import service from '../../utils/service.js';

let url = '/api/app/resourceEquipmentProperty';

// 查询 获得所有列表
export async function getList(params) {
	return await service.request({
		url: `${url}/getList`,
		method: 'get',
		data:params
	});
}

