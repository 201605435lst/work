<template>
  <a-card>
    <SmCrPlanTrack
      :axios="axios"
      :permissions="permissions"
      :organization-id="organizationId"
      :plan-time="planTime"
      :plan-type="planType"
      :number="number"
      :repair-tag-key="repairTagKey"
    />
  </a-card>
</template>
<script>
import SmCrPlanTrack from 'snweb-module/es/sm-cr-statistics/sm-cr-statistics-plan-track';
import { mapGetters } from 'vuex';

export default {
  name: 'PlanTrack',
  components: { SmCrPlanTrack },
  props: ['repairTagKey','organizationId', 'planTime', 'planType', 'number'],
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
