<template>
  <a-card>
    <SmMaterialEntryRecords :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmMaterialEntryRecords from 'snweb-module/es/sm-material/sm-material-entry-records';
import { mapGetters } from 'vuex';
export default {
  name: 'EntryRecords',
  components: { SmMaterialEntryRecords },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
