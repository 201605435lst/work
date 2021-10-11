<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-regulation-view-institution
    :axios="axios"
    :institutioId="id"
    />
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'


export default {
  data(){
    return {
      axios,
      id:null,
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
```
