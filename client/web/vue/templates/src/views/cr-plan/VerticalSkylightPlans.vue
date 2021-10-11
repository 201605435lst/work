<template>
  <a-card>
    <SmCrPlanVerticalSkylightPlans
      ref="SmCrPlanVerticalSkylightPlans"
      :axios="axios"
      :plan-type="PlanType.Vertical"
      :permissions="permissions"
      :repair-tag-key="repairTagKey"
      @view="onView"
      @edit="onEdit"
      @add="onAdd"
    />
  </a-card>
</template>

<script>
import SmCrPlanVerticalSkylightPlans from 'snweb-module/es/sm-cr-plan/sm-cr-plan-skylight-plans';
import { PlanType } from 'snweb-module/es/_utils/enum';
import { mapGetters } from 'vuex';
import { routePrefixes } from '../../config/routers/cr-plan/_util';

export default {
  name: 'VerticalSkylightPlans',
  components: { SmCrPlanVerticalSkylightPlans },
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
      if (this.$route.path.indexOf('vertical-skylight-plans') > -1) {
        this.$refs.SmCrPlanVerticalSkylightPlans.refresh();
      }
    },
  },
  created() {},

  methods: {
    onAdd(planTime, organizationId) {
      this.$router.push({
        name: this.routerPrefix + 'skylight-plan-add',
        params: {
          repairTagKey: this.repairTagKey,
          organizationId,
          planTime,
          planType: PlanType.Vertical,
        },
      });
    },
    onView(id, organizationId, planTime,isChange) {
      this.$router.push({
        name: this.routerPrefix + 'skylight-plan-view',
        params: {
          id,
          repairTagKey: this.repairTagKey,
          organizationId,
          planTime,
          planType: PlanType.Vertical,
          isChange:isChange,
        },
      });
    },
    onEdit(id, organizationId, planTime,isChange) {
      this.$router.push({
        name: this.routerPrefix + 'skylight-plan-edit',
        params: {
          id,
          repairTagKey: this.repairTagKey,
          organizationId,
          planTime,
          planType: PlanType.Vertical,
          isChange,
        },
      });
    },
  },
};
</script>
