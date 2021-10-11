<cn>
#### 多选模式
</cn>

<us>
#### 多选模式
</us>

```tpl
<template>
  <div>
    <sm-construction-base-section-select :axios="axios" :multiple="true" v-model="value" :value="value"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      value:['39fd9b91-1b9f-5d6e-3c5a-c3391a6a267b','39fdd8a8-b900-3fd5-59c5-5865842d7c54']
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
***
```
