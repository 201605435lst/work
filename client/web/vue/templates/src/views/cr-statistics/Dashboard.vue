<template>
  <a-card>
    <SmCrStatisticsDashboard :axios="axios" :permissions="permissions" :repair-tag-key="repairTagKey" />
  </a-card>
</template>
<script>
import SmCrStatisticsDashboard from 'snweb-module/es/sm-cr-statistics/sm-cr-statistics-dashboard';
import { mapGetters } from 'vuex';

export default {
  name: 'Dashboard',
  components: { SmCrStatisticsDashboard },
  props: ['repairTagKey'],
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
