<template>
  <a-card>
    <SmStdBasicProductCategory :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmStdBasicProductCategory from 'snweb-module/es/sm-std-basic/sm-std-basic-product-category';
import { mapGetters } from 'vuex';
export default {
  name: 'ProductCategory',
  components: { SmStdBasicProductCategory },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
