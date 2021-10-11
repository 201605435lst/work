<template>
  <a-card>
    <SmProblemCategories :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmProblemCategories from 'snweb-module/es/sm-problem/sm-problem-problem-categories';
import { mapGetters } from 'vuex';

export default {
  name: 'ProblemCategories',
  components: { SmProblemCategories },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
