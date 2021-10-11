<template>
  <a-card>
    <SmCrPlanYearPlan
      :plan-type="planType"
      :org-id="orgId"
      :repair-tag-key="repairTagKey"
      org-name="orgName"
      :axios="axios"
      :permissions="permissions"
      @change="onChange"
    />
  </a-card>
</template>

<script>
import SmCrPlanYearPlan from 'snweb-module/es/sm-cr-plan/sm-cr-plan-year-plan';
import { mapGetters } from 'vuex';
import { routePrefixes } from '../../config/routers/cr-plan/_util';

export default {
  name: 'YearPlan',
  components: { SmCrPlanYearPlan },
  props: ['repairTagKey', 'planType', 'orgId', 'orgName'],
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
    onChange(planType, orgId, orgName) {
      console.log(this.routerPrefix);
      this.$router.push({
        name: this.routerPrefix + 'year-month-change',
        params: { planType, orgId, orgName, repairTagKey: this.repairTagKey },
      });
    },
  },
};
</script>
