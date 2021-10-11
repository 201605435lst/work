<template>
  <a-card>
    <SmCmsCategoryRltArticles :axios="axios" :permissions="permissions" />
  </a-card>
</template>
<script>
import SmCmsCategoryRltArticles from 'snweb-module/es/sm-cms/sm-cms-category-rlt-articles';
import { mapGetters } from 'vuex';

export default {
  name: 'CategoryRltArticles',
  components: { SmCmsCategoryRltArticles },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
