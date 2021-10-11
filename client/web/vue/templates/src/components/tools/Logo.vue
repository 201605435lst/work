<template>
  <div class="logo">
    <router-link :to="{ name: 'index' }">
      <LogoSvg alt="logo" />
      <div v-if="showTitle" class="title">
        <h2>{{ config.appName }}</h2>
        <!-- <h2 v-if="title.split('.')[1]">
          {{ title.split('.')[1] }}
        </h2> -->
      </div>
    </router-link>
  </div>
</template>

<script>
import LogoSvg from '@/assets/logo.svg?inline';
import config from '@/config/default';

export default {
  name: 'Logo',
  components: {
    LogoSvg,
  },
  props: {
    showTitle: {
      type: Boolean,
      default: true,
      required: false,
    },
  },
  data() {
    return {
      config,
    };
  },
};
</script>
