
<cn>
#### 图片上传模式
</cn>

<us>
#### 图片上传模式
</us>

```tpl
<template>
  <div>
    <sm-file-upload :axios="axios" theme='pic'/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
*** 