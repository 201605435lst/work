
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-schedule-diary  
    :axios="axios"
    :id="id"
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
      pageState:'edit',
      axios,
      id:'39fbb7ec-b05d-cf0b-b259-b7f78608a0aa',
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
