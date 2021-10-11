<template>
  <a-card>
    <SmBasicRailways :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmBasicRailways from 'snweb-module/es/sm-basic/sm-basic-railways';
import { mapGetters } from 'vuex';

export default {
  name: 'Railways',
  components: { SmBasicRailways },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
