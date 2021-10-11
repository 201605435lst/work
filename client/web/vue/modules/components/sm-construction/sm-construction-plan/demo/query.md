
<cn>
#### 查询状态
</cn>

<us>
#### 查询状态
</us>

```tpl
<template>
  <div>
    <sm-construction-plan :axios="axios"  onlyQuery :showSelectRow="false"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
***
