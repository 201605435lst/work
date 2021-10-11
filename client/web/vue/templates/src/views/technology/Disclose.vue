<template>
  <a-card>
    <SmTechnologyDisclose ref="SmTechnologyDisclose" :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmTechnologyDisclose from 'snweb-module/es/sm-technology/sm-technology-disclose';
import { mapGetters } from 'vuex';
export default {
  name: 'Disclose',
  components: { SmTechnologyDisclose },
  computed: {
    ...mapGetters(['permissions']),
  },
   watch: {
    $route: function() {
      if (this.$route.path.indexOf('disclose') > -1) {
        this.$refs.SmTechnologyDisclose.refresh();
      }
    },
  },
};
</script>
