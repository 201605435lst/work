/**
 * 说明：仓库管理接口
 * 作者：easten
 */

let url = '/api/app/inventory';
import qs from 'qs';
export default class Api {
  constructor(axios) {
    this.axios = axios;
  }

  // 获取仓库列表
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }

  // 获取详情
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
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
      params: { ids },
      paramsSerializer: params => {
        return qs.stringify(params, { indices: false });
      },
    });
  }

  //库存数据导出
  async export(data) {
    return await this.axios({
      url: `${url}/export`,
      method: 'post',
      data,
      responseType: 'arraybuffer',
    });
  }

  //单个物资出入库信息导出
  async exportDetail(id) {
    return await this.axios({
      url: `${url}/exportDetail`,
      method: 'post',
      params: { id },
      responseType: 'arraybuffer',
    });
  }
}
