<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-regulation-institution
    :axios="axios"
    :permissions="getPermissions()"

    @view="view"
    @audit="audit"
    />
  </div>
 <!-- :id="id"
    :pageState="pageState" -->
</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'

export default {
  data(){
    return {
      pageState:'add',
      axios,
      id:null,
    }
  },
  created(){
  },
  methods: {
    getPermissions,
    view(id){
      console.log(id);
      this.$message.info("查看详情");
    },

    audit(id){
      console.log(id);
      this.$message.info("制度审核");
    }
  }
}
</script>
```
