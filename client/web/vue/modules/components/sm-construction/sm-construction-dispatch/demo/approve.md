<cn>
#### 派工审批
</cn>

<us>
#### 派工审批
</us>

```tpl
<template>
  <div style="margin-bottom:20px">
    <sm-construction-dispatch
      :axios="axios"
      :id="id"
      :pageState="pageState"
       :permissions="getPermissions()"
      :isApprove="true"
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
      id:'39ff1d52-3f5e-7549-8462-bf55ba959e24',
      axios,
      pageState:'view',
    }
  },
  created(){
  },
  methods: {
    getPermissions,
    onOk(id){
      // console.log('onSuccess',id)
      this.$message.info(`onSuccess: ${id}`)
    },
    onCancel(id){
      // console.log('onCancel',id)
      this.$message.info(`onCancel`)
    },
  }
}
</script>
***
```
