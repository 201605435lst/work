<template>
  <a-card>
    <SmSystemUserChangePassword
      :axios="axios"
      :value="true"
      @success="onSuccess"
    />
  </a-card>
</template>

<script>
import SmSystemUserChangePassword from 'snweb-module/es/sm-system/sm-system-user-change-password';
import { name as storeApp, mt as mtApp, at as atApp } from '@/store/modules/app/types';

import { mapActions, mapGetters, mapState, mapMutations } from 'vuex';

export default {
    name:'UserChangePassword',
    components:{SmSystemUserChangePassword},
    methods:{
    ...mapActions(storeApp, [atApp.Logout]),
    ...mapState(storeApp, ['scope']),

      onSuccess(){
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
    },
};
</script>