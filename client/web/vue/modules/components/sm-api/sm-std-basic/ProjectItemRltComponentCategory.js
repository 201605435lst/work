let url = '/api/app/stdBasicProjectItemRltComponentCategory';
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

  async getListByProjectItemId(params) {
    return await this.axios({
      url: `${url}/getListByProjectItemId`,
      method: 'get',
      processData: false,
      params,
    });
  }
}
