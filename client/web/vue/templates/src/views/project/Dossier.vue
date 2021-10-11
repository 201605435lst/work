<template>
  <a-card>
    <SmProjectDossier ref="SmProjectDossier" :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmProjectDossier from 'snweb-module/es/sm-project/sm-project-dossier';
import { mapGetters } from 'vuex';
export default {
  name: 'Dossier',
  components: { SmProjectDossier },
  computed: {
    ...mapGetters(['permissions']),
  },
  $route: function() {
    if (this.$route.path.indexOf('dossier') > -1) {
      this.$refs.SmProjectDossier.refresh();
    }
  },
};
</script>
