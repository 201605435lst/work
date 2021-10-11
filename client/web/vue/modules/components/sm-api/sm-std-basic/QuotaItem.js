let url = '/api/app/stdBasicQuotaItem';
import qs from 'qs';

export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }
  // 获取单个定额清单
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
  

  // 获取定额清单列表
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

  //添加定额清单
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }
  // 编辑定额清单
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
    });
  }
  //删除定额清单
  async delete(id,computerCodeId) {
    return await this.axios({
      url: `${url}/delete`,
      method: 'delete',
      params: {
        id,
        computerCodeId},
    });
  }
  //上传EXCEL文件
  async upLoad(data,id) {
    return await this.axios({
      url: `${url}/upLoad`,
      method: 'post',
      processData: false,
      data,
      id,
    });
  }
  
}
