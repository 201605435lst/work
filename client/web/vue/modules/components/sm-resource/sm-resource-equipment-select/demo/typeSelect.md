<cn>
#### 根据分类选择设备
</cn>

<us>
#### 根据分类选择设备
</us>

```tpl
<template>
  <div>
    <div>
      <sm-resource-equipment-select
      :axios="axios"
      v-model="value"
      :multiple='multiple'
      :disabled='disabled'
      :isByTypeSelsct="isByTypeSelsct"
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
      value:'',
      multiple:false,
      isByTypeSelsct:true,
      axios,
      disabled:false
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
```
