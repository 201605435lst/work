<template>
  <a-card>
    <SmStdBasicIndividualProject :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmStdBasicIndividualProject from 'snweb-module/es/sm-std-basic/sm-std-basic-individual-project';
import { mapGetters } from 'vuex';
export default {
  name: 'IndividualProject',
  components: { SmStdBasicIndividualProject },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
