<template>
  <a-card>
    <SmConstructionDailysApproval
      ref="SmConstructionDailysApproval"
      :axios="axios"
      :permissions="permissions"
      :approval="true"
      @view="onView"
      @approval="onApproval"
    />
  </a-card>
</template>
<script>
import SmConstructionDailysApproval from 'snweb-module/es/sm-construction/sm-construction-dailys';
import { mapGetters } from 'vuex';

export default {
  name: 'DailyApproval',
  components: { SmConstructionDailysApproval },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('construction-dailys-approval') > -1) {
        this.$refs.SmConstructionDailysApproval.refresh();
      }
    },
  },
  methods: {
    onApproval(id) {
      this.$router.push({
        name: 'construction-dailys-approval-approval',
        params: {
          id,
        },
      });
    },
    onView(id) {
      this.$router.push({
        name: 'construction-dailys-view',
        params: {
          id,
        },
      });
    },
  },
};
</script>
