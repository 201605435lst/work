<template>
  <a-card>
    <SmConstructionDailyTemplate ref="SmConstructionDailyTemplate" :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmConstructionDailyTemplate from 'snweb-module/es/sm-construction/sm-construction-daily-template';
import { mapGetters } from 'vuex';
export default {
  name: 'Contract',
  components: { SmConstructionDailyTemplate },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('daily-template') > -1) {
        this.$refs.SmConstructionDailyTemplate.refresh();
      }
    },
  },
};
</script>
