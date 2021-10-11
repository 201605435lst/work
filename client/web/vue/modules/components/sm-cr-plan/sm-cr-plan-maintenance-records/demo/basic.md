<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-cr-plan-maintenance-records :axios="axios" :permissions="getPermissions()" repairTagKey="RepairTag.RailwayWired" @view="onView"/>
  </div>
<!--RepairTag.RailwayHighSpeed-->
<!--RepairTag.RailwayWired-->
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
    onView(a,a1,a2,a3,a4,a5,a6,a7,b){
      console.log(b)
    }
  }
}
</script>
```
