let url = '/api/app/projectDossierCategory';
import qs from 'qs';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }
 
  // 查询信息
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }
}
