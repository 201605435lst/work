<template>
  <a-card>
    <SmCmsCategory
      ref="SmCmsArticle"
      :axios="axios"
      :category-code="categoryCode"
      action="/api/app/cmsArticle/getList"
      :permissions="permissions"
      @preview="onPreview"
      @back="onBack"
    />
  </a-card>
</template>

<script>
import SmCmsCategory from 'snweb-module/es/sm-cms/sm-cms-category';
import { mapGetters } from 'vuex';

export default {
  name: 'Category',
  components: { SmCmsCategory },
  props: ['categoryCode'],

  data() {
    return {};
  },
  computed: {
    ...mapGetters(['permissions']),
  },

  watch: {
    $route: {
      handler: function(value, oldValue) {
        if (value.path.indexOf('dashboard') === -1) {
          this.$refs.SmCmsCategory.refresh();
        }
      },
    },
  },

  methods: {
    onBack() {
      this.$router.back();
    },
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
