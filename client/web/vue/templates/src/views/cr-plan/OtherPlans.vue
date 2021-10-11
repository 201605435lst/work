<template>
  <a-card>
    <SmCrPlanOtherPlans
      ref="SmCrPlanOtherPlans"
      :axios="axios"
      :permissions="permissions"
      :repair-tag-key="repairTagKey"
      @view="onView"
      @edit="onEdit"
      @add="onAdd"
    />
  </a-card>
</template>
<script>
import SmCrPlanOtherPlans from 'snweb-module/es/sm-cr-plan/sm-cr-plan-other-plans';
import { mapGetters } from 'vuex';
import { routePrefixes } from '../../config/routers/cr-plan/_util';

export default {
  name: 'OtherPlans',
  components: { SmCrPlanOtherPlans },
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
      if (this.$route.path.indexOf('other-plans') > -1) {
        this.$refs.SmCrPlanOtherPlans.refresh();
      }
    },
  },

  methods: {
    onAdd(organizationId, planTime,workAreaId) {
      this.$router.push({
        name: this.routerPrefix + 'vertical-other-plan-add',
        params: {
          organizationId,
          planTime,
          repairTagKey: this.repairTagKey,
          workAreaId,
        },
      });
    },

    onView(id, organizationId, planTime) {
      this.$router.push({
        name: this.routerPrefix + 'vertical-other-plan-view',
        params: {
          id,
          organizationId,
          planTime,
          repairTagKey: this.repairTagKey,
        },
      });
    },

    onEdit(id, organizationId, planTime,isChange) {
      this.$router.push({
        name: this.routerPrefix + 'vertical-other-plan-edit',
        params: {
          id,
          organizationId,
          planTime,
          repairTagKey: this.repairTagKey,
          isChange,
        },
      });
    },
  },
};
</script>
