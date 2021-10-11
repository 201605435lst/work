<cn>
#### 添加派工
</cn>

<us>
#### 添加派工
</us>

```tpl
<template>
  <div style="margin-bottom:20px">
    <sm-construction-dispatch
      :axios="axios"
      :pageState="pageState"
      :permissions="getPermissions()"
      @ok="onOk"
      @cancel="onCancel"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'

export default {
 data(){
    return {
      id: null,
      axios,
      pageState:'add',
    }
  },
  created(){
  },
  methods: {
    getPermissions,
    onOk(id){
      this.$message.info(`onSuccess: ${id}`)
    },
    onCancel(id){
      this.$message.info(`onCancel`)
    },
  }
}
</script>
***
```
