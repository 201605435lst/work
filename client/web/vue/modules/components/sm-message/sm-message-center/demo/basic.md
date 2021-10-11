
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-message-center :axios="axios" :signalr="signalr"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import signalr from '@/utils/signalr.js'

export default {
  data(){
    return {
      axios,
      signalr
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
*** 