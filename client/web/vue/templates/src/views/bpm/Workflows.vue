<template>
  <a-card>
    <SmBpmWorkflows
      ref="sm-bpm-workflows"
      :axios="axios"
      :group="group"
      :permissions="permissions"
      @view="onView"
      @edit="onEdit"
    />
  </a-card>
</template>

<script>
import SmBpmWorkflows from 'snweb-module/es/sm-bpm/sm-bpm-workflows';
import { PageState } from '@/common/enums';
import { mapGetters } from 'vuex';

export default {
  name: 'Workflows',
  components: { SmBpmWorkflows },
  props: ['group'],
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      this.$nextTick(() => {
        this.$refs['sm-bpm-workflows'].refresh();
      });
    },
  },

  methods: {
    onView(id) {
      this.$router.push({
        name: 'workflow-view',
        params: {
          id,
        },
      });
    },
    onEdit(id) {
      this.$router.push({
        name: 'workflow-edit',
        params: {
          id,
        },
      });
    },
  },
};
</script>
