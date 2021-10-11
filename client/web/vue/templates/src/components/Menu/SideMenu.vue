<template>
  <a-layout-sider
    v-model="collapsed"
    :class="['sider', isDesktop() ? null : 'shadow', theme, fixedSidebar ? 'ant-fixed-sidemenu' : null]"
    width="256px"
    :collapsible="collapsible"
    :trigger="null"
  >
    <logo :show-title="!collapsed" />
    <s-menu
      :collapsed="collapsed"
      :menu="menus"
      :theme="theme"
      :mode="mode"
      style="
        padding: 16px 0px;
        overflow: auto;
        height: calc(100% - 64px);
      "
      @select="onSelect"
    />
  </a-layout-sider>
</template>

<script>
import Logo from '@/components/tools/Logo';
import SMenu from './index';
import { mixin, mixinDevice } from '@/utils/mixin';

export default {
  name: 'SideMenu',
  components: { Logo, SMenu },
  mixins: [mixin, mixinDevice],
  props: {
    mode: {
      type: String,
      required: false,
      default: 'inline',
    },
    theme: {
      type: String,
      required: false,
      default: 'dark',
    },
    collapsible: {
      type: Boolean,
      required: false,
      default: false,
    },
    collapsed: {
      type: Boolean,
      required: false,
      default: false,
    },
    menus: {
      type: Array,
      required: true,
    },
  },
  methods: {
    onSelect(obj) {
      this.$emit('menuSelect', obj);
    },
  },
};
</script>

<style lang="less" scoped>

</style>
