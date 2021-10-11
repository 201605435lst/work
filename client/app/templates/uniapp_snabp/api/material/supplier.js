import service from '../../utils/service.js'
let url = '/api/app/materialSupplier';
import qs from "qs";

// 获取库存位置
export async function getList() {
	const res = await service.request({
		url: `${url}/getList`,
		method: 'get'
	});
	return res;
}
