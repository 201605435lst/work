
//年月表计划相关接口
let url = '/api/app/crPlanYearMonthPlan';
import qs from 'qs';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 获得天窗类型列表
  async getSkylightTypeList(params, repairTagKey) {
    return await this.axios({
      url: `${url}/getSkyligetTypeList`,
      method: 'get',
      params: { ...params, repairTagKey },
    });
  }

  // 获得当前年月表变更列表(点击变更按钮后看到的列表)
  async getOwnsChangPlan(params, repairTagKey) {
    return await this.axios({
      url: `${url}/getOwnsChangPlan`,
      method: 'get',
      params: { ...params, repairTagKey },
    });
  }

  // 获得可变更的年月列表
  async getCanChangePlanList(params, repairTagKey) {
    return await this.axios({
      url: `${url}/getCanChangePlanList`,
      method: 'get',
      params: { ...params, repairTagKey },
    });
  }

  // 年表查询
  async getListYear(params, repairTagKey) {
    return await this.axios({
      url: `${url}/getListYear`,
      method: 'get',
      params: { ...params, repairTagKey },
    });
  }

  // 年表记录查询
  async getYearStatisticData(params, repairTagKey) {
    return await this.axios({
      url: `${url}/getYearStatisticData`,
      method: 'get',
      params: { ...params, repairTagKey },
    });
  }

  // 年表查询,查询已提交的数量大于0的项
  async getSubmitedListYear(params, repairTagKey) {
    return await this.axios({
      url: `${url}/getSubmitedListYear`,
      method: 'get',
      params: { ...params, repairTagKey },
    });
  }

  // 月表查询
  async getListMonth(params, repairTagKey) {
    return await this.axios({
      url: `${url}/getListMonth`,
      method: 'get',
      params: { ...params, repairTagKey },
    });
  }

  // 月表记录查询
  async getMonthStatisticData(params, repairTagKey) {
    return await this.axios({
      url: `${url}/getMonthStatisticData`,
      method: 'get',
      params: { ...params, repairTagKey },
    });
  }

  // 月表查询,查询已提交的数量大于0的项
  async getSubmitedListMonth(params, repairTagKey) {
    return await this.axios({
      url: `${url}/getSubmitedListMonth`,
      method: 'get',
      params: { ...params, repairTagKey },
    });
  }

  // 添加（年表、月表、年度月表）
  async create(data, repairTagKey) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data: { ...data, repairTagKey },
    });
  }

  // 保存可变更的年月列表
  async createChangePlan(data) {
    return await this.axios({
      url: `${url}/createChangePlan`,
      method: 'post',
      data,
    });
  }

  // 编辑天窗类型
  async updateSkyligetType(data, repairTagKey) {
    return await this.axios({
      url: `${url}/updateSkyligetType`,
      method: 'put',
      data: { ...data, repairTagKey },
    });
  }

  // 编辑月计划
  async updateYearMonthPlan(data, repairTagKey) {
    return await this.axios({
      url: `${url}/updateYearMonthPlan`,
      method: 'put',
      data: { ...data, repairTagKey },
    });
  }

  // 删除年月表变更数据
  async deletePlanAtler(id, repairTagKey) {
    return await this.axios({
      url: `${url}/deletePlanAtler`,
      method: 'delete',
      params: { id, repairTagKey },
    });
  }

  //上传EXCEL文件
  async upLoad(data, planType, orgId, year, month, repairTagKey) {
    return await this.axios({
      url: `${url}/upLoad?planType=${planType}&orgId=${orgId}&year=${year}${
        month ? '&month=' + month : ''
      }`,
      method: 'post',
      processData: false,
      data: { ...data, repairTagKey },
    });
  }

  //导出EXCEL文件
  async downLoad(data, repairTagKey) {
    return await this.axios({
      url: `${url}/downLoad`,
      responseType: 'arraybuffer',
      method: 'post',
      processData: false,
      // params: {  },
      data: { ...data, repairTagKey },
    });
  }

  //年月表变更导入EXCEL
  async uploadChange(data, planType, orgId, repairTagKey) {
    return await this.axios({
      url: `${url}/uploadChange`,
      method: 'post',
      processData: false,
      params: { planType, orgId },
      data: { ...data, repairTagKey },
    });
  }

  //年月表变更导出EXCEL
  async downLoadChange(data, repairTagKey) {
    return await this.axios({
      url: `${url}/downLoadChange`,
      method: 'post',
      responseType: 'arraybuffer',
      processData: false,
      data: { ...data, repairTagKey },
    });
  }

  // 年月表提交审核
  async submitForExam(data, repairTagKey) {
    return await this.axios({
      url: `${url}/submitForExam`,
      method: 'post',
      data: { ...data, repairTagKey },
    });
  }

  // 年月表变更提交审核
  async submitChangeForExam(data, repairTagKey) {
    return await this.axios({
      url: `${url}/submitChangeForExam`,
      method: 'post',
      data: { ...data, repairTagKey },
    });
  }

  //更新维修项下的测试项至指定年份年月表测试项
  async upgradeTestItems(data, repairTagKey) {
    return await this.axios({
      url: `${url}/upgradeTestItems`,
      method: 'post',
      data: { ...data, repairTagKey },
    });
  }

  async clearFilled(data, repairTagKey) {
    return await this.axios({
      url: `${url}/clearFilled`,
      method: 'post',
      data: { ...data, repairTagKey },
    });
  }

  //导出月表记录
  async exportMonthStatisticData(data, repairTagKey) {
    return await this.axios({
      url: `${url}/exportMonthStatisticData`,
      responseType: 'arraybuffer',
      method: 'post',
      processData: false,
      data: { ...data, repairTagKey },
    });
  }
  //导出年表记录
  async exporYearStatisticData(data, repairTagKey) {
    return await this.axios({
      url: `${url}/exporYearStatisticData`,
      responseType: 'arraybuffer',
      method: 'post',
      processData: false,
      data: { ...data, repairTagKey },
    });
  }
  //统计数据
  async getTotal(params) {
    return await this.axios({
      url: `${url}/getTotal`,
      method: 'get',
      params,
    });
  }
  //统计车间
  async getOrganizationNames(params) {
    return await this.axios({
      url: `${url}/getOrganizationNames`,
      method: 'get',
      params,
      paramsSerializer: params => {
        return qs.stringify(params, { indices: false });
      },
    });
  }

  async getYearMonthAlterRecords(params) {
    return await this.axios({
      url: `${url}/getYearMonthAlterRecords`,
      method: 'get',
      params,
      // paramsSerializer: params => {
      //   return qs.stringify(params, { indices: false });
      // },
    });
  }
  //退回年表月度计划
  async backMonthOfYearPalns(data) {
    return await this.axios({
      url: `${url}/backMonthOfYearPalns`,
      method: 'post',
      data: { ...data },
    });
  }
}
