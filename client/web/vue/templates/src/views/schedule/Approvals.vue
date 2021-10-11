<template>
  <a-card>
    <SmScheduleApprovals
      ref="SmScheduleApprovals"
      :axios="axios"
      :permissions="permissions"
      @add="onAdd"
      @edit="onEdit"
    />
  </a-card>
</template>
<script>
import SmScheduleApprovals from 'snweb-module/es/sm-schedule/sm-schedule-approvals';
import { mapGetters } from 'vuex';

export default {
  name: 'Approvals',
  components: { SmScheduleApprovals },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('schedule-approvals') > -1) {
        this.$refs.SmScheduleApprovals.refresh();
      }
    },
  },
  methods: {
    onAdd() {
      this.$router.push({
        name: 'schedule-approval-add',
        params: {
          id:null,
        },
      });
    },
    onEdit(record) {
      this.$router.push({
        name: 'schedule-approval-edit',
        params: {
          id:record.id,
        },
      });
    },
  },
};
</script>
