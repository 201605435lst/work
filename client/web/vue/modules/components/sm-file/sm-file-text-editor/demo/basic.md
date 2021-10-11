<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <a-button @click="onClick">是否禁用</a-button>
    <sm-file-text-editor :axios="axios" :value="value" :disabled="disabled"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      value:null,
      disabled:false,
    }
  },
  created(){
  },
  methods: {
    onClick(){
      this.disabled=!this.disabled;
    }
  }
}
</script>
```
