<template>
  <a-card>
    <SmCrPlanOtherWorks :axios="axios" :permissions="permissions" :repair-tag-key="repairTagKey" />
  </a-card>
</template>
<script>
import SmCrPlanOtherWorks from 'snweb-module/es/sm-cr-plan/sm-cr-plan-other-works';
import { mapGetters } from 'vuex';

export default {
  name: 'OtherWorks',
  components: { SmCrPlanOtherWorks },
  props: ['repairTagKey'],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {},
};
</script>
