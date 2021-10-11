let url = '/api/app/oaSeal';
import qs from 'qs';

export default class Api{
  constructor(axios){
    this.axios = axios || null;
  }

  // 获取详情
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
    
  //添加值班表
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }
  async getList(params){
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }

  //更新
  async update(data){
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
      params: {ids},
      paramsSerializer: params => {
        return qs.stringify(params, {
          arrayFormat: 'repeat',
        });
      },
    });
  }
}