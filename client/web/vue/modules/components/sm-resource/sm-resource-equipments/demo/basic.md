<cn>
#### 设备
</cn>

<us>
#### 设备
</us>

```tpl
<template>
  <div>
    <sm-resource-equipments :permissions="getPermissions()" :axios="axios"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'

export default {
  data(){
    return {
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
