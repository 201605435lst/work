<template>
  <a-card>
    <SmCrPlanMaintenanceWork :axios="axios" :permissions="permissions" :repair-tag-key="repairTagKey" />
  </a-card>
</template>
<script>
import SmCrPlanMaintenanceWork from 'snweb-module/es/sm-cr-plan/sm-cr-plan-maintenance-work';
import { mapGetters } from 'vuex';

export default {
  name: 'MaintenanceWork',
  components: { SmCrPlanMaintenanceWork },
  props: ['repairTagKey'],
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
