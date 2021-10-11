<template>
  <div class="user-wrapper">
    <!-- <span class="action" @click="onToggleTheme"> <a-icon type="setting" /></span> -->

    <a-dropdown>
      <a class="ant-dropdown-link" @click="e => e.preventDefault()">
        <a-icon type="question-circle-o" /> 帮助 <a-icon type="down" />
      </a>
      <a-menu slot="overlay">
        <a-menu-item>
          <a href="/help.pdf" target="_blank"> <a-icon type="eye" /> 帮助文档查看</a>
        </a-menu-item>
        <a-menu-item>
          <a href="/help.pdf" target="_blank" download="使用帮助.pdf"> <a-icon type="download" /> 帮助文档下载</a>
        </a-menu-item>
        <a-menu-item>
          <a href="/BIM_NJDT.apk" target="_blank" download="BIM_NJDT.apk">
            <a-icon type="download" /> APP下载</a>
        </a-menu-item>
        <a-menu-item>
          <a href="/360browser.exe" target="_blank" download="360极速浏览器.exe">
            <a-icon type="download" /> 360浏览器下载</a>
        </a-menu-item>
      </a-menu>
    </a-dropdown>

    <a-input-group :compact="true" style="width: 220px; display: flex; margin: 0 10px; align-items: center">
      <a-input style="width: 35%" value="组织机构" size="small" disabled />
      <SmSystemOrganizationTreeSelect
        v-model="organizationId"
        size="small"
        :only-current-user-organizations="true"
        style="width: 65%"
        :axios="axios"
        placeholder="请选择组织机构"
        :auto-initial="true"
        @change="onOrganizationChange"
      />
    </a-input-group>

    <a-input-group
      v-if="projects && projects.length > 1"
      compact
      style="width: 200px; display: flex; margin: 0 10px; align-items: center"
    >
      <a-input style="width: 25%" value="项目" size="small" disabled />
      <a-select
        :options="projects"
        size="small"
        placeholder="请选择项目"
        style="width: 75%"
        :value="projectId"
        @change="onProjectChange"
      />
    </a-input-group>

    <SmMessageNotice ref="SmmessageNotce" :signalr="signalr" @detailsClick="messageItemClick" />
    <a-dropdown>
      <span class="action ant-dropdown-link user-dropdown-menu">
        <a-avatar class="avatar" :src="avatar ? avatar : defaultAvatar" />
        <span>{{ userInfo ? userInfo.currentUser.name : '' }}</span>
      </span>
      <a-menu slot="overlay" class="user-dropdown-menu-wrapper">
        <a-menu-item key="2" @click="onBtnChangePassword">
          <a-icon type="lock" />
          <span>修改密码</span>
        </a-menu-item>

        <a-menu-divider />

        <a-menu-item key="3">
          <a href="javascript:;" @click="handleLogout">
            <a-icon type="logout" />
            <span>退出登录</span>
          </a>
        </a-menu-item>
      </a-menu>
    </a-dropdown>
    <SmSystemUserChangePassword v-model="smSystemUserChangePasswordVisible" :axios="axios" />
  </div>
</template>

<script>
import SmSystemOrganizationTreeSelect from 'snweb-module/es/sm-system/sm-system-organization-tree-select';
import SmSystemUserChangePassword from 'snweb-module/es/sm-system/sm-system-user-change-password';
import { mapActions, mapGetters, mapState, mapMutations } from 'vuex';
import { name as storeApp, mt as mtApp, at as atApp } from '@/store/modules/app/types';
import { stringfyScope } from 'snweb-module/es/_utils/utils';
import { BpmMessageType } from 'snweb-module/es/_utils/enum';
import defaultConfig from '@/config/default';
import SmMessageNotice from 'snweb-module/es/sm-message/sm-message-notice';
import defaultAvatar from '@/assets/avatar.png';
import ApiProject from 'snweb-module/es/sm-api/sm-project/Project';
import ApiOrganization from 'snweb-module/es/sm-api/sm-system/Organization';
import { requestIsSuccess } from 'snweb-module/lib/_utils/utils';

let apiProject = new ApiProject();
let apiOrganization = new ApiOrganization();

export default {
  name: 'UserMenu',
  components: {
    SmSystemUserChangePassword,
    SmSystemOrganizationTreeSelect,
    SmMessageNotice,
  },
  data() {
    return {
      organizationId: null,
      projectId: undefined,
      projects: [],
      smSystemUserChangePasswordVisible: false,
      defaultAvatar,
    };
  },
  computed: {
    ...mapGetters(['nickname', 'userInfo', 'avatar', 'projectIds']),
    ...mapState(storeApp, ['scope']),
  },
  async created() {
    apiProject = new ApiProject(this.axios);
    apiOrganization = new ApiOrganization(this.axios);
    await this.getProjects();
    let organizationId = localStorage.getItem(defaultConfig.storageOptions.organizationId);
    if (organizationId) {
      this.organizationId = organizationId;
    }
    let projectId = localStorage.getItem(defaultConfig.storageOptions.projectId);
    if (projectId) {
      this.projectId = projectId;
    }
  },
  methods: {
    ...mapActions(storeApp, [atApp.Logout]),
    ...mapMutations(storeApp, [mtApp.ToggleThemeDrawerVisible, mtApp.SetScope]),
    stringfyScope,
    onToggleTheme() {
      this.$store.commit(storeApp + '/' + mtApp.ToggleThemeDrawerVisible, true);
    },
    onBtnChangePassword() {
      this.smSystemUserChangePasswordVisible = true;
    },
    onChangePasswordSuccess() {
      this.smSystemUserChangePasswordVisible = false;
    },

    async getProjects() {
      let response = null;
      if (this.projectIds && this.projectIds.length > 0) {
        response = await apiProject.getListByIds({
          ids: this.projectIds,
        });
        if (requestIsSuccess(response) && response.data && response.data.length > 0) {
          this.projects = response.data.map(item => {
            return {
              ...item,
              title: item.name,
              value: item.id,
              key: item.id,
            };
          });
          this.projectId = response.data[0].id;
          localStorage.setItem(defaultConfig.storageOptions.projectId, this.projectId);
        }
      } else {
        response = await apiProject.getList({ isAll: true });
        if (requestIsSuccess(response) && response.data && response.data.items && response.data.items.length > 0) {
          this.projects = response.data.items.map(item => {
            return {
              ...item,
              title: item.name,
              value: item.id,
              key: item.id,
            };
          });
        }
      }
    },

    handleLogout() {
      this.$confirm({
        title: '提示',
        content: '真的要注销登录吗 ?',
        onOk: () => {
          return this[atApp.Logout]({})
            .then(() => {
              setTimeout(() => {
                window.location.reload();
              }, 16);
            })
            .catch(err => {
              this.$message.error({
                title: '错误',
                description: err.message,
              });
            });
        },
        onCancel() {},
      });
    },
    async onOrganizationChange(id) {
      this.organizationId = id;
      let organizationId = localStorage.getItem(defaultConfig.storageOptions.organizationId);
      if (id && organizationId != id) {
        let response = await apiOrganization.getLoginUserOrganizationRootTag(id);
        if (requestIsSuccess(response) && response.data) {
          localStorage.setItem(defaultConfig.storageOptions.organizationRootTagId, response.data);
        }
        localStorage.setItem(defaultConfig.storageOptions.organizationId, id);
        window.location.reload();
      }
    },
    onProjectChange(id) {
      this.projectId = id;
      let projectId = localStorage.getItem(defaultConfig.storageOptions.projectId);
      if (id && projectId != id) {
        localStorage.setItem(defaultConfig.storageOptions.projectId, id);
        window.location.reload();
      }
    },
    messageItemClick(data) {
      if (data.Type === BpmMessageType.Notice) {
        this.jumpToRoute('/bpm/workflows-initial');
      } else if (data.Type === BpmMessageType.Approval) {
        this.jumpToRoute('/bpm/workflows-waiting');
      } else if (data.Type === BpmMessageType.Cc) {
        this.jumpToRoute('/bpm/workflows-cc');
      }
      // 点击完成，给组件回馈一下，表明消息已读
    },
    jumpToRoute(page) {
      this.$router.push({ path: page });
    },
  },
};
</script>
