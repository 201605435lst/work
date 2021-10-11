
<cn>
#### 多文件上传
</cn>

<us>
#### 多文件上传
</us>

```tpl
<template>
  <div>
    <sm-file-upload :axios="axios" accept="image/png, image/jpeg"> :multiple='true'/>
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