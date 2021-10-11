<cn>
#### 根据id获取子项
</cn>

<us>
#### 根据id获取子项
</us>

```tpl
<template>
  <div>
    <sm-std-basic-repair-group-select  :axios="axios" :value="value" />
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      count: 5,
      show: true,
      axios,
      value: null
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
```
