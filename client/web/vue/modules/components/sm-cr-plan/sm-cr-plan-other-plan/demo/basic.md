<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
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
    <sm-cr-plan-other-plan
      :axios="axios" :id="id"
      :organizationId="organizationId"
      :planDate="planDate"
      :pageState="pageState"
      repairTagKey="RepairTag.RailwayWired"
      @ok="onOk"
      @cancel="onCancel"/>
  </div>
<!-- RepairTag.RailwayHighSpeed-->
</template>
<script>
import axios from '@/utils/axios.js'
export default {
  data(){
    return {
      id:'455ebbe3-545e-40a6-81d8-b337127eacd8',
      axios,
      pageState:'view',
      planDate: '2021-08',
      organizationId:'39f8757d-ecc8-8d55-c5e0-fd4f7edd0a33'
    }
  },
  created(){
  },
  methods: {
    onPageStateChange(event){
      this.pageState = event.target.value
      if(event.target.value!='add'){
        this.id='0e8b2d09-e275-469e-b7f1-f9d9fd3767da'
      }else{
        this.id=null
      }
    },
    onOk(id){
      this.$message.info(`onSuccess: ${id}`)
    },
    onCancel(id){
      this.$message.info(`onCancel`)
    },
  }
}
</script>
```
