<template>
  <a-card>
    <SmStdBasicManufacturers :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmStdBasicManufacturers from 'snweb-module/es/sm-std-basic/sm-std-basic-manufacturers';
import { mapGetters } from 'vuex';
export default {
  name: 'Manufacturers',
  components: { SmStdBasicManufacturers },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
