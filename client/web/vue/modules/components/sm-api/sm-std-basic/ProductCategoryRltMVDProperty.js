let url = '/api/app/stdBasicProductCategoryRltMVDProperty';
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

  async getListByProductCategoryId(data) {
    return await this.axios({
      url: `${url}/getListByProductCategoryId`,
      method: 'post',
      data
      // processData: false,
      // params,
      // paramsSerializer: params => {
      //   return qs.stringify(params, { indices: false });
      // },
    });
  }
}
