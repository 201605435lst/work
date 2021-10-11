<template>
  <a-card>
    <SmTechnologyInterfaceReport ref="SmTechnologyInterfaceReport" :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmTechnologyInterfaceReport from 'snweb-module/es/sm-technology/sm-technology-interface-report';
import { mapGetters } from 'vuex';
export default {
  name: 'InterfaceReport',
  components: { SmTechnologyInterfaceReport },
  computed: {
    ...mapGetters(['permissions']),
  },
   watch: {
    $route: function() {
      if (this.$route.path.indexOf('interface-manage-report') > -1) {
        this.$refs.SmTechnologyInterfaceReport.refresh();
      }
    },
  },
};
</script>
