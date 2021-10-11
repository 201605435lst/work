<cn>
#### 多选模式
</cn>

<us>
#### 多选模式
</us>

```tpl
<template>
  <div>
    <sm-construction-base-standard-select :axios="axios" :multiple="true" :value="value"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      value:['39fdd5d9-7c57-f773-dc58-6aa84ac99790','39fdd5d9-bfaf-3154-022d-b70de5d51327']
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
