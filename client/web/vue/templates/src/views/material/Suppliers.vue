<template>
  <a-card>
    <SmMaterialSuppliers
      ref="SmMaterialSuppliers"
      :axios="axios"
      :permissions="permissions"
      @add="onAdd"
      @edit="onEdit"
      @view="onView"
    />
  </a-card>
</template>
<script>
import SmMaterialSuppliers from 'snweb-module/es/sm-material/sm-material-suppliers';
import { mapGetters } from 'vuex';

export default {
  name: 'Suppliers',
  components: { SmMaterialSuppliers },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('material-suppliers') > -1) {
        this.$refs.SmMaterialSuppliers.refresh();
      }
    },
  },
  methods: {
    onAdd() {
      this.$router.push({
        name: 'material-supplier-add',
      });
    },
    onEdit(id) {
      this.$router.push({
        name: 'material-supplier-edit',
        params: {
          id,
        },
      });
    },
    onView(id) {
      this.$router.push({
        name: 'material-supplier-view',
        params: {
          id,
        },
      });
    },
  },
};
</script>
