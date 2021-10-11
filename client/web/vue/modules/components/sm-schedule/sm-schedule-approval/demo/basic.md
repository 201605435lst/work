
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-schedule-approval :axios="axios"  :pageState="pageState" :id="id"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
       pageState:'add',
      axios,
      id: "39fb0e4a-65d0-c292-2024-f061e4c7765a"
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
*** 