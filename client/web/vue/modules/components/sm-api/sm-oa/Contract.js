let url = '/api/app/oaContract';
import qs from 'qs';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }
  // 获取单个合同信息
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
  // 查询合同信息
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }
  //导出EXCEL文件
  // async downLoad(ids) {
  //   return await this.axios({
  //     url: `${url}/downLoad`,
  //     responseType: 'arraybuffer',
  //     method: 'post',
  //     params: { ids },
  //     paramsSerializer: params => {
  //       return qs.stringify(params, {
  //         arrayFormat: 'repeat',
  //       });
  //     },
  //   });
  // }
  async downLoad(ids) {
    return await this.axios({
      url: `${url}/downLoad`,
      method: `post`,
      processData: false,
      responseType: 'arraybuffer',
      data: ids,
      
    });
  }
  //创建合同信息
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: `post`,
      data,
    });
  }
  // 编辑合同信息
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
  //获取合同编号中的最大编号
  async getMaxCode() {
    return await this.axios({
      url: `${url}/getMaxCode`,
      method: 'get',
    });
  }
}
