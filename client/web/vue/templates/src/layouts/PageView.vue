<template>
  <div>
    <keep-alive>
      <router-view ref="content" />
    </keep-alive>
    <GlobalFooter />
  </div>
</template>

<script>
import GlobalFooter from '@/components/GlobalFooter';

export default {
  name: 'PageView',
  components: {GlobalFooter},
  props: {},
  data() {
    return {};
  },
  computed: {},
  mounted() {},
  updated() {},
  methods: {},
};
</script>

<style lang="less" scoped>
</style>
