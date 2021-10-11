import { axios } from '@/utils/axios';
import config from '@/config/default';
import qs from 'qs';

export async function getToken(params) {
  const _data = {
    grant_type: 'password',
    scope: config.oAuthConfig.scope,
    ...params,
    client_id: config.oAuthConfig.clientId,
    client_secret: config.oAuthConfig.dummyClientSecret,
  };
  const _params = qs.stringify(_data);
  const res = await axios({
    url: config.oAuthConfig.issuer + '/connect/token',
    method: 'POST',
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
    },
    data: _params,
  });
  return res;
}

export async function getApplicationConfiguration() {
  const res = await axios({
    url: '/api/abp/application-configuration',
    methods: 'get',
  });
  return res;
}

export async function getPermissions() {
  const res = await axios({
    url: '/api/app/appUser/getUserPermissions',
    methods: 'get',
  });
  return res;
}

export async function getFileServerEndPoint() {
  const res = await axios({
    url: '/api/app/fileFileManager/getEndPoint',
    methods: 'get',
  });
  return res;
}

export async function getOpenIdConfig() {
  const res = await axios({
    url: '/.well-known/openid-configuration',
    methods: 'get',
  });
  return res;
}

export async function getJwks(url) {
  const res = await axios({
    url,
    methods: 'get',
  });
  return res;
}

export async function logout() {
  const res = await axios({
    url: '/api/account/logout',
    methods: 'get',
  });
  return res;
}
