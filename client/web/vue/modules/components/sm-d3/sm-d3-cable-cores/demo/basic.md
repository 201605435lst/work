<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div style="height: 600px; width:250px; border: 1px dashed gray; position: relative;">
    <sm-d3-cable-cores :axios="axios" v-model="value" />
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      value: [{equipmentName:"F1",installationSiteCodeName: "机械室1"}],
      visible:false
    }
  },
  created(){
  },
  methods: {
    onClick(){
      this.visible = true;
    },

    onClose(visible){
      this.visible = visible;
    }

  }
}
</script>
```
