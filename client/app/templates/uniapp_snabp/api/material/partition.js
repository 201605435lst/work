import service from '../../utils/service.js'
let url = '/api/app/partition';
import qs from "qs";

// 获取库存位置
export async function getTreeList() {
	const res = await service.request({
		url: `${url}/getTreeList`,
		method: 'get'
	});
	return res;
}
