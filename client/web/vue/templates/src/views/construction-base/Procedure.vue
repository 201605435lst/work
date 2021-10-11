<template>
  <a-card>
    <SmConstructionBaseProcedure :axios="axios" :permission="permissions" />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionBaseProcedure
  from 'snweb-module/lib/sm-construction-base/sm-construction-base-procedure/SmConstructionBaseProcedure';

export default {
  'name': 'Procedure',
  components: {SmConstructionBaseProcedure},
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>

<style scoped>

</style>