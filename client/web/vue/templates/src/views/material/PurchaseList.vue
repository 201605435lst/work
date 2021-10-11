<template>
  <a-card>
    <SmMaterialPurchaseList ref="SmMaterialPurchaseList" :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmMaterialPurchaseList from 'snweb-module/es/sm-material/sm-material-purchase-list';
import { mapGetters } from 'vuex';
export default {
  name: 'PurchaseList',
  components: { SmMaterialPurchaseList },
  computed: {
    ...mapGetters(['permissions']),
  },
    watch: {
    $route: function() {
      if (this.$route.path.indexOf('purchase-list-plan') > -1) {
        this.$refs.SmMaterialPurchaseList.refresh();
      }
    },
  },
};
</script>
