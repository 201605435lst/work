<template>
  <a-card>
    <SmCrPlanState :axios="axios" :permissions="permissions" :repair-tag-key="repairTagKey" @track="onTrack" />
  </a-card>
</template>
<script>
import SmCrPlanState from 'snweb-module/es/sm-cr-statistics/sm-cr-statistics-plan-state';
import { mapGetters } from 'vuex';
import { routePrefixes } from '../../config/routers/cr-plan/_util';

export default {
  name: 'PlanState',
  components: { SmCrPlanState },
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
  methods: {
    onTrack(organizationId, planTime, planType, number) {
      // console.log('---------');
      // console.log(organizationId, planTime);
      // this.$message.info(`onAdd: ${organizationId} ${planTime}`);
      this.$router.push({
        name: this.routerPrefix + 'planTrack',
        params: {
          repairTagKey: this.repairTagKey,
          organizationId,
          planTime,
          planType,
          number,
        },
      });
    },
  },
};
</script>

@track=" on-track-
