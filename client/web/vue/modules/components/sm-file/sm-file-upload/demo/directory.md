
<cn>
#### 上传文件夹
</cn>

<us>
#### 上传文件夹
</us>

```tpl
<template>
  <div>
    <sm-file-upload :axios="axios" :directory='true'/>
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