<cn>
#### 生成工程量
</cn>

<us>
#### 生成工程量
</us>

```tpl
<template>
  <div>
    <a-button :axios="axios" @click="generate">生成工程量{{}}</a-button>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'

export default {
  data(){
    return {
      axios,
      percent:0
    }
  },
  created(){
  },
  methods: {
    getPermissions,
    generate(){

    }
  }
}
</script>
```
