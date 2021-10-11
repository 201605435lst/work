<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <sm-dashboard :axios="axios">
  </div>
</template>
<script>
import axios from '@/utils/axios.js'
import moment from 'moment';
import ApiArticle from '../../../sm-api/sm-cms/Article';

let apiArticle = new ApiArticle();

export default {
  data(){
    return {
      axios,
      action: null,
    }
  },
  computed:{
     columns() {
      return [
        {
          title: '文章标题',
          dataIndex: 'title',
          ellipsis: true,
          width:100,
        },
        {
          title: '创建时间',
          dataIndex: 'date',
          customRender: (text, record, index) => {
            return  moment(text).format('YYYY-MM-DD HH:mm:ss');
          },
          width:120,
          ellipsis: true,
        },
        {
          title: '详情',
          dataIndex: 'summary',
          ellipsis: true,
        },
      ];
    },
  },
  created(){
    this.initAxios()
  },
  methods: {
    initAxios() {
      apiArticle = new ApiArticle(this.axios);
      this.action = apiArticle.getList({skipCount: 0, maxResultCount: 5 });
    },
  }
}
</script>
***
```
