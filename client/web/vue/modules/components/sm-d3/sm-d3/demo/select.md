<cn>
#### 构件选择模式
</cn>

<us>
#### 构件选择模式
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
        :axios="axios"
        :signalr="signalr"
        :snEarthProjectUrl="snEarthProjectUrl"
        globalTerrainUrl="//172.16.1.12:8165/terrain/World"
        globalImageryUrl="//172.16.1.12:8165/imagery/World"
        select
        :selectedEquipments="[
          {id: '39fcdd0b-1c74-8f24-d466-aa01c9e213a5', name: '桥架配件_1',  groupName: '7号线/窑上村站/DongZhao'},
          {id: '39fcdd0b-1ded-f533-b975-ea98ee4604e8', name: '桥架配件_10', groupName: '7号线/窑上村站/DongZhao'}
        ]"
        @selectedEquipmentsChange="selectedEquipmentsChange"
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
    },
    selectedEquipmentsChange(equipments){
      console.log(equipments)
    }
  }
}
</script>
```
