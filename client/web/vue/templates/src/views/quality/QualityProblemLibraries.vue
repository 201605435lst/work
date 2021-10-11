<template>
  <a-card>
    <SmQualityProblemLibraries :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmQualityProblemLibraries from 'snweb-module/es/sm-quality/sm-quality-problem-libraries';
import { mapGetters } from 'vuex';
export default {
  name: 'QualityProblemLibraries',
  components: { SmQualityProblemLibraries },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
