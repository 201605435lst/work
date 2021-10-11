<template>
  <a-card>
    <SmConstructionPlan :axios="axios" :bordered="false" :only-draw="true" @draw="draw" />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionPlan from 'snweb-module/es/sm-construction/sm-construction-plan';
export default {
  name: 'PlanDraw',
  components: { SmConstructionPlan },
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    draw(id) {
      this.$router.push({
        name: 'PlanContentWithGantt',
        params: {
          id,
        },
      });
    },
  },

};
</script>

<style scoped></style>
