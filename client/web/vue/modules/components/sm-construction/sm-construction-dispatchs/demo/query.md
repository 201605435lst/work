<cn>
#### 查询状态
</cn>

<us>
#### 查询状态
</us>

```tpl
<template>
  <div>
    <sm-construction-dispatchs :axios="axios" :permissions="getPermissions()"  :showSelectRow="false" isForDaily passed/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'

export default {
  data(){
    return {
      axios
    }
  },
  created(){
  },
  methods: {
    getPermissions
  }
}
</script>
***
```
