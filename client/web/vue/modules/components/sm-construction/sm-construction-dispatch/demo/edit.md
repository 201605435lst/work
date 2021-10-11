<cn>
#### 编辑派工
</cn>

<us>
#### 编辑派工
</us>

```tpl
<template>
  <div style="margin-bottom:20px">
    <sm-construction-dispatch
      :id="id"
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
      id: '39ff1d52-3f5e-7549-8462-bf55ba959e24',
      axios,
      pageState:'edit',
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
