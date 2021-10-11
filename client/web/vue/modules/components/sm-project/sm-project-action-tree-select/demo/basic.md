<cn>
#### 可操作的树
</cn>

<us>
#### 可操作的树
</us>

```tpl
<template>
  <div>
    <sm-project-action-tree-select :axios="axios"  :permissions="getPermissions()"/>
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
