<template>
  <a-card>
    <SmEmergFaults
      :axios="axios"
      :permissions="permissions"
      @appendNewFault="onAddNew"
      @appendOldFault="onAddOld"
      @edit="onEdit"
      @view="onView"
      @process="onProcess"
      @applyView="applyView"
    />
  </a-card>
</template>
<script>
import SmEmergFaults from 'snweb-module/es/sm-emerg/sm-emerg-faults';
import { PageState, State } from '../../common/enums';
import { mapGetters } from 'vuex';

export default {
  name: 'EmergFaults',
  components: { SmEmergFaults },
  data() {
    return {
      State,
    };
  },
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onAddNew(append) {
      this.$router.push({
        name: 'emerg-fault-add-new',
        params: {
          append,
        },
      });
    },
    onAddOld(append) {
      this.$router.push({
        name: 'emerg-fault-add-old',
        params: {
          append,
        },
      });
    },
    onEdit(id, state, append) {
      this.$router.push({
        name: 'emerg-fault-edit',
        params: {
          faultId: id,
          state,
          append,
        },
      });
    },
    onView(id,isD3) {
      this.$router.push({
        name: 'emerg-fault-view',
        params: {
          faultId: id,
          isD3:isD3,
        },
      });
    },
    applyView(id) {
      this.$router.push({
        name: 'emerg-plan-view',
        params: {
          id,
          isApply: false,
          faultId: null,
        },
      });
      //window.open(href, '_blank');
    },
    onProcess(isApply, faultId,isD3) {
      this.$router.push({
        name: 'emerg-plan-view-fault',
        params: {
          id: '',
          isApply,
          faultId,
          isD3:isD3,
        },
      });
    },
  },
};
</script>
