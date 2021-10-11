<template>
  <a-card>
    <SmConstructionDispatchs 
      ref="SmConstructionDispatchs"
      :permissions="permissions"
      :axios="axios" 
      :bordered="false" 
      @view="onView"
      @edit="onEdit"
      @add="onAdd" 
    />
  </a-card>
</template>

<script>
import { mapGetters } from 'vuex';
import SmConstructionDispatchs from 'snweb-module/es/sm-construction/sm-construction-dispatchs';
export default {
  name: 'Dispatchs',
  components: {SmConstructionDispatchs},
  props: [],
  data() {
    return {};
  },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('dispatchs') > -1) {
        this.$refs.SmConstructionDispatchs.refresh();
      }
    },
  },
  methods:{
    onAdd() {
      this.$router.push({
        name: 'dispatch-add',
      });
    },
    onView(id) {
      this.$router.push({
        name: 'dispatch-view',
        params: {
          id,
        },
      });
    },
    onEdit(id) {
      this.$router.push({
        name: 'dispatch-edit',
        params: {
          id,
        },
      });
    },
  },
};
</script>

<style scoped>

</style>