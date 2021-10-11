
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-cr-plan-year-month-alter-record :axios="axios" @view="onView" :planType="planType"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      planType:2
    }
  },
  created(){
  },
  methods: {
    onView(record,orgId,orgName){
      this.$message.info(`${record.planType}"-"${orgId}"-"${orgName}`)
    }
  }
}
</script>
*** 