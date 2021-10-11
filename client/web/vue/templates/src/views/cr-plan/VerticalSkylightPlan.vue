<template>
  <a-card>
    <SmCrPlanVerticalSkylightPlan
      :id="id"
      ref="SmCrPlanVerticalSkylightPlan"
      :repair-tag-key="$router.history.current.params.repairTagKey"
      :page-state="pageState"
      :plan-type="planType"
      :plan-date="planTime"
      :organization-id="organizationId"
      :axios="axios"
      :permissions="permissions"
      :is-change="isChange"
      @ok="onOk"
      @cancel="onCancel"
    />
  </a-card>
</template>

<script>
import SmCrPlanVerticalSkylightPlan from 'snweb-module/es/sm-cr-plan/sm-cr-plan-skylight-plan';
import { mapGetters } from 'vuex';
import { PageState } from '../../common/enums';

export default {
  name: 'VerticalSkylightPlan',
  components: { SmCrPlanVerticalSkylightPlan },
  props: ['pageState', 'id', 'planTime', 'organizationId', 'planType','isChange'],

  data() {
    return {};
  },
  computed: {
    ...mapGetters(['permissions']),
  },

  watch: {
    $route: {
      handler: function(value, oldValue) {
        this.$refs.SmCrPlanVerticalSkylightPlan.getLastSkylightPlan();
        if (value.path.indexOf('skylight-plans') === -1 && this.pageState !== PageState.Add) {
          this.$refs.SmCrPlanVerticalSkylightPlan.refresh(this.id);
        }
      },
    },
  },

  methods: {
    onOk() {
      this.$router.back();
    },
    onCancel() {
      this.$router.back();
    },
  },
};
</script>
