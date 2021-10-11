<template>
  <a-card>
    <SmCrPlanYearPlanRecords
      :plan-type="planType"
      :org-id="orgId"
      :repair-tag-key="repairTagKey"
      org-name="orgName"
      :axios="axios"
      :permissions="permissions"
      @change="onChange"
    />
  </a-card>
</template>

<script>
import SmCrPlanYearPlanRecords from 'snweb-module/es/sm-cr-plan/sm-cr-plan-year-plan-records';
import { mapGetters } from 'vuex';
import { routePrefixes } from '../../config/routers/cr-plan/_util';

export default {
  name: 'YearPlan',
  components: { SmCrPlanYearPlanRecords },
  props: ['repairTagKey', 'planType', 'orgId', 'orgName'],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
  },
};
</script>
