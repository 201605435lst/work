
import qs from 'qs';


let url = '/api/app/planMaterial';

export default class ApiPlanMaterial {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 查询 施工计划工程量 列表
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }

  // 根据id 查询单个施工计划工程量
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }

  // 添加施工计划工程量
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }

  // 编辑施工计划工程量
  async update(id, data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      params: { id },
      data,
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

  // 根据施工计划id 设置工程量
  async setPlanMaterial(planContentId, equipmentIds) {
    return await this.axios({
      url: `${url}/setPlanMaterial`,
      method: 'post',
      params:{ planContentId},
      data:equipmentIds,
    });
  }
}
