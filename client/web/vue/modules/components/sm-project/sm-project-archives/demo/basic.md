<cn>
#### 档案管理
</cn>

<us>
#### 档案管理
</us>

```tpl
<template>
  <div>
    <sm-project-archives :axios="axios"  :permissions="getPermissions()"/>
  </div>
</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'
export default {
 data(){
    return {
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
