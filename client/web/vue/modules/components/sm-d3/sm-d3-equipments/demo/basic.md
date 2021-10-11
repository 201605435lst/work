<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div style="height: 600px; width:300px;">
      范围选择
    <a-select  style="width: 200px" v-model="scopeCode">
      <a-select-option v-for="item in scopeCodes" :value="item" :key="item">
        {{parseScope(item).name}}
      </a-select-option>
    </a-select>

    <a-button @click="onClick1">选择组1</a-button>
    <a-button @click="onClick2">选择组2</a-button>
    <a-button @click="onClick3">清空</a-button>

    <br/>
    <br/>
    <div
      style="
        position: relative;
        height: 100%;
      "
      >
      <sm-d3-equipments
        :axios="axios"
        v-model="equipments"
        :visible="true"
        @close="onClose"
        :scopeCode="scopeCode"
        :isShowLayer="true"
      />
    </div>
  </div>
</template>
<script>
import axios from '@/utils/axios.js'
import { parseScope } from '@/components/_utils/utils.js';

export default {
  data(){
    return {
      axios,
      visible: false,
      title:'设备列表',
      equipments: [],
      scopeCodes:[
        '1@青岛地铁运维公司@39f9441d-23aa-2c32-9cdb-6065f50efc1e.2@11号线@f21a878c-8298-45ac-80ca-8834ab8ba132.3@鳌山湾站@39f96ce6-2dea-8797-dbb9-9f2aceac77e5',
        '1@青岛地铁运维公司@39f9441d-23aa-2c32-9cdb-6065f50efc1e.2@11号线@f21a878c-8298-45ac-80ca-8834ab8ba132.3@鳌山湾站@39f96ce6-2dea-8797-dbb9-9f2aceac77e5.4@上行东公共区2@0710c3aa-f594-423a-9781-46581f6f6d5b'
      ],
      scopeCode: '1@青岛地铁运维公司@39f9441d-23aa-2c32-9cdb-6065f50efc1e.2@11号线@f21a878c-8298-45ac-80ca-8834ab8ba132.3@鳌山湾站@39f96ce6-2dea-8797-dbb9-9f2aceac77e5',
    }
  },
  created(){
  },
  methods: {
    parseScope(code){
      return parseScope(code)
    },
    onClick1(){
      this.visible = true;
      this.equipments= [
        '室内@F1`0`1',
        '室内@LJJC1`0`6'
      ];
    },

    onClick2(){
      this.visible = true;
       this.equipments= [
        '室外@GH1',
        '室外@DYH3'
      ];
    },

     onClick3(){
      this.visible = true;
       this.equipments= [

      ];
    },


    onClose(visible){
      this.visible=visible;
    },
  }
}
</script>
```
