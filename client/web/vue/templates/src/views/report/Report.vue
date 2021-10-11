<template>
  <a-card>
    <SmReportReport
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
import SmReportReport from 'snweb-module/es/sm-report/sm-report';
import { mapGetters } from 'vuex';

export default {
  name: 'Report',
  components: { SmReportReport },
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
