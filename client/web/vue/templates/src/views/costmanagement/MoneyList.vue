<template>
  <a-card>
    <SmCostmanagementMoneyList :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmCostmanagementMoneyList from 'snweb-module/es/sm-costmanagement/sm-costmanagement-money-list';
import { mapGetters } from 'vuex';
export default {
  name: 'MoneyList',
  components: { SmCostmanagementMoneyList },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
