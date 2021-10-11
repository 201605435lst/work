<template>
  <a-card>
    <SmCrPlanSkylightPlans
      ref="SmCrPlanSkylightPlans"
      :axios="axios"
      :plan-type="PlanType.General"
      :permissions="permissions"
      :repair-tag-key="repairTagKey"
      @view="onView"
      @edit="onEdit"
      @add="onAdd"
    />
  </a-card>
</template>

<script>
import SmCrPlanSkylightPlans from 'snweb-module/es/sm-cr-plan/sm-cr-plan-skylight-plans';
import { PlanType } from '../../common/enums';
import { mapGetters } from 'vuex';
import { routePrefixes } from '../../config/routers/cr-plan/_util';

export default {
  name: 'GaneralSkylightPlans',
  components: {
    SmCrPlanSkylightPlans,
  },
  props: ['repairTagKey'],
  data() {
    return {
      PlanType,
    };
  },
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
      if (this.$route.path.indexOf('skylight-plans') > -1) {
        this.$refs.SmCrPlanSkylightPlans.refresh();
      }
    },
  },
  methods: {
    onAdd(planTime, organizationId) {
      // console.log('onAdd', organizationId, planTime);
      // this.$message.info(`onAdd: ${organizationId} ${planTime}`);
      this.$router.push({
        name: this.routerPrefix + 'skylight-plan-add',
        params: {
          repairTagKey: this.repairTagKey,
          organizationId,
          planTime,
          planType: PlanType.General,
        },
      });
    },
    onView(id, organizationId, planTime) {
      // console.log('onView', id);
      // this.$message.info(`onView: ${id}`);
      this.$router.push({
        name: this.routerPrefix + 'skylight-plan-view',
        params: {
          repairTagKey: this.repairTagKey,
          id,
          organizationId,
          planTime,
          planType: PlanType.General,
        },
      });
    },
    onEdit(id, organizationId, planTime) {
      // console.log('onEdit', id);
      // this.$message.info(`onEdit: ${id} `);
      this.$router.push({
        name: this.routerPrefix + 'skylight-plan-edit',
        params: {
          repairTagKey: this.repairTagKey,
          id,
          organizationId,
          planTime,
          planType: PlanType.General,
        },
      });
    },
  },
};
</script>
