<template>
  <a-card>
    <SmCrPlanMaintenanceRecords
      ref="SmCrPlanMaintenanceRecords"
      :axios="axios"
      :repair-tag-key="repairTagKey"
      :permissions="permissions"
      @view="onView"
    />
  </a-card>
</template>

<script>
import SmCrPlanMaintenanceRecords from 'snweb-module/es/sm-cr-plan/sm-cr-plan-maintenance-records';
import { mapGetters } from 'vuex';

export default {
  name: 'MaintenanceRecords',
  components: { SmCrPlanMaintenanceRecords },
  props: ['repairTagKey'],
  data() {
    return {};
  },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (
        this.$route.path.indexOf(
          this.repairTagKey ? this.repairTagKey + '-' + 'maintenance-records' : 'maintenance-records'
        ) > -1
      ) {
        this.$refs.SmCrPlanMaintenanceRecords.refresh();
      }
    },
  },
  methods: {
    onView(
      organizationId,
      equipmentId,
      repairGroupId,
      equipType,
      equipName,
      equipModelNumber,
      equipModelCode,
      installationSite,
      workOrderIds
    ) {
      // console.log('onView', id);
      this.$router.push({
        name: 'maintenance-record-view',
        params: {
          repairTagKey: this.repairTagKey,
          organizationId,
          equipmentId,
          repairGroupId,
          equipType,
          equipName,
          equipModelNumber,
          equipModelCode,
          installationSite,
          workOrderIds,
        },
      });
    },
  },
};
</script>
