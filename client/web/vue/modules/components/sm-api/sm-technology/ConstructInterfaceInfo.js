import qs from 'qs';

let url = '/api/app/constructInterfaceInfo';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }
  // 标记
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }
  //导出word文件
  async export(params) {
    return await this.axios({
      url: `${url}/export`,
      method: `get`,
      responseType: 'arraybuffer',
      params,
    });
  }
  // 查询标记记录
  async getInterfanceReform(params) {
    return await this.axios({
      url: `${url}/getInterfanceReform`,
      method: 'get',
      params,
    });
  }
  // 标记编辑
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
    });
  }
  // 查询单个
  async get(params) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params,
    });
  }
  // 标记整改
  async interfanceReform(data) {
    return await this.axios({
      url: `${url}/interfanceReform`,
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
}
