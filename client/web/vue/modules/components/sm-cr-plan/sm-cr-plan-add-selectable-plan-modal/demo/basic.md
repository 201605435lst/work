<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <a-button @click="()=>{visible = true}">打开添加待选计划</a-button>
    <sm-cr-plan-add-selectable-plan-modal
      :selected="selected"
      :organizationId="orgId"
      :axios="axios"
      :date="date"
      :skylightType="skylightType"
      repairTagKey="RepairTag.RailwayWired"
      v-model="visible"
      @ok="okClick"
    />
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      visible: false,
      selected: [],         //选中项
      orgId:'39f8757d-ecc8-e597-602c-8088cd7a242a',       //所属组织机构
      date:new Date('2021-01-01'),                              //限定时间 到月份
      skylightType:1,
    }
  },
  created(){
  },
  methods: {
    okClick(value){
      console.log('已选中数据:');
      console.log(value);
    }
  }
}
</script>
```
