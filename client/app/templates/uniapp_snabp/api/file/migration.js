// 文件迁移接口
import service from '../../utils/service.js';
let url = '/api/app/fileMigration';

/**
 * @description 数据对比，对比两个服务中数据的异同
 * @author easten
 * @date 2020-10-09
 * @param {*} data
 * @returns
 * @memberof Api
 */
export async function dataContrast(data) {
	const res = await service.request({
		url: `${url}/dataContrast`,
		method: 'post',
		data,
	});
	return res;
}

/**
 * @description 开始迁移
 * @author easten
 * @date 2020-10-09
 * @returns
 * @memberof Api
 */
export async function start() {
	const res = await service.request({
		url: `${url}/start`,
		method: 'post',
	});
	return res;
}

/**
 * @description 获取进度信息
 * @author easten
 * @date 2020-10-09
 * @returns
 * @memberof Api
 */
export async function getProcess() {
	const res = await service.request({
		url: `${url}/getProcess`,
		method: 'get',
	});
	return res;
}

/**
 * @description 取消迁移
 * @author easten
 * @date 2020-10-09
 * @returns
 * @memberof Api
 */
export async function cancel() {
	const res = await service.request({
		url: `${url}/cancel`,
		method: 'post',
	});
	return res;
}
