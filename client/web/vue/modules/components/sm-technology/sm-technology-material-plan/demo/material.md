
<cn>
#### 物资模块使用
</cn>

<us>
#### 物资模块使用
</us>

```tpl
<template>
  <div>
    <sm-technology-material-plan  material :permissions="getPermissions()" :axios="axios"/>
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
