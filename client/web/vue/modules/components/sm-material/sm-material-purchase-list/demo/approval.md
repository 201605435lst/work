
<cn>
#### 审批
</cn>

<us>
#### 审批
</us>

```tpl
<template>
  <div>
   <sm-material-purchase-list  approval :permissions="getPermissions()" :axios="axios"/>
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
      axios
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
