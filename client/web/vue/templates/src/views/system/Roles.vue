<template>
  <a-card>
    <SmSystemRoles :axios="axios" :permission-black-list="permissionBlacklist" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmSystemRoles from 'snweb-module/es/sm-system/sm-system-roles';
import { mapGetters } from 'vuex';
import configDefault from '@/config/default';
export default {
  name: 'Roles',
  components: { SmSystemRoles },
  data() {
    return {
      permissionBlacklist: configDefault.permissionBlacklist,
    };
  },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
