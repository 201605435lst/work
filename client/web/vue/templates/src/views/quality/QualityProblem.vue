<template>
  <a-card>
    <SmQualityProblems :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmQualityProblems from 'snweb-module/es/sm-quality/sm-quality-problems';
import { mapGetters } from 'vuex';
export default {
  name: 'QualityProblem',
  components: { SmQualityProblems },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
