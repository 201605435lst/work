<template>
  <a-card>
    <SmResourceStoreEquipmentsTests :axios="axios" :permissions="permissions" @view="onView" />
  </a-card>
</template>
<script>
import SmResourceStoreEquipmentsTests from 'snweb-module/es/sm-resource/sm-resource-store-equipments-tests';
import { mapGetters } from 'vuex';
export default {
  name: 'StoreEquipmentsTests',
  components: { SmResourceStoreEquipmentsTests },
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onView(id) {
      this.$router.push({
        name: 'basic-data-store-equipments-test-view',
        params: {
          id,
        },
      });
    },
  },
};
</script>
