<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div style="height: 400px; width:800px; border: 1px dashed gray; position: relative;">
    <sm-d3-emerg-faults
      :axios="axios"
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
      visible: true,
      title:'故障应急',
      height: "90%",
      width: "90%",
    }
  },
  created(){
  },
  methods: {
    
    onClose(visible){
      this.visible=visible;
    },
  }
}
</script>
```
