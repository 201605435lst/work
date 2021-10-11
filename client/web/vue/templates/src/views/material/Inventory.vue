<template>
  <a-card>
    <SmMaterialInventory :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmMaterialInventory from 'snweb-module/es/sm-material/sm-material-inventory';
import { mapGetters } from 'vuex';
export default {
  name: 'Inventory',
  components: { SmMaterialInventory },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
