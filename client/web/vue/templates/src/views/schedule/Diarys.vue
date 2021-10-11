<template>
  <a-card>
    <SmScheduleDiarys
      :axios="axios"
      :permissions="permissions"
      @fill="onFill"
      @logStatistics="onLogStatistics"
      @edit="onEdit"
    />
    <!-- logStatistics,fill -->
  </a-card>
</template>
<script>
import SmScheduleDiarys from 'snweb-module/es/sm-schedule/sm-schedule-diarys';
import { mapGetters } from 'vuex';

export default {
  name: 'Diarys',
  components: { SmScheduleDiarys },
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onFill(id,record) {
      this.$router.push({
        name: 'schedule-diary-fill',
        params: {
          id,
        },
      });
    },
    onEdit(id) {
      this.$router.push({
        name: 'schedule-diary-edit',
        params: {
          id,
        },
      });
    },
    onLogStatistics() {
      this.$router.push({
        name: 'schedule-diary-logStatistics',
      });
    },
  },
};
</script>