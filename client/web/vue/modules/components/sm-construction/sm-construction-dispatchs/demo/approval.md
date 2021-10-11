<cn>
#### 审批状态
</cn>

<us>
#### 审批状态
</us>

```tpl
<template>
  <div>
    <sm-construction-dispatchs :axios="axios" :permissions="getPermissions()" approval @process="process"/>
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
    getPermissions,
    process(id){
      console.log(id)
    }
  }
}
</script>
***
```
