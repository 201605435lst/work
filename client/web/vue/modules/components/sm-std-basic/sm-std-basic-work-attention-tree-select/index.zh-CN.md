## API

| Property           | Description                  | Type            | Default  |
| ------------------ | ---------------------------- | --------------- | -------- |
| axios              | 项目 axios.create 创建的实例 | function        | null     |
| value              | 指定当前选中的条目           | string/string[] | -        |
| disabled           | 是否禁用                     | boolean         | false    |
| multiple           | 是否支持多选                 | boolean         | false    |
| maxTagCount        | 多选状态下最多显示 tag 数    | number          | 2        |
| placeholder        | 选择框默认文字               | string          | '请选择' |
| allowClear         | 是否清除                     | boolean         | true     |
| disabledIds        | 禁用层级 id                  | string[]        | -        |
| childrenIsDisabled | 是否设置子元素禁用状态       | boolean         | false    |

### 事件

| 事件名称 | 说明                   | 回调参数        |
| -------- | ---------------------- | --------------- |
| change   | 选中树节点时调用此函数 | function(value) |
