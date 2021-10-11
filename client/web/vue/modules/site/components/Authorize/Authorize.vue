<template>
  <div>
    <a-button
      key="authorize-button"
      size="small"
      ghost
      class="header-authorize-button"
      type="primary"
      @click="handleAuthorizeClick"
    >
      <span v-if="user.isAuthenticated">已登录（{{ user.name }}）</span>
      <span v-else> {{ isCN ? '登录' : 'Login' }} </span>
    </a-button>
    <!-- <div
      style=" width: 200px;
              float: right;
              margin-top: 20px;
              margin-left: 16px;"
    > -->

    <sm-system-organization-tree-select
      ref="SmSystemOrganizationTreeSelect"
      style=" width: 200px;
              float: right;
              margin-top: 20px;
              margin-left: 16px;
            "
      :value="organizationId"
      :only-current-user-organizations="true"
      :axios="axios"
      :auto-initial="true"
      placeholder="请选择组织机构"
      size="small"
      @change="onOrganizationChange"
    />

    <a-select
      allow-clear
      :options="projects"
      size="small"
      placeholder="请选择项目"
      style=" width: 200px;
              float: right;
              margin-top: 20px;
              margin-left: 16px;
            "
      :value="projectId"
      @change="onProjectChange"
    />

    <!-- <sm-basic-scope-select
      style=" width: 200px;
              float: right;
              margin-top: 20px;
              margin-left: 16px;
            "
      :axios="axios"
      :type="1"
      :value="stringfyScope(organizationId, 1)"
      size="small"
      placeholder="请选择组织机构"
      :auto-initial="true"
      @change="onOrganizationChange"
    /> -->
    <!-- </div> -->

    <AuthorizeModal ref="AuthorizeModal" @success="success" />
  </div>
</template>

<script>
import AuthorizeModal from './AuthorizeModal';
import { isZhCN } from '../../util';
import * as authApi from './api';
import axios from '@/utils/axios.js';
import { stringfyScope, parseScope, requestIsSuccess } from '../../../components/_utils/utils';
import ApiUser from '@/components/sm-api/sm-system/User';
import ApiOrganization from '@/components/sm-api/sm-system/Organization';
import ApiProject from '@/components/sm-api/sm-project/Project';
let apiUser = new ApiUser();
let apiOrganization = new ApiOrganization();
let apiProject = new ApiProject();

export default {
  name: 'Authorize',
  components: {
    AuthorizeModal,
  },
  props: {
    isCN: { default: true, type: Boolean },
  },
  data() {
    return {
      organizationId: null,
      projectId: undefined,
      projects: [],
      axios,
      isLogin: false,
      user: {
        isAuthenticated: false,
        id: null,
        tenantId: null,
        userName: null,
        name: null,
      },
    };
  },
  async created() {
    apiUser = new ApiUser(axios);
    apiOrganization = new ApiOrganization(axios);
    apiProject = new ApiProject(axios);
    let organizationId = localStorage.getItem('OrganizationId');
    if (organizationId) {
      this.organizationId = organizationId;
    }
    let projectId = localStorage.getItem('ProjectId');
    if (projectId) {
      this.projectId = projectId;
    }
    await this.initPrmissions();
    this.refresh();
  },
  methods: {
    stringfyScope,
    parseScope,
    isZhCN,
    handleAuthorizeClick() {
      if (this.user.isAuthenticated) {
        this.logOut();
      } else {
        this.$refs.AuthorizeModal.visible = true;
      }
    },
    success() {
      window.location.reload();
    },
    logOut() {
      localStorage.clear();
      window.location.reload();
    },
    async refresh() {
      let response = await authApi.getAppConfig();
      if (response.status === 200) {
        if (response.data && response.data.currentUser) {
          let user = response.data.currentUser;
          let responseUser = await apiUser.findByUsername(user.userName);
          if (responseUser.status === 200) {
            user.name = responseUser.data.name;
            localStorage.setItem('currentUserName', responseUser.data.name);
          }

          let responseProject = null;
          if (responseUser.data.projectIds && responseUser.data.projectIds.length > 0) {
            responseProject = await apiProject.getListByIds({
              ids: responseUser.data.projectIds,
            });
            if (
              requestIsSuccess(responseProject) &&
              responseProject.data &&
              responseProject.data.length > 0
            ) {
              this.projects = responseProject.data.map(item => {
                return {
                  ...item,
                  title: item.name,
                  value: item.id,
                  key: item.id,
                };
              });
              this.projectId = responseProject.data[0].id;
              localStorage.setItem('ProjectId', this.projectId);
            }
          } else {
            responseProject = await apiProject.getList({ isAll: true });
            if (
              requestIsSuccess(responseProject) &&
              responseProject.data &&
              responseProject.data.items &&
              responseProject.data.items.length > 0
            ) {
              this.projects = responseProject.data.items.map(item => {
                return {
                  ...item,
                  title: item.name,
                  value: item.id,
                  key: item.id,
                };
              });
              // this.projectId = responseProject.data.items[0].id;
              // localStorage.setItem('ProjectId', this.projectId);
            }
          }

          let responseOrgRoot = await apiOrganization.getLoginUserOrganizationRootTag(
            localStorage.getItem('OrganizationId'),
          );
          if (requestIsSuccess(responseOrgRoot)) {
            localStorage.setItem('OrganizationTagId', responseOrgRoot.data);
          }

          this.user = user;
          await this.initPrmissions();

          // 获取文件服务器地址
          response = await authApi.getFileServerEndPoint();
          if (response.status === 200) {
            localStorage.setItem('fileServerEndPoint', response.data);
          }
        }
      } else {
        this.$message.error('服务器连接失败');
      }
    },
    async onOrganizationChange(id) {
      let key = 'OrganizationId';
      this.organizationId = id;
      let organizationId = localStorage.getItem(key);

      if (id && organizationId != id) {
        let response = await apiOrganization.getLoginUserOrganizationRootTag(id);
        if (requestIsSuccess(response) && response.data) {
          localStorage.setItem('OrganizationTagId', response.data);
        }
        localStorage.setItem(key, id);
        window.location.reload();
      }
    },
    onProjectChange(id) {
      let key = 'ProjectId';
      this.projectId = id;
      let projectId = localStorage.getItem(key);
      if (projectId != id) {
        localStorage.setItem(key, id ? id : '');
        window.location.reload();
      }
    },
    async initPrmissions() {
      let response = await authApi.getUserPermissions();
      if (response.status === 200) {
        localStorage.setItem('permissions', JSON.stringify(response.data));
      }
    },
  },
};
</script>
