<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <a-form layout="inline">
      <a-row>
        <a-form-item
      label="二维码内容"
    >
      <a-input placeholder="请输入二维码内容" v-model='content'/>
    </a-form-item>
    <a-form-item
      label="尺寸"
    >
      <a-input-number v-model="size" :min="1" :max="100" />
    </a-form-item>
     <a-form-item
      label="版本"
    >
      <a-input-number  v-model="version" :min="1" :max="30" />
    </a-form-item>
      </a-row>
      <a-row>
         <a-form-item
        label="中间logo"
    >
     <sm-file-upload :single='true' accept='image/*' style='width:300px' :notCommit='true' @selected='picSelected'/>
    </a-form-item>
    <a-form-item
      label="图标大小"
    >
      <a-input-number  v-model="imageSize" :min="1" :max="300" />
    </a-form-item>
    <a-form-item
      label="是否显示边框"
    >
      <a-switch default-checked v-model='border'/>
    </a-form-item>
      </a-row>
    </a-form>
    <sm-qrcode :axios="axios" :image='image' :border='border' :imageSize='imageSize' :size='size' :version='version' :content='content'/>
  </div>
</template>
<script>
import axios from '@/utils/axios.js'
import SmFileUpload from '@/components/sm-file/sm-file-upload'
export default {
  components:{
    SmFileUpload
  },
  data(){
    return {
      axios,
      content:'',
      size:4,
      version:7,
      imageSize:30,
      border:true,
      image:null,
    }
  },
  created(){
  },
  methods: {
    picSelected(data){
      console.log(data)
      this.image=data;
    }
  }
}
</script>
***
```
