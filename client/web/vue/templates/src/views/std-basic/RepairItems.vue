<template>
  <a-card>
    <SmStdBasicRepairItems
      ref="SmStdBasicRepairItems"
      :axios="axios"
      :permissions="permissions"
      :repair-tag-key="repairTagKey"
    />
  </a-card>
</template>
<script>
import SmStdBasicRepairItems from 'snweb-module/es/sm-std-basic/sm-std-basic-repair-items';
import { mapGetters } from 'vuex';
export default {
  name: 'RepairItems',
  components: { SmStdBasicRepairItems },
  props: ['repairTagKey'],
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('repair-items') > -1) {
        this.$nextTick(() => {
          this.$refs.SmStdBasicRepairItems.refresh();
        });
      }
    },
  },
};
</script>
