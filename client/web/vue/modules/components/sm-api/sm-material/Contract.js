/**
 * 说明：物资合同
 * 作者：easten
 */
let url = '/api/app/materialContract';
import qs from 'qs';
export default class Api {
  constructor(axios) {
    this.axios = axios;
  }
  // 获取列表
  async getList(params) {
    return await this.axios({
      url: `${url}/getlist`,
      method: 'get',
      params,
    });
  }

  // 添加数据
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }

  // 更新数据
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
    });
  }
  // 删除数据
  async delete(id) {
    return await this.axios({
      url: `${url}/delete`,
      method: 'delete',
      params: { id },
    });
  }
  // 删除多条
  async deleteRange(ids) {
    return await this.axios({
      url: `${url}/deleteRange`,
      method: 'delete',
      params:{ids},
      paramsSerializer: params => {
        return qs.stringify(params, { indices: false });
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
  // 查询文件
  async getFileByIds(id){
    return await this.axios({
      url: `${ url}/getFileByIds`,
      method: 'get',
      params: { id },
    });
  }
}
