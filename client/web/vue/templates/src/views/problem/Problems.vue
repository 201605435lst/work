<template>
  <a-card>
    <SmProblemProblems :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmProblemProblems from 'snweb-module/es/sm-problem/sm-problem-problems';
import { mapGetters } from 'vuex';

export default {
  name: 'Problems',
  components: { SmProblemProblems },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
