<template>
  <a-card>
    <SmOaSeal :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmOaSeal from 'snweb-module/es/sm-oa/sm-oa-seals';
import { mapGetters } from 'vuex';

export default {
  name: 'Seal',
  components: { SmOaSeal },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
