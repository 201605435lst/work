<cn>
#### 通知消息
</cn>

<us>
#### 通知消息
</us>

```tpl
<template>
  <div>
    <sm-message-notice :signalr="signalr" :axios="axios"/>
  </div>
</template>
<script>
import signalr from '@/utils/signalr.js'
import axios from '@/utils/axios.js'

export default {
  data(){
    return {signalr,axios}
  },
  created(){
  },
  methods: {}
}
</script>
```
