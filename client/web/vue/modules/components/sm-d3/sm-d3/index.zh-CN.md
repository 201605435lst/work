## API

| 参数  | 说明                         | 类型     | 默认值 |
| ----- | ---------------------------- | -------- | ------ |
| axios | 项目 axios.create 创建的实例 | function |
| selectedEquipments | 选中的设备 ```[{groupName: "7号线/窑上村站/DongZhao", id: "39fcdd0b-1c74-8f24-d466-aa01c9e213a5", name: "桥架配件_1"}]``` | array |



### 事件

| 事件名称 | 说明     | 回调参数        |
| -------- | -------- | --------------- |
| selectedEquipmentsChange | 选中的设备发生变化的回调函数，```[{groupName: "7号线/窑上村站/DongZhao", id: "39fcdd0b-1c74-8f24-d466-aa01c9e213a5", name: "桥架配件_1"}]``` | function(array) |
