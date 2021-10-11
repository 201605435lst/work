<template>
  <a-card>
    <SmResourceStoreEquipmentTransfer :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmResourceStoreEquipmentTransfer from 'snweb-module/es/sm-resource/sm-resource-store-equipment-transfer';
import { mapGetters } from 'vuex';
export default {
  name: 'StoreEquipmentTransfer',
  components: { SmResourceStoreEquipmentTransfer },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
