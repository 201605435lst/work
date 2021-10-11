<template>
  <a-card>
    <SmOaContracts
      :axios="axios"
      :permissions="permissions"
      @add="onAdd"
      @view="onView"
      @edit="onEdit"
    />
  </a-card>
</template>
<script>
import SmOaContracts from 'snweb-module/es/sm-oa/sm-oa-contracts';
import { mapGetters } from 'vuex';

export default {
  name: 'Contracts',
  components: { SmOaContracts },
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onAdd() {
      this.$router.push({
        name: 'oa-contract-add',
      });
    },
    onEdit(id) {
      this.$router.push({
        name: 'oa-contract-edit',
        params: {
          id,
        },
      });
    },
    onView(id) {
      this.$router.push({
        name: 'oa-contract-view',
        params: {
          id,
        },
      });
    },
  },
};
</script>