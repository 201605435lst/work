<template>
  <a-card>
    <SmOaDutySchedule :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmOaDutySchedule from 'snweb-module/es/sm-oa/sm-oa-duty-schedule';
import { mapGetters } from 'vuex';

export default {
  name: 'DutySchedule',
  components: { SmOaDutySchedule },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
