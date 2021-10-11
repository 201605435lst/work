
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-construction-base-draw-sub-item :axios="axios" subItemContentId="39fef011-d364-d95f-1f6c-08469ded2b00" @back="back" hasReturnBtn/>
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
    back(){
      console.log('调用端订阅返回事件');
    }
  }
}
</script>
***
