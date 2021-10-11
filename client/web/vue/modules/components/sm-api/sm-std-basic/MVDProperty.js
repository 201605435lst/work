let url = '/api/app/stdBasicMVDProperty';
import qs from 'qs';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: {
        id,
      },
    });
  }

  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
      paramsSerializer: params => {
        return qs.stringify(params, {
          arrayFormat: 'repeat',
        });
      },
    });
  }

  //上传EXCEL文件
  async import(data) {
    return await this.axios({
      url: `${url}/import`,
      method: 'post',
      processData: false,
      data,
    });
  }

  async export(data) {
    return await this.axios({
      url: `${url}/export`,
      method: 'post',
      processData: false,
      data,
    });
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
      params: {
        id,
      },
      paramsSerializer: params => {
        return qs.stringify(params, {
          arrayFormat: 'repeat',
        });
      },
    });
  }
}
