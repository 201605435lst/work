<template>
  <a-card>
    <SmScheduleApproval
      :id="id"
      :axios="axios"
      :page-state="pageState"
      :permissions="permissions"
      @ok="onOk"
      @cancel="onCancel"
    />
  </a-card>
</template>
<script>
import SmScheduleApproval from 'snweb-module/es/sm-schedule/sm-schedule-approval';

import { mapGetters } from 'vuex';

export default {
  name: 'Approval',
  components: { SmScheduleApproval },
  props: ['pageState', 'id'],
  computed: {
    ...mapGetters(['permissions']),
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
