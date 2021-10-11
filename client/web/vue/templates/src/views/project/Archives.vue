<template>
  <a-card>
    <SmProjectArchives ref="SmProjectArchives" :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmProjectArchives from 'snweb-module/es/sm-project/sm-project-archives';
import { mapGetters } from 'vuex';
export default {
  name: 'Archives',
  components: { SmProjectArchives },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('archives') > -1) {
        this.$refs.SmProjectArchives.refresh();
      }
    },
  },
};
</script>
