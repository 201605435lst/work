<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div style="height: 800px; width:800px; border: 1px dashed gray; position: relative;">
    <a-button @click="onClick">点击弹出故障应急面板</a-button>
    <sm-d3-equipment-info
      :axios="axios"
      v-model="value"
      :visible="visible"
      @close="onClose"
      :title="title"
      :bordered="true"
      :width="width"
    />
  </div>
</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      visible: false,
      title:'设备信息',
      height: "90%",
      width: "30%",
      value: 'b30cdf42-11ac-46ad-97a2-19fd825224e6',
    }
  },
  created(){
  },
  methods: {
    onClick(){
      this.visible = !this.visible;
    },

    onClose(visible){
      this.visible=visible;
    },
  }
}
</script>
```
