/**
 * 说明：工程量统计相关接口
 * 作者：easten
 */
let url = '/api/app/technologyquantity';
import qs from 'qs';
export default class Api{
  constructor(axios){
    this.axios=axios;
  }


  async getSpeciality(){
    return await this.axios({
      url: `${ url}/getSpeciality`,
      method: 'get',

    });
  }
  async getList(params){
    return await this.axios({
      url: `${ url}/getlist`,
      method: 'get',
      params,
    });
  }
  async export(data){
    return await this.axios({
      url: `${ url}/export`,
      method: 'post',
      data,
      responseType:'arraybuffer',
      paramsSerializer: params => {
        return qs.stringify(params, { indices: false });
      },
    });
  }

  // 生成用料计划
  async createMaterialPlan(data){
    return await this.axios({
      url: `${ url}/createMaterialPlan`,
      method: 'post',
      data,
    });
  }
}
