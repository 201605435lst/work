let url = '/api/app/projectArchivesCategory';
import qs from 'qs';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }
  // 获取单个信息
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
  // 查询信息
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }
  //导出EXCEL文件
  async export(ids) {
    return await this.axios({
      url: `${url}/export`,
      method: `post`,
      processData: false,
      responseType: 'arraybuffer',
      data: ids,
    });
  }
  //添加
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: `post`,
      data,
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
  // 编辑
  async lock(data) {
    return await this.axios({
      url: `${url}/lock`,
      method: 'post',
      data,
    });
  }
  // 删除
  async delete(id) {
    return await this.axios({
      url: `${url}/delete`,
      method: 'delete',
      params: { id },
    });
  }
}
