<template>
  <a-card>
    <SmTaskTasks
      :axios="axios"
      :permissions="permissions"
      @add="onAdd"
      @edit="onEdit"
      @view="onView"
    />
  </a-card>
</template>
<script>
import SmTaskTasks from 'snweb-module/es/sm-task/sm-task-tasks';
import { mapGetters } from 'vuex';

export default {
  name: 'TaskTasks',
  components: { SmTaskTasks },
  props: [],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onAdd() {
      this.$router.push({
        name: 'task-task-add',
      });
    },
    onEdit(task) {
      this.$router.push({
        name: 'task-task-edit',
        params: {
          id: task.id,
          creatorId: task.creatorId,
          hasParent: task.parentId != null ? true : false,
        },
      });
    },
    onView(task) {
      this.$router.push({
        name: 'task-task-view',
        params: {
          id: task.id,
        },
      });
    },
  },
};
</script>
