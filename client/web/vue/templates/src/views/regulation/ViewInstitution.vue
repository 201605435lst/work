<template>
  <a-card>
    <SmRegulationViewInstitution
      :institution-id="id"
      :axios="axios"
      :page-state="pageState"
      @success="onSuccess"
      @cancel="onCancel"
    />
  </a-card>
</template>
<script>
import SmRegulationViewInstitution from 'snweb-module/es/sm-regulation_/sm-regulation-view-institution';
import { mapGetters } from 'vuex';

export default {
  name: 'ViewInstitution',
  components: { SmRegulationViewInstitution },
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
