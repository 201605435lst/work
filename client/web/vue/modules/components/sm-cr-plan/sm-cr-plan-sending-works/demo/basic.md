<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-cr-plan-sending-works :axios="axios" :permissions="getPermissions()" repairTagKey="RepairTag.RailwayWired" @view="onView"/>
  </div>
<!-- RepairTag.RailwayWired -->
<!-- RepairTag.RailwayHighSpeed -->
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
    decline () {
      let count = this.count - 1
      if (count < 0) {
        count = 0
      }
      this.count = count
    },
    increase () {
      this.count++
    },
    onView(a ,record){
      this.$message.info(record);
    }
  }
}
</script>
```
