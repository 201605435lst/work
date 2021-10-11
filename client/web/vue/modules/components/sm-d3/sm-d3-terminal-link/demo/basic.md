<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div style="position:relative" >

    范围选择
    <a-select  style="width: 200px" v-model="equipmentId">
      <a-select-option v-for="item in equipments" :value="item.id" :key="item.id">
        {{item.name}}
      </a-select-option>
    </a-select>
    <br/>
    <br/>
    <!-- 选中设备：{{value}} -->
    <br/>
    <br/>

    <div style="
        position: relative;
        height: 460px;
        width: 800px;
      "
      >
        <sm-d3-terminal-link
          :position="{ left: '0', top:'0',right:'0' }"
          :equipment="equipments.find(x=>x.id === equipmentId)"
          height="100%"
          :axios="axios"
          :visible="true"
        />
      </div>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      equipments:[
        {
          id:'39f7e051-6c8e-1c19-9b21-046e1766e6e7',
          name:'DMH1',
          group:{name:''}
        },
      ],
      equipmentId:null
    }
  },
  created(){
  },
  methods: {

  }
}
</script>
```
