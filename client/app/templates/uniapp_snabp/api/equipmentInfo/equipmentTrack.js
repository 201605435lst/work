import service from '../../utils/service.js'

let url = '/api/app/resourceComponentTrackRecord';

// 查询安装设备Id
export async function getInstallationEquipmentId(data) {
	return await service.request({
		url: `${url}/getInstallationEquipmentId`,
		method: 'get',
		data
	});
}

// 构件跟踪
export async function getByInstallationEquipmentId(data) {
	return await service.request({
		url: `${url}/getByInstallationEquipmentId`,
		method: 'get',
		data
	});
}
