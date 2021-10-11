<cn>
#### 多选模式
</cn>

<us>
#### 多选模式
</us>

```tpl
<template>
  <div>
    <sm-std-basic-mvd-property-tree-select :axios="axios" v-model="value" :treeCheckable="true" :treeCheckStrictly="true"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      //value: ['39f87ee8-a75f-e3c5-7f4e-d08017e3d9a3'],
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
