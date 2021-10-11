
<cn>
#### 基础用法
</cn>

<us>
#### 基础用法
</us>

```tpl
<template>
  <div>
    <sm-file-upload  :axios="axios" ref='fileUpload'/>
    <br/>
    <a-button @click='commit'>提交方法测试，上传文件到文件管理-‘我的’目录下了，异步调用</a-button>
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
    async commit(){
      await this.$refs.fileUpload.commit();
      this.$message.info('文件已保存')
    }
  }
}
</script>
*** 