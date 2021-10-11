<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <div style="
        height: 900px;
        display: flex;
        /* position: fixed;
        z-index: 100000000;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0; */
    ">
      <sm-d3
        :snEarthProjectUrl="snEarthProjectUrl"
        :axios="axios"
        :signalr="signalr"
        :permissions="getPermissions()"
        :components="['TeminalLink','CableCores','Alarms']"
        globalTerrainUrl="//172.16.1.12:8165/terrain/World"
        globalImageryUrl="//172.16.1.12:8165/imagery/World"
        :imageryUrls="[]"
        :systems="systems"
      />
    </div>
  </div>
</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'
import { parseScope } from '@/components/_utils/utils.js';
import signalr from '@/utils/signalr.js'

export default {
  data(){
    return {
      axios,
      signalr,
      snEarthProjectUrl:window.snEarthProjectUrl,
      systems:[
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
    console.log(signalr)
    // this.snEarthProjectUrl = window.snEarthProjectUrl
  },
  methods: {
    getPermissions,
    onChange(item){
      console.log(item)
    }
  }
}
</script>
```
