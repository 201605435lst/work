<template>
  <a-card>
    <SmResourceStoreHouse :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmResourceStoreHouse from 'snweb-module/es/sm-resource/sm-resource-store-house';
import { mapGetters } from 'vuex';
export default {
  name: 'StoreHouse',
  components: { SmResourceStoreHouse },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
