## API

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| axios | 项目 axios.create 创建的实例 | function | null |
| value | 已选择的条目(v-model 绑定)，value 为 Array; | Array | [{id:'',name:''}] |
| personal | 是否为个人模式，默认只能选择自己创建的或授权给自己的 | Boolean | true |
| visible | 模态框是否可见 | Boolean | false |

### 事件

| 事件名称 | 说明                       | 回调参数        |
| -------- | -------------------------- | --------------- |
| ok   | 确认后的回调函数,返回所选签章。 | function(value) |
| *change   | 取消后的回调函数，返回false，用于关闭模态框。 | function(value) |


示例：

```c#
<a-button onClick={() => {this.visible= true;}}>
  选择签章
</a-button>

<SmOaSelect
  visible={this.visible}
  onChange={}
  onOk={}
></SmOaSelect>
```