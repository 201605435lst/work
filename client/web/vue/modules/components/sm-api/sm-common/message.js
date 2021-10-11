// 文件导入接口定义
let url = '/api/message/bpmMessage';
import qs from 'qs';

export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  async getList(key){
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params: key,
    });
  }

  // 批量标记已读
  async updateRange(ids) {
    return await this.axios({
      url: `${url}/updateRange`,
      method: 'put',
      params: {ids},
      paramsSerializer: params => {
        return qs.stringify(params, {
          arrayFormat: 'repeat',
        });
      },
    });
  }
  // 删除
  async delete(id) {
    return await this.axios({
      url: `${url}/delete`,
      method: 'delete',
      params: {id},
    });
  }
}