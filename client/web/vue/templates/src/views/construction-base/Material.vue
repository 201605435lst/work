<template>
  <a-card>
    <SmConstructionBaseMaterial :axios="axios" :permission="permissions" />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionBaseMaterial
  from 'snweb-module/lib/sm-construction-base/sm-construction-base-material/SmConstructionBaseMaterial';

export default {
  name: 'Material',
  components: {SmConstructionBaseMaterial},
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>

<style scoped>

</style>