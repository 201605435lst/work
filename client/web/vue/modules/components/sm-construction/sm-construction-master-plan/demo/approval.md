
<cn>
#### 审批状态
</cn>

<us>
#### 审批状态
</us>

```tpl
<template>
  <div>
    <sm-construction-master-plan :axios="axios" approval/>
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
