<template>
  <a-card>
    <SmStdBasicWorkAttention 
      ref="SmStdBasicWorkAttention" 
      :repair-tag-key="repairTagKey"
      :permissions="permissions"
      :axios="axios"
    />
  </a-card>
</template>
<script>
import SmStdBasicWorkAttention from 'snweb-module/es/sm-std-basic/sm-std-basic-work-attention/SmStdBasicWorkAttention';
import { mapGetters } from 'vuex';
export default {
  name: 'WorkAttention',
  components: { SmStdBasicWorkAttention },
  props: ['repairTagKey'],
  computed: {
    ...mapGetters(['permissions']),
  },
    watch: {
    $route: function() {
      if (this.$route.path.indexOf('work-attention') > -1) {
        this.$nextTick(() => {
          this.$refs.SmStdBasicWorkAttention.refresh();
        });
      }
    },
  },
};
</script>
