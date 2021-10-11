<template>
  <a-card>
    <SmMaterialConstructionSection :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmMaterialConstructionSection from 'snweb-module/es/sm-material/sm-material-construction-section';
import { mapGetters } from 'vuex';
export default {
  name: 'ConstructionSection',
  components: { SmMaterialConstructionSection },
  
  computed: {
    ...mapGetters(['permissions']),
  },

};
</script>
