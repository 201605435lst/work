<template>
  <a-card>
    <SmConstructionTemplates 
      ref="SmConstructionTemplates"
      :axios="axios" 
      :bordered="false" 
      :permissions="permissions"
    />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionTemplates from 'snweb-module/es/sm-construction/sm-construction-dispatch-template';
export default {
  name: 'DispatchTemplates',
  components: {SmConstructionTemplates},
  props: [],
  data() {
    return {};
  },
  computed: {
    ...mapGetters(['permissions']),
  },
  methods:{},
};
</script>

<style scoped>

</style>