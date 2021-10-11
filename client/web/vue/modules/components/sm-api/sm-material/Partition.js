let url = '/api/app/partition';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }

  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
    });
  }
  async delete(id) {
    return await this.axios({
      url: `${url}/delete`,
      method: 'delete',
      params: { id: id },
    });
  }

  async getTreeList() {
    return await this.axios({
      url: `${url}/getTreeList`,
      method: 'get',
    });
  }

  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
}
