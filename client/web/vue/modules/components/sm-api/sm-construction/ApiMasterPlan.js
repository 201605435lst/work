import qs from 'qs';

let url = '/api/app/masterPlan';

export default class ApiMasterPlan {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 查询 施工计划 列表
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }

  // 根据id 查询单个施工计划
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }

  // 添加施工计划
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }

  // 编辑施工计划
  async update(id, data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      params: { id },
      data,
    });
  }
  // 施工计划是否有编制
  async hasContent(id) {
    return await this.axios({
      url: `${url}/hasContent`,
      method: 'post',
      params: { id },
    });
  }
  // 根据施工计划id 修改开始结束时间
  async changeDateById(id,times) {
    return await this.axios({
      url: `${url}/changeDateById`,
      method: 'post',
      params: { id },
      data:times,
    });
  }
  // 给masterPlan 创建审批流程
  async createWorkFlow(id,workFlowId) {
    return await this.axios({
      url: `${url}/createWorkFlow`,
      method: 'post',
      params: { id ,workFlowId},
    });
  }


  // 删除
  async delete(id) {
    return await this.axios({
      url: `${url}/delete`,
      method: 'delete',
      params: { id },
    });
  }

  // 删除 多个
  async deleteRange(ids) {
    return await this.axios({
      url: `${url}/deleteRange`,
      method: 'delete',
      params: { ids },
      paramsSerializer: params => { // http 请求中 params 里面 传递 重复参数(数组) 具体 类似这样 ?ids=1&ids=2&ids=3
        return qs.stringify(params, {
          arrayFormat: 'repeat',
        });
      },
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
