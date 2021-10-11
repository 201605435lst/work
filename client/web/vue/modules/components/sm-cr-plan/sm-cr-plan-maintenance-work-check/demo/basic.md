<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-cr-plan-maintenance-work-check :axios="axios" :permissions="getPermissions()" repairTagKey="RepairTag.RailwayHighSpeed"/>
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
