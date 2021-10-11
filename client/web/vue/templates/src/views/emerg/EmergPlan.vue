<template>
  <a-card>
    <SmEmergPlan
      :id="id"
      :axios="axios"
      :page-state="pageState"
      :is-apply="isApply"
      :fault-id="faultId"
      :permissions="permissions"
      :is-d3="isD3"
      @ok="onOk"
      @cancel="onCancel"
    />
  </a-card>
</template>
<script>
import SmEmergPlan from 'snweb-module/es/sm-emerg/sm-emerg-plan';
import { mapGetters } from 'vuex';

export default {
  name: 'EmergPlan',
  components: { SmEmergPlan },
  props: ['pageState', 'id', 'isApply', 'faultId', 'isD3'],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onOk() {
      this.$router.back();
    },
    onCancel() {
      console.log(this.isD3);
      if(this.isD3 == 'D3'){
        this.$router.push({ name: 'd3' });
      }else{
        this.$router.back();
      }
    },
  },
};
</script>
