<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <!-- <sm-namespace-module :axios="axios" :signalr="signalr"/> -->
    <sm-alarms :axios="axios" :systems="systems" :signalr="signalr"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import signalr from '@/utils/signalr.js'

export default {
  data(){
    return {
      axios,
      signalr,
      systems: [
        { name: '无线', code: 'SCC.DT.TX.01' },
        { name: '传输', code: 'SCC.DT.TX.02' },
        { name: '电视监控', code: 'SCC.DT.TX.03' },
        { name: '广播', code: 'SCC.DT.TX.04' },
        { name: '时钟', code: 'SCC.DT.TX.05' },
        { name: '公务电话', code: 'SCC.DT.TX.06' },
        { name: '专用电话', code: 'SCC.DT.TX.07' },
        { name: '集中告警', code: 'SCC.DT.TX.08' },
        { name: '综合UPS', code: 'SCC.DT.TX.09' },
        { name: '公众传输', code: 'SCC.DT.TX.10' },
        { name: '公众无线', code: 'SCC.DT.TX.11' },
        { name: '公众通信电源', code: 'SCC.DT.TX.12' },
        { name: '乘客信息', code: 'SCC.DT.TX.13' },
        { name: '光、电缆', code: 'SCC.DT.TX.14' },
      ]
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
```
