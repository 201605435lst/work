let url = '/api/app/stdBasicRevitConnector';
import qs from 'qs';

export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  // async editList(data) {
  //   return await this.axios({
  //     url: `${url}/editList`,
  //     method: 'post',
  //     processData: false,
  //     data,
  //   });
  // }
  async get(id,name) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: {
        id,
        name,
      },
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

  async getListByModelFileId(params) {
    return await this.axios({
      url: `${url}/getListByModelFileId`,
      method: 'get',
      processData: false,
      params,
    });
  }

  //上传EXCEL文件
  async upload(data) {
    return await this.axios({
      url: `${url}/upload`,
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
}
