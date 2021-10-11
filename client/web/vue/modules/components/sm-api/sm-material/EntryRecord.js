let url = '/api/app/materialEntryRecord';
import qs from 'qs';

export default class Api {
  constructor(axios) {
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

  //添加入库记录
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }

  //列表获取
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
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
