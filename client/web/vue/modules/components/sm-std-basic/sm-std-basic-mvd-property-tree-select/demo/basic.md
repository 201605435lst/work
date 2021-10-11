<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-std-basic-mvd-property-tree-select :axios="axios" v-model="value" :showSearch="true"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      value: '',
      axios
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
```
