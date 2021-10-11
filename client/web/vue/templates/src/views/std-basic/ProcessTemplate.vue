<template>
  <a-card>
    <SmStdBasicProcessTemplate :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmStdBasicProcessTemplate from 'snweb-module/es/sm-std-basic/sm-std-basic-process-template';
import { mapGetters } from 'vuex';
export default {
  name: 'ProcessTemplate',
  components: { SmStdBasicProcessTemplate },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
