<cn>
#### 多选模式
</cn>

<us>
#### 多选模式
</us>

```tpl
<template>
  <div>
    <sm-std-basic-individual-project-tree-select :axios="axios" v-model="value" :treeCheckable="true" :treeCheckStrictly="true"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      value: [],
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
