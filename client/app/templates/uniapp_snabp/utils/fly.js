var Fly = require("flyio/dist/npm/wx")
var fly = new Fly; //创建fly实例
import config from '../config.js'

// 配置请求根域名
fly.config.baseURL = config.remoteServiceBaseUrl;

// 配置响应拦截器
fly.interceptors.response.use(
  (response) => {
    console.log('----------------response--------------')
    console.log(response)
    // 权限问题报错
    if (response.data.retcode == 10003 || response.data.retcode == 10004 || response.data.retcode == 10011) {
      uni.showModal({
        title: '温馨提示',
        content: '无权限访问或登录信息已过期，请返回登录页重新登录后重试!'
      })
      return Promise.reject(response.data)
    } else if (response.data.retcode != 0) {
      uni.showModal({
        title: '温馨提示',
        content: response.data.text,
        showCancel: false
      })
      return Promise.reject(response.data)
    } else {
      //只将请求结果的data字段返回
      return Promise.resolve(response.data)
    }
  },
  (err) => {
    console.log(err, 'err')
    //发生网络错误后会走到这里
    uni.showModal({
      title: '温馨提示',
      content: "网络请求异常：" + err.message
    })
    return Promise.reject("网络请求异常：" + err.message)
  }
)
// 配置请求拦截器
fly.interceptors.request.use((request) => {
  console.log('----------------request--------------')
  console.log(request)
  // 请求头携带token，不要问我token怎么来的
  request.headers["token"] = uni.getStorageSync('token');
  return request;
})

export default fly
