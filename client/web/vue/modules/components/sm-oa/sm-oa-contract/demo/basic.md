<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-oa-contract
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
      id:'39fc2445-a52c-58b6-ca48-9d1a61522aa0',
      //  id:'39f95cee-fb00-54dd-4e14-35756ce5aa08'
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
