# SmGantt 使用说明

## 1、甘特图使用说明

### 属性：

- datas ,数组源格式如下：

  ```javascript
  datas:[{
          name: '天津地体项目',
          data: [{
            index:1,
            major:'土建专业',
            name:'继承路基建设',
            start: 1609487006000,
            end: 1609659806000,
            completed: 0.25,
            state: '未开工',
            id: "0",
            y:0,
          },
        }],
  ```

  字段说明：

  | 字段名称   | 字段说明   | 是否必须                                                     |
  | ---------- | ---------- | ------------------------------------------------------------ |
  | index      | 数组索引   | 否                                                           |
  | major      | 专业名称   | 否，根据数据源赋值                                           |
  | name       | 任务名称   | 是，为必填字段，且名称不能修改                               |
  | start      | 开始时间   | 是，为必填字段，且名称不能修改                               |
  | end        | 结束时间   | 是，为必填字段，且名称不能修改                               |
  | completed  | 当前的进度 | 是，可为空，表示进度的百分比                                 |
  | state      | 状态       | 否，根据数据源赋值，可为空                                   |
  | id         | 主键       | 是，必填内容                                                 |
  | y          | 数字索引   | 是，类似主键 ，是数字类型，不能为空，两个相等y值数据将显示在同一行，一般用来控制同一时间段内的父子任务 |
  | milestone  | 里程碑     | 否，boolean类型，为true时，表示当前任务为里程碑。            |
  | dependency | 依赖项     | 否，其值为依赖的父级id，表示任务有前置任务。                 |

  

  

- columns ,表格的列数据，默认值如下：

  ```javascript
  [
    { title: "序号", field: "index" },
    { title: "专业", field: "major" },
    { title: "任务名称", field: "name" },
    { title: "开始时间", field: "start" },
    { title: "结束时间", field: "end" },
    { title: "进度", field: "completed" },
    { title: "状态", field: "state" },
  ];
  ```

  说明：如果自己数据源的字段和默认的字段不一致，则根据字段名称重新定义columns  属性。

- gridCellHeight，表格的高度，默认是40