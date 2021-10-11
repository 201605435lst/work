<template>
  <SmCrPlanMonthOfYearPlan ::axios="axios" :permissions="permissions" :repair-tag-key="repairTagKey" />
</template>

<script>
import SmCrPlanMonthOfYearPlan from 'snweb-module/es/sm-cr-plan/sm-cr-plan-month-of-year-plan';
import { mapGetters } from 'vuex';

export default {
  name: 'MonthOfYearPlan',
  components: { SmCrPlanMonthOfYearPlan },
  props: ['repairTagKey'],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {},
};
</script>
