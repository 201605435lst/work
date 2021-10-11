import service from '../../utils/service.js';
let url = '/api/app/constructionDaily';
// 获取施日志批数据列表
export async function getList(params) {
	const res = await service.request({
		url: `${url}/getList`,
		method: 'get',
		data: params,
	});
	return res;
}

// 获取施日志批详情
export async function get(id) {
	const res = await service.request({
		url: `${url}/get`,
		method: 'get',
		data: {
			id,
		},
	});
	return res;
}

// 添加
export async function create(data) {
	const res = await service.request({
		url: `${url}/create`,
		method: 'post',
		data,
	});
	return res;
}

// 编辑
export async function update(data) {
	const res = await service.request({
		url: `${url}/update`,
		method: 'put',
		data,
	});
	return res;
}

// 根据id 查询总的已完成工作量
export async function getDailyRltPlanMaterial(id) {
	const res = await service.request({
		url: `${url}/getDailyRltPlanMaterial`,
		method: 'get',
		data: {
			id,
		},
	});
	return res;
}

// 删除
export async function remove(id) {
	const res = await service.request({
		url: `${url}/delete`,
		method: 'delete',
		header: {
			'content-type': 'application/x-www-form-urlencoded',
		},
		data: {
			id,
		},
	});
	return res;
}

// 创建审批流程
export async function createWorkFlow(planId, templateId) {
	const res = await service.request({
		url: `${url}/createWorkFlow`,
		header: {
			'content-type': 'application/x-www-form-urlencoded',
		},
		method: 'post',
		data: {
			planId,
			templateId,
		},
	});
	return res;
}

// 提交状态更新
export async function planSubmitStateUpdate(id) {
	const res = await service.request({
		url: `${url}/planSubmitStateUpdate`,
		method: 'put',
		params: {
			id,
		},
	});
	return res;
}

// 流程审批
export async function process(data) {
	const res = await service.request({
		url: `${url}/process`,
		method: 'post',
		data,
	});
	return res;
}

// 添加
export async function createTrackRecord(data) {
	const res = await service.request({
		url: `/api/app/resourceComponentTrackRecord/create`,
		method: 'post',
		data
	});
	return res;
}
