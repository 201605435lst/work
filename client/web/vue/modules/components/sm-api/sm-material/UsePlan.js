let url = '/api/app/materialUsePlan';
import qs from 'qs';

export default class Api{
  constructor(axios){
    this.axios = axios || null;
  }

  //添加用料计划表
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

  async getMaterialList(params){
    return await this.axios({
      url: `${url}/getMaterialList`,
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

  // 导出
  async exportMaterial(data) {
    return await this.axios({
      url: `${url}/exportMaterial`,
      method: 'post',
      data,
      responseType: 'arraybuffer',
    });
  }

  //得到工作流详情
  async getRunFlowInfo(workFlowId,usePlanId) {
    return await this.axios({
      url: `${url}/getRunFlowInfo`,
      method: 'get',
      params: { workFlowId,usePlanId },
    });
  }
}