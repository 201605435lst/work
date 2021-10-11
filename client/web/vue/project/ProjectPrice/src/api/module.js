import { axios } from '@/utils/axios';

export function getList(params) {
  return axios({
    url: '/api/app/projectPriceModule/getList',
    method: 'get',
    params,
  });
}

export function get(id) {
  return axios({
    url: '/api/app/projectPriceModule/get/' + id,
    method: 'get',
  });
}

export function create(data) {
  return axios({
    url: '/api/app/projectPriceModule/create/',
    method: 'post',
    data,
  });
}

export function update(data) {
  return axios({
    url: '/api/app/projectPriceModule/update/',
    method: 'put',
    data,
  });
}

export function remove(id) {
  return axios({
    url: '/api/app/projectPriceModule/delete/',
    method: 'delete',
    params: { id },
  });
}
