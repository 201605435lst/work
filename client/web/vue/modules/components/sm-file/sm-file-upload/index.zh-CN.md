# 文件上传组件 API

## 1、属性

| 属性 | 描述 | 类型 | 默认值 |
| --- | --- | --- | --- |
| axios | axios 实例 | function |  |
| accept | 文件选择所支持的类型 | String | "\*" 所有类型 |
| directory | 是否以文件夹形式上传 | Boolean | false |
| multiple | 是否为多文件上传 | Boolean | false |
| theme | 组件主题样式，目前支持两种，'`default`'、'`pic`'两种模式，`pic`主题只用来上传例如头像，签章等图片类型的文件。 | String | 'default' |
| fileList | 用于组件反向绑定文件的数组 | Array | [] |
| width | 组件的宽度，只对 default 主题有效。 | Number | 500 |
| height | 组件的高度，只对 default 主题有效。 | Number | 40 |
| title | 组件的文字标题 | String | 文件上传 |
| mode | 组件模式，目前有两种模式，'`edit`'、'`view`'，`edit`模式可以点击上传文件，`view`模式只能看，不能点击上传。 | String | 'edit' |
| download | 是否支持文件下载，只在`view`模式下起作用， | Boolean | true |
| custom | 是否为自定义，自定义模式下只调用组件选择文件，其他的样式自定义实现，通过 `selected`事件获取到选择的文件列表。 | Boolean | false |
| tagDirection | 默认主题下标签的显示方式，支持横向显示和纵向显示。默认横向显示。值为：`row`、`col` | String | ‘row’ |

## 2、方法

| 方法名 | 描述 | 实例 |
| --- | --- | --- |
| fileSelect | `custom`主题下，调用`fileSelect`方法打开文件选择对话框。 |  |
| commit | 在调用文件上传组件的其他组件中，保存表单数据之前调用`commit`方法来保存已选择的文件数据。是一个异步方法，调用时注意添加`async`关键字 |  |

## 3、事件

| 事件名   | 说明                   |
| -------- | ---------------------- |
| selected | 返回所选中的文件信息。 |

## 4、其他说明

### 1、文件对象格式

```javascript
let file = {  fileId:'',// 文件id    name: '', // 文件名     type: '', // 文件      size: '', // 文  
  url: '',  // 文件地址
};
```
