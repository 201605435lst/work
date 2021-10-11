
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-schedule-schedule :axios="axios" :id="id" :permissions="getPermissions()"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'

export default {
  data(){
    return {
      axios,
     id:'39fbc2da-3adb-e5a4-3083-b9e93e08d873',
    }
  },
  created(){
  },
  methods: {
    getPermissions
  }
}
</script>
*** 