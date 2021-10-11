<template>
  <a-card>
    <sm-material-out-records :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmMaterialOutRecords from 'snweb-module/es/sm-material/sm-material-out-records';
import { mapGetters } from 'vuex';
export default {
  name: 'OutRecords',
  components: { SmMaterialOutRecords },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
