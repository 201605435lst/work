<template>
  <a-card>
    <sm-resource-equipments :axios="axios" :permissions="permissions" :is-qrcode="true" :multiple="true" />
  </a-card>
</template>
<script>
import SmResourceEquipments from 'snweb-module/es/sm-resource/sm-resource-equipments';
import { mapGetters } from 'vuex';
export default {
  name: 'ComponentQrCodeGenerate',
  components: { SmResourceEquipments },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
