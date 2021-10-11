<template>
  <a-card>
    <SmConstructionBaseStandard :axios="axios" :bordered="false" :permission="permissions" />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionBaseStandard
  from 'snweb-module/lib/sm-construction-base/sm-construction-base-standard/SmConstructionBaseStandard';

// 工序规范管理
export default {
  name: 'Standard',
  components: {SmConstructionBaseStandard},
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {

  },
};
</script>

<style scoped>

</style>