<template>
  <a-card>
    <SmConstructionMasterPlan :axios="axios" :bordered="false" :approval="approval" />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionMasterPlan from 'snweb-module/es/sm-construction/sm-construction-master-plan/SmConstructionMasterPlan';
export default {
  name: 'MasterPlanApprove',
  components: {SmConstructionMasterPlan},
  data() {
    return {
      approval: true,
    };
  },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>

<style scoped>

</style>