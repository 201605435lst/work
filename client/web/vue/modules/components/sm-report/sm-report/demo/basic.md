<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-report
    :axios="axios"
    :id="id"
    :pageState="pageState"
    :permissions="getPermissions()"
    />
  </div>
 <!-- :organizationId="organizationId" -->
</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'

export default {
  data(){
    return {
      pageState:'edit',
      axios,
      id:'39fc0fe1-26bf-e7a4-e149-e8e15e5413e4',
      // id:'39f9c496-a613-429f-f8a6-37fc4f84c360'
      // organizationId:'39f8757d-ecc3-ade2-c816-87326c0a034b'
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
