
<cn>
#### 单选模式
</cn>

<us>
#### 单选模式
</us>

```tpl
<template>
  <div>
    <sm-construction-base-section-select :axios="axios" :multiple="false" :height="30" :value="value"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      value:'39fd9b91-1b9f-5d6e-3c5a-c3391a6a267b'
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
***
