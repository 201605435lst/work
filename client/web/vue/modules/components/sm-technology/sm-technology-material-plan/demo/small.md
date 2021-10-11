
<cn>
#### 小尺寸其他模块弹窗调用
</cn>

<us>
#### 小尺寸其他模块弹窗调用
</us>

```tpl
<template>
  <div>
    <sm-technology-material-plan  small isMaterialPlanSelect multiple :permissions="getPermissions()" :axios="axios"/>
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
