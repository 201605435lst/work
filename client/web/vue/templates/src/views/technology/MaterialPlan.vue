<template>
  <a-card>
    <!-- 用料计划 -->
    <SmTechnologyMaterialPlan ref="SmTechnologyMaterialPlan" :axios="axios" default :permissions="permissions" /> 
  </a-card>
</template>
<script>
import SmTechnologyMaterialPlan from 'snweb-module/es/sm-technology/sm-technology-material-plan';
import { mapGetters } from 'vuex';
export default {
  name: 'MaterialPlan',
  components: { SmTechnologyMaterialPlan },
  computed: {
    ...mapGetters(['permissions']),
  },
     watch: {
    $route: function() {
      if (this.$route.path.indexOf('material-plan') > -1) {
        this.$refs.SmTechnologyMaterialPlan.refresh();
      }
    },
  },
};
</script>
