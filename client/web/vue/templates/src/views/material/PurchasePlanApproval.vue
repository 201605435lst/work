<template>
  <a-card>
    <SmMaterialPurchasePlan ref="SmMaterialPurchasePlan" :axios="axios" :permissions="permissions" :approval="true" />
  </a-card>
</template>
<script>
import SmMaterialPurchasePlan from 'snweb-module/es/sm-material/sm-material-purchase-plan';
import { mapGetters } from 'vuex';
export default {
  name: 'PurchasePlanApproval',
  components: { SmMaterialPurchasePlan },
  computed: {
    ...mapGetters(['permissions']),
  },
    watch: {
    $route: function() {
      if (this.$route.path.indexOf('purchase-plan-approval') > -1) {
        this.$refs.SmMaterialPurchasePlan.refresh();
      }
    },
  },
};
</script>
