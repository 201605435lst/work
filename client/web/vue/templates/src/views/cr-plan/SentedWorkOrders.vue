<template>
  <a-card>
    <SmCrPlanSentedWorkOrders :axios="axios" :permissions="permissions" :repair-tag-key="repairTagKey" />
  </a-card>
</template>
<script>
import SmCrPlanSentedWorkOrders from 'snweb-module/es/sm-cr-plan/sm-cr-plan-sented-work-orders';
import { mapGetters } from 'vuex';

export default {
  name: 'SentedWorkOrders',
  components: { SmCrPlanSentedWorkOrders },
  props: ['repairTagKey'],
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
