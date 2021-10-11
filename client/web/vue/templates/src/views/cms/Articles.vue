<template>
  <a-card>
    <SmCmsArticles :axios="axios" :permissions="permissions" @preview="onPreview" />
  </a-card>
</template>
<script>
import SmCmsArticles from 'snweb-module/es/sm-cms/sm-cms-articles';
import { mapGetters } from 'vuex';

export default {
  name: 'Articles',
  components: { SmCmsArticles },
  computed: {
    ...mapGetters(['permissions']),
  },
  methods: {
    onPreview(id) {
      this.$router.push({
        name: 'article-preview',
        params: {
          id,
        },
      });
    },
  },
};
</script>
