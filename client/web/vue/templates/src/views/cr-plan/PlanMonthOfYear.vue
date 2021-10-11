<template>
  <a-card> <SmCrPlanPlanMonthOfYear :axios="axios" :permissions="permissions" :repair-tag-key="repairTagKey" /></a-card>
</template>

<script>
import SmCrPlanPlanMonthOfYear from 'snweb-module/es/sm-cr-plan/sm-cr-plan-month-of-year-plan';
import { mapGetters } from 'vuex';

export default {
  name: 'PlanMonthOfYear',
  components: { SmCrPlanPlanMonthOfYear },
  props: ['repairTagKey'],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {},
};
</script>
