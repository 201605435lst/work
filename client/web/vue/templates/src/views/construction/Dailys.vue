<template>
  <a-card>
    <SmConstructionDailys
      ref="SmConstructionDailys"
      :axios="axios"
      :permissions="permissions"
      @add="onAdd"
      @view="onView"
      @edit="onEdit"
    />
  </a-card>
</template>
<script>
import SmConstructionDailys from 'snweb-module/es/sm-construction/sm-construction-dailys';
import { mapGetters } from 'vuex';

export default {
  name: 'Dailys',
  components: { SmConstructionDailys },
  computed: {
    ...mapGetters(['permissions']),
  },
    watch: {
    $route: function() {
      if (this.$route.path.indexOf('construction-dailys') > -1) {
        this.$refs.SmConstructionDailys.refresh();
      }
    },
  },
  methods: {
    onAdd() {
      this.$router.push({
        name: 'construction-dailys-add',
      });
    },
    onEdit(id) {
      this.$router.push({
        name: 'construction-dailys-edit',
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
