<cn>
#### 静态文件
</cn>

<us>
#### 静态文件
</us>

```tpl
<template>
  <div>
    <sm-file-manage
      static-key="TechnicalDrawing"
      :multiple="multiple"
      :select='select'
      :height="height"
       :isApprove="true"
      :axios="axios"
    />
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
