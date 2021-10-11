<template>
  <a-card>
    <SmCrPlanOtherPlan
      :id="id"
      ref="SmCrPlanOtherPlan"
      :page-state="pageState"
      :plan-date="planTime"
      :repair-tag-key="repairTagKey"
      :organization-id="organizationId"
      :axios="axios"
      :permissions="permissions"
      :work-area-id="workAreaId"
      :is-change="isChange"
      @ok="onOk"
      @cancel="onCancel"
    />
  </a-card>
</template>
<script>
import SmCrPlanOtherPlan from 'snweb-module/es/sm-cr-plan/sm-cr-plan-other-plan';
import { mapGetters } from 'vuex';
import { PageState } from 'snweb-module/es/_utils/enum';

export default {
  name: 'OtherPlan',
  components: { SmCrPlanOtherPlan },
  props: ['repairTagKey', 'pageState', 'id', 'organizationId', 'planTime','workAreaId','isChange'],
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function(value, oldValue) {
      if (value.path.indexOf('other-plans') === -1 && this.pageState !== PageState.Add) {
        this.$refs.SmCrPlanOtherPlan.refresh(this.id);
      }
    },
  },

  methods: {
    onOk() {
      // console.log(this.planTime);
      this.$router.back();
    },
    onCancel() {
      this.$router.back();
    },
  },
};
</script>
