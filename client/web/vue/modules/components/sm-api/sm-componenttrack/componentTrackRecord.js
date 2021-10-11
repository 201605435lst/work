import qs from 'qs';
let url = '/api/app/componentTrackRecord';
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

  // 创建跟踪记录
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }


}
