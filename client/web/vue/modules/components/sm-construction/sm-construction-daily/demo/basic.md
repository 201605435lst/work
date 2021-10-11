<cn>
#### 添加
</cn>

<us>
#### 添加
</us>

```tpl
<template>
  <div>
    <sm-construction-daily
    :axios="axios"
    :id="id"
    :pageState="pageState"
    :permissions="getPermissions()"
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
    getPermissions,
  }
}
</script>
```
