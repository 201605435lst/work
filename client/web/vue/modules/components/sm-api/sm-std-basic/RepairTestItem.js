let url = '/api/app/stdBasicRepairTestItem';

export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }
  // 查询单个
  async get(params) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params,
    });
  }

  // 添加
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }
  // 编辑顺序
  async updateOrder(data) {
    return await this.axios({
      url: `${url}/updateOrder`,
      method: 'put',
      data,
    });
  }
  // 编辑
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
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
}
