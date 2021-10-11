let url = '/api/app/stdBasicProjectItemRltIndividualProject';

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
      url: `${url}/getListByIndividualProjectId`,
      method: 'get',
      processData: false,
      params,
    });
  }
}
