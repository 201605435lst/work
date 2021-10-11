import qs from 'qs';

let url = '/api/app/componentTrackProcess';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }
  // 获取详情
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
  
  // 查询
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }
  // 查询节点跟踪信息
  async getRecord(params) {
    return await this.axios({
      url: `${url}/getRecord`,
      method: 'get',
      params,
    });
  }
  // 添加
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }
  //创建构建跟踪记录
  async createRecord(){
    return await this.axios({
      url: `${url}/createRecord`,
      method: 'post',
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
  // 删除
  async delete(id) {
    return await this.axios({
      url: `${url}/delete`,
      method: 'delete',
      params: { id },
    });
  }
}
