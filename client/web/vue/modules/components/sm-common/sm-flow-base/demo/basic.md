<cn>
#### 任务流程节点
</cn>

<us>
#### 任务流程节点
</us>

```tpl
<template>
  <div>
    <sm-flow-base :nodes="data" :height="300">
    </sm-flow-base>
  </div>
</template>
<script>
import axios from '@/utils/axios.js'
export default {
  data(){
    return {
      data:[
        {
          id:1,
          title:'测试任务1',
          subTitle:'张三',
          perc:40,
          type:'finshed',
          children:[
            {
               id:2,
               title:'子任务1',
               subTitle:'李四',
               perc:10,
               type:'noStart',
               children:[],
            },
            {
               id:3,
               title:'子任务2',
               subTitle:'王五',
               perc:10,
               type:'noStart',
               children:[],
            },
          ]
        }
      ]
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
```
