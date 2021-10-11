<template>
  <a-card>
    <SmCmsCategories :axios="axios" :permissions="permissions" />
  </a-card>
</template>

<script>
import SmCmsCategories from 'snweb-module/es/sm-cms/sm-cms-categories';
import { mapGetters } from 'vuex';

export default {
  name: 'Categories',
  components: { SmCmsCategories },
  computed: {
    ...mapGetters(['permissions']),
  },
};
</script>
