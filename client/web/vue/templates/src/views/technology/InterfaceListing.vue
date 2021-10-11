<template>
  <a-card>
    <SmTechnologyInterfaceListing ref="SmTechnologyInterfaceListing" :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmTechnologyInterfaceListing from 'snweb-module/es/sm-technology/sm-technology-interface-listing';
import { mapGetters } from 'vuex';
export default {
  name: 'InterfaceListing',
  components: { SmTechnologyInterfaceListing },
  computed: {
    ...mapGetters(['permissions']),
  },
   watch: {
    $route: function() {
      if (this.$route.path.indexOf('interface-manage-listing') > -1) {
        this.$refs.SmTechnologyInterfaceListing.refresh();
      }
    },
  },
};
</script>
