/**
 * 说明：领料单管理接口
 */

let url = '/api/app/materialOfBill';

export default class Api {
  constructor(axios) {
    this.axios = axios;
  }

  // 查询id
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }

  // 获取列表
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }

  //添加
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
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
  // 导出
  async export(id) {
    return await this.axios({
      url: `${url}/export`,
      method: 'post',
      params: { id },
      responseType: 'arraybuffer',
    });
  }
}
