<cn>
#### 二维码
</cn>

<us>
#### 二维码
</us>

```tpl
<template>
  <div>
    <sm-resource-equipments :permissions="getPermissions()" :axios="axios" :isQrcode="true" :multiple="true"/>
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
