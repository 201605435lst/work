<template>
  <a-card>
    <SmStdBasicQuotaCategory :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmStdBasicQuotaCategory from 'snweb-module/es/sm-std-basic/sm-std-basic-quota-category';
import { mapGetters } from 'vuex';
export default {
  name: 'QuotaCategory',
  components: { SmStdBasicQuotaCategory },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
