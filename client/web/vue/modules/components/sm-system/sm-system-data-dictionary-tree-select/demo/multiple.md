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
    <h4>预案等级</h4>  <br/>
    <sm-system-data-dictionary-tree-select groupCode='OrganizationType' :treeCheckStrictly="true" :multiple='multiple' v-model="value" :disabled='disabled' :axios="axios"/>
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
      value:['9082ecdb-c546-4f9e-9564-bd3441647a7b','4b86410b-4368-49fa-9fc9-aa1e500cd567'],
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
