<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <a-button @click='open'>打开选择框</a-button>
    <sm-material-material-select-modal :axios="axios" :visible='visible' @change="change"/>
    <ul>
      <li v-for="item in selected">{{item.name}}</li>
    </ul>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      selected:[],
      visible:false,
    }
  },
  created(){
  },
  methods: {
    open(){
      this.visible=true;
    },
    change(evt,selected){
      this.visible=evt;
      this.selected=selected;
    }
  }
}
</script>
***
```
