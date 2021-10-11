//天窗计划和其他计划相关接口
let url = '/api/app/crPlanSkylightPlan';
let url1 = '/api/app/crPlanMaintenanceWork';

export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 获得天窗列表
  async getList(params, repairTagKey) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params: { ...params, repairTagKey },
    });
  }
  // 获取最后添加的数据
  async getLastPlan(params) {
    return await this.axios({
      url: `${url}/getLastPlan`,
      method: 'get',
      params,
    });
  }
  //获取其他计划列表
  async getOtherPlanList(params, repairTagKey) {
    return await this.axios({
      url: `${url}/getOtherPlanList`,
      method: 'get',
      params: { ...params, repairTagKey },
    });
  }

  // 下发其他计划
  async publishOtherPlan(data, repairTagKey) {
    return await this.axios({
      url: `${url}/publishOtherPlan`,
      method: 'post',
      data: { ...data, repairTagKey },
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

  // 发布天窗计划
  async publishPlan(data, repairTagKey) {
    return await this.axios({
      url: `${url}/publishPlan`,
      method: 'post',
      data: { ...data, repairTagKey },
    });
  }

  // 撤销天窗计划
  async backoutPlan(data) {
    return await this.axios({
      url: `${url}/backoutPlan`,
      method: 'post',
      data,
    });
  }

  // 添加天窗计划
  async create(isOther, data, repairTagKey) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data: { ...data, repairTagKey },
      params: { isOther },
    });
  }

  //编辑天窗计划
  async update(isOther, data, repairTagKey) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data: { ...data, repairTagKey },
      params: { isOther },
    });
  }

  //维修计划二阶段审批 编辑天窗计划
  async simpleUpdate(data) {
    return await this.axios({
      url: `${url}/simpleUpdate`,
      method: 'post',
      data,
    });
  }

  // 删除天窗计划
  async remove(params) {
    return await this.axios({
      url: `${url}/remove`,
      method: 'delete',
      params,
    });
  }

  // 获取单条待选计划
  async getSelectablePlan(params) {
    return await this.axios({
      url: `/api/app/crPlanAlterRecord/getSelectablePlan`,
      method: 'get',
      params,
    });
  }

  /**
                  * 工作票
                  */

  //添加工作票
  async createWorkTicket(data) {
    return await this.axios({
      url: `${url}/createWorkTicket`,
      method: 'post',
      data,
    });
  }

  //获取工作票
  async getWorkTickets(params) {
    return await this.axios({
      url: `${url}/getWorkTickets?id=` + params,
      method: 'get',
    });
  }

  //获取工作票完成数量情况
  async getWorkTicketFinishInfo(params) {
    return await this.axios({
      url: `${url}/getWorkTicketFinishInfo?id=` + params,
      method: 'get',
    });
  }

  //编辑工作票
  async updateWorkTicket(data) {
    return await this.axios({
      url: `${url}/updateWorkTicket`,
      method: 'put',
      data: data,
    });
  }

  //完成工作票
  async finishWorkTicket(data) {
    return await this.axios({
      url: `${url}/finishWorkTicket`,
      method: 'post',
      data: data,
    });
  }

  async removeWorkTicket(params) {
    return await this.axios({
      url: `${url}/deleteWorkTicket`,
      method: 'delete',
      params,
    });
  }

  // 下载工作票
  async exportWorkTicket(workflowId, isWorkflowId) {
    return await this.axios({
      url: `${url1}/exportWorkTicket`,
      method: 'get',
      responseType: 'arraybuffer',
      params: { workflowId, isWorkflowId },
    });
  }
  //代办消息通知
  async confirmTodoMessage(params) {
    return await this.axios({
      url: `${url}/confirmTodoMessage`,
      method: 'post',
      params,
    });
  }
  // 获得天窗列表
  async getWorkTicketList(params) {
    return await this.axios({
      url: `${url}/getWorkTicketList`,
      method: 'get',
      params: { ...params },
    });
  }

  //完成配合作业
  async finishCooperationWork(params) {
    return await this.axios({
      url: `${url}/finishCooperationWork`,
      method: 'post',
      data: { ...params },
    });
  }

  //一键引用月表
  async oneTouchMonthPlan(params) {
    return await this.axios({
      url: `${url}/oneTouchMonthPlan`,
      method: 'post',
      params,
    });
  }
}

