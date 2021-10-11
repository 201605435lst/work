<template>
  <a-card>
    <SmResourceStoreImport :axios="axios" type="import" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmResourceStoreImport from 'snweb-module/es/sm-resource/sm-resource-store-import-alt-export';
import { mapGetters } from 'vuex';
export default {
  name: 'StoreImport',
  components: { SmResourceStoreImport },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
