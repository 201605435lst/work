<template>
  <a-card>
    <SmTaskTask
      :id="id"
      :axios="axios"
      :page-state="pageState"
      :permissions="permissions"
      :creator-id="creatorId"
      :has-parent="hasParent"
      @cancel="onCancel"
      @ok="onOk"
    />
  </a-card>
</template>
<script>
import SmTaskTask from 'snweb-module/es/sm-task/sm-task-task';
import { mapGetters } from 'vuex';

export default {
  name: 'TaskTask',
  components: { SmTaskTask },
  props: ['pageState', 'id', 'creatorId', 'hasParent'],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onOk() {
      this.$router.back();
    },
    onCancel() {
      this.$router.back();
    },
  },
};
</script>
