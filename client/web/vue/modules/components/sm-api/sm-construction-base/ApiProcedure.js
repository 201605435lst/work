let url = '/api/app/procedure';

export default class ApiProcedure {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 查询 施工工序 列表
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }

  // 根据id 查询单个施工工序
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }

  // 添加施工工序
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }

  // 编辑施工工序
  async update(id, data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      params: { id },
      data,
    });
  }
  // 配置 施工工序
  async configProcedure(id, data) {
    return await this.axios({
      url: `${url}/configProcedure`,
      method: 'post',
      params: { id },
      data,
    });
  }
  // 获取 关联 表
  async getRltList(id) {
    return await this.axios({
      url: `${url}/getRltList`,
      method: 'get',
      params: { id },
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
