<template>
  <div id="userLayout" :class="['user-layout-wrapper', device]">
    <div class="container">
      <div class="top">
        <div class="header">
          <a href="/">
            <img src="~@/assets/logo.svg" class="logo" alt="logo">
            <span class="title">{{ config.appName }}</span>
          </a>
        </div>
        <!-- <div class="desc">
          {{ title.split('.')[1] || '' }}
        </div> -->
      </div>

      <route-view />

      <div class="footer">
        <div class="links">
          <a href="_self">帮助</a>
          <a href="_self">隐私</a>
          <a href="_self">条款</a>
        </div>
        <div class="copyright">
          {{ config.copyRight }}
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import RouteView from './RouteView';
import { mixinDevice } from '@/utils/mixin';
import config from '@/config/default';

export default {
  name: 'UserLayout',
  components: { RouteView },
  mixins: [mixinDevice],
  data() {
    return {
      config,
    };
  },
  mounted() {
    document.body.classList.add('userLayout');
  },
  beforeDestroy() {
    document.body.classList.remove('userLayout');
  },
};
</script>

<style lang="less" scoped>
#userLayout.user-layout-wrapper {
  height: 100%;

  &.mobile {
    .container {
      .main {
        max-width: 368px;
        width: 98%;
      }
    }
  }

  .container {
    width: 100%;
    min-height: 100%;
    background: #1e3e6f url(~@/assets/background.svg) no-repeat 50%;
    background-size: 100%;
    // padding: 110px 0 144px;
    position: relative;
    display: flex;
    flex-direction: column;
    justify-content: center;
    padding-bottom: 50px;

    a {
      text-decoration: none;
    }

    .top {
      text-align: center;
      color: #fbfbfb;
      margin-bottom: 40px;

      .header {
        height: 44px;
        line-height: 44px;

        .badge {
          position: absolute;
          display: inline-block;
          line-height: 1;
          vertical-align: middle;
          margin-left: -12px;
          margin-top: -10px;
          opacity: 0.8;
        }

        .logo {
          height: 44px;
          vertical-align: top;
          margin-right: 16px;
          border-style: none;
        }

        .title {
          font-size: 33px;
          color: #fbfbfb;
          font-family: Avenir, 'Helvetica Neue', Arial, Helvetica, sans-serif;
          font-weight: 600;
          position: relative;
          top: 2px;
        }
      }
      .desc {
        font-size: 14px;
        color: rgba(0, 0, 0, 0.45);
        margin-top: 12px;
        margin-bottom: 40px;
      }
    }

    .main {
      min-width: 260px;
      width: 405px;
      margin: 0 auto;
      padding: 13px 35px;
      background: #ffffff14;
      border-radius: 11px;
      box-shadow: #2727270a 4px 4px 2px 3px;
    }

    .footer {
      position: absolute;
      width: 100%;
      bottom: 0;
      padding: 0 16px;
      margin: 62px 0 54px;
      text-align: center;

      .links {
        margin-bottom: 8px;
        font-size: 14px;
        a {
          color: rgba(255, 255, 255, 0.678);
          transition: all 0.3s;
          &:not(:last-child) {
            margin-right: 40px;
          }
        }
      }
      .copyright {
        color: #eee;
        font-size: 14px;
      }
    }
  }
}
</style>
