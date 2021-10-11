let url = '/api/app/regulationLabel';
import qs from 'qs';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  //增加标签信息
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: `post`,
      data,
    });
  }

  //删除标签信息
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

  //更新
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
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

  // 查询标签信息
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: `get`,
      params,
    });
  }
}
