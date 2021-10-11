
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
     <a-button type="primary" @click="handleMenuClick" > 打开模态框 </a-button>
    <sm-construction-plan-content-select-modal @close="close" :axios="axios" :visible="visible"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      visible:false
    }
  },
  created(){
  },
  methods: {
   handleMenuClick(e) {
    this.visible=true;
   },
   close(){
     this.visible=false;
   }

  }
}
</script>
***
