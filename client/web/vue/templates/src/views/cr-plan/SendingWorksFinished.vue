<template>
  <a-card>
    <SmCrPlanSendingWorksFinished
      ref="SmCrPlanSendingWorksFinished"
      :axios="axios"
      :permissions="permissions"
      :repair-tag-key="repairTagKey"
      @view="onView"
      @edit="onEdit"
    />
  </a-card>
</template>
<script>
import SmCrPlanSendingWorksFinished from 'snweb-module/es/sm-cr-plan/sm-cr-plan-sending-works-finished';
import { mapGetters } from 'vuex';
import { routePrefixes } from '../../config/routers/cr-plan/_util';

export default {
  name: 'SendingWorksFinished',
  components: { SmCrPlanSendingWorksFinished },
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
      if (this.$route.path.indexOf('sending-works-finished') > -1) {
        this.$refs.SmCrPlanSendingWorksFinished.refresh();
      }
    },
  },
  methods: {
    onView(operatorType, id) {
      this.$router.push({
        name: this.routerPrefix + 'sending-work',
        params: {
          operatorType,
          id,
          repairTagKey: this.repairTagKey,
        },
      });
    },
    onEdit(operatorType, id) {
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
