<template>
  <a-card>
    <SmRegulationAuditedInstitution
      :institution-id="id"
      :axios="axios"
      :page-state="pageState"
      @success="onSuccess"
      @cancel="onCancel"
    />
  </a-card>
</template>
<script>
import SmRegulationAuditedInstitution from 'snweb-module/es/sm-regulation_/sm-regulation-audited-institution';
import { mapGetters } from 'vuex';

export default {
  name: 'AuditedInstitution',
  components: { SmRegulationAuditedInstitution },
  props: ['pageState', 'id'],
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onSuccess() {
      this.$router.back();
    },
    onCancel() {
      this.$router.back();
    },
  },
};
</script>
