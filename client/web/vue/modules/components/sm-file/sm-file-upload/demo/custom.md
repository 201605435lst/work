
<cn>
#### 自定义样式
</cn>

<us>
#### 自定义样式
</us>

```tpl
<template>
  <div>
    <div class='file-upload-custom'>
      <a-button @click='btnClick' type="primary" ghost >自定义的按钮，只调用了文件选择组件,请点击尝试，嘿嘿嘿</a-button>
      <sm-file-upload :axios="axios" :custom='true' @selected='selected' ref='fileUpload'/>
      <br/>
      已选择的文件：
      <a-list size="small" bordered :data-source="files">
         <a-list-item slot="renderItem" slot-scope="item, index">
         {{ item.name }}
        </a-list-item>
    </a-list>
    </div>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      files:[],
    }
  },
  created(){
  },
  methods: {
    btnClick(){
      this.$refs.fileUpload.fileSelect();
    },
    // 选中的文件
    selected(files){
      this.files.push(...files);
      console.log(files)
    }
  }
}
</script>
*** 