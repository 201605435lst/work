let url = '/api/app/alarmAlarm';

export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 查询
  async getList(params) {
    return await this.axios({
      url: `${url}/getList`,
      method: 'get',
      params,
    });
  }

  //上传EXCEL文件
  async importEquipmentId(data) {
    return await this.axios({
      url: `${url}/importEquipmentId`,
      method: 'post',
      processData: false,
      data,
    });
  }
}
