<template>
  <a-card>
    <SmCrPlanSendingWorks
      ref="SmCrPlanSendingWorks"
      :axios="axios"
      :permissions="permissions"
      :repair-tag-key="repairTagKey"
      @view="onView"
      @finish="onFinish"
      @acceptance="onAcceptance"
    />
  </a-card>
</template>
<script>
import SmCrPlanSendingWorks from 'snweb-module/es/sm-cr-plan/sm-cr-plan-sending-works';
import { mapGetters } from 'vuex';
import { routePrefixes } from '../../config/routers/cr-plan/_util';

export default {
  name: 'SendingWorks',
  components: { SmCrPlanSendingWorks },
  props: ['repairTagKey'],
  computed: {
    ...mapGetters(['permissions']),
    routerPrefix: function() {
      let prefix = this.$route.name.split('.')[0];
      if (routePrefixes.includes(prefix)) {
        prefix += '.';
      } else {
        prefix = '';
      }
      return prefix;
    },
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('sending-works') > -1) {
        this.$refs.SmCrPlanSendingWorks.refresh();
      }
    },
  },
  methods: {
    onView(operatorType, id,orderState) {
      // console.log('onView', id);
      // this.$message.info(`onView: ${id}`);
      this.$router.push({
        name: this.routerPrefix + 'sending-work',
        params: {
          operatorType,
          id,
          repairTagKey: this.repairTagKey,
          orderState,
        },
      });
    },
    onFinish(operatorType, id,orderState) {
      // this.$message.info(`onAdd: ${organizationId} ${planTime}`);
      console.log(operatorType, id);
      this.$router.push({
        name: this.routerPrefix + 'sending-work',
        params: {
          operatorType,
          id,
          repairTagKey: this.repairTagKey,
          orderState,
        },
      });
    },
    onAcceptance(operatorType, id) {
      // console.log('onAdd', organizationId, planTime);
      // this.$message.info(`onAdd: ${organizationId} ${planTime}`);
      this.$router.push({
        name: this.routerPrefix + 'sending-work',
        params: {
          operatorType,
          id,
          repairTagKey: this.repairTagKey,
        },
      });
    },
  },
};
</script>
