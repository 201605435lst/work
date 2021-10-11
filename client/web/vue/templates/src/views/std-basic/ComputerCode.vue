<template>
  <a-card>
    <SmStdBasicComputerCode :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmStdBasicComputerCode from 'snweb-module/es/sm-std-basic/sm-std-basic-computer-code';
import { mapGetters } from 'vuex';
export default {
  name: 'ComputerCode',
  components: { SmStdBasicComputerCode },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
