<template>
  <a-card>
    <SmSystemUsers :axios="axios" :permissions="permissions" />
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
};
</script>
