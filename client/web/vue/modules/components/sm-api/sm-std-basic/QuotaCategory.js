let url = '/api/app/stdBasicQuotaCategory';
import qs from 'qs';

export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }
  // 获取单个定额分类
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
  

  // 获取定额分类列表
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
      paramsSerializer: params => {
        return qs.stringify(params, { indices: false });
      },
    });
  }
  // 获取定额分类中的最大编码
  async getListCode(id) {
    return await this.axios({
      url: `${url}/getListCode`,
      method: 'get',
      params: {
        id,
      },
    });
  }
  //添加定额分类
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }
  // 编辑定额分类
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
    });
  }
  //删除定额分类
  async delete(id) {
    return await this.axios({
      url: `${url}/delete`,
      method: 'delete',
      params: {
        id,
      },
    });
  }
  //上传EXCEL文件
  async upLoad(data) {
    return await this.axios({
      url: `${url}/upLoad`,
      method: 'post',
      processData: false,
      data,
    });
  }
  
  async getListTree(params) {
    return await this.axios({
      url: `${url}/getListTree`,
      method: 'get',
      params,
      paramsSerializer: params => {
        return qs.stringify(params, { indices: false });
      },
    });
  }
  
}
