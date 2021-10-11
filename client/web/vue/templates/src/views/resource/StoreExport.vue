<template>
  <a-card>
    <SmResourceStoreExport :axios="axios" type="export" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmResourceStoreExport from 'snweb-module/es/sm-resource/sm-resource-store-import-alt-export';
import { mapGetters } from 'vuex';
export default {
  name: 'StoreExport',
  components: { SmResourceStoreExport },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
