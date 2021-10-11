<template>
  <a-card>
    <SmMaterialConstructionTeam :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmMaterialConstructionTeam from 'snweb-module/es/sm-material/sm-material-construction-team';
import { mapGetters } from 'vuex';
export default {
  name: 'ConstructionTeam',
  components: { SmMaterialConstructionTeam },
  computed: {
    ...mapGetters(['permissions']),
  },

};
</script>
