<cn>
#### 流程选择
</cn>

<us>
#### 流程选择
</us>

```tpl
<template>
  <div>
    <a-button @click="open">点击打开流程选择框</a-button>
    <SmBpmWorkflowSelectModal ref="flowSelect" :axios="axios" @selected="selected"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import { getPermissions } from '@/utils/utils.js'
import SmBpmWorkflowSelectModal from '../SmBpmWorkflowSelectModal'
export default {
  components:{SmBpmWorkflowSelectModal},
  data(){
    return {
      axios
    }
  },
  created(){
  },
  methods: {
    selected(key){
      this.$message.info(key);
      console.log(key);
    },
    open(){
      this.$refs.flowSelect.show();
    }
  }
}
</script>
```
