## API

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| axios | 项目 axios.create 创建的实例 | function | null |
| action | 获取数据的地址或者执行方法 | string/Function/object | null |
| icon | 卡片标题图标 | string/slots | null |
| extra | 卡片右上角操作区域 | string/slots | null |
| rowKey | 数据 rowKey，如“id”等 | string | 'id' |
| theme | 列表主题，如“default”，“default-thumb”，“simple” | string | 'default' |
| size | 卡片尺寸 | string | 'default' |
| maxListCount | 卡片最多显示条数 | number | 5 |
| thumbUrl | 缩略图字段配置 | string | 'thumb.url' |
| columns | 显示的字段配置,如 [{title: '文章标题', dataIndex: 'title', key: 'title'}] | array | [] |

### 事件

| 事件名称 | 说明               | 回调参数                                                     |
| -------- | ------------------ | ------------------------------------------------------------ |
| click    | 单条数据的单击事件 | function(event,index,id,data) => void 事件、索引、id、行数据 |
| more     | 右上角元素单击事件 | function()                                                   |
