let url = '/api/app/reportReport';
import qs from 'qs';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }
  // 获取单个报告信息
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
  // 查询报告信息
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }
  //导出EXCEL文件
  // 导出
  async export(data) {
    return await this.axios({
      url: `${url}/export`,
      responseType:'arraybuffer',
      method: 'post',
      data,
    });
  }
  //创建报告信息
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: `post`,
      data,
    });
  }
  // 编辑报告信息
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
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
  //获取报告编号中的最大编号
  async getMaxCode() {
    return await this.axios({
      url: `${url}/getMaxCode`,
      method: 'get',
    });
  }
}
