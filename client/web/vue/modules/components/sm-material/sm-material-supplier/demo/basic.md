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
    <sm-material-supplier
      :axios="axios" 
      :id="id"
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
      id:null,
      axios,
      pageState:'add',
    }
  },
  created(){
  },
  methods: {
    onPageStateChange(event){
      this.pageState = event.target.value
      if(event.target.value!='add'){
        this.id='39fcd782-c430-e764-2ffc-954a7f76ed5d'
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
