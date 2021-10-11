let url = '/api/app/materialPurchasePlan';
import qs from 'qs';
export default class Api{
  constructor(axios){
    this.axios = axios || null;
  }

  // 获取详情
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
    
  //添加采购计划表
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }
  // 查询 获得所有列表
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
      paramsSerializer: params => {
        return qs.stringify(params, { indices: false });
      },
    });
  }
  //更新
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
    });
  }

  async updateState(id){
    return await this.axios({
      url: `${url}/updateState`,
      method: 'put',
      params: { id },
    });
  }

  // 删除数据
  async delete(id){
    return await this.axios({
      url: `${ url}/delete`,
      method: 'delete',
      params: { id },
    });
  }

  //导出word文件
  async export(id) {
    console.log(id);
    return await this.axios({
      url: `${url}/export`,
      method: `get`,
      responseType: 'arraybuffer',
      params: { id },
    });
  }

  async getFlowInfo(workflowTemplateId){
    return await this.axios({
      url: `${url}/getFlowInfo`,
      method: 'get',
      params: { workflowTemplateId },
    });
  }
  // 流程审批
  async process(data){
    return await this.axios({
      url: `${ url}/process`,
      method: 'post',
      data,
    });
  }
  //得到工作流详情
  async getRunFlowInfo(workFlowId,purchaseId) {
    return await this.axios({
      url: `${url}/getRunFlowInfo`,
      method: 'get',
      params: { workFlowId,purchaseId },
    });
  }
}