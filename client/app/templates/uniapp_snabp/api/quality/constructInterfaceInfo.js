import service from "../../utils/service.js";
let url = "/api/app/constructInterfaceInfo";

// 标记
export async function create(data) {
  const res = await service.request({
    url: `${url}/create`,
    method: "post",
    data,
  });
  return res;
}

// 查询标记记录
export async function getInterfanceReform(params) {
  const res = await service.request({
    url: `${url}/getInterfanceReform`,
    method: "get",
    data: params,
  });
  return res;
}
// 标记编辑
export async function update(data) {
  const res = await service.request({
    url: `${url}/update`,
    method: "put",
    data,
  });
  return res;
}
// 查询单个
export async function get(id) {
  const res = await service.request({
    url: `${url}/get`,
    method: "get",
    data: {id},
  });
  return res;
}
// 标记整改
export async function interfanceReform(params) {
  const res = await service.request({
    url: `${url}/interfanceReform`,
    method: "post",
    data: params,
  });
  return res;
}
// 删除
export async function delete_(id) {
  const res = await service.request({
    url: `${url}/delete`,
    method: "delete",
    data: { id },
  });
  return res;
}
