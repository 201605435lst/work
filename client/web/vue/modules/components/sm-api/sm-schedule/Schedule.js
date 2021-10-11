let url = '/api/app/scheduleSchedule';
import qs from 'qs';

export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 查询单个
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }

  // 查询 获得所有列表
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

  // 查询  获取简易参数列表
  async getSimpleList(params) {
    return await this.axios({
      url: `${url}/getSimpleList`,
      method: 'get',
      params,
      paramsSerializer: params => {
        return qs.stringify(params, { indices: false });
      },
    });
  }

  //查询多个
  async getByIds(ids) {
    return await this.axios({
      url: `${url}/getByIds`,
      method: 'get',
      params: {
        ids,
      },
      paramsSerializer: params => {
        return qs.stringify(params, {
          indices: false,
        });
      },
    });
  }

  // 添加
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }

  // 编辑
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
    });
  }

  // 删除
  async delete(id) {
    return await this.axios({
      url: `${url}/delete`,
      method: 'delete',
      params: { id },
    });
  }

  //导入EXCEL文件
  async upload(data) {
    return await this.axios({
      url: `${url}/upload`,
      method: 'post',
      processData: false,
      params: {},
      data,
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

  //得到工作流详情
  async getFlowInfo(workFlowId,scheduleId) {
    return await this.axios({
      url: `${url}/getFlowInfo`,
      method: 'get',
      params: { workFlowId,scheduleId },
    });
  }
}
