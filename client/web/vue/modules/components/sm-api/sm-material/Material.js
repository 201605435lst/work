let url = '/api/app/materialMaterial';
import qs from 'qs';

export default class Api{
  constructor(axios) {
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

  //根据专业Id查材料
  async getByProfessionId(id){
    return await this.axios({
      url: `${url}/getByProfessionId`,
      method: 'get',
      params:{id},
    });
  }

  async getAll(){
    return await this.axios({
      url: `${url}/getAll`,
      method: 'get',
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

  // 导出
  async export(data) {
    return await this.axios({
      url: `${url}/export`,
      method: 'post',
      data,
      responseType: 'arraybuffer',
    });
  }
  // 生成二维码
  async generateCode(data) {
    return await this.axios({
      url: `${url}/generateCode`,
      method: 'post',
      data,
    });
  }
  // 下载二维码
  async exportCode(data) {
    return await this.axios({
      url: `${url}/exportCode`,
      method: 'post',
      data,
    });
  }
}