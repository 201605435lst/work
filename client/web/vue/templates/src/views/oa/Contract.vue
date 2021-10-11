<template>
  <a-card>
    <SmOaContract
      :id="id"
      :axios="axios"
      :page-state="pageState"
      :permissions="permissions"
      @ok="onOk"
      @cancel="onCancel"
    />
    <!-- @edit="onEdit" -->
  </a-card>
</template>
<script>
import SmOaContract from 'snweb-module/es/sm-oa/sm-oa-contract';
import { mapGetters } from 'vuex';

export default {
  name: 'Contract',
  components: { SmOaContract },
  props: ['pageState', 'id'],
  computed: {
    ...mapGetters(['permissions']),
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
