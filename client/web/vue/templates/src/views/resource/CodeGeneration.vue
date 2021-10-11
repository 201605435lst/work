<template>
  <a-card>
    <SmResourceCodeGeneration :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmResourceCodeGeneration from 'snweb-module/es/sm-resource/sm-resource-code-generation';
import { mapGetters } from 'vuex';
export default {
  name: 'CodeGeneration',
  components: { SmResourceCodeGeneration },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
