<template>
  <a-card>
    <SmCostmanagementBreakevenAnalysis ref="SmCostmanagementBreakevenAnalysis" :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmCostmanagementBreakevenAnalysis from 'snweb-module/es/sm-costmanagement/sm-costmanagement-breakeven-analysis';
import { mapGetters } from 'vuex';
export default {
  name: 'BreakevenAnalysis',
  components: { SmCostmanagementBreakevenAnalysis },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route:async function() {
      if (this.$route.path.indexOf('breakeven-analysis') > -1) {
       await this.$refs.SmCostmanagementBreakevenAnalysis.refresh();
       await this.$refs.SmCostmanagementBreakevenAnalysis.drawLine();
      }
    },
  },
};
</script>
