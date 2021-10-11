import qs from 'qs';

let url = '/api/app/dispatchTemplate';

export default class ApiDispatchTemplate {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 查询 派工单模板 列表
  async getAllList(params) {
    return await this.axios({
      url: `${url}/getAllList`,
      method: 'get',
      params,
    });
  }

  // 根据id 查询单个派工单模板
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }

  // 获取默认模板名称
  async getDefaultName() {
    return await this.axios({
      url: `${url}/getDefaultName`,
      method: 'get',
    });
  }

  // 添加派工单模板
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }

  // 编辑派工单模板
  async update(id, data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      params: { id },
      data,
    });
  }
  // 设置默认模板
  async setDefault(id) {
    return await this.axios({
      url: `${url}/setDefault`,
      method: 'post',
      params: { id },
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
      paramsSerializer: params => {
        // http 请求中 params 里面 传递 重复参数(数组) 具体 类似这样 ?ids=1&ids=2&ids=3
        return qs.stringify(params, {
          arrayFormat: 'repeat',
        });
      },
    });
  }
}
