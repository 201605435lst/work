<template>
  <a-card>
    <SmConstructionBaseWorker :axios="axios" :permission="permissions" />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionBaseWorker from 'snweb-module/lib/sm-construction-base/sm-construction-base-worker/SmConstructionBaseWorker';
export default {
  'name': 'Worker',
  components: {SmConstructionBaseWorker},
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>

<style scoped>

</style>