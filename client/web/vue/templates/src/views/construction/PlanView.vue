<template>
  <a-card>
    <SmConstructionPlan :axios="axios" only-query :bordered="false" :show-select-row="false" />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionPlan from 'snweb-module/es/sm-construction/sm-construction-plan/SmConstructionPlan';
export default {
  name: 'PlanView',
  components: {SmConstructionPlan},
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>

<style scoped>

</style>