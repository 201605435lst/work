<cn>
#### 三维会审
</cn>

<us>
#### 三维会审
</us>

```tpl
<template>
  <div>
    <sm-file-manage
    :multiple="multiple"
    :select='select'
    :height="height"
    :isApprove="true"
    static-key="FileManageApprove"
    :axios="axios"/>
    
  </div>
</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      count: 5,
      show: true,
      axios,
      multiple:true,
      select:false,
      height:650
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
```
