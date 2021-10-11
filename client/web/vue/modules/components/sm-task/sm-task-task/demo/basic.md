
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-task-task :axios="axios" :id="id" :loadingMemberId="loadingMemberId" :creatorId="creatorId" :hasParent="hasParent"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      id:'39fbbd0c-0616-3621-7a3b-33f810eb34ee',
      loadingMemberId:'39f9be42-5ad2-eeba-c255-a075c1d157e1',
      creatorId:'39f9be42-5ad2-eeba-c255-a075c1d157e1',
      hasParent:true,
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
*** 