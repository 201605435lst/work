
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-gantt :axios="axios" :datas="datas"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      datas:[{
        name: '天津地体项目',
        data: [{
          index:1,
          major:'土建专业',
          name:'继承路基建设',
          start: 1609430400000,
          end: 1609603200000,
          completed: 0.25,
          state: '未开工',
          id: "0",
          y:0,
        }, {
          index:2,
          major:'机电专业',
          name:'空调外机安装',
          start: 1609603200000,
          end: 1609862400000,
          completed: 0.25,
          state: '未开工',
           dependency:"0",
          //milestone: true,
          id: "1",
          y:1,
        },
        {
          index:3,
          major:'机电专业',
          name:'空调调试',
          start: 1609862400000,
          end: 1610121600000,
          completed: 0,
          state: '未开工',
          dependency:"1",
          id: "2",
          y:2,
        },
        {
          index:4,
          major:'机电专业',
          name:'空调调试2',
          start: 1610380800000,
          end: 1610640000000,
          state: '未开工',
          milestone:true,
          id: "3",
          y:3,
        },
        {
          index:5,
          major:'测试专业',
          name:'测试任务11111',
          start: 1610640000000,
          end: 1610899200000,
          completed: 0.5,
          state: '未开工',
          id: "4",
          y:4,
        },
         {
          index:6,
          major:'测试专业',
          name:'测试任务11111',
          start: 1610899200000,
          end: 1611244800000,
          completed: 0.5,
          dependency:"4",
          state: '未开工',
          id: "5",
          y:5,
        },
        {
          index:7,
          major:'测试专业',
          name:'测试任务11111',
          start: 1600000000000,
          end: 1600000000001,
          completed: 0.5,
          dependency:"4",
          state: '未开工',
          id: "6",
          y:6,
        },
        ],
      }],
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
*** 