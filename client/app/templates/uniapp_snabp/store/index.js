import Vue from 'vue';
import Vuex from 'vuex';
import * as apiApp from '../api/login.js';
import * as apiSystem from '@/api/system.js';
import * as apiProject from '@/api/project/project.js';
import config from '../config.js';
import {requestIsSuccess} from '../utils/util.js';

Vue.use(Vuex);

// 创建一个 store
const store = new Vuex.Store({
	// (1)初始 state 对象
	state: {
		token: '',
		creatorId: '',
		organization: '',
		roles: [],
		permissions: [],
		fileServerEndPoint: '',
		isSubmit: false, //判断是否已经开始提交，使按钮不能重复点击
	},
	// (2)定义一些mutation
	mutations: {
		SetToken: (state, token) => {
			state.token = token;
		},
		SetNameId: (state, creatorId) => {
			state.creatorId = creatorId;
		},
		SetOrganization: (state, organization) => {
			state.organization = organization;
		},
		SetRoles: (state, roles) => {
			state.roles = roles;
		},
		SetPermissions: (state, permissions) => {
			state.permissions = permissions;
		},
		SetFileServerEndPoint: (state, fileServerEndPoint) => {
			state.fileServerEndPoint = fileServerEndPoint;
		},
		SetIsSubmit: (state, isSubmit) => {
			state.isSubmit = isSubmit;
		},
	},
	actions: {
		async switchInitOrganization({commit}, params) {
			commit('SetOrganization', params);
			uni.setStorageSync('organization', params);
			/**获取根级组织 */
			let organizationId = params.id;
			let responseOrgRoot = await apiSystem.getLoginUserOrganizationRootTag(organizationId);
			if (requestIsSuccess(responseOrgRoot)) {
				uni.setStorageSync('organizationTagId', responseOrgRoot.data);
			}
			/**获取文件存放服务器地址 */
			let fileResponse = await apiApp.getFileServerEndPoint();
			if (requestIsSuccess(fileResponse)) {
				uni.setStorageSync('fileServerEndPoint', fileResponse.data);
				commit('SetFileServerEndPoint', fileResponse.data);
			}
		},
		async switchProject({commit}, params) {
			uni.setStorageSync('Project', params);
			/**获取文件存放服务器地址 */
			let fileResponse = await apiApp.getFileServerEndPoint();
			if (requestIsSuccess(fileResponse)) {
				uni.setStorageSync('fileServerEndPoint', fileResponse.data);
				commit('SetFileServerEndPoint', fileResponse.data);
			}
		},

		async login({commit}, params) {
			const response = await apiApp.getToken(params);
			if (requestIsSuccess(response) && response.data && response.data.access_token) {
				uni.setStorageSync(config.Token, response.data.access_token);
				commit('SetToken', response.data.access_token);
			}
			return response;
		},

		async getInfo({commit}) {
			let response = await apiApp.getApplicationConfiguration();
			if (requestIsSuccess(response)) {
				let configuration = response.data;

				// 获取当前用户所在组织机构权限
				response = await apiApp.getPermissions();
				if (requestIsSuccess(response)) {
					// 获取用户名
					let responseUser = await apiApp.findByUsername(configuration.currentUser.userName);
					let _user = responseUser.data;
					/**组织机构*/
					if (requestIsSuccess(responseUser)) {
						configuration.currentUser.name = _user.name;
						if (_user.organizations && _user.organizations.length > 0) {
							try {
								const value1 = uni.getStorageSync('organization');
								if (!(value1 && value.id)) {
									uni.setStorageSync('organization', _user.organizations[0].organization);
									commit('SetOrganization', _user.organizations[0].organization);
								}
							} catch (e) {
								// error
							}
						} else {
							let organizationDatas = await apiSystem.getCurrentUserOrganizations({isAll: true});
							if (requestIsSuccess(organizationDatas) && organizationDatas.data && organizationDatas.data.length > 0) {
								try {
									const value1 = uni.getStorageSync('organization');
									if (!(value1 && value.id)) {
										uni.setStorageSync('organization', organizationDatas.data[0]);
										commit('SetOrganization', organizationDatas.data[0]);
									}
								} catch (e) {
									// error
								}
							}
						}
						/**获取根级组织 */
						let organization = uni.getStorageSync('organization') || {}; //内存中组织
						let organizationId = organization.id;
						let responseOrgRoot = await apiSystem.getLoginUserOrganizationRootTag(organizationId);
						if (requestIsSuccess(responseOrgRoot)) {
							uni.setStorageSync('organizationTagId', responseOrgRoot.data);
						}
						/**多项目projectId*/
						let responseProject = null,
							project = null;
						if (_user.projectIds && _user.projectIds.length > 0) {
							responseProject = await apiProject.getListByIds({ids: _user.projectIds});
							if (requestIsSuccess(responseProject) && responseProject.data && responseProject.data.length > 0) {
								try {
									const value2 = uni.getStorageSync('Project');
									if (!(value2 && value.id)) {
										project = responseProject.data[0];
										uni.setStorageSync('Project', project);
									}
								} catch (e) {
									// error
								}
							}
						} else {
							responseProject = await apiProject.getList({isAll: true});
							if (
								requestIsSuccess(responseProject) &&
								responseProject.data &&
								responseProject.data.items &&
								responseProject.data.items.length > 0
							) {
								try {
									const value2 = uni.getStorageSync('Project');
									if (!(value2 && value.id)) {
										project = responseProject.data.items[0];
										uni.setStorageSync('Project', project);
									}
								} catch (e) {
									// error
								}
							}
						}
						//**---- */
					}
					commit('SetNameId', configuration.currentUser.id);
					commit('SetPermissions', response.data);
					uni.setStorageSync('permissions', response.data);
					//获取当前用户详细信息
					let response_ = await apiSystem.get(configuration.currentUser.id);
					if (requestIsSuccess(response_)) {
						uni.setStorageSync('userInfo', response_.data);
					}
					/**获取文件存放服务器地址 */
					let fileResponse = await apiApp.getFileServerEndPoint();
					if (requestIsSuccess(fileResponse)) {
						uni.setStorageSync('fileServerEndPoint', fileResponse.data);
						commit('SetFileServerEndPoint', fileResponse.data);
					}
				}
			}

			return response;
		},

		logout({commit}) {
			commit('SetToken', '');
			commit('SetRoles', []);
			commit('SetOrganization', {});

			let remoteServiceBaseUrl = uni.getStorageSync('remoteServiceBaseUrl');
			uni.clearStorageSync();
			uni.setStorageSync('remoteServiceBaseUrl', remoteServiceBaseUrl);
		},
	},
});

// 导出该模块:以便其他文件可对其进行使用
export default store;
