let url = "/api/app/fileFile";
import service from "../utils/service.js";
/**
 *新增文件
 * @param {*} data 文件字段对象
 * @returns bool
 * @memberof Api
 */
export async function create(data) {
  const res = await service.request({
    url: `${url}/create`,
    method: "post",
    data,
  });
  return res;
}

/**
 * 获取文件上传的签名信息，需要文件的类型生成对应的上传签名地址
 * @param {String} type 文件类型，值传递文件的后缀名即可
 * @returns 文件上传的签名地址
 * @memberof Api
 */

export async function getPresignUrl(params) {
  const res = await service.request({
    url: `${url}/getPresignUrl`,
    // header: {
    //     "content-type": "application/x-www-form-urlencoded"
    // },
    method: "get",
    data: params,
  });
  return res;
}

export async function upload(fileObj) {
  uni.request({
    url: fileObj.uploadUrl, //仅为示例，并非真实接口地址。
    method: "PUT",
    data: fileObj.data,
    header: {
      "content-type": "application/x-www-form-urlencoded", //自定义请求头信息
    },
    success: (res) => {
      console.log(res.data);
      // this.text = 'request success';
    },
  });
}
