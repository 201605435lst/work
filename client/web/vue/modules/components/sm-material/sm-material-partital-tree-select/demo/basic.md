<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-material-partital-tree-select :axios="axios" :value="value"/>
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
      value:'39fd3083-08e9-c9b6-465b-0b3765c7ebc7'
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
```
