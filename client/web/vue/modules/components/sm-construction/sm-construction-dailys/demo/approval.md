
<cn>
#### 审批状态
</cn>

<us>
#### 审批状态
</us>

```tpl
<template>
  <div>
    <sm-construction-dailys  approval :permissions="getPermissions()" :axios="axios"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'

export default {
  data(){
    return {
      count: 5,
      show: true,
      axios
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
