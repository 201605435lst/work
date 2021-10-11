<template>
  <a-card>
    <SmStdBasicStandardEquipments :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmStdBasicStandardEquipments from 'snweb-module/es/sm-std-basic/sm-std-basic-standard-equipments';
import { mapGetters } from 'vuex';
export default {
  name: 'StandardEquipments',
  components: { SmStdBasicStandardEquipments },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
