<template>
  <a-card>
    <SmCrPlanPlanTodos :axios="axios" :permissions="permissions" :repair-tag-key="repairTagKey" />
  </a-card>
</template>
<script>
import SmCrPlanPlanTodos from 'snweb-module/es/sm-cr-plan/sm-cr-plan-plan-todos';
import { mapGetters } from 'vuex';

export default {
  name: 'PlanTodos',
  components: { SmCrPlanPlanTodos },
  props: ['repairTagKey'],
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
