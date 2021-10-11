
<cn>
#### 查看模式，文件绑定
</cn>

<us>
#### 查看模式，文件绑定
</us>

```tpl
<template>
  <div>
    <sm-file-upload :axios="axios" :fileList="fileList" mode='view' :download='false'/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      fileList:[
        {
          id:'guid-xxxx',
          name:'测试文件666',
          type:'.png',
          url:'http://www.test...'
        },
        {
          id:'guid-xxxx',
          name:'测试文件777',
          type:'.png',
          url:'http://www.test...'
        }
      ]
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
*** 