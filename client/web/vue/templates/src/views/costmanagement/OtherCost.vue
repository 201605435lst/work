<template>
  <a-card>
    <SmCostmanagementOtherCost :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmCostmanagementOtherCost from 'snweb-module/es/sm-costmanagement/sm-costmanagement-other-cost';
import { mapGetters } from 'vuex';
export default {
  name: 'OtherCost',
  components: { SmCostmanagementOtherCost },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
