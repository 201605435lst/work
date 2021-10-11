<template>
  <a-card>
    <SmMaterialPurchaseList ref="SmMaterialPurchaseList" :axios="axios" :permissions="permissions" :approval="true" />
  </a-card>
</template>
<script>
import SmMaterialPurchaseList from 'snweb-module/es/sm-material/sm-material-purchase-list';
import { mapGetters } from 'vuex';
export default {
  name: 'PurchaseListApproval',
  components: { SmMaterialPurchaseList },
  computed: {
    ...mapGetters(['permissions']),
  },
    watch: {
    $route: function() {
      if (this.$route.path.indexOf('purchase-list-approval') > -1) {
        this.$refs.SmMaterialPurchaseList.refresh();
      }
    },
  },
};
</script>
