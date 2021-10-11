<template>
  <a-card>
    <SmBpmWorkflowTemplates
      ref="sm-bpm-workflow-templates"
      :axios="axios"
      :for-current-user="forCurrentUser"
      :permissions="permissions"
      @view="onView"
      @edit="onEdit"
      @success="onSuccess"
    />
  </a-card>
</template>

<script>
import SmBpmWorkflowTemplates from 'snweb-module/es/sm-bpm/sm-bpm-workflow-templates';
import { PageState } from '@/common/enums';
import { mapGetters } from 'vuex';

export default {
  name: 'Workflows',
  components: { SmBpmWorkflowTemplates },
  props: ['forCurrentUser'],
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      this.$nextTick(() => {
        this.$refs['sm-bpm-workflow-templates'].refresh();
      });
    },
  },
  methods: {
    onView(id) {
      console.log('onView', id);
      // this.$message.info(`onView: ${id}`);
      this.$router.push({
        name: 'workflow-template-view',
        params: {
          id,
        },
      });
    },
    onEdit(id) {
      console.log('onEdit', id);
      // this.$message.info(`onEdit: ${id}`);
      this.$router.push({
        name: 'workflow-template-edit',
        params: {
          id,
        },
      });
    },
    onSuccess(isInitial) {
      if (isInitial) {
        this.$router.push({
          name: 'workflows-initial',
          params: {},
        });
      }
    },
  },
};
</script>
