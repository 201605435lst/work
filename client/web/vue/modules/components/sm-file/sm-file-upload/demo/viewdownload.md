
<cn>
#### 查看模式，文件绑定,文件可下载
</cn>

<us>
#### 查看模式，文件绑定,文件可下载
</us>

```tpl
<template>
  <div>
    <sm-file-upload :axios="axios" :fileList="fileList" mode='view' :download='true'/>
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
          type:'.jpg',
          url:'/2021/02/39fa6ddc-14d7-aaeb-5c3b-195f75a70adb.jpg'
        },
        {
          id:'guid-xxxx',
          name:'测试文件777',
          type:'.png',
          url:'/2021/02/39fa6ddc-14d7-aaeb-5c3b-195f75a70adb.jpg'
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