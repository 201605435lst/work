let url = '/api/app/stdBasicModelRltMVDPropertyAppSerive';
import qs from 'qs';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  async editList(data) {
    return await this.axios({
      url: `${url}/editList`,
      method: 'post',
      processData: false,
      data,
    });
  }

  async getListByModelId(params) {
    return await this.axios({
      url: `${url}/getListByModelId`,
      method: 'get',
      processData: false,
      params,
      paramsSerializer: params => {
        return qs.stringify(params, { indices: false });
      },
    });
  }
}
