<template>
  <a-card>
    <SmConstructionDispatchApproval
      ref="SmConstructionDispatchApproval"
      :axios="axios"
      :permissions="permissions"
      :bordered="false"
      :approval="true"
      @view="onView"
      @process="onProcess"
    />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionDispatchApproval from 'snweb-module/es/sm-construction/sm-construction-dispatchs';
export default {
  name: 'DispatchApproval',
  components: { SmConstructionDispatchApproval },
  props: [],
  data() {
    return {};
  },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('dispatchs-approval') > -1) {
        this.$refs.SmConstructionDispatchApproval.refresh();
      }
    },
  },
  methods: {
    onView(id) {
      this.$router.push({
        name: 'dispatch-view',
        params: {
          id,
        },
      });
    },
    onProcess(id) {
      this.$router.push({
        name: 'dispatch-approval',
        params: {
          id,
        },
      });
    },
  },
};
</script>

<style scoped></style>
