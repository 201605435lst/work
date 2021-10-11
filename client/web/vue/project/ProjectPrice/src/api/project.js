import { axios } from '@/utils/axios';

export function getList(params) {
  return axios({
    url: '/api/app/projectPriceProject/getList',
    method: 'get',
    params,
  });
}

export function get(id) {
  return axios({
    url: '/api/app/projectPriceProject/get/',
    method: 'get',
    params: { id },
  });
}

export function create(data) {
  return axios({
    url: '/api/app/projectPriceProject/create/',
    method: 'post',
    data,
  });
}

export function update(data) {
  return axios({
    url: '/api/app/projectPriceProject/update/',
    method: 'put',
    data,
  });
}

export function remove(id) {
  return axios({
    url: '/api/app/projectPriceProject/delete/',
    method: 'delete',
    params: { id },
  });
}
