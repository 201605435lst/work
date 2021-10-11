<cn>
#### 项目管理单页
</cn>

<us>
#### 项目管理单页
</us>

```tpl
<template>
  <div>
    <sm-project-project :axios="axios"  :permissions="getPermissions()" :id="id"/>
  </div>
</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'
export default {
 data(){
    return {
      axios,
      id:'39f9e7f5-99c6-2b45-36ea-e1e462f34042',
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
