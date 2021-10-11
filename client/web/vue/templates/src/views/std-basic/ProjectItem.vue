<template>
  <a-card>
    <SmStdBasicProjectItem :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmStdBasicProjectItem from 'snweb-module/es/sm-std-basic/sm-std-basic-project-item';
import { mapGetters } from 'vuex';
export default {
  name: 'ProjectItem',
  components: { SmStdBasicProjectItem },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
