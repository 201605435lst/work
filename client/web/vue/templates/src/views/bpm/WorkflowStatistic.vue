 <sm-bpm-chart-statistics :axios="axios" />

<template>
  <a-card>
    <SmBpmChartStatistics
      :axios="axios"
    />
  </a-card>
</template>

<script>
import SmBpmChartStatistics from 'snweb-module/es/sm-bpm/sm-bpm-chart-statistics';
import { mapGetters } from 'vuex';

export default {
  name: 'WorkflowsStatistic',
  components: { SmBpmChartStatistics },
  computed: {
    // ...mapGetters(['permissions']),
  },
  methods: {
  },
};
</script>