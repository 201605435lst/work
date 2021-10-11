<template>
  <a-card>
    <!-- 用料计划-审批 -->
    <SmTechnologyMaterialPlan ref="SmTechnologyMaterialPlan" :axios="axios" approval :permissions="permissions" />
  </a-card>
</template>
<script>
import SmTechnologyMaterialPlan from 'snweb-module/es/sm-technology/sm-technology-material-plan';
import { mapGetters } from 'vuex';
export default {
  name: 'MaterialPlanApproval',
  components: { SmTechnologyMaterialPlan },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('material-plan-approval') > -1) {
        this.$refs.SmTechnologyMaterialPlan.refresh();
      }
    },
  },
};
</script>
