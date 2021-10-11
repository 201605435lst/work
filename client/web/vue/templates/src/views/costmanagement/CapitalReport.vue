<template>
  <a-card>
    <SmCostmanagementCapitalReport ref="SmCostmanagementCapitalReport" :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmCostmanagementCapitalReport from 'snweb-module/es/sm-costmanagement/sm-costmanagement-capital-report';
import { mapGetters } from 'vuex';
export default {
  name: 'CapitalReport',
  components: { SmCostmanagementCapitalReport },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: async function() {
      if (this.$route.path.indexOf('capital-report') > -1) {
        await this.$refs.SmCostmanagementCapitalReport.refresh();
        await this.$refs.SmCostmanagementCapitalReport.drawLine();
      }
    },
  },
};
</script>
