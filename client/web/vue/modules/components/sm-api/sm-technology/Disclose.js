/**
 * 说明：技术交底相关接口
 * 作者：easten
 */
let url = '/api/app/technologydisclose';
import qs from 'qs';
export default class Api {
  constructor(axios) {
    this.axios = axios;
  }

  async get(params){
    return await this.axios({
      url: `${ url}/get`,
      method: 'get',
      params,
    });
  }
  async getList(params){
    return await this.axios({
      url: `${ url}/getList`,
      method: 'get',
      params,
    });
  }

  async create(data){
    return await this.axios({
      url: `${ url}/create`,
      method: 'post',
      data,
    });
  }

  async update(data){
    return await this.axios({
      url: `${ url}/update`,
      method: 'put',
      data,
    });    
  }
  async updateAttachment(data){
    return await this.axios({
      url: `${ url}/updateAttachment`,
      method: 'put',
      data,
    });
  }

  async delete(id){
    return await this.axios({
      url: `${ url}/delete`,
      method: 'delete',
      params: { id },
    });
  }
  async deleteRange(ids){
    return await this.axios({
      url: `${ url}/deleteRange`,
      method: 'delete',
      params: { ids },
      paramsSerializer: params => {
        return qs.stringify(params, { indices: false });
      },
    });
  }
  

}
