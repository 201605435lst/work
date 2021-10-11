<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <a-radio-group :value="pageState" @change="onPageStateChange">
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
    <sm-cr-plan-plan-change
      :axios="axios"
      :pageState ="pageState"
      :id="id"
      @ok="onOk"
      @cancel="onCancel"
      :organizationId="orgID"
      repairTagKey="RepairTag.RailwayWired"
    />
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      count: 5,
      axios,
      id: 'dfe737b9-80c5-493e-9ff6-f48394bf27ab',
      pageState: 'add',
      orgID: '39f8757d-ecc8-6651-5469-bbf559565943',
    }
  },
  created(){
  },
  methods: {
    onPageStateChange(event){
      this.pageState = event.target.value
    },
    onOk(id){
      console.log('onSuccess',id)
      this.$message.info(`onSuccess: ${id}`)
    },
    onCancel(id){
      console.log('onCancel',id)
      this.$message.info(`onCancel: ${id}`)
    },
  }
}
</script>
```
