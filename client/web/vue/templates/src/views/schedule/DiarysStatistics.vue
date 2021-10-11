<template>
  <a-card>
    <SmScheduleDiarysStatistics
      :axios="axios"
      :permissions="permissions"
      @cancel="onCancel"
    />
    <!-- logStatistics,fill -->
  </a-card>
</template>
<script>
import SmScheduleDiarysStatistics from 'snweb-module/es/sm-schedule/sm-schedule-diarys-statistics';
import { mapGetters } from 'vuex';

export default {
  name: 'DiarysStatistics',
  components: { SmScheduleDiarysStatistics },
  props: [],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onCancel() {
      this.$router.back();
    },
  },
};
</script>