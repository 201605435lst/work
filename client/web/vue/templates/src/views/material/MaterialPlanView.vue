<template>
  <a-card>
    <SmMaterialMaterialPlan ref="SmMaterialMaterialPlan" :axios="axios" :permissions="permissions" material />
  </a-card>
</template>
<script>
import SmMaterialMaterialPlan from 'snweb-module/es/sm-technology/sm-technology-material-plan';
import { mapGetters } from 'vuex';
export default {
  name: 'MaterialPlanView',
  components: { SmMaterialMaterialPlan },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('material-plan-view') > -1) {
        this.$refs.SmMaterialMaterialPlan.refresh();
      }
    },
  },
};
</script>
