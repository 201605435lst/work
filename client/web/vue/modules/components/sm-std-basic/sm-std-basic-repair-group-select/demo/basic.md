<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-std-basic-repair-group-select placeholder="请选择设备" :isTop="true" :axios="axios" :value="value" @change="selected" />
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      count: 5,
      show: true,
      axios,
      value: null
    }
  },
  created(){
  },
  methods: {
     selected(v){
       console.log(v);
     }
  }
}
</script>
```
