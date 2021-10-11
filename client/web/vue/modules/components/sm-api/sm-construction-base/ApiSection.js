import qs from 'qs';

let url = '/api/app/section';

export default class ApiSection {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 查询  施工区段 列表
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }
  // 查询  施工区段 列表
  async getTreeList(params) {
    return await this.axios({
      url: `${url}/getTreeList`,
      method: 'get',
      params,
    });
  }

  // 根据id 查询单个 施工区段
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
  // 根据id 获取多个
  async getByIds(ids) {
    return await this.axios({
      url: `${url}/getByIds`,
      method: 'get',
      params: { ids },
      paramsSerializer: params => { //允许路由params传数组 就要这样写
        return qs.stringify(params, {
          arrayFormat: 'repeat',
        });
      },
    });
  }

  // 添加 施工区段
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }

  // 编辑 施工区段
  async update(id, data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      params: { id },
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
