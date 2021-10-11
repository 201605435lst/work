<template>
  <a-card>
    <SmMaterialSupplier
      :id="id"
      ref="SmMaterialSupplier"
      :axios="axios"
      :page-state="pageState"
      @ok="onOk"
      @cancel="onCancel"
    />
  </a-card>
</template>
<script>
import SmMaterialSupplier from 'snweb-module/es/sm-material/sm-material-supplier';
import { mapGetters } from 'vuex';
import { PageState } from '../../common/enums';

export default {
  name: 'Supplier',
  components: { SmMaterialSupplier },
  props: ['pageState', 'id'],
  data() {
    return {};
  },
  computed: {
    ...mapGetters(['permissions']),
  },
  watch: {
    $route: {
      handler: function (value, oldValue) {
        if (value.path.indexOf('material-suppliers') === -1 && this.pageState !== PageState.Add) {
          this.$refs.SmMaterialSupplier.refresh(this.id);
        }
      },
    },
  },
  created() {},
  methods: {
    onOk() {
      this.$router.back();
    },

    onCancel() {
      this.$router.back();
    },
  },
};
</script>
