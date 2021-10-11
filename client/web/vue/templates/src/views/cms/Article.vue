<template>
  <a-card>
    <SmCmsArticle :id="id" ref="SmCmsArticle" :axios="axios" action="/api/app/cmsArticle/get" :permissions="permissions" @back="onBack" />
  </a-card>
</template>

<script>
import SmCmsArticle from 'snweb-module/es/sm-cms/sm-cms-article';
import { mapGetters } from 'vuex';

export default {
  name: 'Atricle',
  components: { SmCmsArticle },
  props: ['id'],

  data() {
    return {};
  },
  computed: {
    ...mapGetters(['permissions']),
  },

  watch: {
    $route: {
      handler: function(value, oldValue) {
        if (value.path.indexOf('articles') === -1) {
          this.$refs.SmCmsArticle.refresh(this.id);
        }
      },
    },
  },

  methods: {
    onBack() {
      this.$router.back();
    },
  },
};
</script>
