<cn>
#### 全部
</cn>

<us>
#### 全部
</us>

```tpl
<template>
  <div>
    <a-radio-group v-model="pageState" @change="onPageStateChange">
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
    <sm-cr-plan-skylight-plan
      :axios="axios" :id="id"
      :organizationId="organizationId"
      :planType="planType"
      :planDate="planDate"
      repairTagKey="RepairTag.RailwayWired"
      :pageState="pageState"
      @ok="onOk"
      @cancel="onCancel"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      id:'',//0b42dac6-27a8-4d70-88a6-bc1dd27b5858
      axios,
      pageState:'add',
      planType: 3,
      planDate: '2020-12-1',
      organizationId:'39f97b50-ddbc-d874-dcec-c4516e92ce7e'
    }
  },
  created(){
  },
  methods: {
    onPageStateChange(event){
      this.pageState=event.target.value
    },
  
    onOk(id){
      // console.log('onSuccess',id)
      this.$message.info(`onSuccess: ${id}`)
    },
    onCancel(id){
      // console.log('onCancel',id)
      this.$message.info(`onCancel`)
    },
  }
}
</script>
```
