<template>
  <a-card>
    <SmConstructionBaseSection :axios="axios" :bordered="false" :permission="permissions" />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionBaseSection
  from 'snweb-module/lib/sm-construction-base/sm-construction-base-section/SmConstructionBaseSection';

// 工序规范管理
export default {
  name: 'Section',
  components: {SmConstructionBaseSection},
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {

  },
};
</script>

<style scoped>

</style>