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
          <a href="/360browser.exe" target="_blank" download="360极速浏览器.exe">
            <a-icon type="download" /> 360浏览器下载</a>
        </a-menu-item>
      </a-menu>
    </a-dropdown>

    <SmSystemOrganizationTreeSelect
      v-model="organizationId"
      :only-current-user-organizations="true"
      style="width: 160px; margin: 0 10px"
      :axios="axios"
      placeholder="请选择组织机构"
      :auto-initial="true"
      @change="onOrganizationChange"
    />
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
import NoticeIcon from '@/components/NoticeIcon';
import { mapActions, mapGetters, mapState, mapMutations } from 'vuex';
import { name as storeApp, mt as mtApp, at as atApp } from '@/store/modules/app/types';
import { stringfyScope, parseScope, requestIsSuccess } from 'snweb-module/es/_utils/utils';
import { ScopeType,BpmMessageType } from 'snweb-module/es/_utils/enum';
import defaultConfig from '@/config/default';
import ApiOrganization from 'snweb-module/lib/sm-api/sm-system/Organization';
import SmMessageNotice from 'snweb-module/es/sm-message/sm-message-notice';
let apiOrganization = new ApiOrganization();
import defaultAvatar from '@/assets/avatar.png';

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
      smSystemUserChangePasswordVisible: false,
      defaultAvatar,
    };
  },
  computed: {
    ...mapGetters(['nickname', 'userInfo', 'avatar']),
    ...mapState(storeApp, ['scope']),
  },
  async created() {
    apiOrganization = new ApiOrganization(this.axios);
    let organizationId = localStorage.getItem(defaultConfig.storageOptions.organizationId);
    if (organizationId) {
      this.organizationId = organizationId;
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
    onOrganizationChange(id) {
      let organizationId = localStorage.getItem(defaultConfig.storageOptions.organizationId);
      if (id && organizationId != id) {
        localStorage.setItem(defaultConfig.storageOptions.organizationId, id);
        window.location.reload();
      }
    },
    messageItemClick(data){
      if(data.Type===BpmMessageType.Notice){
        this.jumpToRoute('/bpm/workflows-initial');
      }else if(data.Type===BpmMessageType.Approval){
         this.jumpToRoute('/bpm/workflows-waiting');
      }else if(data.Type===BpmMessageType.Cc){
         this.jumpToRoute('/bpm/workflows-cc');
      }
      // 点击完成，给组件回馈一下，表明消息已读

    },
    jumpToRoute(page){
      this.$router.push({ path: page });
    },
  },
};
</script>
