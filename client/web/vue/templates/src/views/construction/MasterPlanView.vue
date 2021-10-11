<template>
  <a-card>
    <SmConstructionMasterPlan :axios="axios" only-query :bordered="false" :show-select-row="false" />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionMasterPlan from 'snweb-module/es/sm-construction/sm-construction-master-plan/SmConstructionMasterPlan';
export default {
  name: 'Worker',
  components: {SmConstructionMasterPlan},
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>

<style scoped>

</style>