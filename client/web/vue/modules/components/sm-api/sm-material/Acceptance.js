import qs from 'qs';

let url = '/api/app/materialAcceptance';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 查询
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
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

  // 编辑
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
    });
  }
  // 物料验收
  async updateCheck(data) {
    return await this.axios({
      url: `${url}/updateCheck`,
      method: 'put',
      data,
    });
  }
  // 提交
  async submit(data) {
    return await this.axios({
      url: `${url}/submit`,
      method: 'post',
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
  //导出word文件
  async export(id) {
    return await this.axios({
      url: `${url}/export`,
      method: `post`,
      responseType: 'arraybuffer',
      params: { id },
    });
  }
}
