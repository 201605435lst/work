<template>
  <a-card>
    <SmCrPlanYearMonthChange
      ref="SmCrPlanYearMonthChange"
      :axios="axios"
      :plan-type="planType"
      :org-id="orgId"
      :repair-tag-key="repairTagKey"
      :org-name="orgName"
      :permissions="permissions"
      :alter-record-id="alterRecordId"
      :is-create-record="isCreateRecord"
      @cancel="onCancel"
    />
  </a-card>
</template>

<script>
import SmCrPlanYearMonthChange from 'snweb-module/es/sm-cr-plan/sm-cr-plan-year-month-change';
import { mapGetters } from 'vuex';

export default {
  name: 'YearMonthChange',
  components: { SmCrPlanYearMonthChange },
  props: ['repairTagKey', 'planType', 'orgId', 'orgName','alterRecordId','isCreateRecord'],
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('year-month-change') > -1) {
        this.$refs.SmCrPlanYearMonthChange.refresh();
      }
    },
  },
  methods: {
    onCancel() {
      // console.log('bbbb');
      // console.log(this.planType + ':' + this.orgId + ':' + this.orgName);
      this.$router.back();
    },
  },
};
</script>
