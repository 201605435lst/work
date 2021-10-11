<template>
  <a-card>
    <SmRegulationInstitution
      :axios="axios"
      @audit="onAudit"
      @view="onView"
    />
  </a-card>
</template>
<script>
import SmRegulationInstitution from 'snweb-module/es/sm-regulation_/sm-regulation-institution';
import { mapGetters } from 'vuex';

export default {
  name: 'Institution',
  components: { SmRegulationInstitution },
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onAudit(id) {
      this.$router.push({
        name: 'regulation-institution-audit',
        params: {
          id,
        },
      });
    },
    onView(id) {
      console.log(id);
      this.$router.push({
        name: 'regulation-institution-view',
        params: {
          id,
        },
      });
    },
  },
};
</script>