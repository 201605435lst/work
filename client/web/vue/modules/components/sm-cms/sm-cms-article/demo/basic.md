<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-cms-article
      :axios="axios"
      :permissions="getPermissions()"
      action="/api/app/cmsArticle/get"
      id="8bc58aac-d103-4dcd-bae3-f7e7efe8f1f2"
      @back="onBack"
    />
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'

export default {
  data(){
    return {
      count: 5,
      show: true,
      axios
    }
  },
  created(){
  },
  methods: {
    getPermissions,
    onBack(){
      console.log('返回')
    }
  }
}
</script>
```
