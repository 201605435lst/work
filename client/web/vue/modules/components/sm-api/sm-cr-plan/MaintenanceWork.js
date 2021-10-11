//天窗计划和其他计划相关接口
let url = '/api/app/crPlanMaintenanceWork';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 获得维修计划管理
  async getList(params, repairTagKey) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params: { ...params, repairTagKey },
    });
  }
  // 获取详情
  async get(params) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params,
    });
  }
  // 添加维修计划
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }
  // 删除维修计划
  async delete(params) {
    return await this.axios({
      url: `${url}/delete`,
      method: 'delete',
      params,
    });
  }

  ///获取 维修项对应的天窗计划
  async getMaintenanceWork(params) {
    return await this.axios({
      url: `${url}/getMaintenanceWork`,
      method: 'get',
      params,
    });
  }

  async sumbitFirsrFlow(data) {
    return await this.axios({
      url: `${url}/sumbitFirsrFlow`,
      method: 'post',
      data,
    });
  }

  // 获取第二阶段审批的维修作业
  async getListForSecondStep(params, repairTagKey) {
    return await this.axios({
      url: `${url}/getListForSecondStep`,
      method: 'get',
      params: { ...params, repairTagKey },
    });
  }

  //提交shenp
  async sumbitSecondFlow(id) {
    return await this.axios({
      url: `${url}/sumbitSecondFlow?id=` + id,
      method: 'post',
    });
  }

  // 删除维修计划与垂直天窗关联关系
  async removeMaintenanceWorkRltSkylightPlan(params) {
    return await this.axios({
      url: `${url}/removeMaintenanceWorkRltSkylightPlan`,
      method: 'delete',
      params,
    });
  }

  // 导出维修计划表
  async exportMaintenanceWorkPlan(params) {
    return await this.axios({
      url: `${url}/exportMaintenanceWorkPlan`,
      method: 'get',
      params,
      responseType: 'arraybuffer',
    });
  }
}


