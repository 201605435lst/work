<cn>
#### 编辑模式
</cn>

<us>
#### 编辑模式
</us>

```tpl
<template>
  <div >
    <a-radio-group v-model="pageState">
      <a-radio-button value="add">
        Add
      </a-radio-button>
      <a-radio-button value="edit">
        Edit
      </a-radio-button>
      <a-radio-button value="view">
        View
      </a-radio-button>
    </a-radio-group>
    <br><br>
    <div style="width: 100%; height: 700px;">
      <sm-bpm-workflow-template :axios="axios" :id="id" :pageState="pageState" @ok="onOk" @cancel="onCancel"/>
    </div>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      pageState:'edit',
      id:"b6214db9-6c04-41d8-a82f-d0d8c7bc6f30"
    }
  },
  created(){
  },
  methods: {

    onOk(){
      console.log('onOk')
      this.$message.info(`onOk`)
    },
    onCancel(){
      console.log('onCancel')
      this.$message.info(`onCancel`)
    },
  }
}
</script>
```
