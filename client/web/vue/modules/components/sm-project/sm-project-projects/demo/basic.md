<cn>
#### 项目管理
</cn>

<us>
#### 项目管理
</us>

```tpl
<template>
  <div>
    <sm-project-projects :axios="axios" :permissions="getPermissions()"/>
  </div>
</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'
export default {
  data(){
    return {axios}
  },
  created(){
  },
  methods: {
    getPermissions,
  }
}
</script>
```
