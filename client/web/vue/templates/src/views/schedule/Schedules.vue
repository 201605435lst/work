<template>
  <a-card>
    <SmScheduleSchedules
      ref="SmScheduleSchedules"
      :axios="axios"
      :permissions="permissions"
      @add="onAdd"
      @addChild="onAddChild"
      @edit="onEdit"
      @view="onView"
    />
  </a-card>
</template>
<script>
import SmScheduleSchedules from 'snweb-module/es/sm-schedule/sm-schedule-schedules';
import { mapGetters } from 'vuex';

export default {
  name: 'Schedules',
  components: { SmScheduleSchedules },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('schedule-schedules') > -1) {
        this.$refs.SmScheduleSchedules.refresh();
      }
    },
  },
  methods: {
    onAdd() {
      this.$router.push({
        name: 'schedule-schedule-add',
        params: {
          id:null,
        },
      });
    },
    onAddChild(record){
      this.$router.push({
        name: 'schedule-schedule-add',
        params: {
          id:record.id,
        },
      });
    },
    onEdit(record) {
      this.$router.push({
        name: 'schedule-schedule-edit',
        params: {
          id:record.id,
        },
      });
    },
    onView(record) {
      this.$router.push({
        name: 'schedule-schedule-view',
        params: {
          id:record.id,
        },
      });
    },
  },
};
</script>
