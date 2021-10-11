<template>
  <a-card>
    <SmCrPlanSendingWork
      ref="SmCrPlanSendingWork"
      :axios="axios"
      :permissions="permissions"
      :operator-type="operatorType"
      :repair-tag-key="repairTagKey"
      :sending-work-id="id"
      :order-state="orderState"
      @cancel="onCancel"
    />
  </a-card>
</template>
<script>
import SmCrPlanSendingWork from 'snweb-module/es/sm-cr-plan/sm-cr-plan-sending-work';
import { mapGetters } from 'vuex';
import { SendWorkOperatorType } from '../../common/enums';

export default {
  name: 'SendingWork',
  components: { SmCrPlanSendingWork },
  props: ['repairTagKey', 'operatorType', 'id','orderState'],
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function(value, oldValue) {
      if (value.path.indexOf('sending-works') === -1 && this.operatorType !== SendWorkOperatorType.View) {
        this.$refs.SmCrPlanSendingWork.refresh(this.id);
      }
    },
  },
  methods: {
    onCancel() {
      this.$router.back();
    },
  },
};
</script>
