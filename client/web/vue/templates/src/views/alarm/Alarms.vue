<template>
  <a-card>
    <SmAlarms :axios="axios" :permissions="permissions" :systems="systems" />
  </a-card>
</template>
<script>
import SmAlarms from 'snweb-module/es/sm-alarm/sm-alarms';
import { mapGetters } from 'vuex';

export default {
  name: 'Alarms',
  components: { SmAlarms },
  props: [],
  data() {
    return {
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
  },
  methods: {},
};
</script>
