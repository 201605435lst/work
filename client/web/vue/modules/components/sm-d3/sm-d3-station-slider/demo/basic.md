
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div style="height:200px" >
    <a-button @click="()=>{visible = !visible}"> 切换 </a-button>
    <br/>
    <div style="
      position: relative;
      background: linear-gradient(0deg, #0d31b7, #8644afd6);
      width: 100%;
      height: 300px;
    ">
      <sm-d3-station-slider theme="dark" :axios="axios" :visible="visible" @change="onChange"/>
    </div>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      visible:true
    }
  },
  created(){
  },
  methods: {
    onChange:(value)=>{
      console.log(value)
    }
  }
}
</script>
***
