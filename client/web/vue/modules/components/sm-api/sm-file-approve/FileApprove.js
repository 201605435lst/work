let url = '/api/app/fileApproveFileApprove';
import qs from 'qs';

export default class Api {
  constructor(axios) {
    this.axios = axios;
  }
  /**
   * 根据节点获取资源信息
   * @param {*} data
   * @returns 资源分页列表
   * @memberof Api
   */
  async getResourceList(params) {
    return await this.axios({
      url: `${url}/GetResourceList`,
      method: 'get',
      params,
      paramsSerializer: params => {
        return qs.stringify(params, { indices: false });
      },
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
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }
  // 流程审批
  async process(data) {
    return await this.axios({
      url: `${url}/process`,
      method: 'post',
      data,
    });
  }
}
