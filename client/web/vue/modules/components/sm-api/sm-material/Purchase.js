let url = '/api/app/materialPurchase';
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
  async getList(params){
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }

  //更新
  async update(data){
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

  // 导出
  async export(data) {
    return await this.axios({
      url: `${url}/export`,
      method: 'post',
      data,
      responseType: 'arraybuffer',
    });
  }

  async getFlowInfo(workflowTemplateId){
    return await this.axios({
      url: `${url}/getFlowInfo`,
      method: 'get',
      params: { workflowTemplateId },
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