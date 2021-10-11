<cn>
#### 反向绑定
</cn>

<us>
#### 反向绑定
</us>

```tpl
<template>
  <div>
    <div class='control'>
      <span>文件标签显示方式</span>
      <a-radio-group v-model="direction" @change="onChange" size='small'>
        <a-radio-button value="row">
          行显示
        </a-radio-button>
        <a-radio-button value="col">
          列显示
        </a-radio-button>
    </a-radio-group>
    </div>
    <sm-file-upload :axios="axios" :fileList="fileList" mode='edit' :tagDirection='direction'/>
  </div>

</template>
<style lang="">
  .control{
    margin-bottom:20px;
  }
</style>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      direction:'col',
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
    onChange(e){
      console.log(e)
    }
  }
}
</script>
***
```
