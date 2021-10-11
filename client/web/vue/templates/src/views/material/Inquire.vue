<template>
  <a-card>
    <SmMaterialInquire :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmMaterialInquire from 'snweb-module/es/sm-material/sm-material-inquire';
import { mapGetters } from 'vuex';
export default {
  name: 'Inquire',
  components: { SmMaterialInquire },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
