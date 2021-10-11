<template>
  <a-card>
    <SmTechnologyInterfaceFlag ref="SmTechnologyInterfaceFlag" :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmTechnologyInterfaceFlag from 'snweb-module/es/sm-technology/sm-technology-interface-flag';
import { mapGetters } from 'vuex';
export default {
  name: 'InterfaceFlag',
  components: { SmTechnologyInterfaceFlag },
  computed: {
    ...mapGetters(['permissions']),
  },
  
   watch: {
    $route: function() {
      if (this.$route.path.indexOf('interface-manage-flag') > -1) {
        this.$refs.SmTechnologyInterfaceFlag.refresh();
      }
    },
  },
};
</script>
