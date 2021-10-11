<template>
  <a-card>
    <SmSystemUsers :axios="axios" :permissions="permissions" ref="SmSystemUsers"/>
  </a-card>
</template>
<script>
import SmSystemUsers from 'snweb-module/es/sm-system/sm-system-users';
import { mapGetters } from 'vuex';
export default {
  name: 'Users',
  components: { SmSystemUsers },
  computed: {
    ...mapGetters(['permissions']),
  },
    watch: {
    $route: async function() {
      if (this.$route.path.indexOf('users') > -1) {
        await this.$refs.SmSystemUsers.refresh();
      }
    },
  },
};
</script>
