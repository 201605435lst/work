let url = '/api/app/stdBasicComponentCategoryRltMVDProperty';

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

  async getListByComponentCategoryId(data) {
    return await this.axios({
      url: `${url}/getListByComponentCategoryId`,
      method: 'post',
      // processData: false,
      data,
      // paramsSerializer: params => {
      //   return qs.stringify(params, { indices: false });
      // },
    });
  }
}
