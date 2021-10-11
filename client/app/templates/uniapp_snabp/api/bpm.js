let url = '/api/app/bpmWorkflowTemplate';
import service from "../utils/service.js";
//工作流模板查询
export async function getList(params) {
	const res = await service.request({
		url: `${url}/getList`,
		method: 'get',
		data: params
	});
	return res;
}

