<template>
  <a-card>
    <SmFileOss :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmFileOss from 'snweb-module/es/sm-file/sm-file-oss';
import { mapGetters } from 'vuex';
export default {
  name: 'OssConfig',
  components: { SmFileOss },
  data() {
    return {};
  },
  computed: {
    ...mapGetters(['permissions']),
  },
  created() {},
  methods: {},
};
</script>
