
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-material-material-select :axios="axios" :keyword="keyword"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      keyword:['工器具']
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
*** 