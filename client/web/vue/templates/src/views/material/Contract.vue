<template>
  <a-card>
    <SmMaterialContract :axios="axios" />
  </a-card>
</template>
<script>
import SmMaterialContract from 'snweb-module/es/sm-material/sm-material-contract';
import { mapGetters } from 'vuex';
export default {
  name: 'Contract',
  components: { SmMaterialContract },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
