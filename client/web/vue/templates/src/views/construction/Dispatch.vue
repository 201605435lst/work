<template>
  <a-card>
    <SmConstructionDispatch
      :id="id"
      ref="SmConstructionDispatch"
      :page-state="pageState"
      :axios="axios"
      :permissions="permissions"
      :is-approve="isApprove"
      @ok="onOk"
      @cancel="onCancel"
    />
  </a-card>
</template>

<script>
import SmConstructionDispatch from 'snweb-module/es/sm-construction/sm-construction-dispatch';
import { mapGetters } from 'vuex';
import { PageState } from '../../common/enums';

export default {
  name: 'Dispatch',
  components: { SmConstructionDispatch },
  props: ['pageState', 'id','isApprove'],

  data() {
    return {};
  },
  computed: {
    ...mapGetters(['permissions']),
  },

  watch: {
    $route: {
      handler: function(value, oldValue) {
        if (value.path.indexOf('construction-dispatchs') === -1 && this.pageState !== PageState.Add) {
          this.$refs.SmConstructionDispatch.refresh(this.id);
        }
      },
    },
  },

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
