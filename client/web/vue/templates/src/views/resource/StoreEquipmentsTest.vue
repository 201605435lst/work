<template>
  <a-card>
    <SmResourceStoreEquipmentsTest :id="id" :axios="axios" :permissions="permissions" @cancel="onCancel" />
  </a-card>
</template>
<script>
import SmResourceStoreEquipmentsTest from 'snweb-module/es/sm-resource/sm-resource-store-equipments-test';
import { mapGetters } from 'vuex';
export default {
  name: 'StoreEquipmentsTest',
  components: { SmResourceStoreEquipmentsTest },
  props: ['id'],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onCancel() {
      this.$router.back();
    },
  },
};
</script>
