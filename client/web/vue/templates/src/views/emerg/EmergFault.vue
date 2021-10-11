<template>
  <a-card>
    <SmEmergFault
      :fault-id="faultId"
      :state="state"
      :axios="axios"
      :page-state="pageState"
      :permissions="permissions"
      :append="append"
      :is-d3="isD3"
      @success="onOk"
      @cancel="onCancel"
    />
  </a-card>
</template>
<script>
import SmEmergFault from 'snweb-module/es/sm-emerg/sm-emerg-fault';
import { mapGetters } from 'vuex';

export default {
  name: 'EmergFault',
  components: { SmEmergFault },
  props: ['pageState', 'faultId', 'append', 'state', 'isD3'],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onOk() {
      //this.$router.push({ name: 'emerg-faults' });
      window.localtion.href='emerg-faults';
    },
    onCancel() {
      if (this.isD3 == 'D3') {
        this.$router.push({ name: 'd3' });
      } else {
        this.$router.back();
      }
    },
    // onPresent() {
    //   this.$router.push({ name: 'emerg-faults' });
    // },
  },
};
</script>
