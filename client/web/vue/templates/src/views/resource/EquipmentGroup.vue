<template>
  <a-card>
    <SmResourceEquipmentGroup :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmResourceEquipmentGroup from 'snweb-module/es/sm-resource/sm-resource-equipment-group';
import { mapGetters } from 'vuex';
export default {
  name: 'EquipmentGroup',
  components: { SmResourceEquipmentGroup },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
