let url = '/api/app/resourceOrganizationRltLayer';
import qs from 'qs';

export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 根据组织机构id获取图层数据
  async getLayerIdsByOrganizationId(organizationId) {
    return await this.axios({
      url: `${url}/getLayerIdsByOrganizationId`,
      method: 'get',
      params: { organizationId },
    });
  }

  // 给组织机构关联图层
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }
}
