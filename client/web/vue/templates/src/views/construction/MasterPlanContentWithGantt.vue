<template>
  <a-card>
    <SmConstructionMasterPlanContentWithGantt
      ref="gantt"
      :master-plan-id="id"
      :axios="axios"
      :permissions="permissions"
      @back="onBack"
    />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionMasterPlanContentWithGantt
  from 'snweb-module/es/sm-construction/sm-construction-master-plan-content-with-gantt/SmConstructionMasterPlanContentWithGantt';

export default {
  name: 'MasterPlanContentWithGantt',
  components: { SmConstructionMasterPlanContentWithGantt },
  props: ['id'],
  data() {
    return {};
  },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: {
      handler: function(value, oldValue) {
      },
    },
  },
  methods: {
    onBack() {
      this.$router.back();
    },
  },
};
</script>

<style scoped>

</style>