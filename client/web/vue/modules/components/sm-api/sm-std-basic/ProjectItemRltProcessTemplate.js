let url = '/api/app/stdBasicProjectItemRltProcessTemplate';

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

  async getListByProcessTemplateId(params) {
    return await this.axios({
      url: `${url}/getListByProcessTemplateId`,
      method: 'get',
      processData: false,
      params,
    });
  }
}
