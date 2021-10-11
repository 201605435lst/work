<template>
  <a-card>
    <SmStdBasicQuota :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmStdBasicQuota from 'snweb-module/es/sm-std-basic/sm-std-basic-quota';
import { mapGetters } from 'vuex';
export default {
  name: 'Quota',
  components: { SmStdBasicQuota },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
