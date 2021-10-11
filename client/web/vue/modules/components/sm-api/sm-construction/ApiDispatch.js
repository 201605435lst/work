//派工管理
let url = '/api/app/constructionDispatch';
import qs from 'qs';
export default class ApiDispatch {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 查询 派工单列表
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }

  // 根据id 查询单个派工单
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }

  // 添加派工单
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }

  // 编辑派工单
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
    });
  }
  //提交审批
  async forSubmit(params) {
    return await this.axios({
      url: `${url}/forSubmit`,
      method: 'post',
      params,
    });
  }
  //审批流程操作
  async process(data) {
    return await this.axios({
      url: `${url}/process`,
      method: 'post',
      data,
    });
  }
  // 导出
  async export(id) {
    return await this.axios({
      url: `${url}/export`,
      method: 'post',
      params: { id },
      responseType: 'arraybuffer',
    });
  }
  // 删除 多个
  async delete(ids) {
    return await this.axios({
      url: `${url}/delete`,
      method: 'delete',
      params: { ids },
      paramsSerializer: params => {
        // http 请求中 params 里面 传递 重复参数(数组) 具体 类似这样 ?ids=1&ids=2&ids=3
        return qs.stringify(params, {
          arrayFormat: 'repeat',
        });
      },
    });
  }
}
