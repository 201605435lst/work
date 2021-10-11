<cn>
#### 单选模式
</cn>

<us>
#### 单选模式
</us>

```tpl
<template>
  <div>
    <div>
      <sm-oa-seals-select-Modal
      :axios="axios"
      :value="value"
      @change="change"
      :personal='personal'
      :visible='visible'
      />
    </div>
    <div></div>
  </div>
</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      value:[{id:'39f97b61-333b-2aba-076c-5ac5db2da097',name:'马保国1'}],
      axios,
      personal:false,
      visible:false
    }
  },
  created(){
  },
  methods: {
    change(value){
      console.log(value);
    }
  }
}
</script>
```
