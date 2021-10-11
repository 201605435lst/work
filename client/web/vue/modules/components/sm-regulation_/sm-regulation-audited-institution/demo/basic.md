<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-regulation-audited-institution
    :axios="axios"
    :institutionId="id"
    :pageState="pageState"
    />
  </div>
</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'

export default {
  data(){
    return {
      pageState:'add',
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
