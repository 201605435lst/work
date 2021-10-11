/**
 * 说明：材料维护相关接口
 * 作者：easten
 */
let url = '/api/app/technologyMaterial';
import qs from 'qs';

export default class Api{
  constructor(axios){
    this.axios=axios;
  }

  // 材料管理
  async getList(params){
    return await this.axios({
      url: `${ url}/getList`,
      method: 'get',
      params: params,
    });
  }

  // 根据类型id 获取材料信息
  async getAllList(typeId){
    return await this.axios({
      url: `${ url}/getAllList`,
      method: 'get',
      params: { typeId },
    });
  }

  async create(data){
    return await this.axios({
      url: `${ url}/create`,
      method: 'post',
      data,
    });
  }

  async delete(id){
    return await this.axios({
      url: `${ url}/delete`,
      method: 'delete',
      params: { id },
    });
  }

  // 批量删除
  async deleteRange(ids){
    return await this.axios({
      url: `${ url}/deleteRange`,
      method: 'delete',
      params: { ids },
      paramsSerializer: params => {
        return qs.stringify(params, { indices: false });
      },
    });
  }

  async update(data){
    return await this.axios({
      url: `${ url}/update`,
      method: 'put',
      data,
    });
  }

  // 导出
  async export(data){
    return await this.axios({
      url: `${ url}/export`,
      method: 'post',
      data,
      responseType:'arraybuffer',
    });
  }
  
  //导出word文件
  async exportPlan(id) {
    console.log(id);
    return await this.axios({
      url: `${url}/exportPlan`,
      method: `get`,
      responseType: 'arraybuffer',
      params: { id },
    });
  }
  // 产品分类信息同步
  async synchronize(taskKey){
    return await this.axios({
      url: `${ url}/synchronize`,
      method: 'post',
      params:{taskKey},
    });
  }


  // 用料计划相关接口
  async getPlanList(params){
    return await this.axios({
      url: `${ url}/getPlanList`,
      method: 'get',
      params,
    });
  }

  // 根据用料计划id获取关联的材料信息
  async getPlanMaterials(planId){
    return await this.axios({
      url: `${ url}/getPlanMaterials`,
      method: 'get',
      params: { planId },
    });
  }


  async planCreate(data){
    return await this.axios({
      url: `${ url}/planCreate`,
      method: 'post',
      data,
      paramsSerializer: params => {
        return qs.stringify(params, { indices: false });
      },
    });
  }

  async planUpdate(data){
    return await this.axios({
      url: `${ url}/planUpdate`,
      method: 'put',
      data,
    });
  }

  async planDelete(id){
    return await this.axios({
      url: `${ url}/planDelete`,
      method: 'delete',
      params: {id},
    });
  }

  // 删除用料计划中的材料信息
  async planMaterialDelete(id){
    return await this.axios({
      url: `${ url}/PlanMaterialDelete`,
      method: 'delete',
      params: {id },
    });
  }

  // 创建用料计划审批流程
  async createWorkFlow(planId,templateId){
    return await this.axios({
      url: `${ url}/createWorkFlow`,
      method: 'post',
      params: {planId,templateId},
    });
  }

  // 用料计划提交状态更新
  async planSubmitStateUpdate(id){
    return await this.axios({
      url: `${ url}/planSubmitStateUpdate`,
      method: 'put',
      params: {id},
    });
  }

  // 流程审批
  async process(data){
    return await this.axios({
      url: `${ url}/process`,
      method: 'post',
      data,
    });
  }
}
