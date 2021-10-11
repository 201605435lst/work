<template>
  <a-card>
    <SmScheduleDiary
      :id="id"
      :axios="axios"
      :page-state="pageState"
      :permissions="permissions"
      @ok="onOk"
      @cancel="onCancel"
    />
  </a-card>
</template>
<script>
import SmScheduleDiary from 'snweb-module/es/sm-schedule/sm-schedule-diary';

import { mapGetters } from 'vuex';

export default {
  name: 'Diary',
  components: { SmScheduleDiary },
  props: ['pageState', 'id'],
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
