<template>
  <a-card>
    <SmCrPlanMaintenanceWorkCheck :axios="axios" :permissions="permissions" :repair-tag-key="repairTagKey" />
  </a-card>
</template>
<script>
import SmCrPlanMaintenanceWorkCheck from 'snweb-module/es/sm-cr-plan/sm-cr-plan-maintenance-work-check';
import { mapGetters } from 'vuex';

export default {
  name: 'MaintenanceWork',
  components: { SmCrPlanMaintenanceWorkCheck },
  props: ['repairTagKey'],
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
