<template>
  <a-card>
    <SmEmergPlans
      :axios="axios"
      :is-apply="isApply"
      :permissions="permissions"
      @add="onAdd"
      @edit="onEdit"
      @view="onView"
    />
  </a-card>
</template>
<script>
import SmEmergPlans from 'snweb-module/es/sm-emerg/sm-emerg-plans';
import { mapGetters } from 'vuex';

export default {
  name: 'EmergPlans',
  components: { SmEmergPlans },
  props: ['isApply'],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onAdd() {
      this.$router.push({
        name: 'emerg-plan-add',
      });
    },
    onEdit(id) {
      this.$router.push({
        name: 'emerg-plan-edit',
        params: {
          id,
        },
      });
    },
    onView(id) {
      this.$router.push({
        name: 'emerg-plan-view',
        params: {
          id,
          isApply: false,
          faultId: null,
        },
      });
    },
  },
};
</script>
