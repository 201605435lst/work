<template>
  <a-card>
    <SmConstructionBaseEquipmentTeam :axios="axios" :permission="permissions" />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionBaseEquipmentTeam from 'snweb-module/lib/sm-construction-base/sm-construction-base-equipment-team/SmConstructionBaseEquipmentTeam';

export default {
  'name': 'EquipmentTeam',
  components: {SmConstructionBaseEquipmentTeam},
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>

<style scoped>

</style>