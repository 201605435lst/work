
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
   <div >
    <div style="
        position: relative;
        height: 460px;
        width: 1000px;
      "
      >
       <sm-d3-construction-progress
        :position="{ left: '0', bottom:'0'}"
          height="100%"
          width="1200px"
          :axios="axios"
          :visible="true"
        />
      </div>
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
