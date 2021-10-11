let url = '/api/app/qualityProblem';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }
  //
  async getWaitingImproveList(params) {
    return await this.axios({
      url: `${url}/getWaitingImproveList`,
      method: 'get',
      params,
    });
  }
  // 查询
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }
  // 获取报告详情
  async getQualityProblemReport(id) {
    return await this.axios({
      url: `${url}/getQualityProblemReport`,
      method: 'get',
      params: { id },
    });
  }
  // 查询问题整改验证记录
  async getRecordList(id) {
    return await this.axios({
      url: `${url}/getRecordList`,
      method: 'get',
      params: { id },
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

  // 添加标记
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }

  // 添加记录
  async createRecord(data) {
    return await this.axios({
      url: `${url}/createRecord`,
      method: 'post',
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
  // 编辑
  async update(data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'post',
      data,
    });
  }

  //导出
  async export(id) {
    return await this.axios({
      url: `${url}/export`,
      responseType: 'arraybuffer',
      method: 'post',
      params: { id },
    });
  }
}
