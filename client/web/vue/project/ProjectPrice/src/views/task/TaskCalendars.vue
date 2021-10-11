<template>
  <a-card>
    <SmTaskCalendars
      :axios="axios"
      :permissions="permissions"
      @add="onAdd"
      @edit="onEdit"
      @view="onView"
    />
  </a-card>
</template>
<script>
import SmTaskCalendars from 'snweb-module/es/sm-task/sm-task-calendars';
import { mapGetters } from 'vuex';

export default {
  name: 'TaskCalendars',
  components: { SmTaskCalendars },
  props: [],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onAdd() {
      this.$router.push({
        name: 'task-Calendar-add',
      });
    },
    onEdit(task) {
      this.$router.push({
        name: 'task-Calendar-edit',
        params: {
          id: task.id,
        },
      });
    },
    onView(task) {
      this.$router.push({
        name: 'task-Calendar-view',
        params: {
          id: task.id,
        },
      });
    },
  },
};
</script>
