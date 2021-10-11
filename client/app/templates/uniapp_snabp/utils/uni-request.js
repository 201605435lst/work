import configs from '@/config.js';
export default class uniRequest {
	// 默认配置
	config = {
		baseUrl: '', //请求公共地址
		url: '',
		fileUrl: '', //公共文件上传请求地址
		header: {}, //默认请求头
	};

	//	拦截器
	interceptors = {
		request: null, // 请求拦截器
		response: null, // 响应拦截器
	};

	constructor(options) {
		this.config = Object.assign({}, this.config, options);
	}
	async request(options) {
		this.config.baseUrl = configs.remoteServiceBaseUrl;
		// 合并请求前拦截器配置项
		if (this.interceptors.request) {
			var config = this.interceptors.request(this.config);
			this.config = Object.assign({}, this.config, config);
		}

		// 发起请求
		let url = '';
		if (options.url.indexOf('http://') === -1) {
			url = uni.getStorageSync('remoteServiceBaseUrl');
		}
		let ProjectId = uni.getStorageSync('Project').id || '';
		let OrganizationTagId = uni.getStorageSync('organizationTagId') || '';
		const data = {
			url: url + options.url,
			data: options.data,
			method: options.method || 'POST',
			header: Object.assign({}, this.config.header, options.header, {
				OrganizationId:
					uni.getStorageSync('organization') && uni.getStorageSync('organization').id
						? uni.getStorageSync('organization').id
						: '',
				ProjectId: ProjectId || '',
				OrganizationTagId: OrganizationTagId || '',
			}),
		};

		var [error, response] = await uni.request(data);
		var _response = response;
		if (error) {
			error.response = response;
			_response = error;
		}

		// 请求后拦截器
		if (this.interceptors.response) {
			_response = this.interceptors.response(error, response);
		}

		return _response;
	}
}
