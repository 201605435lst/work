
<cn>
#### 任务计划审核
</cn>

<us>
#### 任务计划审核
</us>

```tpl
<template>
  <div>
    <sm-construction-plan-with-gantt :axios="axios" planId="39fef062-89bb-cf71-2bab-a2f20cb02163"  isApproval/>
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
