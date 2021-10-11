<template>
  <a-card>
    <SmMaterialMaterial
      ref="SmMaterialMaterial"
      :axios="axios"
      :permissions="permissions"
    />
  </a-card>
</template>
<script>
import SmMaterialMaterial from 'snweb-module/es/sm-material/sm-material-material';
import { mapGetters } from 'vuex';

export default {
  name: 'Material',
  components: { SmMaterialMaterial },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('material-materials') > -1) {
        this.$refs.SmMaterialMaterial.refresh();
      }
    },
  },
  methods: {
  },
};
</script>
