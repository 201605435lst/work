<template>
  <a-card>
    <SmConstructionPlanWithGantt
      ref="gantt"
      :plan-id="id"
      :axios="axios"
      :permissions="permissions"
      @back="onBack"
    />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionPlanWithGantt
  from 'snweb-module/es/sm-construction/sm-construction-plan-with-gantt/SmConstructionPlanWithGantt';

export default {
  name: 'PlanContentWithGantt',
  components: { SmConstructionPlanWithGantt },
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