<cn>
#### 垂直
</cn>

<us>
#### 垂直
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
      repairTagKey="RepairTag.RailwayHighSpeed"
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
      id:'144db8ff-c086-48da-a8ab-dc597863b3ee',
      axios,
      pageState:'edit',
      planType: 1,
      planDate: '2021-06-24',
      organizationId:'39f97b50-ddb7-994e-578f-34bd75a2c642'
    }
  },
  created(){
  },
  methods: {
    onPageStateChange(event){
      this.pageState=event.target.value
    },
     onIdChange(event){
      this.id = event.target.value
      // if(event.target.value!='add'){
      //   this.id='4b1c5c80-4998-490b-8b79-2d499eec09fa'
      // }else{
      //   this.id=null
      // }
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
