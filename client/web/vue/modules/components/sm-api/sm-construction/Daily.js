let url = '/api/app/constructionDaily';
import qs from 'qs';
export default class Api {
  constructor(axios) {
    this.axios = axios;
  }
  // 获取列表
  async getList(params) {
    return await this.axios({
      url: `${url}/getlist`,
      method: 'get',
      params,
    });
  }
  // 根据id 查询
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
  // 根据id 查询总的已完成工作量
  async getDailyRltPlanMaterial(id) {
    return await this.axios({
      url: `${url}/getDailyRltPlanMaterial`,
      method: 'get',
      params: { id },
    });
  }
  // 添加数据
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }
 
  // 更新数据
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
    });
  }
  // 删除数据
  async delete(id) {
    return await this.axios({
      url: `${url}/delete`,
      method: 'delete',
      params: { id },
    });
  }
  // 创建审批流程
  async createWorkFlow(planId,templateId){
    return await this.axios({
      url: `${ url}/createWorkFlow`,
      method: 'post',
      params: {planId,templateId},
    });
  }

  // 提交状态更新
  async planSubmitStateUpdate(id){
    return await this.axios({
      url: `${ url}/planSubmitStateUpdate`,
      method: 'put',
      params: {id},
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
}
 