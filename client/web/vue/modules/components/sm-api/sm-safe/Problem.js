import qs from 'qs';

let url = '/api/app/safeProblem';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }
  // 
  async getWaitingImproveList(params) {
    return await this.axios({
      url: `${url}/getWaitingImproveList`,
      method: 'get',
      params,
    });
  }
  // 获取报告详情
  async getSafeProblemReport(id) {
    return await this.axios({
      url: `${url}/getSafeProblemReport`,
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

  // 添加
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }
  // 添加
  async createRecord(data) {
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
      method: 'post',
      data,
    });
  }

  // 删除
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
}
