<cn>
#### 我的汇报
</cn>

<us>
#### 我的汇报
</us>

```tpl
<template>
  <div>
    <sm-reports
    :axios="axios"
    reportsType="send"
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
      // reportsType:"send",
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
