<template>
  <a-card>
    <SmStdBasicMvdProperty ref="SmStdBasicMvdProperty" :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmStdBasicMvdProperty from 'snweb-module/es/sm-std-basic/sm-std-basic-mvd-property';
import { mapGetters } from 'vuex';
export default {
  name: 'MvdProperty',
  components: { SmStdBasicMvdProperty },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('mvd-property') > -1) {
        this.$refs.SmStdBasicMvdProperty.initAxios();
      }
    },
  },
};
</script>
