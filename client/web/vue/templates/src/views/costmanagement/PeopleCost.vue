<template>
  <a-card>
    <SmCostmanagementPeopleCost :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmCostmanagementPeopleCost from 'snweb-module/es/sm-costmanagement/sm-costmanagement-people-cost';
import { mapGetters } from 'vuex';
export default {
  name: 'OtherCost',
  components: { SmCostmanagementPeopleCost },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
