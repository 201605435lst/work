<cn>
#### 基本用法-单一流程模式
</cn>

<us>
#### 基本用法-单一流程模式
</us>

```tpl
<template>
  <div >
    <div style="width: 100%; height: 700px;">
      <sm-bpm-workflow-template :axios="axios" :id="id" :pageState="pageState" @ok="onOk" @cancel="onCancel"  :permissions="getPermissions()"/>
    </div>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'

export default {
  data(){
    return {
      axios,
      pageState:'edit',
      id:'506948a9-6d74-4eec-80bc-ff76103664f5'
    }
  },
  created(){
  },
  methods: {
    getPermissions,
    onOk(){
      console.log('onOk')
      this.$message.info(`onOk`)
    },
    onCancel(){
      console.log('onCancel')
      this.$message.info(`onCancel`)
    },
  }
}
</script>
```
