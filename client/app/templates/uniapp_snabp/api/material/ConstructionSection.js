import service from '../../utils/service.js'

let url = '/api/app/materialConstructionSection';

// 查询
export async function getList(params) {
  const res = await service.request({
    url: `${url}/getList`,
    method: 'get',
    params,
  });
  return res;
}

  // 添加
  export async function create(data) {
    const res = await service.request({
      url: `${url}/create`,
      method: 'post',
      data,
    });
    return res;
  }

  // 编辑
  export async function update(data) {
    const res = await service.request({
      url: `${url}/update`,
      method: 'put',
      data,
    });
    return res;
  }

  // 删除
  export async function delete_(id) {
    const res = await service.request({
      url: `${url}/delete`,
      method: 'delete',
      params: { id },
    });
    return res;
  }
