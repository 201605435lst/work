<template>
  <a-card>
    <!-- 工程量统计 -->
    <SmTechnologyQuantity ref="SmTechnologyQuantity" :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmTechnologyQuantity from 'snweb-module/es/sm-technology/sm-technology-quantity';
import { mapGetters } from 'vuex';
export default {
  name: 'QuantityList',
  components: { SmTechnologyQuantity },
  computed: {
    ...mapGetters(['permissions']),
  },
 watch: {
    $route: function() {
      if (this.$route.path.indexOf('quantity') > -1) {
        this.$refs.SmTechnologyQuantity.refresh();
      }
    },
  },
};
</script>
