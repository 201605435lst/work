<cn>
#### 多选模式
</cn>

<us>
#### 多选模式
</us>

```tpl
<template>
  <div>
    <div>
    <h4>仓库地点</h4>  <br/>
    <sm-material-partital-tree-select :treeCheckStrictly="true" :multiple='multiple' v-model="value" :disabled='disabled' :axios="axios"/>
  </div>
  </div>
</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      multiple:true,
      axios,
      value:['39fd44d5-c0be-ec4a-67b3-9300cf834607','39fd3083-08e9-c9b6-465b-0b3765c7ebc7'],
      disabled:null,
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
```
