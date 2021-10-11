<template>
  <a-card>
    <SmStdBasicComponentCategory :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmStdBasicComponentCategory from 'snweb-module/es/sm-std-basic/sm-std-basic-component-category';
import { mapGetters } from 'vuex';
export default {
  name: 'ComponentCategory',
  components: { SmStdBasicComponentCategory },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
