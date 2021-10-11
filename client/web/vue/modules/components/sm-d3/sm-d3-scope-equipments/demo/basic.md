<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div >

    范围选择
    <a-select  style="width: 200px" v-model="scopeCode">
      <a-select-option v-for="item in scopeCodes" :value="item" :key="item">
        {{parseScope(item).name}}
      </a-select-option>
    </a-select>
    <br/>
    <br/>
    选中设备：{{value}}
    <br/>
    <br/>
    <sm-d3-scope-equipments
      :axios="axios"
      :scopeCode="scopeCode"
      v-model="value"
      @close="onClose"
      :multiple="true"
    />
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import { parseScope } from '@/components/_utils/utils.js';

export default {
  data(){
    return {
      axios,
      scopeCodes:[
        '1@长阳线路所@39f7a383-e548-826a-cd73-f93a621e4023',
        '1@长阳线路所@39f7a383-e548-826a-cd73-f93a621e4023.2@长阳-北京@b424f56a-b5b9-4b33-819e-e07a677c3760',
        '1@长阳线路所@39f7a383-e548-826a-cd73-f93a621e4023.2@长阳-北京@b424f56a-b5b9-4b33-819e-e07a677c3760.3@长阳站@99074cc2-3d6e-445d-b45c-5e4ee887bbe3',
        '1@长阳线路所@39f7a383-e548-826a-cd73-f93a621e4023.2@长阳-北京@b424f56a-b5b9-4b33-819e-e07a677c3760.3@长阳站@99074cc2-3d6e-445d-b45c-5e4ee887bbe3.4@原继电器室@5a0abe2f-06dc-482a-883e-b26b12154e2c',
      ],
      scopeCode: '1@长阳线路所@39f7a383-e548-826a-cd73-f93a621e4023.2@长阳-北京@b424f56a-b5b9-4b33-819e-e07a677c3760.3@长阳站@99074cc2-3d6e-445d-b45c-5e4ee887bbe3.4@原继电器室@5a0abe2f-06dc-482a-883e-b26b12154e2c',
      // value: ['室内@F1`0`1', '室内@LJJC1`0`6'],
      value: [],
    }
  },
  created(){
  },
  methods: {
    parseScope(code){
      return parseScope(code)
    },
    onClose(){
      console.log('close')
    }

  }
}
</script>
```
