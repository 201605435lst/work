<template>
  <a-card>
    <SmConstructionMasterPlan :axios="axios" :bordered="false" :only-draw="true" @draw="draw" />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionMasterPlan from 'snweb-module/es/sm-construction/sm-construction-master-plan/SmConstructionMasterPlan';
export default {
  name: 'MasterPlanDraw',
  components: { SmConstructionMasterPlan },
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    draw(id) {
      this.$router.push({
        name: 'MasterPlanContentWithGantt',
        params: {
          id,
        },
      });
    },
  },

};
</script>

<style scoped></style>
