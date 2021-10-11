<template>
  <a-card>
    <SmMaterialAcceptance
      ref="SmMaterialAcceptance"
      :axios="axios"
      :permissions="permissions"
    />
  </a-card>
</template>
<script>
import SmMaterialAcceptance from 'snweb-module/es/sm-material/sm-material-acceptance';
import { mapGetters } from 'vuex';

export default {
  name: 'Acceptance',
  components: { SmMaterialAcceptance },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('material-acceptance') > -1) {
        this.$refs.SmMaterialAcceptance.refresh();
      }
    },
  },
  methods: {
  },
};
</script>
