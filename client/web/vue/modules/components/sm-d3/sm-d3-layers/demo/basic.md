<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div style="height: 600px; width:250px; border: 1px dashed gray; position: relative;">
    <sm-d3-layers :axios="axios" v-model="value" @change="onChange" :dataSource="dataSource" :bordered="false"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      value: ['1','4'],
     dataSource:[
       {
         id:'1',
         name:'土建',
         children:null
       },
       {
         id:'2',
         name:'机电',
         children:[
           {
              id:'2-1',
              name:'机电一',
              children:[
                {
                    id:'2-1-1',
                    name:'机电一01',
                    children:[
                      {
                          id:'2-1-1-1',
                          name:'机电一0101',
                          children:null
                        },
                        {
                          id:'2--1-1-2',
                          name:'机电二0102',
                          children:null
                        },
                    ]
                  },
                  {
                    id:'2-1-2',
                    name:'机电二02',
                    children:null
                  },
              ]
            },
            {
              id:'2-2',
              name:'机电二',
              children:null
            },
         ]
       },
       {
         id:'3',
         name:'图层1',
         children:null
       },
       {
         id:'4',
         name:'图层2',
         children:null
       },
       {
         id:'5',
         name:'图层3',
         children:null
       },
       {
         id:'6',
         name:'图层4',
         children:null
       },
       {
         id:'7',
         name:'图层5',
         children:null
       },
      ]
    }
  },
  created(){
  },
  methods: {
    onChange(value){
      console.log(value)
    }

  }
}
</script>
```
