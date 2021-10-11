<template>
  <a-card>
    <SmStdBasicMvdCategory :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmStdBasicMvdCategory from 'snweb-module/es/sm-std-basic/sm-std-basic-mvd-category';
import { mapGetters } from 'vuex';
export default {
  name: 'MvdCategory',
  components: { SmStdBasicMvdCategory },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
