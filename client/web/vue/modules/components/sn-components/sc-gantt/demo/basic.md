
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sc-gantt :axios="axios" :data="data"/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {
  data(){
    return {
      axios,
      data:[
        {
          content: '去永辉买面包', // 任务标题
          parentId: null, // 父id
          children: [ // 子任务
            {
              content: '付钱',
              parentId: '39fdfe09-73ec-daf9-8f0d-1e717ffed971',
              children: [],
              isMilestone: false,
              id: '39fe01db-46d7-0ddb-18fc-4ca3397d97b3',
              startDate: '2021-06-15',
              endDate: '2021-07-15',
              desc: '',
              duration: 14,
              collapsed: false,
              preTaskIds: [
                '39fdfe09-73ec-daf9-8f0d-1e717ffed971',
              ],
              disabled: false,
            },
          ],
          isMilestone: false, //是否是里程碑
          id: '39fdfe09-73ec-daf9-8f0d-1e717ffed971', // 任务id
          startDate: '2021-07-01', // 任务开始时间
          endDate: '2021-07-31', // 任务结束时间
          desc: '嘟嘟嘟嘟', // 任务描述
          duration: 30, // 任务工期
          collapsed: false, // 是否闭合(点击三角按钮展开闭合)
          preTaskIds: [], // 前置任务ids
          disabled: false, // 是否禁用
        },
      ],
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
***
