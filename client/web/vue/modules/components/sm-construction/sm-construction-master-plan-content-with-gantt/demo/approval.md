<cn>
#### 总体计划审核
</cn>

<us>
#### 总体计划审核
</us>

```tpl
<template>
  <div>
    <sm-construction-master-plan-content-with-gantt :axios="axios" masterPlanId="39feef0a-109f-74cd-3d71-80d2075e7de5" isApproval/>
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
```
