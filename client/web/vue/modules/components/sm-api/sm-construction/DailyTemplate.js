let url = '/api/app/constructionDailyTemplate';
import qs from 'qs';
export default class Api {
  constructor(axios) {
    this.axios = axios;
  }
  // 获取列表
  async getList(params) {
    return await this.axios({
      url: `${url}/getlist`,
      method: 'get',
      params,
    });
  }
  // 根据id 查询
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
  // 根据id 查询
  async setDefault(id) {
    return await this.axios({
      url: `${url}/setDefault`,
      method: 'get',
      params: { id },
    });
  }
  // 添加数据
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }
 
  // 更新数据
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
    });
  }
  // 删除数据
  async delete(id) {
    return await this.axios({
      url: `${url}/delete`,
      method: 'delete',
      params: { id },
    });
  }
}
 