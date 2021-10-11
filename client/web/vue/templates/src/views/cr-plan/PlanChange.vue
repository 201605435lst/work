<template>
  <a-card>
    <SmCrPlanPlanChange
      :id="id"
      ref="SmCrPlanPlanChange"
      :page-state="pageState"
      :organization-id="organizationId"
      :axios="axios"
      :repair-tag-key="repairTagKey"
      :permissions="permissions"
      @ok="onOk"
      @cancel="onCancel"
    />
  </a-card>
</template>

<script>
import SmCrPlanPlanChange from 'snweb-module/es/sm-cr-plan/sm-cr-plan-plan-change';
import { mapGetters } from 'vuex';
import { PageState } from 'snweb-module/es/_utils/enum';

export default {
  name: 'PlanChange',
  components: { SmCrPlanPlanChange },
  props: ['repairTagKey', 'pageState', 'id', 'organizationId'],
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function(value, oldValue) {
      if (value.path.indexOf('plan-changes') === -1 && this.pageState !== PageState.Add) {
        this.$refs.SmCrPlanPlanChange.initData(this.id);
      }
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
