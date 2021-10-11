<template>
  <a-card>
    <SmSafeProblem :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmSafeProblem from 'snweb-module/es/sm-safe/sm-safe-problem';
import { mapGetters } from 'vuex';
export default {
  name: 'SafeProblem',
  components: { SmSafeProblem },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
