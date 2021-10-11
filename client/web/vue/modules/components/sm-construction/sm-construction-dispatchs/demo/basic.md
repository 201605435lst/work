<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-construction-dispatchs :axios="axios" :permissions="getPermissions()" @add="onAdd" @edit="onEdit" @view="onView"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'

export default {
  data(){
    return {
      axios
    }
  },
  created(){
  },
  methods: {
    getPermissions,
  onAdd(){
    console.log('add:添加跳转')
  },
  onEdit(id){
    console.log('编辑id=',id)
  },
  onView(id){
    console.log('详情id=',id)
  },
  }
}
</script>
***
```
