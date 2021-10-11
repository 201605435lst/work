<template>
  <a-card>
    <SmSafeProblemLibrary :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmSafeProblemLibrary from 'snweb-module/es/sm-safe/sm-safe-problem-library';
import { mapGetters } from 'vuex';
export default {
  name: 'SafeProblemLibrary',
  components: { SmSafeProblemLibrary },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
