
<cn>
#### 技术交底
</cn>

<us>
#### 技术交底
</us>

```tpl
<template>
  <div>
    <sm-technology-disclose :permissions="getPermissions()" :axios="axios"/>
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
