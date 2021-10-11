<cn>
#### 文件管理
</cn>

<us>
#### 文件管理
</us>

```tpl
<template>
  <div>
    <sm-project-dossier :permissions="getPermissions()" :axios="axios"/>
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
  }
}
</script>

```

