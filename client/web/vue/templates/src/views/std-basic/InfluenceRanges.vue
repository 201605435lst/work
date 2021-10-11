<template>
  <a-card>
    <SmStdBasicInfluenceRanges
      ref="SmStdBasicInfluenceRanges"
      :axios="axios"
      :permissions="permissions"
      :repair-tag-key="repairTagKey"
    />
  </a-card>
</template>
<script>
import SmStdBasicInfluenceRanges from 'snweb-module/es/sm-std-basic/sm-std-basic-influence-ranges';
import { mapGetters } from 'vuex';
export default {
  name: 'InfluenceRanges',
  components: { SmStdBasicInfluenceRanges },
  props: ['repairTagKey'],
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('influenceranges') > -1) {
        this.$nextTick(() => {
          this.$refs.SmStdBasicInfluenceRanges.refresh();
        });
      }
    },
  },
};
</script>
