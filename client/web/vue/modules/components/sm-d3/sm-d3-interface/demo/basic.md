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
        width: 840px;
      "
      >
       <sm-d3-interface
       :permissions="getPermissions()"
          :position="{ left: '0', top:'0',right:'0' }"
          height="100%"
          :axios="axios"
          :visible="true"
        />
      </div>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'
export default {
  data(){
    return {
      axios,
    }
  },
  created(){
  },
  methods: {
    getPermissions,
  }
}
</script>
```
