<template>
  <a-card>
    <SmCostmanagementContract :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmCostmanagementContract from 'snweb-module/es/sm-costmanagement/sm-costmanagement-contract';
import { mapGetters } from 'vuex';
export default {
  name: 'Contract',
  components: { SmCostmanagementContract },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
