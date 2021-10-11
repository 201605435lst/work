<template>
  <a-card>
    <SmCrPlanMaintenanceRecord
      ref="SmCrPlanMaintenanceRecord"
      :organization-id="organizationId"
      :equipment-id="equipmentId"
      :repair-group-id="repairGroupId"
      :equip-type="equipType"
      :equip-name="equipName"
      :equip-model-number="equipModelNumber"
      :equip-model-code="equipModelCode"
      :installation-site="installationSite"
      :axios="axios"
      :permissions="permissions"
      :repair-tag-key="$router.history.current.params.repairTagKey"
      :work-order-ids="workOrderIds"
      @ok="onOk"
      @cancel="onCancel"
    />
  </a-card>
</template>

<script>
import SmCrPlanMaintenanceRecord from 'snweb-module/es/sm-cr-plan/sm-cr-plan-maintenance-record';
import { mapGetters } from 'vuex';

export default {
  name: 'MaintenanceRecord',
  components: {
    SmCrPlanMaintenanceRecord,
  },
  props: [
    'repairTagKey',
    'organizationId',
    'equipmentId',
    'repairGroupId',
    'equipType',
    'equipName',
    'equipModelNumber',
    'equipModelCode',
    'installationSite',
    'workOrderIds',
  ],

  computed: {
    ...mapGetters(['permissions']),
  },

  watch: {
    $route: function(value, oldValue) {
      if (value.path.indexOf('maintenance-records') === -1) {
        this.$refs.SmCrPlanMaintenanceRecord.refresh();
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
