<template>
  <a-card>
    <sm-resource-equipments :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmResourceEquipments from 'snweb-module/es/sm-resource/sm-resource-equipments';
import { mapGetters } from 'vuex';
export default {
  name: 'Equipments',
  components: { SmResourceEquipments },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
