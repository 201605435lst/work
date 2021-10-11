/**
 * 说明：二维码组件封装
 * 作者：easten
 */
let url = '/api/app/commonqrcode';
export default class Api {
  constructor(axios) {
    this.axios = axios || null;
  }

  // 获取二维信息
  async getQRCode(data){
    return await this.axios({
      url: `${url}/qRCode`,
      method: 'post',
      data,
    });
  }

  // 更新数据
  async update(data){
    return await this.axios({
      url: `${url}/update`,
      method: 'put',
      data,
    });
  }

  // 下载
  async download(content){
    return await this.axios({
      url: `${ url}/download`,
      method: 'get',
      params: { content },
      responseType:'arraybuffer',
    });
  }

  async get(content){
    return await this.axios({
      url: `${ url}/get`,
      method: 'get',
    });
  }

  async getImage(content){
    return await this.axios({
      url: `${ url}/getQRcode`,
      method: 'get',
      params:{content},
    });
  }
}
