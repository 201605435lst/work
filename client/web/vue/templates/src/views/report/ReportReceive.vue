<template>
  <a-card>
    <SmReportReportsReceive
      :axios="axios"
      :permissions="permissions"
      reports-type="receive"
    />
    <!-- @add="onAdd"
      @view="onView"
      @edit="onEdit" -->
  </a-card>
</template>
<script>
import SmReportReportsReceive from 'snweb-module/es/sm-report/sm-reports';
import { mapGetters } from 'vuex';

export default {
  name: 'ReportsReceive',
  components: { SmReportReportsReceive },
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    // onAdd() {
    //   this.$router.push({
    //     name: 'report-report-add',
    //   });
    // },
    // onEdit(id) {
    //   this.$router.push({
    //     name: 'report-report-edit',
    //     params: {
    //       id,
    //     },
    //   });
    // },
    // onView(id) {
    //   this.$router.push({
    //     name: 'report-report-view',
    //     params: {
    //       id,
    //     },
    //   });
    // },
  },
};
</script>