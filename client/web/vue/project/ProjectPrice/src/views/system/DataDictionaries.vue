<template>
  <a-card>
    <SmSystemDataDictionaries :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmSystemDataDictionaries from 'snweb-module/es/sm-system/sm-system-data-dictionaries';
import { mapGetters } from 'vuex';
export default {
  name: 'DataDictionaries',
  components: { SmSystemDataDictionaries },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
