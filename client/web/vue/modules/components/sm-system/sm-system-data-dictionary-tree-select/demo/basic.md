<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <h4>预案等级</h4>  <br/>
    <sm-system-data-dictionary-tree-select groupCode='EmergPlanLevel' :axios="axios"/>
    <br/>  <br/>
    <h4>专业</h4>  <br/>
    <sm-system-data-dictionary-tree-select groupCode='Profession' :axios="axios" value="value"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      count: 5,
      show: true,
      axios,
      value:'7ed0a3c7-0097-4bb7-934c-76900b946cc7'
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
```
