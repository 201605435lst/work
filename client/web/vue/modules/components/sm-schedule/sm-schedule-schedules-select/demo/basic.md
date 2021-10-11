<cn>
#### 单选模式
</cn>

<us>
#### 单选模式
</us>

```tpl
<template>
  <div>
    <div>
      <sm-schedule-schedules-select
      :axios="axios"
      :value="value"
      :disabled='disabled'
      :multiple='multiple'
      />
    </div>
    <div></div>
  </div>
</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      value:'',
      axios,
      multiple:false,
      disabled:false
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
```
