<cn>
#### 我发起的
</cn>

<us>
#### 我发起的
</us>

```tpl
<template>
  <div>
    <sm-task-tasks :axios="axios" :group="1" :permissions="getPermissions()"/>
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
    onView(id){
      console.log('onView',id)
      this.$message.info(`onView: ${id}`)
    },
    getPermissions,
  }
}
</script>
```
