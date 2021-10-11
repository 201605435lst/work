<cn>
#### 基本用法-添加模式
</cn>

<us>
#### 基本用法-添加模式
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
      <sm-bpm-workflow-template :axios="axios" :id="id" :pageState="pageState" @ok="onOk" @cancel="onCancel"  :permissions="getPermissions()"/>
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
      pageState:'edit',
      id:'52491d3f-0e9e-4274-9be8-d635cbd43459'//af4a3525-1d23-45cc-aee5-46446d3e1603
    }
  },
  created(){
  },
  methods: {
    getPermissions,
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
