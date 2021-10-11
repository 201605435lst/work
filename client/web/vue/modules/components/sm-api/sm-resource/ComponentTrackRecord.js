import qs from 'qs';
let url = '/api/app/resourceComponentTrackRecord';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }
  // 查询单个实体
  async get(id) {
    return await this.axios({
      url: `${url}/get`,
      method: 'get',
      params: { id },
    });
  }

  // 查询单个实体
  async getByInstallationEquipmentId(installationEquipmentId) {
    return await this.axios({
      url: `${url}/getByInstallationEquipmentId`,
      method: 'get',
      params: { installationEquipmentId },
    });
  }

  // 创建跟踪记录
  async create(data) {
    return await this.axios({
      url: `${url}/create`,
      method: 'post',
      data,
    });
  }
}
