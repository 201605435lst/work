let url = '/api/app/regulationInstitution';
import qs from 'qs';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  //创建制度信息
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: `post`,
      data,
    });
  }

  //删除制度信息
  async delete(ids) {
    return await this.axios({
      url: `${url}/delete`,
      method: 'delete',
      params: { ids },
      paramsSerializer: params => {
        return qs.stringify(params, {
          arrayFormat: 'repeat',
        });
      },
    });
  }

  // 编辑
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
    });
  }

  async updatePermission(ids, data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
    });
  }

  // 查询制度信息
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: `get`,
      params,
    });
  }

  // 获取详情
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
  //导出制度信息
  async downLoad(ids) {
    return await this.axios({
      url: `${url}/downLoad`,
      method: `post`,
      processData: false,
      responseType: 'arraybuffer',
      data: ids,
    });
  }
}
