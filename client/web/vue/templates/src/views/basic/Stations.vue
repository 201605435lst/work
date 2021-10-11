<template>
  <a-card>
    <SmBasicStations :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmBasicStations from 'snweb-module/es/sm-basic/sm-basic-stations';
import { mapGetters } from 'vuex';

export default {
  name: 'Stations',
  components: { SmBasicStations },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
