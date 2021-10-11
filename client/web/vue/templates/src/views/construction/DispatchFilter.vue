<template>
  <a-card>
    <SmConstructionDispatchFilter
      ref="SmConstructionDispatchFilter"
      :permissions="permissions"
      :axios="axios"
      :show-select-row="false"
      :bordered="false"
      :passed="true"
      @view="onView"
    />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionDispatchFilter from 'snweb-module/es/sm-construction/sm-construction-dispatchs';
export default {
  name: 'DispatchFilter',
  components: { SmConstructionDispatchFilter },
  props: [],
  data() {
    return {};
  },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('dispatch-filter') > -1) {
        this.$refs.SmConstructionDispatchFilter.refresh();
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
  },
};
</script>

<style scoped></style>
