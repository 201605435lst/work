<template>
  <a-card>
    <SmConstructionDaily
      :id="id"
      ref="SmConstructionDaily"
      :axios="axios"
      :page-state="pageState"
      :permissions="permissions"
      @ok="onOk"
      @cancel="onCancel"
    />
  </a-card>
</template>
<script>
import SmConstructionDaily from 'snweb-module/es/sm-construction/sm-construction-daily';

import { mapGetters } from 'vuex';

export default {
  name: 'Daily',
  components: { SmConstructionDaily },
  props: ['pageState', 'id', 'approval'],
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('construction-dailys') > -1) {
        this.$refs.SmConstructionDaily.refresh();
      }
    },
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
