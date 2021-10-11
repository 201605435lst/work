<template>
  <a-card>
    <SmSystemOrganizations :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmSystemOrganizations from 'snweb-module/es/sm-system/sm-system-organizations';
import { mapGetters } from 'vuex';
export default {
  name: 'OrganizationUnits',
  components: { SmSystemOrganizations },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
