## API

### 属性

| 参数 | 描述                                          | 类型   | 默认值 |
| -------- | ---------------------------------------------------- | -------- | ------- |
| axios    | the instance function that from project axios.create | function ||
| allowClear | 支持清除 |  |true|
| autoClearSearchValue | 是否在选中项后清空搜索框，只在 mode 为 multiple 或 tags 时有效 |  |true|
| autoFocus | 默认获取焦点                                                 |  |false|
| disabled | 是否禁用 |  |false|
| dropdownClassName | 下拉菜单的 className 属性 |  |common-select|
| dropdownStyle | 下拉菜单的 style 属性 |  |null|
| filterOption | 是否根据输入项进行筛选。当其为一个函数时，会接收 inputValue option 两个参数，当 option 符合筛选条件时，应返回 true，反之则返回 false |  |() => {}|
| firstActiveValue | 默认高亮的选项 |  |null|
| maxTagCount | 最多显示多少个 tag |  |5|
| mode | 是否多选 | 'default' \| 'multiple' \| 'tags' \| 'combobox' |default|
| placeholder | 选择框提示文字 |  |请选择|
| showSearch | 使单选模式可搜索 |  |false|
| API | 模块查询数据的接口（初始化之后的） | Object |null|
| value | 值 |  |undefined|

### 事件

| 事件名称 | 描述                                                         | 类型                                         |
| -------- | ------------------------------------------------------------ | -------------------------------------------- |
| change   | 选中 option，或 input 的 value 变化（combobox 模式下）时，调用此函数 | function(value, record) |
| select   | 被选中时调用，参数为选中项的 value (或 key) 值               | function(value, record)               |

