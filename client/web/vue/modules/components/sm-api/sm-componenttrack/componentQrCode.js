import qs from 'qs';

let url = '/api/app/componentComponentRltQRCode';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }
  // 生成二维码
  async exportCode(ids) {
    return await this.axios({
      url: `${url}/exportCode`,
      method: 'post',
      data: { ids },
      processData: false,
      responseType:'arraybuffer',
    });
  }
  // 获取构件二维码跟踪记录信息
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
  // 获取构件二维码跟踪记录信息
  async getView(id) {
    return await this.axios({
      url: `${url}/getView`,
      method: 'get',
      params: { id },
    });
  }
  // 获取二维码信息
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }
}
