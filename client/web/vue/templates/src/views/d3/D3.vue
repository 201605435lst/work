<template>
  <SmD3
    style="height: 100%"
    :sn-earth-project-url="config.snEarthProjectUrl"
    :global-terrain-url="config.globalTerrainUrl"
    :global-imagery-url="config.globalImageryUrl"
    :imagery-urls="config.imageryUrls"
    :axios="axios"
    :scope-code="scope ? scope.scope : null"
    :components="['TeminalLink']"
    :permissions="permissions"
    @view="onView"
    @allView="onAllView"
    @applyView="applyView"
    @process="onProcess"
  />
</template>
<script>
import { mapActions, mapGetters, mapState, mapMutations } from 'vuex';
import { name as storeApp, mt as mtApp, at as atApp } from '@/store/modules/app/types';
import { stringfyScope, parseScope } from 'snweb-module/es/_utils/utils';
import SmD3 from 'snweb-module/es/sm-d3/sm-d3';
import config from '@/config/default';

export default {
  name: 'D3',
  components: { SmD3 },
  data() {
    return {
      config,
      systems: [
        { name: '微机检测系统', code: 'SCC.015.090.008' },
        { name: '道口信号系统', code: 'SCC.015.090.007' },
        { name: '驼峰信号系统', code: 'SCC.015.090.006' },
        { name: '行车调度指挥系统', code: 'SCC.015.090.005' },
        { name: '列车运行控制系统', code: 'SCC.015.090.004' },
        { name: '闭塞系统', code: 'SCC.015.090.003' },
        { name: '车站联锁系统', code: 'SCC.015.090.002' },
        { name: '信号基础设备', code: 'SCC.015.090.001' },
        { name: '信号防灾系统', code: 'SCC.015.090.009' },
        { name: '其他附属系统', code: 'SCC.015.090.099' },
      ],
    };
  },

  computed: {
    ...mapGetters(['permissions']),
    ...mapState(storeApp, ['scope']),
  },
  created() {},
  methods: {
    onView(id) {
      this.$router.push({
        name: 'emerg-fault-view',
        params: {
          faultId: id,
        },
      });
    },
    onAllView() {
      this.$router.push({ name: 'emerg-faults' });
    },
    applyView(id) {
      this.$router.push({
        name: 'emerg-plan-view',
        params: {
          id,
          isApply: false,
          faultId: null,
        },
      });
    },
    onProcess(faultId) {
      console.log(faultId);
      this.$router.push({
        name: 'emerg-plan-view-fault',
        params: {
          id: '',
          isApply: true,
          faultId,
        },
      });
    },
  },
};
</script>
