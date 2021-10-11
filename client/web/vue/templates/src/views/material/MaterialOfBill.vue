<template>
  <a-card>
    <SmMaterialMaterialOfBill
      ref="SmMaterialMaterialOfBill"
      :axios="axios"
      :permissions="permissions"
    />
  </a-card>
</template>
<script>
import SmMaterialMaterialOfBill from 'snweb-module/es/sm-material/sm-material-material-ofbill';
import { mapGetters } from 'vuex';

export default {
  name: 'MaterialOfBill',
  components: { SmMaterialMaterialOfBill },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: function() {
      if (this.$route.path.indexOf('material-ofbill') > -1) {
        this.$refs.SmMaterialMaterialOfBill.refresh();
      }
    },
  },
  methods: {
  },
};
</script>
