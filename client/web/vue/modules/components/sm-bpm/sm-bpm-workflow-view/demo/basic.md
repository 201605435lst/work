<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <a-input :value="workflowId"/>
    <a-button @click="openViewer" >打开流程查看器</a-button>
    <sm-bpm-workflow-view :axios="axios" ref="workflowViewer"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      workflowId:'bc074d8f-9376-4f9d-ae27-4b764d5d4a2f',// 工作流id
    }
  },
  created(){
  },
  methods: {
    openViewer(){
      this.$refs.workflowViewer.view(this.workflowId);
    }
  }
}
</script>
***
```
