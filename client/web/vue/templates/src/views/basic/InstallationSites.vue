<template>
  <a-card>
    <SmBasicInstallationSites :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmBasicInstallationSites from 'snweb-module/es/sm-basic/sm-basic-installation-sites';
import { mapGetters } from 'vuex';

export default {
  name: 'Installationsites',
  components: {
    SmBasicInstallationSites,
  },

  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
