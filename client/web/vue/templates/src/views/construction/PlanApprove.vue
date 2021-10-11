<template>
  <a-card>
    <SmConstructionPlan :axios="axios" :bordered="false" approval="approval" />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionPlan from 'snweb-module/es/sm-construction/sm-construction-plan';
export default {
  name: 'PlanApprove',
  components: { SmConstructionPlan },
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