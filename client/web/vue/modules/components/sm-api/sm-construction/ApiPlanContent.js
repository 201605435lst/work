
import qs from 'qs';


let url = '/api/app/planContent';

export default class ApiPlanContent {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 查询 施工计划详情 列表
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }

  // 根据 施工计划id 获取 详情 树(单树)
  async getSingleTree(planId,filter) {
    return await this.axios({
      url: `${url}/getSingleTree`,
      method: 'get',
      params:{planId,...filter},
    });
  }

  // 删除 多个
  async getByIds(ids) {
    return await this.axios({
      url: `${url}/getByIds`,
      method: 'get',
      params: { ids },
      paramsSerializer: params => { // http 请求中 params 里面 传递 重复参数(数组) 具体 类似这样 ?ids=1&ids=2&ids=3
        return qs.stringify(params, {
          arrayFormat: 'repeat',
        });
      },
    });
  }

  // 根据 施工计划id 获取 详情 树(单树)
  async getTreeExcludeSelf(id, planId) {
    return await this.axios({
      url: `${url}/getTreeExcludeSelf`,
      method: 'get',
      params:{id,planId},
    });
  }
  // 根据id 查询单个施工计划详情
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }
  // 根据id 查询单个施工计划详情
  async test(obj) {
    return await this.axios({
      url: `${url}/test`,
      method: 'post',
      data: obj,
    });
  }
  // 根据plan id 获取  年份 列表
  async getYears(planId) {
    return await this.axios({
      url: `${url}/getYears`,
      method: 'get',
      params: { planId },
    });
  }

  // 添加施工计划详情
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }


  // 给详情加 前置任务
  async changeFrontTask(id,contentIds) {
    return await this.axios({
      url: `${url}/changeFrontTask`,
      method: 'post',
      params: {id},
      data:contentIds,
    });
  }
  // 引用 分部分项
  async import(id,subItemContentIds) {
    return await this.axios({
      url: `${url}/importSubItem`,
      method: 'post',
      params: {id},
      data:subItemContentIds,
    });
  }
  // 批量保存
  async batchSave(data) {
    return await this.axios({
      url: `${url}/batchSave`,
      method: 'post',
      data,
    });
  }
  // 升级
  async lvUp(id) {
    return await this.axios({
      url: `${url}/lvUp`,
      method: 'post',
      params: {id},
    });
  }
  // 降级
  async lvDown(id) {
    return await this.axios({
      url: `${url}/lvDown`,
      method: 'post',
      params: {id},
    });
  }
  // 编辑施工计划详情
  async update(id, data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      params: { id },
      data,
    });
  }

  // 移动任务
  async moveTask(id,pId) {
    return await this.axios({
      url: `${url}/moveTask`,
      method: 'post',
      params: { id,pId },
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
  // 删除 多个
  async deleteRange(ids) {
    return await this.axios({
      url: `${url}/deleteRange`,
      method: 'delete',
      params: { ids },
      paramsSerializer: params => { // http 请求中 params 里面 传递 重复参数(数组) 具体 类似这样 ?ids=1&ids=2&ids=3
        return qs.stringify(params, {
          arrayFormat: 'repeat',
        });
      },
    });
  }
  // 引用 总体计划 里面 的 content 树
  async importMasterPlan(planId,masterPlanId) {
    return await this.axios({
      url: `${url}/importMasterPlan`,
      method: 'post',
      params: { planId,masterPlanId },
      // data: {planId,masterPlanId},
    });

  }

  // 进度模拟
  async getAnalogData(params){
    return await this.axios({
      url: `${ url}/getAnalogData`,
      method: 'get',
      params,
    });
  }

  // 获取计划下的所有设备信息
  async getAllRelationEquipment(id){
    return await this.axios({
      url: `${ url}/GetAllRelationEquipment`,
      method: 'get',
      params: { id },
    });
  }
}
