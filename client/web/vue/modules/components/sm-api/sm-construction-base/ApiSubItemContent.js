let url = '/api/app/subItemContent';
export default class ApiSubItemContent {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 查询  分部分项-详情 列表
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }

  // 根据id 查询单个 分部分项-详情
  async getSingleTree(id) {
    return await this.axios({
      url: `${url}/getSingleTree`,
      method: 'get',
      params: { id },
    });
  }
  // 根据id 查询单个 分部分项-详情
  async getSingleTreeWithProcedure(id) {
    return await this.axios({
      url: `${url}/getSingleTreeWithProcedure`,
      method: 'get',
      params: { id },
    });
  }
  // 根据 contentId 获取关联工序表
  async getRltProceduresByContentId(contentId) {
    return await this.axios({
      url: `${url}/getRltProceduresByContentId`,
      method: 'get',
      params: { contentId },
    });
  }
  // 获取 关联工序表 关联 的其他 表 的 列表 (worker ,material ,equipment )
  async getRltProcedureRltOtherList(rltProcedureId) {
    return await this.axios({
      url: `${url}/getRltProcedureRltOtherList`,
      method: 'get',
      params: { rltProcedureId },
    });
  }
  // 根据  RltProcedureId 获取 关联 的选择 id 列表
  async getSelectRltIdsByRltProcedureId(rltProcedureId) {
    return await this.axios({
      url: `${url}/getSelectRltIdsByRltProcedureId`,
      method: 'get',
      params: { rltProcedureId },
    });
  }


  // 添加 分部分项-详情
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }

  // 编辑 分部分项-详情
  async update(id, data) {
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      params: { id },
      data,
    });
  }
  // 往content 里面 加 关联工序表
  async addProcedure(id, data) {
    return await this.axios({
      url: `${url}/addProcedure`,
      method: 'post',
      params: { id },
      data,
    });
  }
  // 移动 （上移 下移） 分部分项-详情
  async move(id,moveType) {
    return await this.axios({
      url: `${url}/move`,
      method: 'post',
      params: { id,moveType },
    });
  }
  // 移动 （上移 下移） 分部分项 关联 procedure
  async moveRltProcedure(id,moveType) {
    return await this.axios({
      url: `${url}/moveRltProcedure`,
      method: 'post',
      params: { id,moveType },
    });
  }
  // 第一次点编制的时候 初始化一个 content
  async initContent(subItemId) {
    return await this.axios({
      url: `${url}/initContent`,
      method: 'post',
      params: { subItemId},
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
  // 删除 关联工序
  async deleteRltProcedure(rltProcedureId) {
    return await this.axios({
      url: `${url}/deleteRltProcedure`,
      method: 'delete',
      params: { rltProcedureId },
    });
  }
}
