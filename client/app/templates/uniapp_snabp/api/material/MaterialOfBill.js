import service from "../../utils/service.js";
let url = "/api/app/materialOfBill";

// 查询id
export async function get(id) {
  const res = await service.request({
    url: `${url}/get`,
    method: "get",
    data: { id },
  });
  return res;
}
// 查询列表
export async function getList(data) {
  const res = await service.request({
    url: `${url}/getList`,
    method: "get",
    data,
  });
  return res;
}

// 添加
export async function create(data) {
  const res = await service.request({
    url: `${url}/create`,
    method: "post",
    data,
  });
  return res;
}

// 编辑
export async function update(data) {
  const res = await service.request({
    url: `${url}/update`,
    method: "put",
    data,
  });
  return res;
}

// 删除
export async function remove(id) {
  const res = await service.request({
    url: `${url}/delete`,
    method: "delete",
    header: {
      "content-type": "application/x-www-form-urlencoded"
    },
    data: { id },
  });
  return res;
}
