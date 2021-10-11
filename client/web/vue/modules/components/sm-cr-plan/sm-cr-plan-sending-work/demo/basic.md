<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-cr-plan-sending-work
     :axios="axios" 
     :permissions="getPermissions()" 
     :operatorType="1" 
     :sendingWorkId="sendingWorkId" 
     :orderState='orderState'
     @cancel="onCancel"
    repairTagKey="RepairTag.RailwayWired"/> 
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
      axios,
      sendingWorkId:'627ed7be-282b-40ab-992e-abc38d519e65',
      orderState:2
    }
  },
  created(){
  },
  methods: {
    getPermissions,
    decline () {
      let count = this.count - 1
      if (count < 0) {
        count = 0
      }
      this.count = count
    },
    onCancel(id){
      this.$message.info(`onCancel`)
    },
  }
}
</script>
```
