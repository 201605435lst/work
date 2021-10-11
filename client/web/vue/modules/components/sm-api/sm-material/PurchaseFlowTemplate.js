let url = '/api/app/PurchaseFlowTemplate';
import qs from 'qs';

export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 添加
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }

  async get() {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
    });
  }

}