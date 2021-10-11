<template>
  <a-card>
    <sm-material-partital :axios="axios" />
  </a-card>
</template>
<script>
import SmMaterialPartital from 'snweb-module/es/sm-material/sm-material-partital';
import { mapGetters } from 'vuex';

export default {
  name: 'Partital',
  components: { SmMaterialPartital },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    // $route: function() {
    //   if (this.$route.path.indexOf('material-material') > -1) {
    //     this.$refs.SmMaterialMaterial.refresh();
    //   }
    // },
  },
  methods: {
  },
};
</script>
