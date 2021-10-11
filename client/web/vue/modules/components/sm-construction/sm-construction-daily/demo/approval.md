<cn>
#### 审批
</cn>

<us>
#### 审批
</us>

```tpl
<template>
  <div>
    <sm-construction-daily
    :axios="axios"
    :id="id"
    :approval="true"
    :pageState="pageState"
    :permissions="getPermissions()"
    />
  </div>
</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'

export default {
  data(){
    return {
      pageState:'approval',
      axios,
      id:'39fdfd4b-1a8b-7dc5-6d62-ad8274dc2d1c',
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
