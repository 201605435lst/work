<cn>
#### 汇报给我
</cn>

<us>
#### 汇报给我
</us>

```tpl
<template>
  <div>
    <sm-reports
    :axios="axios"
    reportsType="receive"
    :permissions="getPermissions()"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'

export default {
  data(){
    return {
      axios,
    //   reportsType:"receive",
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
